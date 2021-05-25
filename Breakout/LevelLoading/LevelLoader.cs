using System.IO;

using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

using Breakout.LevelLoading.SectionLoading;
using Breakout.GamePlay.BlockEntity;

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
        /// Compute the block extent such based on height and width such that
        /// block pattern cover top half of map
        /// </summary>
        /// <returns>Vec2F BlockExtent</returns>
        private Vec2F ComputeBlockExtent() {
            float blockExtentX = 1.0f / map.Width;
            float blockExtentY = 0.5f / map.Height;
            return new Vec2F(blockExtentX, blockExtentY);
        }

        /// <summary>
        /// Compute the block position based on ASCII map coordinate and extent
        /// </summary>
        /// <param name="i" - row coordinate></param>
        /// <param name="j" - column coordinate></param>
        /// <param name="blockExtent"></param>
        /// <returns>Vec2F BlockPosistion</returns>
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
                                    Block block;
                                    string[] imagePathArr = new string[] { 
                                        ProjectPath.getPath(), "Breakout", 
                                            "Assets","Images", Legend.LegendList[l].Item2 };
                                    string imagePath = Path.Combine(imagePathArr); 
                                    
                                    if (cell == Meta.Hardened) {
                                        //extract image name and add "-damaged"
                                        string damageImgName = 
                                            Legend.LegendList[l].Item2.Split(".png")[0]
                                                + "-damaged.png";
                                        string[] damageImgPathArr = new string[] { 
                                            ProjectPath.getPath(), "Breakout", 
                                                "Assets","Images", damageImgName};
                                        string damageImgPath = Path.Combine(damageImgPathArr); 
                                        block = new Hardened (
                                            new DynamicShape(position, blockExtent), 
                                            new Image(imagePath), 
                                            new Image(damageImgPath));
                                    } 
                                    else if (cell == Meta.Unbreakable) {
                                        numberOfUnbreakables++;
                                        block = new Unbreakable (
                                            new DynamicShape(position, blockExtent), 
                                            new Image(imagePath));
                                    } 
                                    else if (cell == Meta.Powerup) {
                                        block = new PowerUp(new DynamicShape(position, blockExtent), 
                                            new Image(imagePath));
                                    }
                                    else {
                                        block = new Block(new DynamicShape(position, blockExtent), 
                                            new Image(imagePath));
                                    }
                                    Blocks.AddEntity(block);
                                    BreakoutBus.GetBus().Subscribe(
                                        GameEventType.ControlEvent, block);
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
