namespace UniCommon {
    public static class Predicates {
        public static bool AreNotNull(params object[] objs) {
            for (var i = 0; i < objs.Length; i++) if (objs[i] == null) return false;
            return true;
        }
    }
}