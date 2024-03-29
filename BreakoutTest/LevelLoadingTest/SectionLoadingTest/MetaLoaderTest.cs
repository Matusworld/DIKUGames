using System.IO;

using NUnit.Framework;

using Breakout;
using Breakout.LevelLoading;
using Breakout.LevelLoading.SectionLoading;

namespace BreakoutTest.GamePlayTest.PowerUpOrbEntityTest {
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
            validFile = Path.Combine(ProjectPath.getPath(),
                "Breakout", "Assets", "Levels", "level1.txt");

            invalidFile = Path.Combine(ProjectPath.getPath(), "Breakout", "Assets", "Levels", 
                "wrongsectioncontent.txt");

            reader = new SectionStreamReader();
        }

        // Testing the MetaLoader on a valid file
        [Test]
        public void ValidMetaTest() {
            loader = new MetaLoader(reader);

            loader.LoadSection(validFile);

            Assert.AreEqual(loader.Name, name);

            Assert.AreEqual(loader.Time, time);

            Assert.AreEqual(loader.Hardened, hardened);

            Assert.AreEqual(loader.Powerup, powerup);

            loader.ClearLoader();
        }

        // Testing the MetaLoader on a invalid file
        [Test]
        public void InvalidMetaTest() {
            loader = new MetaLoader(reader);
            
            bool check = false;

            try {
                loader.LoadSection(invalidFile);
                loader.ClearLoader();
            } catch (InvalidDataException) {
                check = true;
                Assert.True(check);
            }

        }
    }
}