using SwinGameSDK;

/// <summary>

/// ''' The EndingGameController is responsible for managing the interactions at the end

/// ''' of a game.

/// ''' </summary>
namespace Battleship
{
    public static class EndingGameController
    {

        /// <summary>
        ///     ''' Draw the end of the game screen, shows the win/lose state
        ///     ''' </summary>
        public static void DrawEndOfGame()
        {
            UtilityFunctions.DrawField(GameController.ComputerPlayer.PlayerGrid, GameController.ComputerPlayer, true);
            UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);

            if (GameController.HumanPlayer.IsDestroyed || BattleShipsGame._endGameNow)
                SwinGame.DrawBitmap(GameResources.GameImage("Dead"), 0, 0);
            else
                SwinGame.DrawTextLines("-- WINNER --", Color.White, Color.Transparent, GameResources.GameFont("ArialLarge"), FontAlignment.AlignCenter, 0, 250, SwinGame.ScreenWidth(), SwinGame.ScreenHeight());
        }

        /// <summary>
        ///     ''' Handle the input during the end of the game. Any interaction
        ///     ''' will result in it reading in the highsSwinGame.
        ///     ''' </summary>
        public static void HandleEndOfGameInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.vk_RETURN) || SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
            {
                if (GameController.ComputerPlayer.IsDestroyed)
                    HighScoreController.ReadHighScore(GameController.HumanPlayer.Score); //only go to score if win
                GameController.EndCurrentState(); //go back to menu after clicking
            }
        }
    }
}