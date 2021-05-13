using System.IO;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using Breakout.LevelLoading.SectionLoading;
using Breakout.Blocks;

namespace Breakout.LevelLoading {
    public class LevelLoader {
        private SectionStreamReader reader;
        public int numberOfUnbreakables = 0;
        private MapLoader map;
        public MetaLoader Meta { get; private set; }
        public LegendLoader Legend { get; private set; }
        public SectionsValidator Validator { get; private set; }
        public EntityContainer<Block> Blocks { get; private set; } = new EntityContainer<Block>();

        public LevelLoader() {
            reader = new SectionStreamReader();
            map = new MapLoader(reader);
            Meta = new MetaLoader(reader);
            Legend = new LegendLoader(reader);
            Validator = new SectionsValidator();
        }

        /// <summary>
        /// Computes the block extent such based on height and width such that
        /// block pattern cover top half of map
        /// </summary>
        /// <returns>Vec2F BlockExtent</returns>
        private Vec2F ComputeBlockExtent() {
            float blockExtentX = 1.0f / map.Width;
            float blockExtentY = 0.5f / map.Height;
            return new Vec2F(blockExtentX, blockExtentY);
        }

        private Vec2F ComputeBlockPosition(int i, int j, Vec2F blockExtent) {
            float posX = blockExtent.X * j;
            float posY = 1.0f - (i + 1) * blockExtent.Y;
            return new Vec2F(posX, posY);
        }

        /// <summary>
        /// Clear fields prior to new level read
        /// </summary>
        private void ClearLoader() {
            numberOfUnbreakables = 0;
            map.ClearLoader();
            Meta.ClearLoader();
            Legend.ClearLoader();
            Blocks.ClearContainer();
        }

        /// <summary>
        /// Returns true if the current level has ended, i.e.
        /// when only unbreakable blocks are left
        /// </summary>
        public bool LevelEnded() {
            if(Blocks.CountEntities() == numberOfUnbreakables) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Perform the loading of all data
        /// After run, block container will contain correct blocks for the given level
        /// </summary>
        public void LoadLevel(string path) {
            // Only load if sections are validated
            if (Validator.ValidateSections(path)) {
                //Set path of SectionStreamReader
                reader.SetPath(path);
                //Clear fields
                ClearLoader();
                //Load sections
                map.LoadSection();
                Meta.LoadSection();
                Legend.LoadSection();

                Vec2F blockExtent = ComputeBlockExtent();

                for (int i = 0; i < map.Height; i++) {    
                    for (int j = 0; j < map.Width; j++) {
                        //define character
                        char cell = map.RowList[i][j];
                        if (cell != '-') {
                            for(int l = 0; l < Legend.LegendList.Count; l++) {
                                if (cell == Legend.LegendList[l].Item1) {

                                    Vec2F position = ComputeBlockPosition(i, j, blockExtent);

                                    string[] imagePath = new string[] { 
                                        ProjectPath.getPath(), "Breakout", 
                                            "Assets","Images", Legend.LegendList[l].Item2 };
                                    
                                    if (cell == Meta.Hardened) {
                                        string damageImg = 
                                            Legend.LegendList[l].Item2.Split(".png")[0]
                                                + "-damaged.png";
                                        string[] damageImgPath = new string[] { 
                                            ProjectPath.getPath(), "Breakout", 
                                                "Assets","Images", damageImg}; 
                                        HardenedBlock block = new HardenedBlock (
                                            new DynamicShape(position, blockExtent), 
                                            new Image(Path.Combine(imagePath)), 
                                            new Image(Path.Combine(damageImgPath)));

                                        Blocks.AddEntity(block);
                                        BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, block);
                                    } 
                                    else if (cell == Meta.Unbreakable) {
                                        numberOfUnbreakables++;
                                        UnbreakableBlock block = new UnbreakableBlock(
                                            new DynamicShape(position, blockExtent), 
                                            new Image(Path.Combine(
                                                "Assets","Images", Legend.LegendList[l].Item2)));
                                        Blocks.AddEntity(block);
                                        BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, block);
                                    } 
                                    else {
                                        Block block = new Block(new DynamicShape(position, blockExtent), 
                                            new Image(Path.Combine(
                                                "Assets","Images", Legend.LegendList[l].Item2)));
                                        Blocks.AddEntity(block);
                                        BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, block);
                                    }
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
