using System.IO;

namespace Breakout.LevelLoading {

    /// <summary>
    /// Validates the given ASCII map
    /// </summary>
    public class SectionsValidator {
        private string line;

        public SectionsValidator() {}

        /// <summary>
        /// Validates that the sections of the given ASCII map are Map, Meta, and Legends in that order
        /// also checks that these sections are correctly ended
        /// </summary>
        /// <returns>True if valid</returns>
        public bool ValidateSections(string path) {
            //init new reader
            StreamReader reader = BreakoutStreamReader.GetReader(path);
            int sectionState = 0;

            while((line = reader.ReadLine()) != null) {
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
                    BreakoutStreamReader.ResetReader();
                    return true; 
                }
            }
            return false;
        }

    }
}