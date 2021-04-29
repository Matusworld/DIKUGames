using System;
using DIKUArcade.GUI;

using Breakout.LevelLoading;
using Breakout.LevelLoading.SectionLoading;
using System.IO;
using DIKUArcade.Entities;

namespace Breakout
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var winArgs = new WindowArgs();
            var game = new Game(winArgs);
            game.Run();
            

            /*
            string filepath = Path.Combine("Assets", "Levels", "level3.txt");

            LevelLoader loader = new LevelLoader(filepath);

            loader.LoadLevel();

            System.Console.WriteLine(loader.Blocks.CountEntities());
            

            SectionsValidator validator = new SectionsValidator(filepath);
            Console.WriteLine("Validation check gives: " + validator.ValidateSections());

            
            SectionStreamReader reader = new SectionStreamReader(filepath, "Legend");
            string line;
            while((line = reader.ReadSectionLine()) != null) {
                Console.WriteLine(line);
            }
            
            
            MapLoader loader = new MapLoader(filepath);
            loader.LoadSection();
            System.Console.WriteLine(loader.height);
            */
        }
    }
}
