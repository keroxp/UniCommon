using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UniCommon {
    public interface IResrouceLoader {
        IEnumerable<T> LoadAll<T>(string dirpath, string pattern, SearchOption option) where T : Object;
        IEnumerable<T> LoadAllPrefabs<T>(string dirpath) where T : Object;
    }

    public class ResourceLoader : IResrouceLoader {
        private static readonly ResourceLoader _instance = new ResourceLoader();

        public static ResourceLoader Instance {
            get { return _instance; }
        }

        private ResourceLoader() {
        }

        public IEnumerable<T> LoadAll<T>(string dirpath, string pattern, SearchOption option) where T : Object {
            return Resources.LoadAll<T>(dirpath);
        }

        public IEnumerable<T> LoadAllPrefabs<T>(string dirpath) where T : Object {
            return LoadAll<T>(dirpath, "*.prefab", SearchOption.TopDirectoryOnly);
        }
    }
}