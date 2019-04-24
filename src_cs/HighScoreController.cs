using System;
using System.Collections.Generic;
using System.IO;
using SwinGameSDK;

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
        private const int SCORES_LEFT = 490;

        private const String HighScoreFont = "Gameplay"; // set the font once and have the whole code refer to this for fonts (makes switching font in the future easy)

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

        private static List<Score> _Scores = new List<Score>();

        /// <summary>
        ///     ''' Loads the scores from the highscores text file.
        ///     ''' </summary>
        ///     ''' <remarks>
        ///     ''' The format is
        ///     ''' # of scores
        ///     ''' name score
        ///     '''
        ///     ''' saparated by space
        ///     ''' </remarks>
        private static void LoadScores()
        {
            string filename;
            filename = SwinGame.PathToResource("highscores.txt");

            StreamReader input;
            try
            {
                input = new StreamReader(filename);
            }
            catch (Exception) //if file not found, create it and open it
            {
                File.Create("Resources/highscores.txt").Close(); //when file is created it returns a file object, so we close it (or else the streamreader cant open it)
                input = new StreamReader(filename);
            }

            String line;

            _Scores.Clear(); // clear score list

            while((line = input.ReadLine()) != null) //read every line
            {
                Score score = new Score();
                String[] splitScore = line.Split(' '); //split the score
                score.Name = splitScore[0];
                score.Value = Int32.Parse(splitScore[1]); // convert to int first

                _Scores.Add(score); // add score to the list
            }
            input.Close();
        }

        /// <summary>
        ///     ''' Saves the scores back to the highscores text file.
        ///     ''' </summary>
        ///     ''' <remarks>
        ///     ''' The format is
        ///     ''' # of scores
        ///     ''' name score
        ///     '''
        ///     ''' saparated by space
        ///     ''' </remarks>
        private static void SaveScores()
        {
            string filename;
            filename = SwinGame.PathToResource("highscores.txt");

            StreamWriter output;
            output = new StreamWriter(filename);

            foreach (Score s in _Scores)
                output.WriteLine(s.Name + ' ' + s.Value);

            output.Close();
        }

        /// <summary>
        ///     ''' Draws the high scores to the screen.
        ///     ''' </summary>
        public static void DrawHighScores()
        {
            const int SCORES_HEADING = 40;
            const int SCORES_TOP = 80;
            const int SCORE_GAP = 30;

            if (_Scores.Count == 0)
                LoadScores();

            _Scores.Sort(); // sort the score

            SwinGame.DrawText("   High Scores   ", Color.Green, GameResources.GameFont(HighScoreFont), SCORES_LEFT, SCORES_HEADING);

            // For all of the scores
            int i;
            for (i = 0; i <= _Scores.Count - 1; i++)
            {
                Score s;

                s = _Scores[i];

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
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.vk_ESCAPE) || SwinGame.KeyTyped(KeyCode.vk_RETURN))
                GameController.EndCurrentState();
        }

        /// <summary>
        ///     ''' Read the user's name for their highsSwinGame.
        ///     ''' </summary>
        ///     ''' <param name="value">the player's sSwinGame.</param>
        ///     ''' <remarks>
        ///     ''' This verifies if the score is a highsSwinGame.
        ///     ''' </remarks>
        public static void ReadHighScore(int value)
        {
            const int ENTRY_TOP = 500;

            if (_Scores.Count == 0)
                LoadScores();

            // if there is less than 10 records or if the current score is better than the last place score
            if (_Scores.Count < 10 || value > _Scores[_Scores.Count - 1].Value)
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

                    UtilityFunctions.DrawBackground();
                    DrawHighScores();
                    SwinGame.DrawText("Name: ", Color.Green, GameResources.GameFont(HighScoreFont), SCORES_LEFT, ENTRY_TOP);
                    SwinGame.RefreshScreen();
                }

                s.Name = SwinGame.TextReadAsASCII();

                //if (s.Name.Length < 3)
                //    s.Name = s.Name + new string(System.Convert.ToChar(" "), 3 - s.Name.Length);
                // we dont want 3 letters names anymore

                //only delete last entry if score list is more than 10
               if (_Scores.Count >= 10)
                _Scores.RemoveAt(_Scores.Count - 1);

                _Scores.Add(s);
                _Scores.Sort();

                SaveScores(); // save score to file

                GameController.EndCurrentState();
            }
        }
    }
}