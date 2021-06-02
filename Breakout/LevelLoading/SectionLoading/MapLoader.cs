using System.Collections.Generic;

namespace Breakout.LevelLoading.SectionLoading {
    /// <summary>
    /// Facilitates the loading, i.e. processing and storage, of the Map section 
    /// into a List of strings that represent rows.
    /// </summary>
    public class MapLoader : SectionLoader {
        public List<string> RowList { get; private set; } = new List<string>();
        //Dimensions of map (counted in number of Blocks)
        public int Height { get; private set; }
        public int Width { get; private set; }
        
        public MapLoader(SectionStreamReader reader) : base(reader) {
            section = "Map";
        }

        protected override void ProcessSectionLine(string line) {
            RowList.Add(line);
        }


        /// <summary>
        /// Load text from section into internal fields. 
        /// Compute Width and Height of map.
        /// Assume reader is reset prior to loading.
        /// </summary>
        public override void LoadSection() {
            reader.SetSection(section);

            string line;
            
            while ((line = reader.ReadSectionLine()) != null) {
                ProcessSectionLine(line);
            }

            Width = RowList[0].Length;
            Height = RowList.Count;

            reader.Reset();
        }

        public override void ClearLoader()
        {
            RowList = new List<string>();
            Height = 0;
            Width = 0;
        }
    }
}