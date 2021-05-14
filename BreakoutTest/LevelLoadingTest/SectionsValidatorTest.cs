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

            validSectionsFile = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", 
                "level1.txt");

            invalidSectionsFile = Path.Combine(TestProjectPath.getPath(), "Assets", "Levels", 
                "wrongsectionorder.txt");
        }

        [Test]
        public void TestValidSectionsValidator() {
            sectionsValidator = new SectionsValidator();

            Assert.IsTrue(sectionsValidator.ValidateSections(validSectionsFile));
        }

        [Test]
        public void TestInvalidSectionsValidator() {
            sectionsValidator = new SectionsValidator();

            Assert.IsFalse(sectionsValidator.ValidateSections(invalidSectionsFile));
        }
    }
}