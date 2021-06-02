using System.IO;
using System.Collections.Generic;

using DIKUArcade.Graphics;

namespace Breakout.LevelLoading.SectionLoading {
    /// <summary>
    /// Facilitates the loading, i.e. processing and storage, of the Legend section 
    /// into a Dictionary.
    /// </summary>
    public class LegendLoader : SectionLoader {
        /// <summary>
        /// Key is the character on the ASCII map.
        /// Value is a pair non-damaged and damaged Images of Block.
        /// </summary>
        public Dictionary<char, (IBaseImage, IBaseImage)> LegendDict;

        public LegendLoader(SectionStreamReader reader) : base(reader) {
            section = "Legend";

            LegendDict = new Dictionary<char, (IBaseImage, IBaseImage)>();
        }

        /// <summary>
        /// Process one line of LegendData and store the data as an entry in LegendDict.
        /// </summary>
        /// <param name="line">Processed line</param>
        protected override void ProcessSectionLine(string line) {
            string[] splitArray = line.Split(") ");
            if (splitArray.Length == 2){
                char symb = splitArray[0][0];

                string fileName = splitArray[1];
                string dmgFileName = fileName.Split(".png")[0] + "-damaged.png";
                //filepaths
                string filePath = Path.Combine(new string[] {
                    ProjectPath.getPath(), "Breakout", "Assets","Images", fileName});
                string dmgFilePath = Path.Combine(new string[] {
                    ProjectPath.getPath(), "Breakout", "Assets","Images", dmgFileName});

                LegendDict.Add(symb, (new Image(filePath), new Image(dmgFilePath)));
            } else {
                throw new InvalidDataException("Invalid legend data in the level file");
            }
        }

        public override void ClearLoader()
        {
            LegendDict.Clear();
        }
    }
}