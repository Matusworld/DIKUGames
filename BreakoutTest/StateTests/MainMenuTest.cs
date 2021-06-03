using NUnit.Framework;

using Breakout.States;
using Breakout.States.Buttons;

using DIKUArcade.GUI;
using DIKUArcade.Input;


namespace BreakoutTest {
    public class MainMenuTest {
        MainMenu menu;

        public MainMenuTest() {
            Window.CreateOpenGLContext();
        }

        [SetUp]
        public void Setup() {
            menu = new MainMenu();
        }

        [Test]
        public void TestInitialbutton() {
            Assert.That(menu.buttonManager.ActiveButton.Value, Is.InstanceOf<NewGameButton>());
        }

        [Test]
        public void TestChangeActivebuttonKeyUP() {
            menu.HandleKeyEvent(KeyboardAction.KeyRelease, KeyboardKey.Up);

            Assert.That(menu.buttonManager.ActiveButton.Value, Is.InstanceOf<QuitGameButton>());
        }

        [Test]
        public void TestChangeActivebuttonKeyDown() {
            menu.HandleKeyEvent(KeyboardAction.KeyRelease, KeyboardKey.Down);

            Assert.That(menu.buttonManager.ActiveButton.Value, Is.InstanceOf<QuitGameButton>());
        }

        [Test]
        public void TestResetState() {
            menu.HandleKeyEvent(KeyboardAction.KeyRelease, KeyboardKey.Down);

            Assert.That(menu.buttonManager.ActiveButton.Value, Is.InstanceOf<QuitGameButton>());

            menu.ResetState();

            Assert.That(menu.buttonManager.ActiveButton.Value, Is.InstanceOf<NewGameButton>());
        }
    }
}