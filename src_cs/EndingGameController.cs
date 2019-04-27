using SwinGameSDK;
/// <summary>

/// ''' The EndingGameController is responsible for managing the interactions at the end

/// ''' of a game.

/// ''' </summary>
namespace Battleship
{
    public static class EndingGameController
    {
        private static Bitmap _AnimationDead;

        //draw the dead screen function
        //does not work well because it keeps looping until the handleendofgameinput get the inputs
        private static void DrawDeadScreen()
        {
            _AnimationDead = SwinGame.LoadBitmap(SwinGame.PathToResource("deadLayer.png", ResourceKind.BitmapResource));
            
            for (int i = 2; i <= 3; i++)
            {
                SwinGame.DrawBitmapPart(_AnimationDead, (i / 4) * 800, (i % 4) * 600, 800, 600, 0, 0);
                SwinGame.Delay(100);
                SwinGame.RefreshScreen();
                SwinGame.ProcessEvents();
            }
            SwinGame.FreeBitmap(_AnimationDead);
        }

        /// <summary>
        /// Draw the end of the game screen, shows the win/lose state
        /// </summary>
        public static void DrawEndOfGame()
        {

            UtilityFunctions.DrawField(GameController.ComputerPlayer.PlayerGrid, GameController.ComputerPlayer, true);
            UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);

            if (GameController.HumanPlayer.IsDestroyed || BattleShipsGame._endGameNow)
            {
                _AnimationDead = SwinGame.LoadBitmap(SwinGame.PathToResource("deadLayer.png", ResourceKind.BitmapResource));
                for (int i = 0; i <= 3; i++)
                {
                    SwinGame.DrawBitmapPart(_AnimationDead, (i / 4) * 800, (i % 4) * 600, 800, 600, 0, 0);
                }
                SwinGame.FreeBitmap(_AnimationDead);
            }
            else
                SwinGame.DrawBitmap(GameResources.GameImage("Win"), 0, 0);
        }

        /// <summary>
        /// Handle the input during the end of the game. Any interaction
        /// will result in it reading in the highsSwinGame.
        /// </summary>
        public static void HandleEndOfGameInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.vk_RETURN) || SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
            {
                if (GameController.ComputerPlayer.IsDestroyed)
                    HighScoreController.ReadHighScore(GameController.HumanPlayer.Score, GameController.Difficulty); //only go to score if win
                GameController.EndCurrentState(); //go back to menu after clicking
            }
        }
    }
}