using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniCommon {
    public interface ILazy<out T> {
        T Value { get; }
    }

    // Basic
    public class Lazy<TParam, TOut> : ILazy<TOut> {
        protected TParam param;
        protected TOut value;
        protected Func<TParam, TOut> provider;

        public static ILazy<TO> New<TP, TO>(TP param, Func<TP, TO> provider) {
            return new Lazy<TP, TO>(param, provider);
        }

        internal Lazy(TParam param, Func<TParam, TOut> provider) {
            this.param = param;
            this.provider = provider;
        }

        public TOut Value {
            get { return value != null ? value : (value = provider(param)); }
        }
    }

    // Without initialization params
    public sealed class LazyObject<T> : Lazy<object, T> {
        public LazyObject(Func<T> provider) : base(null, o => provider()) {
        }
    }

    // Resource
    public sealed class LazyResource<T> : Lazy<string, T> where T : Object {
        public LazyResource(string path) : base(path, Resources.Load<T>) {
        }
    }

    public sealed class LazyGameObject {
        public static ILazy<GameObject> Tag(string tag) {
            return new Lazy<string, GameObject>(tag, GameObject.FindGameObjectWithTag);
        }

        public static ILazy<T> Tag<T>(string tag) {
            return new LazyObject<T>(() => GameObject.FindGameObjectWithTag(tag).GetComponent<T>());
        }

        public static ILazy<T> Type<T>() where T : Object {
            return new LazyObject<T>(Object.FindObjectOfType<T>);
        }
    }

    // Component
    public sealed class LazyComponent<T> : Lazy<GameObject, T> where T : Component {
        public LazyComponent(GameObject param) : base(param, o => o.AddComponent<T>()) {
        }

        public LazyComponent(Func<GameObject> lobj) : base(null, o => lobj().AddComponent<T>()) {
        }

        public LazyComponent(MonoBehaviour param) : base(null, o => param.gameObject.AddComponent<T>()) {
        }

        public LazyComponent(LazyObject<GameObject> lobj) : base(null, o => lobj.Value.AddComponent<T>()) {
        }
    }
}