using System.Collections.Generic;
using SwinGameSDK;

namespace Battleship
{
    static class GameResources
    {
        private static void LoadFonts()
        {
            NewFont("ArialLarge", "arial.ttf", 80);
            NewFont("Arial", "arial.ttf", 16);
            NewFont("Gameplay", "Gameplay.ttf", 30);
            NewFont("HighScoreTitle", "Gameplay.ttf", 26);
            NewFont("HighScoreDifficulty", "Gameplay.ttf", 14);
            NewFont("HighScore", "Gameplay.ttf", 16);
            NewFont("Courier", "cour.ttf", 18);
            NewFont("CourierSmall", "cour.ttf", 8);
            NewFont("Menu", "ffaccess.ttf", 14);
        }

        private static void LoadImages()
        {
            // Backgrounds
            NewImage("Menu", "main_page1.jpg");
            NewImage("Help", "test1.jpg");
            NewImage("Discovery", "discover.jpg");
            NewImage("Deploy", "deploy.jpg");
            NewImage("Win", "win.png");

            // Deployment
            NewImage("LeftRightButton", "deploy_dir_button_horiz.png");
            NewImage("UpDownButton", "deploy_dir_button_vert.png");
            NewImage("SelectedShip", "deploy_button_hl.png");
            NewImage("PlayButton", "deploy_play_button.png");
            NewImage("RandomButton", "deploy_randomize_button.png");
            NewImage("MusicEnabled", "music.png");
            NewImage("MusicDisabled", "no_music.png");

            // Ships
            int i;
            for (i = 1; i <= 5; i++)
            {
                NewImage("ShipLR" + i, "ship_deploy_horiz_" + i + ".png");
                NewImage("ShipUD" + i, "ship_deploy_vert_" + i + ".png");
            }

            // Explosions
            NewImage("Explosion", "explosion.png");
            NewImage("Splash", "splash.png");
        }

        private static void LoadSounds()
        {
            NewSound("Error", "error.wav");
            NewSound("Hit", "hit1.wav");
            NewSound("Sink", "sink1.wav");
            NewSound("Siren", "siren.wav");
            NewSound("Miss", "shotfired.wav");
            NewSound("Winner", "win.wav");
            NewSound("Lose", "dead.wav");
        }

        private static void LoadMusic()
        {
            NewMusic("Background", "ingamemusic.wav");
        }

        /// <summary>
        ///     ''' Gets a Font Loaded in the Resources
        ///     ''' </summary>
        ///     ''' <param name="font">Name of Font</param>
        ///     ''' <returns>The Font Loaded with this Name</returns>
        public static Font GameFont(string font)
        {
            return _Fonts[font];
        }

        /// <summary>
        ///     ''' Gets an Image loaded in the Resources
        ///     ''' </summary>
        ///     ''' <param name="image">Name of image</param>
        ///     ''' <returns>The image loaded with this name</returns>

        public static Bitmap GameImage(string image)
        {
            return _Images[image];
        }

        /// <summary>
        ///     ''' Gets an sound loaded in the Resources
        ///     ''' </summary>
        ///     ''' <param name="sound">Name of sound</param>
        ///     ''' <returns>The sound with this name</returns>

        public static SoundEffect GameSound(string sound)
        {
            return _Sounds[sound];
        }

        /// <summary>
        ///     ''' Gets the music loaded in the Resources
        ///     ''' </summary>
        ///     ''' <param name="music">Name of music</param>
        ///     ''' <returns>The music with this name</returns>

        public static Music GameMusic(string music)
        {
            return _Music[music];
        }

        public static Dictionary<string, Bitmap> _Images = new Dictionary<string, Bitmap>();
        private static Dictionary<string, Font> _Fonts = new Dictionary<string, Font>();
        private static Dictionary<string, SoundEffect> _Sounds = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, Music> _Music = new Dictionary<string, Music>();

        private static Bitmap _Background;
        private static Bitmap _Animation;
        private static Bitmap _LoaderFull;
        private static Bitmap _LoaderEmpty;
        private static Font _LoadingFont;
        private static SoundEffect _StartSound;

        /// <summary>
        ///     ''' The Resources Class stores all of the Games Media Resources, such as Images, Fonts
        ///     ''' Sounds, Music.
        ///     ''' </summary>

        public static void LoadResources()
        {
            int width, height;

            width = SwinGame.ScreenWidth();
            height = SwinGame.ScreenHeight();

            SwinGame.ChangeScreenSize(800, 600);

            ShowLoadingScreen();

            ShowMessage("Loading fonts...", 0);
            LoadFonts();
            SwinGame.Delay(100);

            ShowMessage("Loading images...", 1);
            LoadImages();
            SwinGame.Delay(100);

            ShowMessage("Loading sounds...", 2);
            LoadSounds();
            SwinGame.Delay(100);

            ShowMessage("Loading music...", 3);
            LoadMusic();
            SwinGame.Delay(100);

            SwinGame.Delay(100);
            ShowMessage("Game loaded...", 5);
            SwinGame.Delay(100);
            EndLoadingScreen(width, height);
        }

        /// <summary>
        ///     ''' Shows the loading screen
        ///     ''' </summary>
        private static void ShowLoadingScreen()
        {
            if (UserTheme.IsThemeFileEmpty())
                _Background = SwinGame.LoadBitmap(SwinGame.PathToResource("SplashBack1.png", ResourceKind.BitmapResource));
            else
            {
                if (UserTheme.IsFileExist("images\\SplashBack1.png"))
                    _Background = SwinGame.LoadBitmap(SwinGame.PathToResource("SplashBack1.png", "Themes\\images"));
                else
                    _Background = SwinGame.LoadBitmap(SwinGame.PathToResource("SplashBack1.png", ResourceKind.BitmapResource));
            }
            SwinGame.DrawBitmap(_Background, 0, 0);
            SwinGame.RefreshScreen();
            SwinGame.ProcessEvents();

            if (UserTheme.IsThemeFileEmpty())
            {
                _Animation = SwinGame.LoadBitmap(SwinGame.PathToResource("SwinGameAni1.jpg", ResourceKind.BitmapResource));
                _LoadingFont = SwinGame.LoadFont(SwinGame.PathToResource("arial.ttf", ResourceKind.FontResource), 12);
                _StartSound = Audio.LoadSoundEffect(SwinGame.PathToResource("SwinGameStart.ogg", ResourceKind.SoundResource));

                _LoaderFull = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_full.png", ResourceKind.BitmapResource));
                _LoaderEmpty = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_empty.png", ResourceKind.BitmapResource));
            }
            else
            {
                if(UserTheme.IsFileExist("images\\SwinGameAni1.jpg"))
                    _Animation = SwinGame.LoadBitmap(SwinGame.PathToResource("SwinGameAni1.jpg", "Themes\\images"));
                else
                    _Animation = SwinGame.LoadBitmap(SwinGame.PathToResource("SwinGameAni1.jpg", ResourceKind.BitmapResource));

                if (UserTheme.IsFileExist("fonts\\arial.ttf"))
                    _LoadingFont = SwinGame.LoadFont(SwinGame.PathToResource("arial.ttf", "Themes\\fonts"), 12);
                else
                    _LoadingFont = SwinGame.LoadFont(SwinGame.PathToResource("arial.ttf", ResourceKind.FontResource), 12);

                if (UserTheme.IsFileExist("sounds\\SwinGameStart.ogg"))
                    _StartSound = Audio.LoadSoundEffect(SwinGame.PathToResource("SwinGameStart.ogg", "Themes\\sounds"));
                else
                    _StartSound = Audio.LoadSoundEffect(SwinGame.PathToResource("SwinGameStart.ogg", ResourceKind.SoundResource));

                if (UserTheme.IsFileExist("images\\loader_full.png"))
                    _LoaderFull = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_full.png", "Themes\\images"));
                else
                    _LoaderFull = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_full.png", ResourceKind.BitmapResource));

                if (UserTheme.IsFileExist("images\\loader_empty.png"))
                    _LoaderEmpty = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_empty.png", "Themes\\images"));
                else
                    _LoaderEmpty = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_empty.png", ResourceKind.BitmapResource));
            }

            PlaySwinGameIntro();
        }

        /// <summary>
        /// Plays the SwinGame Intro before Game Starts
        /// </summary>

        private static void PlaySwinGameIntro()
        {
            const int ANI_X = 143;
            const int ANI_Y = 134;
            const int ANI_W = 546;
            const int ANI_H = 327;
            const int ANI_V_CELL_COUNT = 6;
            const int ANI_CELL_COUNT = 11;

            Audio.PlaySoundEffect(_StartSound);
            SwinGame.Delay(200);

            int i;
            for (i = 0; i <= ANI_CELL_COUNT - 1; i++)
            {
                SwinGame.DrawBitmap(_Background, 0, 0);
                SwinGame.DrawBitmapPart(_Animation, (i / ANI_V_CELL_COUNT) * ANI_W, (i % ANI_V_CELL_COUNT) * ANI_H, ANI_W, ANI_H, ANI_X, ANI_Y);
                SwinGame.Delay(20);
                SwinGame.RefreshScreen();
                SwinGame.ProcessEvents();
            }

            SwinGame.Delay(1500);
        }

        /// <summary>
        /// Shows the messages
        /// </summary>
        /// <param name="message">The message to be shown</param>
        /// <param name="number">Length of the message</param>

        private static void ShowMessage(string message, int number)
        {
            const int TX = 310;
            const int TY = 493;
            const int TW = 200;
            const int TH = 25;
            const int STEPS = 5;
            const int BG_X = 279;
            const int BG_Y = 453;

            int fullW;

            fullW = 260 * number / STEPS;
            SwinGame.DrawBitmap(_LoaderEmpty, BG_X, BG_Y);
            SwinGame.DrawBitmapPart(_LoaderFull, 0, 0, fullW, 66, BG_X, BG_Y);

            SwinGame.DrawTextLines(message, Color.White, Color.Transparent, _LoadingFont, FontAlignment.AlignCenter, TX, TY, TW, TH);

            SwinGame.RefreshScreen();
            SwinGame.ProcessEvents();
        }

        /// <summary>
        /// Ending loading screen
        /// </summary>
        /// <param name="width">Width of window</param>
        /// <param name="height">Height of window</param>

        private static void EndLoadingScreen(int width, int height)
        {
            SwinGame.ProcessEvents();
            SwinGame.Delay(500);
            SwinGame.ClearScreen();
            SwinGame.RefreshScreen();
            SwinGame.FreeFont(_LoadingFont);
            SwinGame.FreeBitmap(_Background);
            SwinGame.FreeBitmap(_Animation);
            SwinGame.FreeBitmap(_LoaderEmpty);
            SwinGame.FreeBitmap(_LoaderFull);
            Audio.FreeSoundEffect(_StartSound);
            SwinGame.ChangeScreenSize(width, height);
        }

        /// <summary>
        /// New fonts to be used in game
        /// </summary>
        /// <param name="fontName">Font name</param>
        /// <param name="filename">Location of font file</param>
        /// <param name="size">Size of font file</param>

        private static void NewFont(string fontName, string filename, int size)
        {
            if (UserTheme.IsThemeFileEmpty())
            {
                _Fonts.Add(fontName, SwinGame.LoadFont(SwinGame.PathToResource(filename, ResourceKind.FontResource), size));
            }
            else
            {
                if (UserTheme.IsFileExist("fonts\\" + filename))
                    _Fonts.Add(fontName, SwinGame.LoadFont(SwinGame.PathToResource(filename, "Themes\\fonts"), size));
                else
                    _Fonts.Add(fontName, SwinGame.LoadFont(SwinGame.PathToResource(filename, ResourceKind.FontResource), size));
            }
        }

        /// <summary>
        /// New images to be used in game
        /// </summary>
        /// <param name="imageName">Image name</param>
        /// <param name="filename">Name of the file</param>

        public static void NewImage(string imageName, string filename)
        {
            if (UserTheme.IsThemeFileEmpty())
            {
                _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(filename, ResourceKind.BitmapResource)));
            }
            else
            {
                if (UserTheme.IsFileExist("images\\" + filename))
                    _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(filename, "Themes\\images")));
                else
                    _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(filename, ResourceKind.BitmapResource)));
            }
        }

        /// <summary>
        /// New Transparent Color Image to be used in game
        /// </summary>
        /// <param name="imageName">Image name</param>
        /// <param name="filename">Name of the file</param>
        /// <param name="transColor">Color of transparency</param>

        private static void NewTransparentColorImage(string imageName, string fileName, Color transColor)
        {
            if (UserTheme.IsThemeFileEmpty())
            {
                _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, ResourceKind.BitmapResource), true, transColor));
            }
            else
            {
                if (UserTheme.IsFileExist("images\\" + fileName))
                    _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, "Themes\\images"), true, transColor));
                else
                    _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, ResourceKind.BitmapResource), true, transColor));
            }
        }

        /// <summary>
        /// Creating new object of its own
        /// </summary>
        /// <param name="imageName">Image name</param>
        /// <param name="filename">Name of the file</param>
        /// <param name="transColor">Color of transparency</param>

        private static void NewTransparentColourImage(string imageName, string fileName, Color transColor)
        {
            NewTransparentColorImage(imageName, fileName, transColor);
        }

        /// <summary>
        /// New Sound to be used in game
        /// </summary>
        /// <param name="soundName">Sound name</param>
        /// <param name="filename">Name of the file</param>

        private static void NewSound(string soundName, string filename)
        {
            if (UserTheme.IsThemeFileEmpty())
                _Sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
            else
            {
                if (UserTheme.IsFileExist("sounds\\" + filename))
                    _Sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(filename, "Themes\\sounds")));
                else
                    _Sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
            }

        }

        /// <summary>
        /// New Sound to be used in game
        /// </summary>
        /// <param name="soundName">Sound name</param>
        /// <param name="filename">Name of the file</param>

        private static void NewMusic(string musicName, string filename)
        {
            if (UserTheme.IsThemeFileEmpty())
                _Music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
            else
            {
                if (UserTheme.IsFileExist("sounds\\" + filename))
                    _Music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(filename, "Themes\\sounds")));
                else
                    _Music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
            }
        }

        /// <summary>
        /// Unload fonts from memory
        /// </summary>

        private static void FreeFonts()
        {
            foreach (Font obj in _Fonts.Values)
                SwinGame.FreeFont(obj);
        }

        /// <summary>
        /// Unload images from memory
        /// </summary>

        private static void FreeImages()
        {
            foreach (Bitmap obj in _Images.Values)
                SwinGame.FreeBitmap(obj);
        }

        /// <summary>
        /// Unload sounds from memory
        /// </summary>

        private static void FreeSounds()
        {
            foreach (SoundEffect obj in _Sounds.Values)
                Audio.FreeSoundEffect(obj);
        }

        /// <summary>
        /// Unload musics from memory
        /// </summary>

        private static void FreeMusic()
        {
            foreach (Music obj in _Music.Values)
                Audio.FreeMusic(obj);
        }

        /// <summary>
        /// Unload all the resources in just one method
        /// </summary>

        public static void FreeResources()
        {
            FreeFonts();
            FreeImages();
            FreeMusic();
            FreeSounds();
            SwinGame.ProcessEvents();
        }
    }
}