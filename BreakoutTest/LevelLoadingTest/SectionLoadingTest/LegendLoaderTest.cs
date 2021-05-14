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
    public class LegendLoaderTest {
        SectionStreamReader reader;
        LegendLoader loader;

        List<(char,string)> legendList = new List<(char, string)> {('#', "teal-block.png"), 
            ('1', "blue-block.png"), ('2', "green-block.png"), ('q', "darkgreen-block.png")};

        string validFile;

        string invalidFile;

        [SetUp]
        public void Setup() {
            validFile = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", "level1.txt");

            invalidFile = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", 
                "wrongsectioncontent.txt");

            reader = new SectionStreamReader();
        }

        [Test]
        public void ValidLegendTest() {
            reader.SetPath(validFile);

            loader = new LegendLoader(reader);
            loader.LoadSection();

            Assert.AreEqual(loader.LegendList, legendList);
        }

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
    }
}