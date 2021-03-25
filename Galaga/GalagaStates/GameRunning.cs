using DIKUArcade.State;
using DIKUArcade.EventBus;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using Galaga.Squadron;
using Galaga.MovementStrategy;
using System.Collections.Generic;
using DIKUArcade.Physics;
using System;

namespace Galaga.GalagaStates {
    public class GameRunning : IGameState, IGameEventProcessor<object> {
        private Player player;
        private ISquadron squadron;
        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;
        private AnimationContainer enemyExplosions;
        private List<Image> explosionStrides;
        private List<Image> images;
        private List<Image> enemyStridesRed;
        private Score score;
        private const int EXPLOSION_LENGTH_MS = 500;
        private float currentMovementSpeed = 0.0003f;
        private float enemySpeedMultiplier = 1.2f;

        public GameRunning() {

            InitializeGameState();
        }

        public void GameLoop() {}

        public void InitializeGameState() {
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine(@"..\", "Assets", "Images", "Player.png")));
            GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, this.player);

            score = new Score(new Vec2F (0.485f, -0.2f), new Vec2F (0.3f, 0.3f));

            images = ImageStride.CreateStrides(4, Path.Combine(@"..\", "Assets", 
                "Images", "BlueMonster.png"));

            enemyStridesRed = ImageStride.CreateStrides(2,
                Path.Combine(@"..\", "Assets", "Images", "RedMonster.png"));

            squadron = new SquadronVertiLines();
            squadron.CreateEnemies(images, enemyStridesRed, currentMovementSpeed);

            playerShots = new EntityContainer<PlayerShot>();
            playerShotImage = new Image(Path.Combine(@"..\", "Assets", "Images", "BulletRed2.png"));

            enemyExplosions = new AnimationContainer(squadron.MaxEnemies);
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine(@"..\", "Assets", "Images", "Explosion.png"));
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
        }
        public void KeyPress(string key) {
            switch (key) {
                case "KEY_LEFT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "true", "SetMoveLeft", ""));
                    break;
                case "KEY_RIGHT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "true", "SetMoveRight", ""));
                    break;
                default:
                    break;
            }
        }
        public void KeyRelease(string key) {
            switch (key) {
                case "KEY_LEFT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "false", "SetMoveLeft", ""));
                    break;
                case "KEY_RIGHT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "false", "SetMoveRight", ""));
                    break;
                case "KEY_SPACE":
                    PlayerShot shot = new PlayerShot(player.GetPosition()+ new Vec2F(0.047f,0.065f),
                        playerShotImage);
                    playerShots.AddEntity(shot);
                    break;
                default:
                    break;
            }
        }

        public void UpdateGameLogic() {
            if (!CheckGameEnded()) {
            
                GalagaBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "", "Move", ""));

                IterateShots();

                ZigZagDown.MoveEnemies(squadron.Enemies);

                IncreaseDifficulty();
            }

        }

        public void RenderState() {
            if (!CheckGameEnded()) {

                player.Render();

                squadron.Enemies.RenderEntities();

                playerShots.RenderEntities();

                enemyExplosions.RenderAnimations();

                score.RenderScore();

            } else {

                score.RenderScore();

            }
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {}
        public void AddExplosion(Vec2F position, Vec2F extent) {
            StationaryShape explosion = new StationaryShape(position, extent);
            ImageStride explosionStride = new ImageStride(EXPLOSION_LENGTH_MS/8, explosionStrides);
            enemyExplosions.AddAnimation(explosion, EXPLOSION_LENGTH_MS, explosionStride);
        }
        public void ProcessEvent(GameEventType type, GameEvent<object> gameEvent) {
            if (type == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                    case "KEY_PRESS":
                        KeyPress(gameEvent.Message);
                        break;
                    case "KEY_RELEASE":
                        KeyRelease(gameEvent.Message);
                        break;
                    default:
                        break;
                }
            }
            if (type == GameEventType.GraphicsEvent) {
                switch (gameEvent.Parameter1) {
                    case "Explosions":
                        Enemy test = (Enemy)gameEvent.From;
                        AddExplosion(((Enemy) gameEvent.From).Shape.Position, 
                            ((Enemy) gameEvent.From).Shape.Extent);
                        score.AddPoint();
                        break;
                }
            }
        }
        private void IterateShots() {
            playerShots.Iterate(shot => {
                //first move shot
                shot.Shape.Move();
                //check if shot is out of bounds
                if (shot.Shape.Position.X >= 1.0f) {
                    shot.DeleteEntity();
                //check if enemies are hit
                } else {
                    squadron.Enemies.Iterate(enemy => {
                        CollisionData check = CollisionDetection.Aabb(shot.Shape.AsDynamicShape(),
                            enemy.Shape);
                        if (check.Collision) {
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                    GameEventType.ControlEvent, this, enemy, "", "Damage", ""));
                            shot.DeleteEntity();
                        }
                    });
                }
            });
        }
        // True enemies is in the container
        // False there is no enemies left in the container
        private bool CheckEnemies(EntityContainer<Enemy> enemies) {
            if (enemies.CountEntities() <= 0) {
                return false;
            } else {
                return true;
            }
        }
        // Creates new Squadron with multiplied speed
        private void CreateNewSquadron(ISquadron squadron) {
            this.squadron = squadron;
            squadron.CreateEnemies(images, enemyStridesRed, currentMovementSpeed*enemySpeedMultiplier);
            currentMovementSpeed = currentMovementSpeed*enemySpeedMultiplier;
        }
        // Increases difficulty and randomly spawns new squadrons when all enemies are dead.
        private void IncreaseDifficulty() {
            if (!CheckEnemies(squadron.Enemies)) {
                var rand = new Random();
                int number = rand.Next(3);
                switch (number) {
                    case 0:
                        CreateNewSquadron(new SquadronCross());
                        break;
                    case 1:
                        CreateNewSquadron(new SquadronHoriLine());
                        break;
                    case 2:
                        CreateNewSquadron(new SquadronVertiLines());
                        break;
                }
            }
        }
        private bool CheckGameEnded() {
            bool ret = false;
            squadron.Enemies.Iterate (enemy => {
                if (enemy.Shape.Position.Y < 0f) {
                    ret = true;
                }
            });
            return ret;
        }
    }
}