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
        }
    }
}
