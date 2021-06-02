using System.IO;
using System.Collections.Generic;

using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.GamePlay.PlayerEntity {
    /// <summary>
    /// Healthbar that can display the number of lives left for the Player.
    /// Lives are gained via PowerUps and lost by having no balls remaining.
    /// The Healthbar detects when no lives are left and broadcasts this information.
    /// </summary>
    public class Healthbar : IGameEventProcessor {
        public uint StartLives { get; private set; }
        public uint Lives { get; private set; }
        public uint MaxLives { get; private set; }

        //Extent of each individual heart symbol
        private float extentDimension = 0.05f;
        private IBaseImage heartFilled = new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "heart_filled.png"));
        private IBaseImage heartEmpty = new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "heart_empty.png"));
        //List containing hearts to be rendered
        public List<Entity> HealthList { get; private set; } = new List<Entity>();

        public Healthbar(uint lives, uint maxLives) {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);

            StartLives = lives;
            Lives = lives;
            MaxLives = maxLives;

            //Initialize HealthList to be length of maxLives
            Vec2F healthExtent = new Vec2F(extentDimension, extentDimension);
            for (int i = 0; i < maxLives; i++) {
                Vec2F healthpos = 
                    new Vec2F((1 - extentDimension * maxLives) + extentDimension * i, 0f);
                Entity health;
                if (i < lives) {
                    health = new Entity(
                        new StationaryShape(healthpos, healthExtent), heartFilled);
                } else {
                    health = new Entity(
                        new StationaryShape(healthpos, healthExtent), heartEmpty);
                }
                HealthList.Add(health);
            }
        }

        /// <summary>
        /// Reset to initial state with (StartLives/MaxLives) lives.
        /// </summary>
        public void Reset() {
            Lives = StartLives;

            for (int i = 0; i < MaxLives; i++) {
                if (i < StartLives) {
                    HealthList[i].Image = heartFilled;
                } else {
                    HealthList[i].Image = heartEmpty;
                }
            }
        }

        /// <summary>
        /// Render row of hearts that visually represent the Healthbar.
        /// </summary>
        public void Render() {
            foreach (Entity health in HealthList) {
                health.RenderEntity();
            }
        }

        /// <summary>
        /// Detract one life from Healthbar. 
        /// If 0 lives is reached, broadcast that the game is lost.
        /// </summary>
        public void HealthLost() {
            if (Lives > 1) {
                HealthList[(int) Lives - 1].Image = heartEmpty;
                Lives--;
            } else { 
                //next life lost will result in loss
                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_LOST"});
            }
        }

        /// <summary>
        /// Increment the amount of lives by one. Lives cannot exceed MaxLives.
        /// </summary>
        private void HealthGained() {
            if (!(Lives > MaxLives - 1)) {
                HealthList[(int) Lives].Image = heartFilled;
                Lives++;
            }
        }
        
        /// <summary>
        /// Process Events related to lives/health.
        /// </summary>
        /// <param name="gameEvent"></param>
        public void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.StringArg1) {
                case "HEALTH_LOST":
                    HealthLost();
                    break;

                case "HEALTH_GAINED":
                    HealthGained();
                    break;
            }
        }
    }
}