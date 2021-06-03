using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using Breakout.LevelLoading;
using Breakout.LevelLoading.SectionLoading;

namespace BreakoutTest.GamePlayTest.PowerUpOrbEntityTest {

    public class MapLoaderTest {
        SectionStreamReader reader;
        MapLoader loader;
        string path;
        int width = 12;
        int height = 25;

        List<string> rowlist = new List<string>{"------------", "------------", "-qqqqqqqqqq-", "-qqqqqqqqqq-",
            "-111----111-", "-111-##-111-", "-111-22-111-", "-111-##-111-", "-111----111-", "-##########-",
            "-##########-", "------------", "------------", "------------", "------------", "------------",
            "------------", "------------", "------------", "------------", "------------", "------------",
            "------------", "------------", "------------"};

        [SetUp]
        public void Setup() {
            path = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", "level1.txt");

            reader = new SectionStreamReader();
        }

        // Testing the Map loader gives the correct results
        [Test]
        public void Testloader() {
            reader.SetPath(path);

            loader = new MapLoader(reader);

            loader.LoadSection();

            Assert.AreEqual(loader.RowList, rowlist);

            Assert.AreEqual(loader.Height, height);

            Assert.AreEqual(loader.Width, width);
        }



    }
}