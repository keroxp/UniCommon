#if UNITY_IOS
using System.Runtime.InteropServices;

#endif
namespace UniCommon.Crypt {
    public static class Secret {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern string Hexat_Secret_iOS();
#endif
        public static string GetSecret(string version) {
#if UNITY_EDITOR
            return "password";
#elif UNITY_IOS
            return Hexat_Secret_iOS();
#elif UNITY_ANDROID
            return "password";
#else
            return "password";
#endif
        }
    }
}