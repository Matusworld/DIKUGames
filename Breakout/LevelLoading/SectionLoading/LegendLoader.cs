using System.IO;
using System.Collections.Generic;

using DIKUArcade.Graphics;

namespace Breakout.LevelLoading.SectionLoading {
    public class LegendLoader : SectionLoader {
        private string section = "Legend"; 
        //public List<(char,string)> LegendList { get; private set; } = new List<(char, string)>();
        public Dictionary<char, (IBaseImage, IBaseImage)> LegendDict;

        public LegendLoader(SectionStreamReader reader) : base(reader) {
            LegendDict = new Dictionary<char, (IBaseImage, IBaseImage)>();
        }

        protected override void ProcessSectionLine(string line) {
            string[] splitArray = line.Split(") ");
            if (splitArray.Length == 2){
                char symb = splitArray[0][0];

                string filename = splitArray[1];
                string dmgFilename = filename.Split(".png")[0] + "-damaged.png";
                //filepaths
                string filepath = Path.Combine(new string[] {
                    ProjectPath.getPath(), "Breakout", "Assets","Images", filename});
                string dmgFilepath = Path.Combine(new string[] {
                    ProjectPath.getPath(), "Breakout", "Assets","Images", dmgFilename});

                LegendDict.Add(symb, (new Image(filepath), new Image(dmgFilepath)));
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
            LegendDict.Clear();
        }
    }
}