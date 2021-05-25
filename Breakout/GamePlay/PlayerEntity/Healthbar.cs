using System.IO;
using System.Collections.Generic;

using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

using Breakout;

namespace Breakout.GamePlay.PlayerEntity {
    public class Healthbar : IGameEventProcessor {

        public uint Lives { get; private set; }
        private uint maxLives;
        private float dimension = 0.05f;

        private IBaseImage heartFilled = new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "heart_filled.png"));

        private IBaseImage heartEmpty = new Image (Path.Combine(ProjectPath.getPath(),  
                "Breakout", "Assets", "Images", "heart_empty.png"));

        private List<Entity> healthList = new List<Entity>();
        public Healthbar(uint lives, uint maxLives) {
            BreakoutBus.GetBus().Subscribe(GameEventType.ControlEvent, this);

            this.Lives = lives;
            this.maxLives = maxLives;

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
                healthList.Add(health);
            }
        }

        public void Render() {
            foreach (Entity health in healthList) {
                health.RenderEntity();
            }
        }

        // Made public to fix, problem with eventbus delayed actions, made the program crash sometimes.
        public void HealthLost() {
            if (Lives > 0) {
                healthList[(int) Lives - 1].Image = heartEmpty;
                Lives--;
            }
        }

        private void HealthGained() {
            if (!(Lives > maxLives - 1)) {
                healthList[(int) Lives].Image = heartFilled;
                Lives++;
            }
        }
        
        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch (gameEvent.StringArg1) {
                    case "HealthLost":
                        HealthLost();
                        break;
                    case "HealthGained":
                        HealthGained();
                        break;
                }
            }
        }
    }
}