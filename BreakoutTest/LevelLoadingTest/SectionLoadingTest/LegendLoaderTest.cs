using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using DIKUArcade.Graphics;

using Breakout.LevelLoading;
using Breakout.LevelLoading.SectionLoading;

namespace BreakoutTest.LevelLoadingTest.SectionLoadingTest {
    public class LegendLoaderTest {
        SectionStreamReader reader;
        LegendLoader loader;

        Dictionary<char, (IBaseImage, IBaseImage)> legenddict;

        string validFile;

        string invalidFile;

        public LegendLoaderTest() {
            legenddict = new Dictionary<char, (IBaseImage, IBaseImage)>();

            char symb1 = '#';

            Image img1 = new Image(Path.Combine( 
                TestProjectPath.getPath(),"Assets", "Images", "teal-block.png"));

            Image dmgimg1 = new Image(Path.Combine( 
                TestProjectPath.getPath(),"Assets", "Images", "teal-block-damaged.png"));

            legenddict.Add(symb1, (img1, dmgimg1));

            char symb2 = '1';

            Image img2 = new Image(Path.Combine( 
                TestProjectPath.getPath(),"Assets", "Images", "blue-block.png"));

            Image dmgimg2 = new Image(Path.Combine( 
                TestProjectPath.getPath(),"Assets", "Images", "blue-block-damaged.png"));

            legenddict.Add(symb2, (img2, dmgimg2));

            char symb3 = '2';

            Image img3 = new Image(Path.Combine( 
                TestProjectPath.getPath(),"Assets", "Images", "green-block.png"));

            Image dmgimg3 = new Image(Path.Combine( 
                TestProjectPath.getPath(),"Assets", "Images", "green-block-damaged.png"));

            legenddict.Add(symb3, (img3, dmgimg3));

            char symb4 = 'q';

            Image img4 = new Image(Path.Combine( 
                TestProjectPath.getPath(),"Assets", "Images", "darkgreen-block.png"));

            Image dmgimg4 = new Image(Path.Combine( 
                TestProjectPath.getPath(),"Assets", "Images", "darkgreen-block-damaged.png"));

            legenddict.Add(symb4, (img4, dmgimg4));
        }

        [SetUp]
        public void Setup() {
            validFile = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", "level1.txt");

            invalidFile = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", 
                "wrongsectioncontent.txt");

            reader = new SectionStreamReader();
        }

        // Testing the LegendLoader on a valid file
        [Test]
        public void ValidLegendTest() {
            reader.SetPath(validFile);

            loader = new LegendLoader(reader);
            loader.LoadSection();

            Assert.AreEqual(loader.LegendDict.Count, legenddict.Count);
            Assert.AreEqual(loader.LegendDict.Keys, legenddict.Keys);
            Assert.AreEqual(loader.LegendDict.Values.ToString(), legenddict.Values.ToString());
        }

        // Testing the LegendLoader on a invalid file
        [Test]
        public void InvalidLegendTest() {
            reader.SetPath(invalidFile);

            loader = new LegendLoader(reader);

            bool check = false;

            try {
                loader.LoadSection();
                loader.ClearLoader();
            } catch (InvalidDataException) {
                check = true;
                Assert.True(check);
            }
        }

        // Testing clearing the legend loader data (resetting)
        [Test]
        public void ClearLoaderTest() {
            reader.SetPath(validFile);

            loader = new LegendLoader(reader);
            loader.LoadSection();

            Assert.AreEqual(4,loader.LegendDict.Count);

            loader.ClearLoader();

            Assert.AreEqual(0,loader.LegendDict.Count);

        }
    }
}