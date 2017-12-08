using System;
using System.Text;
using System.Text.RegularExpressions;

namespace UniCommon {
    public static class Strings {
        public static bool Except(this string self, params string[] strings) {
            for (var i = 0; i < strings.Length; i++) {
                if (self == strings[i]) return false;
            }
            return true;
        }

        public static bool Any(this string self, params string[] strings) {
            for (var i = 0; i < strings.Length; i++) {
                if (self == strings[i]) return true;
            }
            return false;
        }

        private static readonly Regex Parenthesis = new Regex("( +)|(\\(.*?\\))");

        public static string TrimSpaceAndParenthesis(this string self) {
            var result = Parenthesis.Replace(self, "");
            return result;
        }

        public static string Slice(this string self, int cnt = 1, char separator = '\n') {
            var ret = new StringBuilder();
            var sum = 0;
            for (var i = 0; i < self.Length; i++) {
                if (self[i] != separator) {
                    ret.Append(self[i]);
                } else if (++sum == cnt) {
                    break;
                }
            }
            return ret.ToString();
        }

        public static Exception ToException(this string self) {
            return new Exception(self);
        }
    }
}