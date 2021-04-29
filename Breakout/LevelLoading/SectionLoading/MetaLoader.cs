using System.IO;

namespace Breakout.LevelLoading.SectionLoading {
    public class MetaLoader : SectionLoader {
        private SectionStreamReader reader;
        private string section = "Meta";
        public string Name { get; private set; }
        public int Time { get; private set; }
        public char Hardened { get; private set; }
        public char Powerup { get; private set; }
        public char Unbreakable { get; private set; }


        public MetaLoader(string filepath) {
            reader = new SectionStreamReader(filepath, section);
        }

        protected override void ProcessSectionLine(string line) {
            string[] splitArray = line.Split(": ");
            if (splitArray.Length == 2){
                switch(splitArray[0]) {
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


        public override void LoadSection() {
            string line;
            
            while((line = reader.ReadSectionLine()) != null) {
                ProcessSectionLine(line);
            }

            reader.File.Close();
        } 
    }
}