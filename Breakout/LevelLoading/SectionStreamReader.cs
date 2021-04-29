using System.IO;

namespace Breakout.LevelLoading {
    class SectionStreamReader {
        public StreamReader File { get; private set; }
        private string section;
        private string line;
        private int state = 0;


        public SectionStreamReader(string filepath, string section) {
            File = new StreamReader(filepath);
            this.section = section; 
        }

        /// <summary>
        /// Act like a basic StreamReader's ReadLine method with the following modifications:
        /// - First line returned will be the the first line of the given section.
        /// - When the section end is reached, null is returned 
        /// </summary>
        /// <returns> the read line </returns>
        public string ReadSectionLine(){
            while ((line = File.ReadLine()) != null) {
                //skip if section is not reached
                if (state != 0 || (state == 0 && line == section + ":")) {
                    //increase state if section start is reached
                    if (state == 0 && line == section + ":") {
                        state++;
                    }
                    //return null if section end is reached
                    if (state == 1 && line == section + "/") {
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