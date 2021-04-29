using System.IO;
using System.Collections.Generic;

namespace Breakout.LevelLoading.SectionLoading {
    public class LegendLoader : SectionLoader {
        private SectionStreamReader reader;
        private string section = "Legend"; 
        public List<(char,string)> LegendList { get; private set; } = new List<(char, string)>();

        public LegendLoader(string filepath) {
            reader = new SectionStreamReader(filepath, section);
        }

        protected override void ProcessSectionLine(string line) {
            string[] splitArray = line.Split(") ");
            if (splitArray.Length == 2){
                LegendList.Add(((char) splitArray[0][0],splitArray[1]));
            } else {
                throw new InvalidDataException("Invalid legend data in the level file");
            }
        }


        /// <summary>
        /// Load text from section into internal fields
        /// </summary>
        public override void LoadSection() {
            string line;
            
            while((line = reader.ReadSectionLine()) != null) {
                ProcessSectionLine(line);
            }

            reader.File.Close();
        } 
    }
}