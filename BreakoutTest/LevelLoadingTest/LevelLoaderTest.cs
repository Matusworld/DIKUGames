using System.IO;

using NUnit.Framework;

using Breakout.LevelLoading;

namespace BreakoutTest {
    public class LevelLoaderTest {
        LevelLoader loader;

        string validFile;

        string inValidFile;

        [SetUp]
        public void Setup() {
            validFile = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", "level4.txt");

            inValidFile = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", 
                "wrongsectionorder.txt");

            loader = new LevelLoader();
        }

        [Test]
        // Tested Visaully, that blocks have the correct position compared to level files. 
        public void validFileLevelLoaderTest() {
            loader.LoadLevel(validFile);
        }

        [Test]
        public void InvalidFileLevelLoaderTest() {
            bool check = false;

            try {
                loader.LoadLevel(inValidFile);
            } catch (InvalidDataException) {
                check = true;
                Assert.True(check);
            }
        }
    }
}