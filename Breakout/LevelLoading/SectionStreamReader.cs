using System.IO;

namespace Breakout.LevelLoading {
    /// <summary>
    /// Given a path and a section this SectionStreamReader will read from 
    /// the given section of the file acting as a basic StreamReader, but only
    /// on the section.
    /// </summary>
    public class SectionStreamReader {
        private string path;
        private string section;
        private string line;
        private bool sectionReached = false;


        public SectionStreamReader() {}

        /// <summary>
        /// Set section to prepare reading
        /// </summary>
        /// <param name="section">Section of ASCII file</param>
        public void SetSection(string section) {
            this.section = section; 
        }

        /// <summary>
        /// Reset reader position.
        /// </summary>
        public void Reset() {
            BreakoutStreamReader.ResetReader();
        }
        
        /// <summary>
        /// Set path for reading
        /// </summary>
        /// <param name="path"> The file-path to the file that should be read</param>
        public void SetPath(string path) {
            this.path = path;
        }

        /// <summary>
        /// Act like a basic StreamReader's ReadLine method with the following modifications:
        /// - First line returned will be the the first line of the given section.
        /// - When the section end is reached, null is returned 
        /// </summary>
        /// <returns> the read section line </returns>
        public string ReadSectionLine() {
            StreamReader reader = BreakoutStreamReader.GetReader(path);
            while ((line = reader.ReadLine()) != null) {
                //skip if section is not reached
                if (sectionReached || (!sectionReached && line == section + ":")) {
                    //increase state if section start is reached
                    if (!sectionReached && line == section + ":") {
                        sectionReached = true;
                    }
                    //return null if section end is reached and reset state
                    if (sectionReached && line == section + "/") {
                        sectionReached = false;
                        return null;
                    }
                    //return line if within section
                    if (sectionReached && line != section + ":" && line != section + "/") {
                        return line;
                    }
                } 
            }
            //will be null if escaped while loop 
            return line;
        }
    }
}