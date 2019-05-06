using System;
using System.Collections.Generic;
using System.IO;
using SwinGameSDK;
using Newtonsoft.Json;

/// <summary>
/// ''' Controls displaying and collecting high score data.
/// ''' </summary>
/// ''' <remarks>
/// ''' Data is saved to a file.
/// ''' </remarks>

namespace Battleship
{
    public class HighScoreController
    {
        private const int NAME_WIDTH = 20;
        private const int SCORES_LEFT = 100;

        private const int btnStartX = 70;
        private const int btnStartY = 120;
        private const int btnWidth = 120;
        private const int btnHeight = 20;

        //dictionary to hold all scores, key is difficulty, value is lists containing scores
        private static Dictionary<AIOption, List<Score>> _AllScores = new Dictionary<AIOption, List<Score>>();

        private static AIOption _difficulty = AIOption.Easy; //highscore page always starts in easy mode

        private const String HighScoreFont = "HighScore"; // set the font once and have the whole code refer to this for fonts (makes switching font in the future easy)

        /// <summary>
        ///     ''' The score structure is used to keep the name and
        ///     ''' score of the top players together.
        ///     ''' </summary>
        private struct Score : IComparable
        {
            public string Name;
            public int Value;

            /// <summary>
            ///         ''' Allows scores to be compared to facilitate sorting
            ///         ''' </summary>
            ///         ''' <param name="obj">the object to compare to</param>
            ///         ''' <returns>a value that indicates the sort order</returns>
            public int CompareTo(object obj) //probably not needed as we can do _scores.Sort()
            {
                if (obj is Score)
                {
                    Score other = (Score)obj;

                    return other.Value - this.Value;
                }
                else
                    return 0;
            }

        }
            /// <summary>
            /// Loads the scores from the highscores text file.
            /// </summary>
            private static void LoadScores()
        {
            string filename;
            filename = SwinGame.PathToResource("highscores.json"); //json lai

            StreamReader input;
            try
            {
                input = new StreamReader(filename); //try to open file
            }
            catch (Exception) //if file not found, create it and open it
            {
                File.Create("Resources/highscores.json").Close(); //when file is created it returns a file object, so we close it (or else the streamreader cant open it)
                StreamWriter s = new StreamWriter(filename); //open file to be written to

                //initialise all the lists for the different difficulties
                _AllScores[AIOption.Easy] = new List<Score>();
                _AllScores[AIOption.Medium] = new List<Score>();
                _AllScores[AIOption.Hard] = new List<Score>();
                _AllScores[AIOption.Challenge] = new List<Score>();

                //write initial json structure into file
                s.Write(JsonConvert.SerializeObject(_AllScores));
                s.Close(); //close the streamwriter

                input = new StreamReader(filename); // open file to be read again (even if theres nothing haha)
            }

            _AllScores.Clear(); // clear scores list

            _AllScores = JsonConvert.DeserializeObject<Dictionary<AIOption, List<Score>>>(input.ReadLine()); // reads the json string from file and parse to _AllScores

            input.Close(); // close file
        }

        /// <summary>
        /// Saves the scores back to the highscores json file.
        /// </summary>

        private static void SaveScores()
        {
            string filename;
            filename = SwinGame.PathToResource("highscores.json");

            StreamWriter output;
            output = new StreamWriter(filename);

            //write the dictionary to file
            output.Write(JsonConvert.SerializeObject(_AllScores));

            output.Close();
        }

        /// <summary>
        ///     ''' Draws the high scores to the screen.
        ///     ''' </summary>
        public static void DrawHighScores()
        {
            const int SCORES_TOP = 170;
            const int SCORE_GAP = 30;

            LoadScores();

            _AllScores[AIOption.Easy].Sort(); // sort the scores
            _AllScores[AIOption.Medium].Sort();
            _AllScores[AIOption.Hard].Sort();
            _AllScores[AIOption.Challenge].Sort();

            SwinGame.DrawRectangle(SwinGame.RGBAColor(0, 0, 0, 127), true, 50, 50, 700, 500); //create shaded area so text is easier to read

            SwinGame.DrawText("High Score", Color.White, GameResources.GameFont("HighScoreTitle"), 70, 70);

            //draw the buttons
            if(_difficulty == AIOption.Easy) SwinGame.DrawTextLines("Easy", Color.Pink, Color.Blue, GameResources.GameFont("HighScoreDifficulty"), FontAlignment.AlignCenter, btnStartX, btnStartY, btnWidth, btnHeight);
            else SwinGame.DrawTextLines("Easy", Color.Blue, Color.Pink, GameResources.GameFont("HighScoreDifficulty"), FontAlignment.AlignCenter, btnStartX, btnStartY, btnWidth, btnHeight);

            if (_difficulty == AIOption.Medium) SwinGame.DrawTextLines("Medium", Color.Pink, Color.Blue, GameResources.GameFont("HighScoreDifficulty"), FontAlignment.AlignCenter, btnStartX + btnWidth, btnStartY, btnWidth, btnHeight);
            else SwinGame.DrawTextLines("Medium", Color.Blue, Color.Pink, GameResources.GameFont("HighScoreDifficulty"), FontAlignment.AlignCenter, btnStartX + btnWidth, btnStartY, btnWidth, btnHeight);

            if (_difficulty == AIOption.Hard) SwinGame.DrawTextLines("Hard", Color.Pink, Color.Blue, GameResources.GameFont("HighScoreDifficulty"), FontAlignment.AlignCenter, btnStartX + btnWidth*2, btnStartY, btnWidth, btnHeight);
            else SwinGame.DrawTextLines("Hard", Color.Blue, Color.Pink, GameResources.GameFont("HighScoreDifficulty"), FontAlignment.AlignCenter, btnStartX + btnWidth * 2, btnStartY, btnWidth, btnHeight);

            if (_difficulty == AIOption.Challenge) SwinGame.DrawTextLines("Challenge", Color.Pink, Color.Blue, GameResources.GameFont("HighScoreDifficulty"), FontAlignment.AlignCenter, btnStartX + btnWidth * 3, btnStartY, btnWidth, btnHeight);
            else SwinGame.DrawTextLines("Challenge", Color.Blue, Color.Pink, GameResources.GameFont("HighScoreDifficulty"), FontAlignment.AlignCenter, btnStartX + btnWidth * 3, btnStartY, btnWidth, btnHeight);


            // For all of the scores
            int i;
            for (i = 0; i <= _AllScores[_difficulty].Count - 1; i++)
            {
                Score s;

                s = _AllScores[_difficulty][i];

                // for scores 1 - 9 use 01 - 09
                if (i < 9)
                    SwinGame.DrawText(" " + (i + 1) + ":   " + s.Name + "   " + s.Value, Color.Yellow, GameResources.GameFont(HighScoreFont), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
                else
                    SwinGame.DrawText(i + 1 + ":   " + s.Name + "   " + s.Value, Color.Yellow, GameResources.GameFont(HighScoreFont), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
            }
        }

        /// <summary>
        ///     ''' Handles the user input during the top score screen.
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        public static void HandleHighScoreInput()
        {
            if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE) || SwinGame.KeyTyped(KeyCode.vk_RETURN))
                GameController.EndCurrentState();

            if (SwinGame.MouseClicked(MouseButton.LeftButton)) //if user clicked on another difficulty
            {
                if (IsMouseOverButton(AIOption.Easy)) _difficulty = AIOption.Easy;
                else if (IsMouseOverButton(AIOption.Medium)) _difficulty = AIOption.Medium;
                else if (IsMouseOverButton(AIOption.Hard)) _difficulty = AIOption.Hard;
                else if (IsMouseOverButton(AIOption.Challenge)) _difficulty = AIOption.Challenge;
            }

        }

        private static bool IsMouseOverButton(AIOption diff)
        {
            int btnX;
            int btnY = btnStartY;

            switch (diff)
            {
                case AIOption.Easy:
                    btnX = btnStartX;
                    break;
                case AIOption.Medium:
                    btnX = btnStartX + btnWidth * 1;
                    break;
                case AIOption.Hard:
                    btnX = btnStartX + btnWidth * 2;
                    break;
                case AIOption.Challenge:
                    btnX = btnStartX + btnWidth * 3;
                    break;
                default:
                    return false;
            }
            return UtilityFunctions.IsMouseInRectangle(btnX, btnY, btnWidth, btnHeight);
        }

        /// <summary>
        ///     ''' Read the user's name for their highsSwinGame.
        ///     ''' </summary>
        ///     ''' <param name="value">the player's sSwinGame.</param>
        ///     ''' <remarks>
        ///     ''' This verifies if the score is a highsSwinGame.
        ///     ''' </remarks>
        public static void ReadHighScore(int value, AIOption difficulty)
        {
            const int ENTRY_TOP = 500;

            difficulty = GameController.Difficulty; // set to display the difficulty being played

            LoadScores(); // load scores from file

            // if there is less than 10 records or if the current score is better than the last place score
            if (_AllScores[difficulty].Count < 10 || value > _AllScores[difficulty][_AllScores[difficulty].Count - 1].Value)
            {
                Score s = new Score();
                s.Value = value;

                GameController.AddNewState(GameState.ViewingHighScores);

                int x;
                x = SCORES_LEFT + SwinGame.TextWidth(GameResources.GameFont(HighScoreFont), "Name: ");

                SwinGame.StartReadingText(Color.White, NAME_WIDTH, GameResources.GameFont(HighScoreFont), x, ENTRY_TOP);

                // Read the text from the user
                while (SwinGame.ReadingText())
                {
                    SwinGame.ProcessEvents();

                    UtilityFunctions.DrawBackground(ref MenuController.menuBackgroundName, ref MenuController.helpBackgroundName);
                    DrawHighScores();
                    SwinGame.DrawText("Name: ", Color.Green, GameResources.GameFont(HighScoreFont), SCORES_LEFT, ENTRY_TOP);
                    SwinGame.RefreshScreen();
                }

                s.Name = SwinGame.TextReadAsASCII();

                //only delete last entry if score list is more than 10
                if (_AllScores[difficulty].Count >= 10)
                    _AllScores[difficulty].RemoveAt(_AllScores[difficulty].Count - 1);

                _AllScores[difficulty].Add(s);
                _AllScores[difficulty].Sort();

                SaveScores(); // save score to file

                GameController.EndCurrentState();
            }
        }
    }
}