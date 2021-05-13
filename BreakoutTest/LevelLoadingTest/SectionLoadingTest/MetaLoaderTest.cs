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