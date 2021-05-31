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

        /// <summary>
        /// Validates  that ObjectArg1 is of type T.
        /// </summary>
        /// <param name="gameEvent"></param>
        protected void EventValidator(GameEvent gameEvent) {
            if (gameEvent.ObjectArg1 is T) {
                return;
            }
            else {
                throw new ArgumentException(
                    "Event to EntityOrganizer must have ObjectArg1 as generic type of Organizer");
            }
        }

        public abstract void ProcessEvent(GameEvent gameEvent);
    }
}