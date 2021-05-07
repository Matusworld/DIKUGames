using System.Collections.Generic;
using System.IO;
using System;
using NUnit.Framework;
using Breakout;
using Breakout.States;
using Breakout.States.Buttons;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Graphics;

namespace BreakoutTest {
    public class MainMenuTest {
        MainMenu menu;

        [SetUp]
        public void Setup() {
            Window.CreateOpenGLContext();
            menu = new MainMenu();
        }

        [Test]
        public void TestInitialbutton() {
            Assert.That(menu.activeButton.Value, Is.InstanceOf<NewGameButton>());
        }

        [Test]
        public void TestChangeActivebuttonKeyUP() {
            menu.HandleKeyEvent(KeyboardAction.KeyRelease, KeyboardKey.Up);

            Assert.That(menu.activeButton.Value, Is.InstanceOf<QuitGameButton>());
        }

        [Test]
        public void TestChangeActivebuttonKeyDown() {
            menu.HandleKeyEvent(KeyboardAction.KeyRelease, KeyboardKey.Down);

            Assert.That(menu.activeButton.Value, Is.InstanceOf<QuitGameButton>());
        }
    }
}