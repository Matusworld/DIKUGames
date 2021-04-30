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

    public class SectionStreamReaderTest {
        SectionStreamReader mapReader;

        SectionStreamReader metaReader;

        SectionStreamReader legendReader;

        string path;

        List<string> map = new List<string>{"------------", "------------", "-qqqqqqqqqq-", "-qqqqqqqqqq-",
            "-111----111-", "-111-##-111-", "-111-22-111-", "-111-##-111-", "-111----111-", "-##########-",
            "-##########-", "------------", "------------", "------------", "------------", "------------",
            "------------", "------------", "------------", "------------", "------------", "------------",
            "------------", "------------", "------------"};


        List<string> meta = new List<string> {"Name: LEVEL 1", "Time: 300", "Hardened: #", "PowerUp: 2"};
    
        List<string> legend = new List<string> {"#) teal-block.png", "1) blue-block.png", "2) green-block.png",
            "q) darkgreen-block.png"};
        
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

            path = Path.Combine(dir.ToString(), "Assets", "Levels", "level1.txt");

            mapReader = new SectionStreamReader(path, "Map");

            metaReader = new SectionStreamReader(path, "Meta");

            legendReader = new SectionStreamReader(path, "Legend");
        }

        [Test]
        public void TestMapReadSection() {

            string line;

            int i = 0;

            while((line = mapReader.ReadSectionLine()) != null) {
                Assert.AreEqual(line, map[i]);
                i++;
            }
        }

        [Test]
        public void TestMetaReadSection() {
            string line;

            int i = 0;

            while((line = metaReader.ReadSectionLine()) != null) {
                Assert.AreEqual(line, meta[i]);
                i++;
            }
        }

        [Test]
        public void TestLegendReadSection() {
            string line;

            int i = 0;

            while((line = legendReader.ReadSectionLine()) != null) {
                Assert.AreEqual(line, legend[i]);
                i++;
            }
        }
    }
}