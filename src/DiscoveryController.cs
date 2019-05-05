using System;
using System.Collections.Generic;
using SwinGameSDK;

/// <summary>
/// ''' The battle phase is handled by the DiscoveryController.
/// ''' </summary>
/// 
namespace Battleship
{
    public static class DiscoveryController
    {
        public static bool cheating = false;
        private static List<KeyCode> keySequence = new List<KeyCode>();
        private static KeyCode[] keysToCheck = { KeyCode.vk_c, KeyCode.vk_h, KeyCode.vk_e, KeyCode.vk_a, KeyCode.vk_t };

        /// <summary>
        ///     ''' Handles input during the discovery phase of the game.
        ///     ''' </summary>
        ///     ''' <remarks>
        ///     ''' Escape opens the game menu. Clicking the mouse will
        ///     ''' attack a location.
        ///     ''' </remarks>
        public static void HandleDiscoveryInput()
        {
            if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
                GameController.AddNewState(GameState.ViewingGameMenu);

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
                DoAttack();

            foreach (KeyCode k in keysToCheck) // if the keys pressed is one of the char for the cheat word
            {
                if (keySequence.Count >= 5) keySequence.RemoveAt(0); // if there is more than 5 keys in the list, remove first one
                if (SwinGame.KeyTyped(k)) keySequence.Add(k); // add the pressed key into the list
            }
            
        }

        /// <summary>
        ///     ''' Attack the location that the mouse if over.
        ///     ''' </summary>
        private static void DoAttack()
        {
            Point2D mouse;

            mouse = SwinGame.MousePosition();

            // Calculate the row/col clicked
            int row, col;
            row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions.FIELD_TOP) / (double)(UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
            col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions.FIELD_LEFT) / (double)(UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

            if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height)
            {
                if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width) { }
                    GameController.Attack(row, col);
            }
        }

        /// <summary>
        ///     ''' Draws the game during the attack phase.
        ///     ''' </summary>s
        public static void DrawDiscovery()
        {
            //start position to draw the text
            const int SCORES_LEFT = 172;
            const int SHOTS_TOP = 150;
            const int HITS_TOP = 200;
            const int SPLASH_TOP = 250;

            //check if user inputed cheat code
            if (keySequence.Count >= 5)
            {
                if (keySequence[0] == KeyCode.vk_c | keySequence[1] == KeyCode.vk_h | keySequence[2] == KeyCode.vk_e | keySequence[3] == KeyCode.vk_a | keySequence[4] == KeyCode.vk_t)
                    if (!cheating) cheating = true;
            }

            //check if cheat is enabled
            if (cheating)
            {
                UtilityFunctions.DrawField(GameController.ComputerPlayer.PlayerGrid, GameController.ComputerPlayer, true);
            }

            if ((SwinGame.KeyDown(KeyCode.vk_LSHIFT) | SwinGame.KeyDown(KeyCode.vk_RSHIFT)) & SwinGame.KeyDown(KeyCode.vk_c))
                UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
            else
                UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);

            UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
            UtilityFunctions.DrawMessage();
            //show max moves for challenge
            if (MenuController.PlayMode == AIOption.Challenge)
                SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString() + " / 80", Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
            else SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
            SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, HITS_TOP);
            SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
        }
    }
}