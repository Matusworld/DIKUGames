using System;
using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout {
    public class LevelLoader {
        private StreamReader textMap;
        private int width;
        private int height;
        private string line;
        private string name;
        private int time;
        private char hardened;
        private char powerup;
        private char unbreakable;
        private string path;
        private List<(char,string)> LegendList = new List<(char, string)>();
        private EntityContainer<Block> blocks;

        public LevelLoader (string path) {
            this.path = path;
            blocks = new EntityContainer<Block>();
            textMap = new StreamReader(path);
        }

        public EntityContainer<Block> ReadFile() {
            VerifyMapFile();
            float blockExtentX = 1.0f / width;
            float blockExtentY = 0.5f / height;
            Vec2F blockExtent = new Vec2F(blockExtentX, blockExtentY);
            textMap = new StreamReader(path);
            int checkpoint = 0;
            int countJ = 0;
            while((line = textMap.ReadLine()) != null) {
                if (checkpoint == 0 && line == "Map:") {
                    checkpoint++;
                }
                if (checkpoint == 1 && line != "Map:" && line != "Map/") {
                    for(int i = 0; i < width; i++) {
                        if (line[i] != '-') {
                            for(int l = 0; l < LegendList.Count; l++) {
                                if (line[i] == LegendList[l].Item1) {
                                    float posX = blockExtentX * i;
                                    float posY = 1.0f - (countJ + 1) * blockExtentY;
                                    Vec2F position = new Vec2F(posX, posY);
                                    Block block = new Block(new DynamicShape(position, blockExtent), 
                                        new Image(Path.Combine(
                                            "Assets","Images", LegendList[l].Item2)), 
                                        15);
                                    blocks.AddEntity(block);
                                }
                            }
                        }
                    }
                    countJ++;
                }
                if (checkpoint == 1 && line == "Map/") {
                    checkpoint++;
                }
            }
            return blocks;
        }
        private void MetaData(string line) {
            string[] splitArray = line.Split(": ");
            if (splitArray.Length == 2){
                switch(splitArray[0]) {
                    case "Name":
                        name = splitArray[1];
                        break;
                    case "Time":
                        time = int.Parse(splitArray[1]);
                        break;
                    case "PowerUp":
                        powerup = splitArray[1][0];
                        break;
                    case "Hardened":
                        hardened = splitArray[1][0];
                        break;
                    case "Unbreakable":
                        unbreakable = splitArray[1][0];
                        break;
                } 
            } else {
                throw new InvalidDataException("Invalid meta data in the level file");
            }
        }
        private void LegendData(string line) {
            string[] splitArray = line.Split(") ");
            if (splitArray.Length == 2){
                LegendList.Add(((char) splitArray[0][0],splitArray[1]));
            } else {
                throw new InvalidDataException("Invalid legend data in the level file");
            }
        }
        private void WidthCheck(string line) {
            width = line.Length;
        }
        private void VerifyMapFile() {
            int checkpoint = 0;
            int height = 0;
            while((line = textMap.ReadLine()) != null) {                 
                if (line == "Map:" && checkpoint == 0) {  
                    checkpoint++;
                }
                if (checkpoint == 1 && line != "Map:" && line != "Map/"){
                    WidthCheck(line);
                    height++;
                }                
                if (line == "Map/" && checkpoint == 1){
                    checkpoint++;
                    this.height = height;
                }
                if (line == "Meta:" && checkpoint == 2){
                    checkpoint++;
                }
                if (checkpoint == 3 && line != "Meta:" && line != "Meta/"){
                    MetaData(line);
                }
                if (line == "Meta/" && checkpoint == 3){
                    checkpoint++;
                }
                if (line == "Legend:" && checkpoint == 4){
                    checkpoint++;
                }
                if (checkpoint == 5 && line != "Legend:" && line != "Legend/") {
                    LegendData(line);
                }
                if (line == "Legend/" && checkpoint == 5){
                    return;
                }              
            } 
            throw new InvalidDataException("The map file was not valid");
        }  
    }
} 
