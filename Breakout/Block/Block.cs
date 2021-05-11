using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Breakout.Blocks {
    public class Block : Entity, IGameEventProcessor {
        public bool Alive { get; private set; }

        public int HP { get; private set; }
        public string Value { get; private set; } 

        public Block(DynamicShape shape, IBaseImage image, int HP): base(shape, image) {
            this.HP = HP;
        }
        public Vec2F GetPosition() {
            return this.Shape.Position; 
        }

        private void Damage() {
            HP--;
        }

        private bool IsAlive() {
            if(HP <= 0) {
                Alive = false; 
                return Alive;
            } else {
                Alive = true;
                return Alive;
            }
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if(gameEvent.EventType == GameEventType.ControlEvent ) {
                if((Block) gameEvent.To == this) {
                    /*
                    if(gameEvent.StringArg1 == "Damage") {
                        Damage();
                        if (!IsAlive()) {
                            // Unsubscribe deleted blocks
                            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, this);
                            DeleteEntity();
                        }
                    }*/
                    if(gameEvent.StringArg1 == "BlockCollision") {
                        Damage();
                        if (!IsAlive()) {
                            // Unsubscribe deleted blocks
                            BreakoutBus.GetBus().Unsubscribe(GameEventType.ControlEvent, this);
                            DeleteEntity();

                            // Could add points here
                            BreakoutBus.GetBus().RegisterEvent( new GameEvent { 
                                EventType = GameEventType.ControlEvent, StringArg1 = "ADD_SCORE",
                                    Message = "1"});
                        }
                    }
                }
            }
        }
    }
}