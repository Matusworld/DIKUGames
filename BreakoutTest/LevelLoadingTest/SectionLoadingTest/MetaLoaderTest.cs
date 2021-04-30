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
    public class MetaLoaderTest {
        MetaLoader loader;
        string name = "LEVEL 1";
        int time = 300;
        char hardened = '#';
        char powerup = '2';

        string validFile;

        string invalidFile;

        [SetUp]
        public void Setup() {

            // DIKUGames\BreakoutTest\bin\Debug\net5.0
            var dir = new DirectoryInfo(Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location));

            while (dir.Name != "bin") {
                dir = dir.Parent;
            }
            
            // DIKUGames\BreakoutTest
            dir = dir.Parent;

            validFile = dir + @"\Assets\Levels\level1.txt";

            invalidFile = dir + @"\Assets\Levels\wrongsectioncontent.txt";
        }

        [Test]
        public void ValidMetaTest() {
            loader = new MetaLoader(validFile);

            loader.LoadSection();

            Assert.AreEqual(loader.Name, name);

            Assert.AreEqual(loader.Time, time);

            Assert.AreEqual(loader.Hardened, hardened);

            Assert.AreEqual(loader.Powerup, powerup);
        }

        [Test]
        public void InvalidMetaTest() {
            loader = new MetaLoader(invalidFile);
            
            bool check = false;

            try {
                loader.LoadSection();
            } catch (InvalidDataException) {
                check = true;
                Assert.True(check);
            }

        }
    }
}