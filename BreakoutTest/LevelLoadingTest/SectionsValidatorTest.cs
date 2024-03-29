using System.IO;

using NUnit.Framework;

using Breakout;
using Breakout.LevelLoading;

namespace BreakoutTest.LevelLoadingTest {

    public class SectionValidatorTest {
        SectionsValidator sectionsValidator;

        string validSectionsFile;

        string invalidSectionsFile;

        [SetUp]
        public void Setup() {

            validSectionsFile = Path.Combine(ProjectPath.getPath(), "Breakout", "Assets", "Levels", 
                "level1.txt");

            invalidSectionsFile = Path.Combine(ProjectPath.getPath(), 
                "Breakout", "Assets", "Levels", "wrongsectionorder.txt");
        }

        // Testing the section validator on a valid file
        [Test]
        public void TestValidSectionsValidator() {
            sectionsValidator = new SectionsValidator();

            Assert.IsTrue(sectionsValidator.ValidateSections(validSectionsFile));
        }

        // Testing the section validator on a invalid file
        [Test]
        public void TestInvalidSectionsValidator() {
            sectionsValidator = new SectionsValidator();

            Assert.IsFalse(sectionsValidator.ValidateSections(invalidSectionsFile));
        }
    }
}