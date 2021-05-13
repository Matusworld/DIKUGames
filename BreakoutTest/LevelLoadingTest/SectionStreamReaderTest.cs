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
        SectionStreamReader reader;
        //SectionStreamReader mapReader;
        //SectionStreamReader metaReader;
        //SectionStreamReader legendReader;

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
            path = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", "level1.txt");

            reader = new SectionStreamReader();
            reader.SetPath(path);
        }

        [Test]
        public void TestMapReadSection() {
            reader.SetSection("Map");

            string line;

            int i = 0;

            while((line = reader.ReadSectionLine()) != null) {
                Assert.AreEqual(line, map[i]);
                i++;
            }

            reader.Reset();
        }

        [Test]
        public void TestMetaReadSection() {
            reader.SetSection("Meta");

            string line;

            int i = 0;

            while((line = reader.ReadSectionLine()) != null) {
                Assert.AreEqual(line, meta[i]);
                i++;
            }

            reader.Reset();
        }

        [Test]
        public void TestLegendReadSection() {
            reader.SetSection("Legend");

            string line;

            int i = 0;

            while((line = reader.ReadSectionLine()) != null) {
                Assert.AreEqual(line, legend[i]);
                i++;
            }

            reader.Reset();
        }
    }
}