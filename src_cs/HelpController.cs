using SwinGameSDK;

namespace Battleship
{
    class HelpController
    {
        /// <summary>
        ///     ''' Draws help page.
        ///     ''' </summary>
        public static void DrawHelpPage()
        {
            SwinGame.DrawText("Help Page", Color.White, GameResources.GameFont("ArialLarge"), 50, 50);
            SwinGame.DrawText("make a picture and set as background here i guess", Color.White, GameResources.GameFont("Courier"), 50, 300);
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
