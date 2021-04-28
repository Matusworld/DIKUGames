using System.IO;

namespace Breakout.LevelLoading {

    /// <summary>
    /// Validates the given ASCII map
    /// </summary>
    public class SectionsValidator {

        private StreamReader file;
        private string line;

        public SectionsValidator(string filepath) {
            file = new StreamReader(filepath);
        }

        /// <summary>
        /// Validates that the sections of the given ASCII map are Map, Meta, and Legends in that order
        /// also checks that these sections are correctly ended
        /// </summary>
        /// <returns>boolean</returns>
        public bool ValidateSections() {
            int sectionState = 0;

            while((line = file.ReadLine()) != null) {
                if (line == "Map:" && sectionState == 0) {  
                    sectionState++;
                } 
                if (line == "Map/" && sectionState == 1) {
                    sectionState++;
                }
                if (line == "Meta:" && sectionState == 2) {
                    sectionState++;
                }
                if (line == "Meta/" && sectionState == 3) {
                    sectionState++;
                }
                if (line == "Legend:" && sectionState == 4) {
                    sectionState++;
                }
                if (line == "Legend/" && sectionState == 5) {
                    return true; 
                }
            }
            return false;
        }

    }
}