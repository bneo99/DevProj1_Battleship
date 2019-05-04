using SwinGameSDK;
using System;
using System.Windows.Forms;
using System.IO;
using System.Security.Permissions;

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


        //public static int ran = 0;
        public static string path = Directory.GetCurrentDirectory();
        public static string newPath = path.Substring(0, (path.Length - 10));


        /// <summary>
        /// Public variables string to allow users to change the filename that is to be used in the UtitlityFunctions.DrawBackground()
        /// </summary>
        public static string menuBackgroundName = "Menu";
        public static string helpBackgroundName = "Help";

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

        private const int MENU_TOP = 540;
        private const int MENU_LEFT = 30;
        private const int MENU_GAP = 0;
        private const int BUTTON_HEIGHT = 20;
        private static int BUTTON_WIDTH = 120; // button width
        private static int BUTTON_SEP = BUTTON_WIDTH + MENU_GAP; //or last button width
        private static int BUTTON_OFFSET; // offset for next button to be drawn
        private const int TEXT_OFFSET = 0;

        private const int MAIN_MENU = 0;
        private const int GAME_MENU = 1;
        private const int DIFFICULTY_MENU = 2;

        private const int MAIN_MENU_PLAY_BUTTON = 0;
        private const int MAIN_MENU_HELP_BUTTON = 1;
        private const int MAIN_MENU_SETUP_BUTTON = 2;
        private const int MAIN_MENU_TOP_SCORES_BUTTON = 3;
        private const int MAIN_MENU_QUIT_BUTTON = 4;

        private const int DIFFICULTY_MENU_EASY_BUTTON = 0;
        private const int DIFFICULTY_MENU_MEDIUM_BUTTON = 1;
        private const int DIFFICULTY_MENU_HARD_BUTTON = 2;
        //CHALLENGE
        private const int DIFFICULTY_MENU_CHALLENGE_BUTTON = 3;
        private const int DIFFICULTY_MENU_EXIT_BUTTON = 4;

        private const int GAME_MENU_RETURN_BUTTON = 0;
        private const int GAME_MENU_SURRENDER_BUTTON = 1;
        private const int GAME_MENU_QUIT_BUTTON = 2;

        private static readonly Color MENU_COLOR = SwinGame.RGBAColor(2, 167, 252, 255);
        private static readonly Color HIGHLIGHT_COLOR = SwinGame.RGBAColor(1, 57, 86, 255);

        public static int picNum = 0;

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
        /// Added a watcher to detect new pictures in that file
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void DetectNewFile()
        {
            string path = Directory.GetCurrentDirectory();
            string newPath2;
            
            //debug
            newPath2 = path.Substring(0, (path.Length - 10)) + "\\bin\\Debug\\Resources\\images"; //"\\Resources\\images";

            // A FileSystemWatcher Object is created with the parameter of the path
            FileSystemWatcher watcher = new FileSystemWatcher(newPath2);

            // Watch for changes of lastwrite items
            watcher.NotifyFilter = NotifyFilters.LastWrite;

            // A message to notify programmers that it is working as expected
            Console.WriteLine("Detecting {0}.", newPath2);

            // Watch all type of files.
            watcher.Filter = "*";

            // Add event handlers.
            watcher.Changed += Watcher_Changed;

            // Enable to allow events to pop out
            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Allow something to happen after knowing that there is a new file
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">FileSystemEventArgs</param>
        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {   
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");

            string newName;
            newName = e.Name.Substring(0, e.Name.Length - 4);

            // If there is no such image name in the GameResources file, create a NewImage to GameResources and to be used for the background image
            try
            {
                if (!(GameResources._Images.ContainsKey(newName)))
                {
                    if (newName == "userPic0")
                    {
                        GameResources.NewImage(newName, e.Name);
                        menuBackgroundName = newName;
                    }
                    else if (newName == "userPic1")
                    {
                        GameResources.NewImage(newName, e.Name);
                        helpBackgroundName = newName;
                    }

                    SwinGame.Delay(1000);
                    SwinGame.ClearScreen();
                    SwinGame.RefreshScreen();
                }
            }
            catch (InvalidCastException eer)
            {
                throw new Exception("Error: {0}", eer);
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
        public static void HandleDifficultyMenuInput()
        {
            bool handled;
            handled = HandleMenuInput(DIFFICULTY_MENU, 1, 1);

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
                if (GameController.CurrentState != GameState.ViewingMainMenu) GameController.EndCurrentState(); //dont let escape quit game if in main menu
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

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                if (UtilityFunctions.IsMouseInRectangle(720, 563, 65, 31))
                {
                    //string path = Directory.GetCurrentDirectory();
                    //string newPath;

                    //debug
                    //Console.WriteLine("The current directory is {0}", path);
                    //newPath = path.Substring(0, (path.Length - 10));
                    //debug
                    //Console.WriteLine("The current directory is {0}", newPath + "\\Resources\\images\\userPic" + picNum + ".png");

                    OpenFileDialog open = new OpenFileDialog();

                    open.Filter = "Image Files (*.png;*.jpg)|*.png;*.jpg|All Files(*.*)|*.*";
                    open.FilterIndex = 1;
                    open.Multiselect = true;

                    if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        DetectNewFile();

                        //try
                        foreach (string file in open.FileNames)
                        {
                            if (open.CheckFileExists)
                            {
                                try
                                {
                                    System.IO.File.Copy(file, (newPath + "\\bin\\Debug\\Resources\\images\\userPic" + picNum + ".png"));
                                    Console.WriteLine("Copy Success");
                                    picNum++;
                                }
                                catch (InvalidCastException e)
                                {
                                    throw new Exception("Error: {0}", e);
                                }
                            }
                        }


                        /*if (open.CheckFileExists)
                        {
                            System.IO.File.Copy(open.FileName, (newPath + "\\bin\\Debug\\Resources\\images\\userPic" + picNum + ".png"));
                            Console.WriteLine("Success");
                            //picNum++;
                        }*/
                    }
                }
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
            DrawButtons(DIFFICULTY_MENU, 1, 1);
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
                if (i > 0) BUTTON_SEP = _menuStructure[menu][i - 1].Length * 14 + 40; //last button width; i-1 coz first one ofc wont have a button before it
                else BUTTON_SEP = 0;

                BUTTON_WIDTH = _menuStructure[menu][i].Length * 14 + 40; // current button width
                BUTTON_OFFSET += BUTTON_SEP; //add up last button width into offset

                SwinGame.DrawTextLines(_menuStructure[menu][i], MENU_COLOR, Color.Black, GameResources.GameFont("Menu"), FontAlignment.AlignCenter, BUTTON_OFFSET + TEXT_OFFSET, btnTop + TEXT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT);

                if (SwinGame.MouseDown(MouseButton.LeftButton) & IsMouseOverMenu(menu, i, level, xOffset))
                    SwinGame.DrawRectangle(HIGHLIGHT_COLOR, BUTTON_OFFSET, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
            }

            SwinGame.DrawBitmap(GameResources.GameImage("ImportButton"), 720, 563);
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

                case DIFFICULTY_MENU:
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
                case DIFFICULTY_MENU_EASY_BUTTON:
                    {
                        GameController.SetDifficulty(AIOption.Easy);
                        _playMode = AIOption.Easy;
                        break;
                    }

                case DIFFICULTY_MENU_MEDIUM_BUTTON:
                    {
                        GameController.SetDifficulty(AIOption.Medium);
                        _playMode = AIOption.Medium;
                        break;
                    }

                case DIFFICULTY_MENU_HARD_BUTTON:
                    {
                        GameController.SetDifficulty(AIOption.Hard);
                        _playMode = AIOption.Hard;
                        break;
                    }
                case DIFFICULTY_MENU_CHALLENGE_BUTTON:
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