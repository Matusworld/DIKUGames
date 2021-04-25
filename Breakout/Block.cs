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
    public class Block : Entity, IGameEventProcessor {
        //private Entity entity;
        //private DynamicShape shape;
        private bool Alive;

        private int HP;
        public Block(DynamicShape shape, IBaseImage image, int HP): base(shape, image) {
            this.HP = HP;
        }
        public Vec2F GetPosition() {
            return this.Shape.Position; 
        }

        public void Damage() {
            HP--;
        }

        public int GetHP() {
            return HP;
        }

        public bool IsAlive() {
            if (HP <= 0) {
                Alive = false; 
                return Alive;
            } else {
                Alive = true;
                return Alive;
            }
        }

        public void Render() {
            this.RenderEntity();
        }

        public void ProcessEvent(GameEvent gameEvent) {
            //Kig lige på den her igen når man skal implementere kollision
            /*if (gameEvent.EventType == GameEventType.ControlEvent) {
                switch (gameEvent.StringArg1) {

                }
            }*/
        }
    }
}