using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using System;
namespace Galaga {
    public class Player {
        private Entity entity;
        private DynamicShape shape;
        private float moveLeft = 0.0f;
        private float moveRight = 0.0f;
        private const float MOVEMENT_SPEED = 0.01f;
        public Player(DynamicShape shape, IBaseImage image) {
            entity = new Entity(shape, image);
            this.shape = shape;
        }

        public void Render() {
            entity.RenderEntity();
        }
        public void UpdateDirection(){
            shape.Direction.X = moveLeft + moveRight;
        }
        public void Move(){
            if (shape.Position.X+shape.Direction.X < 0.0f){
                shape.Position.X = 0.01f;
            }
            if (shape.Position.X+shape.Direction.X > 1.0f){
                shape.Position.X = 0.99f;
            } else {
                shape.Move();
            }
        }
        
        public void SetMoveLeft(bool val){
            if (val){
                moveLeft -= MOVEMENT_SPEED;
                UpdateDirection();
            } /*else { Spørg om det tredje punkt i opgave 6
                moveLeft = moveLeft;
            }*/
        }
        public void SetMoveRight(bool val){
            if (val){
                moveRight += MOVEMENT_SPEED;
                UpdateDirection();
            } /*else {
                moveRight = moveRight;
            }*/
            
            }
        }
    }

