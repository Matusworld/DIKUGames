using System.IO;

using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

using Breakout.LevelLoading.SectionLoading;
using Breakout.GamePlay.BlockEntity;

namespace Breakout.LevelLoading {
    public class LevelLoader {
        private SectionStreamReader reader;
        private MapLoader map;
        public MetaLoader Meta { get; private set; }
        public LegendLoader Legend { get; private set; }
        public SectionsValidator Validator { get; private set; }

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
        /// Clear SectionLoaders and BlockOrganizer prior to new level read
        /// </summary>
        private void ClearLoader() {
            map.ClearLoader();
            Meta.ClearLoader();
            Legend.ClearLoader();
            BlockOrganizer.ResetOrganizer();
        }

        /// <summary>
        /// Compute the Block Extent based on height and width such that
        /// block pattern cover top half of map
        /// </summary>
        /// <returns>BlockExtent of Block to be loaded</returns>
        private Vec2F ComputeBlockExtent() {
            float blockExtentX = 1.0f / map.Width;
            float blockExtentY = 0.5f / map.Height;
            return new Vec2F(blockExtentX, blockExtentY);
        }

        /// <summary>
        /// Compute the Block Position based on ASCII map coordinate and Extent
        /// </summary>
        /// <param name="i">row coordinate.</param>
        /// <param name="j">column coordinate.</param>
        /// <param name="blockExtent"></param>
        /// <returns>Vec2F BlockPosistion</returns>
        private Vec2F ComputeBlockPosition(int i, int j, Vec2F blockExtent) {
            float posX = blockExtent.X * j;
            float posY = 1.0f - (i + 1) * blockExtent.Y;
            return new Vec2F(posX, posY);
        }

        /// <summary>
        /// Generate Block based on input. The cell character determines block type and Image.
        /// </summary>
        /// <param name="cell">Maps to both Block type and Image.</param>
        /// <param name="position"></param>
        /// <param name="extent"></param>
        /// <returns>The generated Block.</returns>
        private Block GenerateBlock(char cell, Vec2F position, Vec2F extent) {
                IBaseImage image = Legend.LegendDict[cell].Item1;
                IBaseImage damageImage = Legend.LegendDict[cell].Item2;
                Block block;

                if (cell == Meta.Hardened) {
                    block = new Hardened(
                        new DynamicShape(position, extent), image, damageImage);
                } else if (cell == Meta.Unbreakable) {
                    BlockOrganizer.NumberOfUnbreakables++;
                    block = new Unbreakable(
                        new DynamicShape(position, extent), image, damageImage);
                } else if (cell == Meta.Powerup) {
                    block = new PowerUp(
                        new DynamicShape(position, extent), image, damageImage);
                } else {
                    block = new Block(
                        new DynamicShape(position, extent), image, damageImage);
                }

                return block;
            }

        /// <summary>
        /// Perform the loading of all data.
        /// After run, block container will contain correct blocks for the given level.
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

                //Compute and add blocks
                Vec2F blockExtent = ComputeBlockExtent();
                for (int i = 0; i < map.Height; i++) {    
                    for (int j = 0; j < map.Width; j++) {
                        //define character
                        char cell = map.RowList[i][j];
                        if (cell != '-') {
                            Vec2F blockPosition = ComputeBlockPosition(i, j, blockExtent);
                            
                            Block block = GenerateBlock(cell, blockPosition, blockExtent);
                            
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
