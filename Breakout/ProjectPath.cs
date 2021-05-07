using System.IO;

namespace Breakout {
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