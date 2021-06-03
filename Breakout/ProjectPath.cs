using System.IO;

namespace Breakout {
    /// <summary>
    /// Responsible for finding the path to the root folder of the project. 
    /// </summary>
    public static class ProjectPath {
        public static string getPath() {
            var dir = new DirectoryInfo(Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location));

            while (dir.Name != "DIKUGames") {
                dir = dir.Parent;
            }
            return dir.ToString();
        }
    }
}