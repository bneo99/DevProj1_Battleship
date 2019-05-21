using NUnit.Framework;
using SwinGameSDK;

namespace Battleship
{
    [TestFixture]
    class TestBattleship
    {
        [Test]
        public void TestDifficulty()
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

        [Test]
        public void TestDeployment()
        {
            DeploymentController deploy = new DeploymentController();
            Point2D point = new Point2D();

            //point at grid 0, 0 (around the stated coords below)
            point.X = 371f;
            point.Y = 144f;

            int[] pointCoord = deploy.DoDeployClickTest(point);

            Assert.AreEqual(0, pointCoord[0]);
            Assert.AreEqual(0, pointCoord[1]);
        }
    }
}
