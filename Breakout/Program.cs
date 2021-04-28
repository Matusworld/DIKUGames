using System;
using DIKUArcade.GUI;

using Breakout.LevelLoading;
using System.IO;

namespace Breakout
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Normal game start 
            var winArgs = new WindowArgs();
            var game = new Game(winArgs);
            game.Run();
            */

            string filepath = Path.Combine("Assets", "Levels", "level3.txt");

            SectionsValidator validator = new SectionsValidator(filepath);
            Console.WriteLine("Validation check gives: " + validator.ValidateSections());

            SectionStreamReader reader = new SectionStreamReader(filepath, "Map");
            string line;
            while((line = reader.ReadSectionLine()) != null) {

                Console.WriteLine(line);
            }
        }
    }
}
