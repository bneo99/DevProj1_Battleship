using SwinGameSDK;

/// <summary>
/// ''' The menu controller handles the drawing and user interactions
/// ''' from the menus in the game. These include the main menu, game
/// ''' menu and the settings m,enu.
/// ''' </summary>

namespace Battleship
{
    public static class MenuController
    {
        /// <summary>
        /// A public variable to check the current gameMode
        /// </summary>
        public static AIOption _playMode;


        /// <summary>
        ///     ''' The menu structure for the game.
        ///     ''' </summary>
        ///     ''' <remarks>
        ///     ''' These are the text captions for the menu items.
        ///     ''' </remarks>
        private static readonly string[][] _menuStructure = new[] {
            new string[] { "PLAY", "HELP", "DIFFICULTY", "HIGHSCORE", "QUIT" },
            new string[] { "RETURN", "SURRENDER", "QUIT" },
            new string[] { "EASY", "MEDIUM", "HARD", "CHALLENGE"} };

        private const int MENU_TOP = 575;
        private const int MENU_LEFT = 30;
        private const int MENU_GAP = 0;
        private const int BUTTON_HEIGHT = 20;
        private static int BUTTON_WIDTH = 120; // button width
        private static int BUTTON_SEP = BUTTON_WIDTH + MENU_GAP; //or last button width
        private static int BUTTON_OFFSET; // offset for next button to be drawn
        private const int TEXT_OFFSET = 0;

        private const int MAIN_MENU = 0;
        private const int GAME_MENU = 1;
        private const int SETUP_MENU = 2;

        private const int MAIN_MENU_PLAY_BUTTON = 0;
        private const int MAIN_MENU_HELP_BUTTON = 1;
        private const int MAIN_MENU_SETUP_BUTTON = 2;
        private const int MAIN_MENU_TOP_SCORES_BUTTON = 3;
        private const int MAIN_MENU_QUIT_BUTTON = 4;

        private const int SETUP_MENU_EASY_BUTTON = 0;
        private const int SETUP_MENU_MEDIUM_BUTTON = 1;
        private const int SETUP_MENU_HARD_BUTTON = 2;
        //CHALLENGE
        private const int SETUP_MENU_CHALLENGE_BUTTON = 3;
        private const int SETUP_MENU_EXIT_BUTTON = 4;

        private const int GAME_MENU_RETURN_BUTTON = 0;
        private const int GAME_MENU_SURRENDER_BUTTON = 1;
        private const int GAME_MENU_QUIT_BUTTON = 2;

        private static readonly Color MENU_COLOR = SwinGame.RGBAColor(2, 167, 252, 255);
        private static readonly Color HIGHLIGHT_COLOR = SwinGame.RGBAColor(1, 57, 86, 255);



        /// <summary>
        /// A public variable to check the current gameMode
        /// </summary>
        public static AIOption PlayMode
        {
            get
            {
                return _playMode;
            }
            set
            {
                _playMode = value;
            }
        }
        
        /// <summary>
        ///     ''' Handles the processing of user input when the main menu is showing
        ///     ''' </summary>
        public static void HandleMainMenuInput()
        {
            HandleMenuInput(MAIN_MENU, 0, 0);
        }

        /// <summary>
        ///     ''' Handles the processing of user input when the main menu is showing
        ///     ''' </summary>
        public static void HandleSetupMenuInput()
        {
            bool handled;
            handled = HandleMenuInput(SETUP_MENU, 1, 1);

            if (!handled)
                HandleMenuInput(MAIN_MENU, 0, 0);
        }

        /// <summary>
        ///     ''' Handle input in the game menu.
        ///     ''' </summary>
        ///     ''' <remarks>
        ///     ''' Player can return to the game, surrender, or quit entirely
        ///     ''' </remarks>
        public static void HandleGameMenuInput()
        {
            HandleMenuInput(GAME_MENU, 0, 0);
        }

        /// <summary>
        ///     ''' Handles input for the specified menu.
        ///     ''' </summary>
        ///     ''' <param name="menu">the identifier of the menu being processed</param>
        ///     ''' <param name="level">the vertical level of the menu</param>
        ///     ''' <param name="xOffset">the xoffset of the menu</param>
        ///     ''' <returns>false if a clicked missed the buttons. This can be used to check prior menus.</returns>
        private static bool HandleMenuInput(int menu, int level, int xOffset)
        {
            if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
            {
                if(GameController.CurrentState != GameState.ViewingMainMenu) GameController.EndCurrentState(); //dont let escape quit game if in main menu
                return true;
            }

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                int i;
                for (i = 0; i <= _menuStructure[menu].Length - 1; i++)
                {
                    // IsMouseOver the i'th button of the menu
                    if (IsMouseOverMenu(menu, i, level, xOffset))
                    {
                        PerformMenuAction(menu, i);
                        return true;
                    }
                }

                if (level > 0)
                    // none clicked - so end this sub menu
                    GameController.EndCurrentState();
            }

            return false;
        }

        /// <summary>
        ///     ''' Draws the main menu to the screen.
        ///     ''' </summary>
        public static void DrawMainMenu()
        {
            DrawButtons(MAIN_MENU);
        }

        /// <summary>
        ///     ''' Draws the Game menu to the screen
        ///     ''' </summary>
        public static void DrawGameMenu()
        {
            DrawButtons(GAME_MENU);
        }

        /// <summary>
        ///     ''' Draws the difficulty menu to the screen.
        ///     ''' </summary>
        ///     ''' <remarks>
        ///     ''' Also shows the main menu
        ///     ''' </remarks>
        public static void DrawSettings()
        {
            DrawButtons(MAIN_MENU);
            DrawButtons(SETUP_MENU, 1, 1);
        }

        /// <summary>
        ///     ''' Draw the buttons associated with a top level menu.
        ///     ''' </summary>
        ///     ''' <param name="menu">the index of the menu to draw</param>
        private static void DrawButtons(int menu)
        {
            DrawButtons(menu, 0, 0);
        }

        /// <summary>
        ///     ''' Draws the menu at the indicated level.
        ///     ''' </summary>
        ///     ''' <param name="menu">the menu to draw</param>
        ///     ''' <param name="level">the level (height) of the menu</param>
        ///     ''' <param name="xOffset">the offset of the menu (levels of indents)</param>
        ///     ''' <remarks>
        ///     ''' The menu text comes from the _menuStructure field. The level indicates the height
        ///     ''' of the menu, to enable sub menus. The xOffset repositions the menu horizontally
        ///     ''' to allow the submenus to be positioned correctly.
        ///     ''' </remarks>
        private static void DrawButtons(int menu, int level, int xOffset)
        {
            int btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
            int i;

            BUTTON_OFFSET = MENU_LEFT + xOffset * 50; //offset to draw next button, MENU_LEFT is the offset from the left side of screen

            for (i = 0; i <= _menuStructure[menu].Length - 1; i++) // generate buttons based on number of elements in array
            {
                if (i > 0) BUTTON_SEP = _menuStructure[menu][i-1].Length * 14 + 40; //last button width; i-1 coz first one ofc wont have a button before it
                else BUTTON_SEP = 0;

                BUTTON_WIDTH = _menuStructure[menu][i].Length * 14 + 40; // current button width
            //ori: btnLeft = MENU_LEFT + BUTTON_SEP * (i + xOffset);
                BUTTON_OFFSET += BUTTON_SEP; //add up last button width into offset

                SwinGame.DrawTextLines(_menuStructure[menu][i], MENU_COLOR, Color.Black, GameResources.GameFont("Menu"), FontAlignment.AlignCenter, BUTTON_OFFSET + TEXT_OFFSET, btnTop + TEXT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT);

                //if (SwinGame.MouseDown(MouseButton.LeftButton) & UtilityFunctions.IsMouseInRectangle(BUTTON_OFFSET, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT))
                    if (SwinGame.MouseDown(MouseButton.LeftButton) & IsMouseOverMenu(menu, i, level, xOffset))
                        SwinGame.DrawRectangle(HIGHLIGHT_COLOR, BUTTON_OFFSET, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
            }
        }

        /// <summary>
        ///     ''' Checks if the mouse is over one of the buttons in a menu.
        ///     ''' </summary>
        ///     ''' <param name="button">the index of the button to check</param>
        ///     ''' <param name="level">the level of the menu (height level)</param>
        ///     ''' <param name="xOffset">the xOffset of the menu (level of indent)</param>
        ///     ''' <returns>true if the mouse is over the button</returns>
        private static bool IsMouseOverMenu(int menu, int button, int level, int xOffset)
        {
            int btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;

            int btnLeft = MENU_LEFT + xOffset * 50; //offset for first button to start, MENU_LEFT is the offset from the left side of screen
            int btnSep, btnWdth = _menuStructure[menu][button].Length * 14 + 40; // current button width;

            for (int i = 0; i < button + 1; i++) // cycle through buttons till the button to check
            {
                if (i > 0) btnSep = _menuStructure[menu][i - 1].Length * 14 + 40; //last button width; i-1 coz first one ofc wont have a button before it
                else btnSep = 0;

                btnLeft += btnSep; //add up last button width into offset
            }
            return UtilityFunctions.IsMouseInRectangle(btnLeft, btnTop, btnWdth, BUTTON_HEIGHT);
        }

        /// <summary>
        ///     ''' A button has been clicked, perform the associated action.
        ///     ''' </summary>
        ///     ''' <param name="menu">the menu that has been clicked</param>
        ///     ''' <param name="button">the index of the button that was clicked</param>
        private static void PerformMenuAction(int menu, int button)
        {
            switch (menu)
            {
                case MAIN_MENU:
                    {
                        PerformMainMenuAction(button);
                        break;
                    }

                case SETUP_MENU:
                    {
                        PerformSetupMenuAction(button);
                        break;
                    }

                case GAME_MENU:
                    {
                        PerformGameMenuAction(button);
                        break;
                    }
            }
        }

        /// <summary>
        ///     ''' The main menu was clicked, perform the button's action.
        ///     ''' </summary>
        ///     ''' <param name="button">the button pressed</param>
        private static void PerformMainMenuAction(int button)
        {
            switch (button)
            {
                case MAIN_MENU_PLAY_BUTTON:
                    {
                        GameController.StartGame();
                        break;
                    }

                case MAIN_MENU_HELP_BUTTON:
                    {
                        GameController.AddNewState(GameState.ViewingHelpPage);
                        break;
                    }

                case MAIN_MENU_SETUP_BUTTON:
                    {
                        GameController.AddNewState(GameState.AlteringSettings);
                        break;
                    }

                case MAIN_MENU_TOP_SCORES_BUTTON:
                    {
                        GameController.AddNewState(GameState.ViewingHighScores);
                        break;
                    }

                case MAIN_MENU_QUIT_BUTTON:
                    {
                        GameController.EndCurrentState();
                        break;
                    }
            }
        }

        /// <summary>
        ///     ''' The setup menu was clicked, perform the button's action.
        ///     ''' </summary>
        ///     ''' <param name="button">the button pressed</param>
        private static void PerformSetupMenuAction(int button)
        {
            switch (button)
            {
                case SETUP_MENU_EASY_BUTTON:
                    {
                        GameController.SetDifficulty(AIOption.Easy);
                        _playMode = AIOption.Easy;
                        break;
                    }

                case SETUP_MENU_MEDIUM_BUTTON:
                    {
                        GameController.SetDifficulty(AIOption.Medium);
                        _playMode = AIOption.Medium;
                        break;
                    }

                case SETUP_MENU_HARD_BUTTON:
                    {
                        GameController.SetDifficulty(AIOption.Hard);
                        _playMode = AIOption.Hard;
                        break;
                    }
                case SETUP_MENU_CHALLENGE_BUTTON:
                    {
                        GameController.SetDifficulty(AIOption.Challenge);
                        _playMode = AIOption.Challenge;
                        break;
                    }
            }
            // Always end state - handles exit button as well
            GameController.EndCurrentState();
        }

        /// <summary>
        ///     ''' The game menu was clicked, perform the button's action.
        ///     ''' </summary>
        ///     ''' <param name="button">the button pressed</param>
        private static void PerformGameMenuAction(int button)
        {
            switch (button)
            {
                case GAME_MENU_RETURN_BUTTON:
                    {
                        GameController.EndCurrentState();
                        break;
                    }

                case GAME_MENU_SURRENDER_BUTTON:
                    {
                        GameController.EndCurrentState(); // end game menu
                        GameController.EndCurrentState(); // end game
                        break;
                    }

                case GAME_MENU_QUIT_BUTTON:
                    {
                        GameController.AddNewState(GameState.Quitting);
                        break;
                    }
            }
        }
    }
}