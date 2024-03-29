using System.IO;

namespace Breakout.LevelLoading.SectionLoading {
    /// <summary>
    /// Facilitates the loading, i.e. processing and storage, of the Meta section into fields.
    /// </summary>
    public class MetaLoader : SectionLoader {
        public string Name { get; private set; }
        public int Time { get; private set; }
        public char Hardened { get; private set; }
        public char Powerup { get; private set; }
        public char Unbreakable { get; private set; }


        public MetaLoader(SectionStreamReader reader) : base(reader) {
            section = "Meta";
        }

        
        protected override void ProcessSectionLine(string line) {
            string[] splitArray = line.Split(": ");
            if (splitArray.Length == 2){
                switch (splitArray[0]) {
                    case "Name":
                        Name = splitArray[1];
                        break;
                    case "Time":
                        Time = int.Parse(splitArray[1]);
                        break;
                    case "PowerUp":
                        Powerup = splitArray[1][0];
                        break;
                    case "Hardened":
                        Hardened = splitArray[1][0];
                        break;
                    case "Unbreakable":
                        Unbreakable = splitArray[1][0];
                        break;
                } 
            } else {
                throw new InvalidDataException("Invalid meta data in the level file");
            }
        }

        public override void ClearLoader()
        {
            Name = "";
            Time = 0;
            Hardened = ' ';
            Powerup = ' ';
            Unbreakable = ' ';
        }
    }
}