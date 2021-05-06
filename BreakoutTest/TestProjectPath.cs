using System.IO;

namespace BreakoutTest {
    public static class TestProjectPath {
        public static string getPath() {
            var dir = new DirectoryInfo(Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location));

            while (dir.Name != "BreakoutTest") {
                dir = dir.Parent;
            }
            return dir.ToString();
        }
    }
}