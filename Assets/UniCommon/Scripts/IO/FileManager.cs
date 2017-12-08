using System;
using System.IO;

namespace UniCommon {
    public static class FileManager {
        public static string UniqueTempFilePath(string dir) {
            return TempPath(dir + "/" + DateTime.Now.ToBinary().ToString());
        }

        public static void TryReplace(string src, string dest) {
            var backup = TempPath(dest);
            try {
                if (File.Exists(dest)) {
                    // destがあればswap
                    File.Move(dest, backup);
                    File.Move(src, dest);
                    File.Delete(src);
                } else {
                    // なければmove
                    File.Move(src, dest);
                }
            } catch (Exception e) {
                Debugs.Error("FileManager]",
                    string.Format("failed to swap files: #{0}, #{1}, #{2}", src, dest, e.Message));
                throw;
            } finally {
                File.Delete(backup);
            }
        }

        public static void TryWrite(string path, byte[] data, FileMode mode = FileMode.OpenOrCreate) {
            var lck = Locks.Get(path);
            lock (lck) {
                var _path = path;
                var exists = File.Exists(path);
                if (exists) {
                    // ファイルが存在する場合は一度書き込みをtmpファイルへとする
                    _path = TempPath(path);
                }
                using (lck) {
                    using (var fileStream = new FileStream(_path, mode)) {
                        using (var writer = new BinaryWriter(fileStream)) {
                            writer.Write(data);
                            writer.Flush();
                        }
                    }
                }
                if (exists) {
                    // 1. 既存のファイルを.backupに変える
                    TryReplace(path, BackupPath(path));
                    // 2. 保存した.tmpファイルを真ファイルに変える
                    TryReplace(_path, path);
                }
            }
        }

        public static byte[] TryRead(string path) {
            var lck = Locks.Get(path);
            lock (lck) {
                using (lck) {
                    using (var fileStream = new FileStream(path, FileMode.Open)) {
                        var length = (int) fileStream.Length;
                        var ret = new byte[length];
                        var len = 0;
                        using (var reader = new BinaryReader(fileStream)) {
                            while (len < length) {
                                len = reader.Read(ret, len, length);
                            }
                        }
                        return ret;
                    }
                }
            }
        }

        public static byte[] TryReadBackup(string path) {
            return TryRead(BackupPath(path));
        }

        public static bool HasBackup(string path) {
            try {
                return File.Exists(BackupPath(path));
            } catch (Exception e) {
                Debugs.Exception(e);
            }
            return false;
        }

        public static bool Delete(string path) {
            try {
                File.Delete(path);
                return true;
            } catch (Exception e) {
                Debugs.Error("FileManager", "failed delete file: " + path + "," + e.Message);
                return false;
            }
        }

        private static string BackupPath(string path) {
            return path + ".backup";
        }

        private static string TempPath(string path) {
            return path + ".tmp";
        }
    }
}