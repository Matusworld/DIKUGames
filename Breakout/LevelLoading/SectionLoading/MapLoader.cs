using System.IO;
using System.Collections.Generic;

namespace Breakout.LevelLoading.SectionLoading {
    public class MapLoader : SectionLoader {
        private SectionStreamReader reader;
        private string section = "Map"; 

        public List<string> RowList { get; private set; } = new List<string>();

        public int Height { get; private set; }

        public int Width { get; private set; }
        
        public MapLoader(string filepath) {
            reader = new SectionStreamReader(filepath, section);
        }

        protected override void ProcessSectionLine(string line) {
            //find height og width
            RowList.Add(line);
        }


        /// <summary>
        /// Load text from section into internal fields
        /// </summary>
        public override void LoadSection() {
            string line;
            
            while((line = reader.ReadSectionLine()) != null) {
                ProcessSectionLine(line);
            }

            Width = RowList[0].Length;
            Height = RowList.Count;

            reader.File.Close();
        } 
    }
}