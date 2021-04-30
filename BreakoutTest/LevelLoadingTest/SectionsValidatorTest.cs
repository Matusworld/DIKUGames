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

    public class SectionValidatorTest {
        SectionsValidator sectionsValidator;

        string validSectionsFile;

        string invalidSectionsFile;

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

            validSectionsFile = dir + @"\Assets\Levels\level1.txt";

            invalidSectionsFile = dir + @"\Assets\Levels\wrongsectionorder.txt";
        }

        [Test]
        public void TestValidSectionsValidator() {
            sectionsValidator = new SectionsValidator(validSectionsFile);

            Assert.IsTrue(sectionsValidator.ValidateSections());
        }

        [Test]
        public void TestInvalidSectionsValidator() {
            sectionsValidator = new SectionsValidator(invalidSectionsFile);

            Assert.IsFalse(sectionsValidator.ValidateSections());
        }
    }
}