using System.IO;

namespace Breakout.LevelLoading{
    public static class BreakoutStreamReader {
        private static StreamReader reader;
        private static string currentPath;

        /// <summary>
        /// Get the reader object for a given path. If the path is new, 
        /// the reader is implicitly reset
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static StreamReader GetReader(string path) {
            //set and get reader
            if(currentPath != path) {
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
        /// Reset reader to start of current file.
        /// </summary>
        public static void ResetReader() {
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
        }
    }
}