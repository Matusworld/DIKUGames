using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Galaga;
using DIKUArcade.EventBus;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;
using System;

namespace GalagaTests {
    public class TestEnemy {

        Enemy enemy;
        GameEventBus<object> eventBus;

        List<Image> enemyStrides;

        List<Image> alternativeEnemystrideStrides;

        float BeforeMovementspeed;

        int BeforeHitpoints;

        float tolerance;

        [SetUp]
        public void Setup() {
            DIKUArcade.Window.CreateOpenGLContext();

            enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", 
                "Images", "BlueMonster.png"));

            alternativeEnemystrideStrides = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "RedMonster.png"));

            enemy = new Enemy (new DynamicShape(
                                new Vec2F(0.5f, 0.5f), 
                                new Vec2F(0.1f, 0.1f)),
                            new ImageStride(80, enemyStrides),
                            new ImageStride (80, alternativeEnemystrideStrides), 0.003f);

            BeforeMovementspeed = enemy.MOVEMENT_SPEED;

            BeforeHitpoints = enemy.Hitpoints;

            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.ControlEvent});
            eventBus.Subscribe(GameEventType.ControlEvent, enemy);

            tolerance = 0.0001f;

        }

        [Test]
        public void TestEnemyDamage() {

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.ControlEvent, this, enemy, "", "Damage", ""));

            eventBus.ProcessEvents();

            Assert.AreEqual(enemy.Hitpoints, BeforeHitpoints - 10);
        }

        [Test]
        public void TestisDead() {

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.ControlEvent, this, enemy, "", "Damage", ""));

            eventBus.ProcessEvents();

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.ControlEvent, this, enemy, "", "Damage", ""));

            eventBus.ProcessEvents();

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.ControlEvent, this, enemy, "", "Damage", ""));

            eventBus.ProcessEvents();

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.ControlEvent, this, enemy, "", "Damage", ""));

            eventBus.ProcessEvents();

            eventBus.RegisterEvent(
                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.ControlEvent, this, enemy, "", "Damage", ""));

            eventBus.ProcessEvents();

            Assert.AreEqual(true, enemy.Dead);
        }

        [Test]
        public void TestNotEnrage() {
            
            for (int i = 0; i < 3; i++) {
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                        GameEventType.ControlEvent, this, enemy, "", "Damage", ""));
            }

            eventBus.ProcessEvents();

            Assert.IsFalse(enemy.Enraged);
        }

        [Test]
        public void TestEnrage() {

            for (int i = 0; i < 4; i++) {
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                        GameEventType.ControlEvent, this, enemy, "", "Damage", ""));
            }

            eventBus.ProcessEvents();

            Assert.IsTrue(enemy.Enraged);

            float diff = Math.Abs(enemy.MOVEMENT_SPEED
                - (BeforeMovementspeed * enemy.EnrangedMultiplier));

            Assert.LessOrEqual(diff, tolerance);
        }

    }
}