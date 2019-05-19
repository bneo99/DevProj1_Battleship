using NUnit.Framework;

namespace Battleship.src
{
    [TestFixture]
    class Test
    {
        [Test]
        public void TestMethod()
        {
            MenuController menu = new MenuController();

            int button1 = 0;
            int button2 = 1;
            int button3 = 2;

            AIOption result = menu.PerformSetupMenuActionTest(button1);
            AIOption result2 = menu.PerformSetupMenuActionTest(button2);
            AIOption result3 = menu.PerformSetupMenuActionTest(button3);

            Assert.AreEqual(AIOption.Easy, result);
            Assert.AreEqual(AIOption.Medium, result2);
            Assert.AreEqual(AIOption.Hard, result3);
        }
    }
}
