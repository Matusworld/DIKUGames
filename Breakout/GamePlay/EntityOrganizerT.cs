using System;
using System.Collections.Generic;

using DIKUArcade.Entities;
using DIKUArcade.Events;

namespace Breakout.GamePlay {
    public abstract class EntityOrganizer<T> : IGameEventProcessor where T: Entity {
        public EntityContainer<T> Entities { get; protected set; }

        public EntityOrganizer() {
            Entities = new EntityContainer<T>();
        }

        public void AddEntity(T entity) {
            Entities.AddEntity(entity);
        } 

        public virtual void ResetOrganizer() {
            Entities.ClearContainer();
        }

        public virtual void MoveEntities() {
            Entities.Iterate(entity => {
                entity.Shape.AsDynamicShape().Move();
            });
        }

        public void RenderEntities() {
            Entities.Iterate(entity => {
                entity.RenderEntity();
            });
        }

        public abstract void ProcessEvent(GameEvent gameEvent);
    }
}