using System.IO;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using Breakout.LevelLoading.SectionLoading;
using Breakout.Blocks;

namespace Breakout.LevelLoading {
    public class LevelLoader {
        private MapLoader map;
        public MetaLoader Meta { get; private set; }
        public LegendLoader Legend { get; private set; }
        public SectionsValidator Validator { get; private set; }
        public EntityContainer<Block> Blocks { get; private set; } = new EntityContainer<Block>();


        public LevelLoader(string filepath) {
            map = new MapLoader(filepath);
            Meta = new MetaLoader(filepath);
            Legend = new LegendLoader(filepath);
            Validator = new SectionsValidator(filepath);
        }

        /// <summary>
        /// Perform the loading of all data
        /// </summary>
        public void LoadLevel() {
            float blockExtentX = 1.0f / map.Width;
            float blockExtentY = 0.5f / map.Height;
            Vec2F blockExtent = new Vec2F(blockExtentX, blockExtentY);

            if (Validator.ValidateSections()) {
                // Only load if sections are validated
                map.LoadSection();
                Meta.LoadSection();
                Legend.LoadSection();
                for (int j = 0; j < map.Height; j++) {    
                    for (int i = 0; i < map.Width; i++) {
                        if (map.RowList[j][i] != '-') {
                            for(int l = 0; l < Legend.LegendList.Count; l++) {
                                if (map.RowList[j][i] == Legend.LegendList[l].Item1) {
                                    float posX = blockExtentX * i;
                                    float posY = 1.0f - (j + 1) * blockExtentY;
                                    Vec2F position = new Vec2F(posX, posY);
                                    Block block = new Block(new DynamicShape(position, blockExtent), 
                                        new Image(Path.Combine(
                                            "Assets","Images", Legend.LegendList[l].Item2)), 
                                        1);
                                    Blocks.AddEntity(block);
                                    // Subscribe every block 
                                    BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, block);
                                }
                            }
                        }
                    }
                }
                return;
            }
            throw new InvalidDataException("Incorrect sections of file");
        }
    }
}
