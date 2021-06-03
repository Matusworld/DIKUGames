using System.IO;

namespace Breakout.LevelLoading{
    /// <summary>
    /// BreakoutStreamReader is a Singleton StreamReader that can reset to initial reading position.
    /// If the same path is given consecutively the StreamReader object will persist.
    /// If a new path is given, a new StreamReader will be initialized.
    /// </summary>
    public static class BreakoutStreamReader {
        private static StreamReader reader;
        private static string currentPath;

        /// <summary>
        /// Get a StreamReader for a given path. If the path is new, 
        /// the reader is reinitialized.
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>The StreamReader with the given path</returns>
        public static StreamReader GetReader(string path) {
            //set and get reader
            if(currentPath != path) {
                if (reader != null) {
                    reader.Close();
                }
                currentPath = path;
                reader = new StreamReader(path);
                return reader;
            }
            //get reader
            else {
                return reader;
            }
        }

        /// <summary>
        /// Reset this StreamReader to start of current file.
        /// </summary>
        public static void ResetReader() {
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
        }
    }
}