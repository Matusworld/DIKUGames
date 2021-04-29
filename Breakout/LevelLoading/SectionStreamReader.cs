using System.IO;

namespace Breakout.LevelLoading {
    /// <summary>
    /// Given path and specified section
    /// Responsibility of closing Stream passes to user
    /// </summary>
    class SectionStreamReader {
        public StreamReader File { get; private set; }
        private string section;
        private string line;
        private int state = 0;


        public SectionStreamReader(string filepath, string section) {
            File = new StreamReader(filepath);
            this.section = section; 
        }

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