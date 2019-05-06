using SwinGameSDK;

namespace Battleship
{
    public class GameLogic
    {
        //[STAThread]
        public static void Main()
        {
            // Opens a new Graphics Window
            SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);
            
            // Load Resources
            GameResources.LoadResources();
            SwinGame.PlayMusic(GameResources.GameMusic("Background"));

            new GameController();
            // Game Loop
            do
            {
                GameController.HandleUserInput();
                GameController.DrawScreen();
            }
            while (!SwinGame.WindowCloseRequested() && GameController.CurrentState != GameState.Quitting);

            SwinGame.StopMusic();

            // Free Resources and Close Audio, to end the program.
            GameResources.FreeResources();
        }
    }
}