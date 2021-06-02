using DIKUArcade.Entities;
using DIKUArcade.Events;

namespace Breakout.GamePlay {
    /// <summary>
    /// Generic Organizing class for containing and giving mass functionality to Entities.
    /// Processes Events on behalf of contained Entities.
    /// </summary>
    public abstract class EntityOrganizer<T> : IGameEventProcessor where T : Entity {
        public EntityContainer<T> Entities { get; protected set; }

        public EntityOrganizer() {
            Entities = new EntityContainer<T>();
        }

        /// <summary>
        /// Add Entity to this EntityOrganizer.
        /// </summary>
        /// <param name="entity">Entity added to this EntityOrganizer.</param>
        public void AddEntity(T entity) {
            Entities.AddEntity(entity);
        } 

        /// <summary>
        /// Reset this EntityOrganizer to its initial state.
        /// </summary>
        public virtual void ResetOrganizer() {
            Entities.ClearContainer();
        }

        /// <summary>
        /// Move all entities contained in this EnitityOrganizer.
        /// </summary>
        public virtual void MoveEntities() {
            Entities.Iterate(entity => {
                entity.Shape.AsDynamicShape().Move();
            });
        }

        /// <summary>
        /// Render all entities contained in this EntityOrganizer.
        /// </summary>
        public void RenderEntities() {
            Entities.Iterate(entity => {
                entity.RenderEntity();
            });
        }

        public abstract void ProcessEvent(GameEvent gameEvent);
    }
}