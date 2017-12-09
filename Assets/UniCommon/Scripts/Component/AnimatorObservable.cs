using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UniCommon {
    public struct AnimatorStateArgs {
        public Animator animator;
        public AnimatorStateInfo info;
        public int layerIndex;
        public AnimatorEvent @event;
    }

    public struct AnimatorTransitionArgs {
        public Animator animator;
        public AnimatorTransitionInfo info;
        public AnimatorStateInfo prevState;
        public int layerIndex;
        public AnimatorEvent @event;
    }

    public enum AnimatorEvent {
        Enter,
        Stay,
        Exit
    }

    public class AnimatorObservable {
        private readonly Subject<AnimatorStateArgs> _stateSubject = new Subject<AnimatorStateArgs>();
        private readonly Subject<AnimatorTransitionArgs> _transitionSubject = new Subject<AnimatorTransitionArgs>();
        private readonly Dictionary<int, AnimatorStateInfo> _prevStates = new Dictionary<int, AnimatorStateInfo>();

        private readonly Dictionary<int, AnimatorTransitionInfo> _prevTransitions =
            new Dictionary<int, AnimatorTransitionInfo>();

        private AnimatorStateArgs _stateArgs;
        private AnimatorTransitionArgs _transitionArgs;
        private readonly Animator _animator;

        public AnimatorObservable(Animator animator) {
            _animator = animator;
            Observable.EveryUpdate().Subscribe(Update);
        }

        private AnimatorStateArgs StateArgs(int layerIndex, AnimatorStateInfo info, AnimatorEvent ev) {
            _stateArgs.animator = _animator;
            _stateArgs.@event = ev;
            _stateArgs.layerIndex = layerIndex;
            _stateArgs.info = info;
            return _stateArgs;
        }

        private AnimatorTransitionArgs TransitionArgs(int layerIndex, AnimatorEvent ev, AnimatorTransitionInfo info,
            AnimatorStateInfo prevState) {
            _transitionArgs.animator = _animator;
            _transitionArgs.@event = ev;
            _transitionArgs.layerIndex = layerIndex;
            _transitionArgs.prevState = prevState;
            _transitionArgs.info = info;
            return _transitionArgs;
        }

        public void Update(long _) {
            if (_animator == null || !_animator.gameObject.activeInHierarchy) return;
            for (var i = 0; i < _animator.layerCount; i++) {
                if (_animator.IsInTransition(i)) {
                    var prev = _prevStates.ContainsKey(i)
                        ? _prevStates[i]
                        : (_prevStates[i] = _animator.GetCurrentAnimatorStateInfo(i));
                    var trans = _animator.GetAnimatorTransitionInfo(i);
                    if (!_prevTransitions.ContainsKey(i)) {
                        _stateSubject.OnNext(StateArgs(i, prev, AnimatorEvent.Exit));
                        _transitionSubject.OnNext(TransitionArgs(i, AnimatorEvent.Enter, trans, prev));
                        _prevTransitions[i] = trans;
                    } else {
                        _transitionSubject.OnNext(TransitionArgs(i, AnimatorEvent.Stay, trans, prev));
                    }
                } else {
                    var state = _animator.GetCurrentAnimatorStateInfo(i);
                    if (!_prevStates.ContainsKey(i)) {
                        // first
                        _stateSubject.OnNext(StateArgs(i, state, AnimatorEvent.Enter));
                        _prevStates[i] = state;
                    }
                    var prev = _prevStates[i];
                    if (state.fullPathHash != prev.fullPathHash) {
                        if (_prevTransitions.ContainsKey(i)) {
                            // state changed with transition
                            var pt = _prevTransitions[i];
                            _transitionSubject.OnNext(TransitionArgs(i, AnimatorEvent.Exit, pt, prev));
                            _prevTransitions.Remove(i);
                        } else {
                            // state changed with no transition
                            _stateSubject.OnNext(StateArgs(i, prev, AnimatorEvent.Exit));
                        }
                        _stateSubject.OnNext(StateArgs(i, state, AnimatorEvent.Enter));
                    } else {
                        _stateSubject.OnNext(StateArgs(i, state, AnimatorEvent.Stay));
                    }
                    _prevStates[i] = state;
                }
            }
        }

        public IObservable<AnimatorStateArgs> StateObservable {
            get { return _stateSubject; }
        }

        public IObservable<AnimatorTransitionArgs> TransitionObservable {
            get { return _transitionSubject; }
        }
    }
}