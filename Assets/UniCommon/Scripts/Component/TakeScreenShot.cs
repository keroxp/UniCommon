using System;
using UnityEngine;

namespace UniCommon {
    public class TakeScreenShot : ACommonBehaviour {
        public int superSize = 1;
        public string filePath = "~/Desktop";
#if UNITY_EDITOR
        protected override void OnUpdate() {
            if (Input.GetKeyDown(KeyCode.S)) {
                var fn = string.Format("{0}-{1:yyyy-MM-dd-hh:mm:ss.zzz}.png", Application.productName, DateTime.Now);
                ScreenCapture.CaptureScreenshot(filePath + "/" + fn, superSize);
            }
        }
#endif
    }
}