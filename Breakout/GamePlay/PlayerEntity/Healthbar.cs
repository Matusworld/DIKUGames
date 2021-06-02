using System.IO;
using System.Collections.Generic;

using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.GamePlay.PlayerEntity {
    public class Healthbar : IGameEventProcessor {
        public uint startLives;
        public uint Lives { get; private set; }
        public uint MaxLives { get; private set; }
        private float dimension = 0.05f;

        private IBaseImage heartFilled = new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "heart_filled.png"));

        private IBaseImage heartEmpty = new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "heart_empty.png"));

        public List<Entity> HealthList { get; private set; } = new List<Entity>();

        public Healthbar(uint lives, uint maxLives) {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);

            startLives = lives;
            Lives = lives;
            MaxLives = maxLives;

            Vec2F healthExtent = new Vec2F(dimension, dimension);

            for (int i = 0; i < maxLives; i++) {
                Vec2F healthpos = new Vec2F((1 - dimension * maxLives) + dimension * i,0f);
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

        public void Reset() {
            Lives = startLives;

            for (int i = 0; i < MaxLives; i++) {
                if (i < startLives) {
                    HealthList[i].Image = heartFilled;
                }
                else {
                    HealthList[i].Image = heartEmpty;
                }
            }
        }

        public void Render() {
            foreach (Entity health in HealthList) {
                health.RenderEntity();
            }
        }

        // Made public to fix, problem with eventbus delayed actions, made the program crash sometimes.
        public void HealthLost() {
            if (Lives > 1) {
                HealthList[(int) Lives - 1].Image = heartEmpty;
                Lives--;
            } 
            //next life lost will result in loss
            else {
                BreakoutBus.GetBus().RegisterEvent( new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_LOST"});
            }
        }

        private void HealthGained() {
            if (!(Lives > MaxLives - 1)) {
                HealthList[(int) Lives].Image = heartFilled;
                Lives++;
            }
        }
        
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