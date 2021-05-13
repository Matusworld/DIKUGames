using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;
using NUnit.Framework;
using Breakout;
using Breakout.LevelLoading;
using Breakout.LevelLoading.SectionLoading;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace BreakoutTest {

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