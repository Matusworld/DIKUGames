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
        public int NumberOfUnbreakables { get; private set; } = 0;
        private MapLoader map;
        public MetaLoader Meta { get; private set; }
        public LegendLoader Legend { get; private set; }
        public SectionsValidator Validator { get; private set; }
        //public EntityContainer<Block> Blocks { get; private set; } = new EntityContainer<Block>();
        public BlockOrganizer BlockOrganizer { get; private set; } 

        public LevelLoader() {
            BlockOrganizer = new BlockOrganizer();
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
            NumberOfUnbreakables = 0;
            map.ClearLoader();
            Meta.ClearLoader();
            Legend.ClearLoader();
            BlockOrganizer.Entities.ClearContainer();
        }

        /*
        /// <summary>
        /// Returns true if the current level has ended, i.e.
        /// when only unbreakable blocks are left
        /// </summary>
        public bool LevelEnded() {
            if(BlockOrganizer.Entities.CountEntities() == NumberOfUnbreakables) {
                return true;
            }
            return false;
        } */

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
                            Block block;
                            Vec2F position = ComputeBlockPosition(i, j, blockExtent);
                            IBaseImage image = Legend.LegendDict[cell].Item1;
                            IBaseImage damageImage = Legend.LegendDict[cell].Item2;

                            if (cell == Meta.Hardened) {
                                block = new Hardened(
                                    new DynamicShape(position, blockExtent), image, damageImage);
                            }
                            else if (cell == Meta.Unbreakable) {
                                NumberOfUnbreakables++;
                                block = new Unbreakable(
                                    new DynamicShape(position, blockExtent), image, damageImage);
                            }
                            else if (cell == Meta.Powerup) {
                                block = new PowerUp(
                                    new DynamicShape(position, blockExtent), image, damageImage);
                            }
                            else {
                                block = new Block(
                                    new DynamicShape(position, blockExtent), image, damageImage);
                            }
                            BlockOrganizer.Entities.AddEntity(block);
                        }
                    }
                }
                return;
            }
            throw new InvalidDataException("Incorrect sections of file");
        }
    }
}
