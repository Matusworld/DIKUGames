using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;
using NUnit.Framework;
using Breakout;
using Breakout.LevelLoading;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace BreakoutTest {
    public class LevelLoaderTest {
        LevelLoader loader;

        string validFile;

        string inValidFile;

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

            inValidFile = dir + @"\Assets\Levels\wrongsectionorder.txt";   
        }

        [Test]
        // Tested Visaully, that blocks have the correct position compared to level files. 
        public void validFileLevelLoaderTest() {
            loader = new LevelLoader(validFile);

            loader.LoadLevel();
        }

        [Test]
        public void InvalidFileLevelLoaderTest() {
            loader = new LevelLoader(inValidFile);

            bool check = false;

            try {
                loader.LoadLevel();
            } catch (InvalidDataException) {
                check = true;
                Assert.True(check);
            }
        }
    }
}