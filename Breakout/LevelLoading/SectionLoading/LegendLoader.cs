using System.IO;
using System.Collections.Generic;

namespace Breakout.LevelLoading.SectionLoading {
    public class LegendLoader : SectionLoader {
        private string section = "Legend"; 
        public List<(char,string)> LegendList { get; private set; } = new List<(char, string)>();

        public LegendLoader(SectionStreamReader reader) : base(reader) {}

        protected override void ProcessSectionLine(string line) {
            string[] splitArray = line.Split(") ");
            if (splitArray.Length == 2){
                LegendList.Add(((char) splitArray[0][0],splitArray[1]));
            } else {
                throw new InvalidDataException("Invalid legend data in the level file");
            }
        }

        public override void LoadSection() {
            reader.SetSection(section);

            string line;
            
            while((line = reader.ReadSectionLine()) != null) {
                ProcessSectionLine(line);
            }

            reader.Reset();
        }

        public override void ClearLoader()
        {
            LegendList = new List<(char, string)>();
        }
    }
}