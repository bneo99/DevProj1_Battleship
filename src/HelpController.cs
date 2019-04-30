using SwinGameSDK;

namespace Battleship
{
    class HelpController
    {
        /// <summary>
        ///     ''' Draws help page.
        ///     ''' </summary>
        ///     
       
        public static void DrawHelpPage()
        {
            SwinGame.DrawRectangle(SwinGame.RGBAColor(0, 0, 0, 127), true, 25, 25, 750, 550);
           

            SwinGame.DrawText("Help Page", Color.White, GameResources.GameFont("Gameplay"), 300, 50);
            SwinGame.DrawText("How to play", Color.Cyan, GameResources.GameFont("Arial"), 100, 110);
            SwinGame.DrawText("Press the 'Play' button to enter Deployment Page. Deploy your ships.", Color.White, GameResources.GameFont("Arial"), 100, 140);
            SwinGame.DrawText("Your task is to sink every enemy ship before the enemy sinks yours.", Color.White, GameResources.GameFont("Arial"), 100, 170);
            SwinGame.DrawText("Keyboard Shortcuts and Mouse Control", Color.Cyan, GameResources.GameFont("Arial"), 100, 250);
            SwinGame.DrawText("Press Esc or clicking left click on mouse to Return from a page.", Color.White, GameResources.GameFont("Arial"), 100, 280);
            SwinGame.DrawText("Press Arrow keys to switch vertical or horizontol deployment.", Color.White, GameResources.GameFont("Arial"), 100, 310);
        }
        /// <summary>
        ///     ''' Handles the user input in the help page.
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        public static void HandleHelpPageInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.vk_ESCAPE) || SwinGame.KeyTyped(KeyCode.vk_RETURN))
                GameController.EndCurrentState();
        }
    }
}
