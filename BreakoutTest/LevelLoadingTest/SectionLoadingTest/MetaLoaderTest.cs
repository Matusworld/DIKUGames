using System.IO;

using NUnit.Framework;

using Breakout.LevelLoading;
using Breakout.LevelLoading.SectionLoading;

namespace BreakoutTest {
    public class MetaLoaderTest {
        SectionStreamReader reader;
        MetaLoader loader;
        string name = "LEVEL 1";
        int time = 300;
        char hardened = '#';
        char powerup = '2';

        string validFile;

        string invalidFile;

        [SetUp]
        public void Setup() {
            validFile = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", "level1.txt");

            invalidFile = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", 
                "wrongsectioncontent.txt");

            reader = new SectionStreamReader();
        }

        // Testing the MetaLoader on a valid file
        [Test]
        public void ValidMetaTest() {
            reader.SetPath(validFile);

            loader = new MetaLoader(reader);

            loader.LoadSection();

            Assert.AreEqual(loader.Name, name);

            Assert.AreEqual(loader.Time, time);

            Assert.AreEqual(loader.Hardened, hardened);

            Assert.AreEqual(loader.Powerup, powerup);

            loader.ClearLoader();
        }

        // Testing the MetaLoader on a invalid file
        [Test]
        public void InvalidMetaTest() {
            reader.SetPath(invalidFile);

            loader = new MetaLoader(reader);
            
            bool check = false;

            try {
                loader.LoadSection();
                loader.ClearLoader();
            } catch (InvalidDataException) {
                check = true;
                Assert.True(check);
            }

        }
    }
}