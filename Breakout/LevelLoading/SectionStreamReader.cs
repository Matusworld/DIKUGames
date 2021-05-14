using System.IO;

namespace Breakout.LevelLoading {
    /// <summary>
    /// Given path and specified section
    /// Responsibility of closing Stream passes to user
    /// </summary>
    public class SectionStreamReader {
        private string path;
        private string section;
        private string line;
        private int state = 0;


        public SectionStreamReader() {}

        /// <summary>
        /// Set section to prepare reading
        /// </summary>
        /// <param name="section"></param>
        public void SetSection(string section) {
            this.section = section; 
        }

        /// <summary>
        /// Reset reader position
        /// </summary>
        public void Reset() {
            BreakoutStreamReader.ResetReader();
        }
        
        /// <summary>
        /// Set path for reading
        /// </summary>
        /// <param name="path"></param>
        public void SetPath(string path) {
            this.path = path;
        }

        /// <summary>
        /// Act like a basic StreamReader's ReadLine method with the following modifications:
        /// - First line returned will be the the first line of the given section.
        /// - When the section end is reached, null is returned 
        /// </summary>
        /// <returns> the read line </returns>
        public string ReadSectionLine() {
            StreamReader reader = BreakoutStreamReader.GetReader(path);
            while ((line = reader.ReadLine()) != null) {
                //skip if section is not reached
                if (state != 0 || (state == 0 && line == section + ":")) {
                    //increase state if section start is reached
                    if (state == 0 && line == section + ":") {
                        state++;
                    }
                    //return null if section end is reached and reset state
                    if (state == 1 && line == section + "/") {
                        state = 0;
                        return null;
                    }
                    //return line if within section
                    if (state == 1 && line != section + ":" && line != section + "/") {
                        return line;
                    }
                } 
            }
            //will be null if escaped while loop 
            return line;
        }
    }
}