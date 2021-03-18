using DIKUArcade;
using DIKUArcade.Timers;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;
using DIKUArcade.EventBus;
using DIKUArcade.Physics;
using Galaga.Squadron;
using Galaga.MovementStrategy;
using System;

namespace Galaga
{
    public class Game : IGameEventProcessor<object> {
        private Window window;
        private GameTimer gameTimer;
        private Player player;
        public static GameEventBus<object> eventBus {get; private set;}
        private ISquadron squadron;
        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;
        private AnimationContainer enemyExplosions;
        private List<Image> explosionStrides;
        private const int EXPLOSION_LENGTH_MS = 500;
        private List<Image> images;
        private List<Image> enemyStridesRed;
        private Score score;
        private float currentMovementSpeed = 0.0003f;
        private float enemySpeedMultiplier = 1.2f;



        public Game() {
            window = new Window("Galaga", 500, 500);
            gameTimer = new GameTimer(60, 60);
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));

            score = new Score(new Vec2F (0.485f, -0.2f), new Vec2F (0.3f, 0.3f));

            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent,
                GameEventType.PlayerEvent, GameEventType.EnemyEvent, GameEventType.GraphicsEvent});

            window.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.PlayerEvent, this.player);
            eventBus.Subscribe(GameEventType.GraphicsEvent, this);
            
            images = ImageStride.CreateStrides(4, Path.Combine("Assets", 
                "Images", "BlueMonster.png"));
            enemyStridesRed = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "RedMonster.png"));
            
            squadron = new SquadronVertiLines();
            squadron.CreateEnemies(images, enemyStridesRed, currentMovementSpeed);

            playerShots = new EntityContainer<PlayerShot>();
            playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
            enemyExplosions = new AnimationContainer(squadron.MaxEnemies);
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
        }
        
        //call SetMoves with true if appropriate keys have been pressed
        public void KeyPress(string key) {
            switch (key) {
                case "KEY_LEFT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "true", "SetMoveLeft", ""));
                    break;
                case "KEY_RIGHT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "true", "SetMoveRight", ""));
                    break;
                default:
                    break;
            }
        }

        //call SetMoves with "false" if appropriate keys have been released
        public void KeyRelease(string key) {
            switch (key) {
                case "KEY_LEFT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "false", "SetMoveLeft", ""));
                    break;
                case "KEY_RIGHT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "false", "SetMoveRight", ""));
                    break;
                case "KEY_ESCAPE":
                    window.CloseWindow();
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
                            eventBus.RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                    GameEventType.EnemyEvent, this, enemy, "", "Damage", ""));
                            shot.DeleteEntity();
                        }
                    });
                }
            });
        }

        // Add explosion animation at given position to animation container
        public void AddExplosion(Vec2F position, Vec2F extent) {
            StationaryShape explosion = new StationaryShape(position, extent);
            ImageStride explosionStride = new ImageStride(EXPLOSION_LENGTH_MS/8, explosionStrides);
            enemyExplosions.AddAnimation(explosion, EXPLOSION_LENGTH_MS, explosionStride);
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

        public void Run() {
            while(window.IsRunning()) {
                gameTimer.MeasureTime();


                while (gameTimer.ShouldUpdate()) {
                    if (!CheckGameEnded()) {
                        window.PollEvents();

                        //Handle input events
                        eventBus.ProcessEvents();

                        eventBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.PlayerEvent, this, "", "Move", ""));

                        IterateShots();

                        ZigZagDown.MoveEnemies(squadron.Enemies);

                        IncreaseDifficulty();
                    } else {
                        window.PollEvents();
                        
                        //Handle input events
                        eventBus.ProcessEvents();
                    }
                }
                if (gameTimer.ShouldRender()) {
                    if (!CheckGameEnded()) {
                        window.Clear();

                        player.Render();

                        squadron.Enemies.RenderEntities();

                        playerShots.RenderEntities();

                        enemyExplosions.RenderAnimations();

                        score.RenderScore();

                        window.SwapBuffers();
                    } else {
                        window.Clear();

                        score.RenderScore();

                        window.SwapBuffers();
                    }
                }


                if (gameTimer.ShouldReset()) {
                    // this update happens once every second
                    window.Title = $"Galaga | (UPS,FPS): ({gameTimer.CapturedUpdates},{gameTimer.CapturedFrames})";  
                }
            }
        }

    }
}