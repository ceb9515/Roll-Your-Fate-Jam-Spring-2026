using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GameJam
{
    // Joshua Smith and Charlie Besgen
    // 03/21/2026
    //
    // Our team was not able to make it, but we stay absolutely dialed and we're gonna make a game anyways!!!
    public class Game1 : Game
    {
        #region Utils
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _rt;
        private Random rng;
        #endregion

        #region Input

        Point mScaled;
        MouseState ms;
        MouseState pms;

        #endregion

        enum GameState
        {
            Menu,
            Game,
            ChooseDice,
            GameOver,
        }
        GameState gameState;

        private Rectangle screen;
        private Rectangle backdrop;
        private Rectangle hunkusFrame;
        private Rectangle hunkusPortraitFrame;
        private Rectangle logoframe;
        private Vector2 totalDrawVec;
        private Vector2 targetDrawVec;
        private Vector2 roundDrawVec;
        private Vector2 leftOrigin;
        private Vector2 rightOrigin;
        private Vector2 highestRollDrawVec;
        private Vector2 furthestRoundDrawVec;

        #region Buttons
        private Button rollButton;
        private Button skipButton;
        private Button playButton;
        private Button quitButton;
        #endregion

        private bool rolled;
        private int rollTotal;
        private int targetScore;
        private readonly List<Dice> dice = [];
        private Bag bag;
        private Dice left;
        private Dice right;
        private double rollTimer;
        private int galaxyTicker;
        private int highestRoundRoll;
        private int highestRoll;
        private int furthestRound;
        private int round;
        private FileStream output;

        private SpriteFont diceText;
        private SpriteFont largeText;
        private Texture2D tabletop;
        private Texture2D hunkus;
        private Texture2D hunkusPortrait;
        private Texture2D logo;
        #region Standard
        private Dice standardD20;
        private Dice standardD12;
        private Dice standardD8;
        private Dice standardD6;
        private Dice standardD4;
        #endregion
        #region Sprinkle
        private Dice sprinkleD20;
        private Dice sprinkleD12;
        private Dice sprinkleD8;
        private Dice sprinkleD6;
        private Dice sprinkleD4;
        #endregion
        #region Fickle
        private Dice fickleD20;
        private Dice fickleD12;
        private Dice fickleD8;
        private Dice fickleD6;
        private Dice fickleD4;
        #endregion
        #region Avalanche
        private Dice avalancheD20;
        private Dice avalancheD12;
        private Dice avalancheD8;
        private Dice avalancheD6;
        private Dice avalancheD4;
        #endregion
        #region Danger
        private Dice dangerD20;
        private Dice dangerD12;
        private Dice dangerD8;
        private Dice dangerD6;
        private Dice dangerD4;
        #endregion
        #region Space
        private Dice spaceD20;
        private Dice spaceD12;
        private Dice spaceD8;
        private Dice spaceD6;
        private Dice spaceD4;
        #endregion
        #region Galaxy
        private Dice galaxyD20;
        private Dice galaxyD12;
        private Dice galaxyD8;
        private Dice galaxyD6;
        private Dice galaxyD4;
        #endregion
        #region Advantage
        private Dice advantageD20;
        private Dice advantageD12;
        private Dice advantageD8;
        private Dice advantageD6;
        private Dice advantageD4;
        #endregion
        private Song backgroundTrack;
        private SoundEffect[] rollNoises;
        private SoundEffect[] goodLines;
        private SoundEffect[] badLines;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            #region Resolution
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            Window.IsBorderless = true;
            #endregion;
        }

        protected override void Initialize()
        {
            _rt = new RenderTarget2D(GraphicsDevice, 640, 360);
            rng = new();

            gameState = GameState.Menu;

            rolled = false;
            rollTotal = 0;
            highestRoundRoll = 0;
            targetScore = 15;
            rollTimer = 3.0;
            bag = new();
            round = 1;
            rollNoises = new SoundEffect[5];
            goodLines = new SoundEffect[4];
            badLines = new SoundEffect[4];

            // Load saved data.
            if (File.Exists($"SaveData"))
            {
                FileStream fs = File.OpenRead($"SaveData");
                BinaryReader br = new(fs);
                try
                {
                    highestRoll = br.ReadInt32();
                    furthestRound = br.ReadInt32();
                }
                catch
                {
                    highestRoll = 0;
                    furthestRound = 1;
                }
                finally
                {
                    br.Close();
                }
            }
            else
            {
                FileStream fs = File.Create($"SaveData");
                BinaryWriter bw = new(fs);
                highestRoll = 0;
                furthestRound = 1;
                bw.Write(0);
                bw.Write(1);
                bw.Close();
            }

            screen = new(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            backdrop = new(0, 0, 640, 360);
            hunkusFrame = new(512, 0, 128, 128);
            hunkusPortraitFrame = new(0, 100, 192, 248);
            logoframe = new(180, 30, 360, 180);
            totalDrawVec = new(435, 205);
            targetDrawVec = new(335, 45);
            roundDrawVec = new(335, 5);
            leftOrigin = new(80, 90);
            rightOrigin = new(430, 90);
            highestRollDrawVec = new(180, 250);
            furthestRoundDrawVec = new(180, 310);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            diceText = Content.Load<SpriteFont>($"MediumJersey10");
            largeText = Content.Load<SpriteFont>($"LargeJersey10");
            tabletop = Content.Load<Texture2D>($"Tabletop");
            hunkus = Content.Load<Texture2D>($"Hunkus");
            hunkusPortrait = Content.Load<Texture2D>($"HunkusPortrait");
            logo = Content.Load<Texture2D>($"logo");
            #region Standard
            standardD20 = new(Dice.SpecialEffect.None, Content.Load<Texture2D>($"Dice/standardD20"), new(80, 200, 64, 64), 20, $"Standard D20", $"Just a regular die!");
            standardD12 = new(Dice.SpecialEffect.None, Content.Load<Texture2D>($"Dice/standardD12"), new(150, 200, 64, 64), 12, $"Standard D12", $"Just a regular die!");
            standardD8 = new(Dice.SpecialEffect.None, Content.Load<Texture2D>($"Dice/standardD8"), new(220, 200, 64, 64), 8, $"Standard D8", $"Just a regular die!");
            standardD6 = new(Dice.SpecialEffect.None, Content.Load<Texture2D>($"Dice/standardD6"), new(290, 200, 64, 64), 6, $"Standard D6", $"Just a regular die!");
            standardD4 = new(Dice.SpecialEffect.None, Content.Load<Texture2D>($"Dice/standardD4"), new(360, 200, 64, 64), 4, $"Standard D4", $"Just a regular die!");
            #endregion
            #region Sprinkle
            sprinkleD20 = new(Dice.SpecialEffect.Sprinkle, Content.Load<Texture2D>($"Dice/sprinkleD20"), new(80, 200, 64, 64), 20, $"Sprinkle D20", $"20% chance to add\n   15 to your roll!");
            sprinkleD12 =new(Dice.SpecialEffect.Sprinkle, Content.Load<Texture2D>($"Dice/sprinkleD12"), new(150, 200, 64, 64), 12, $"Sprinkle D12", $"20% chance to add\n   15 to your roll!");
            sprinkleD8 =   new(Dice.SpecialEffect.Sprinkle, Content.Load<Texture2D>($"Dice/sprinkleD8"), new(220, 200, 64, 64), 8, $"Sprinkle D8", $"20% chance to add\n   15 to your roll!");
            sprinkleD6 =   new(Dice.SpecialEffect.Sprinkle, Content.Load<Texture2D>($"Dice/sprinkleD6"), new(290, 200, 64, 64), 6, $"Sprinkle D6", $"20% chance to add\n   15 to your roll!");
            sprinkleD4 =   new(Dice.SpecialEffect.Sprinkle, Content.Load<Texture2D>($"Dice/sprinkleD4"), new(360, 200, 64, 64), 4, $"Sprinkle D4", $"20% chance to add\n   15 to your roll!");
            #endregion
            #region Fickle
            fickleD20 = new(Dice.SpecialEffect.Fickle, Content.Load<Texture2D>($"Dice/fickleD20"), new(80, 200, 64, 64), 20, $"Fickle D20", $" Odd numbers increased by 8,\neven numbers decreased by 8.");
            fickleD12 = new(Dice.SpecialEffect.Fickle, Content.Load<Texture2D>($"Dice/fickleD12"), new(150, 200, 64, 64), 12, $"Fickle D12", $" Odd numbers increased by 8,\neven numbers decreased by 8.");
            fickleD8 =  new(Dice.SpecialEffect.Fickle, Content.Load<Texture2D>($"Dice/fickleD8"), new(220, 200, 64, 64), 8, $"Fickle D8", $" Odd numbers increased by 8,\neven numbers decreased by 8.");
            fickleD6 =  new(Dice.SpecialEffect.Fickle, Content.Load<Texture2D>($"Dice/fickleD6"), new(290, 200, 64, 64), 6, $"Fickle D6", $" Odd numbers increased by 8,\neven numbers decreased by 8.");
            fickleD4 =  new(Dice.SpecialEffect.Fickle, Content.Load<Texture2D>($"Dice/fickleD4"), new(360, 200, 64, 64), 4, $"Fickle D4", $" Odd numbers increased by 8,\neven numbers decreased by 8.");
            #endregion
            #region Avalanche
            avalancheD20 = new(Dice.SpecialEffect.Avalanche, Content.Load<Texture2D>($"Dice/avalancheD20"), new(80, 200, 64, 64), 20, $"Avalanche D20", $"Adds the values of all\n   dice to the left.");
            avalancheD12 = new(Dice.SpecialEffect.Avalanche, Content.Load<Texture2D>($"Dice/avalancheD12"), new(150, 200, 64, 64), 12, $"Avalanche D12", $"Adds the values of all\n   dice to the left.");
            avalancheD8 = new(Dice.SpecialEffect.Avalanche, Content.Load<Texture2D>($"Dice/avalancheD8"), new(220, 200, 64, 64), 8, $"Avalanche D8", $"Adds the values of all\n   dice to the left.");
            avalancheD6 = new(Dice.SpecialEffect.Avalanche, Content.Load<Texture2D>($"Dice/avalancheD6"), new(290, 200, 64, 64), 6, $"Avalanche D6", $"Adds the values of all\n   dice to the left.");
            avalancheD4 = new(Dice.SpecialEffect.Avalanche, Content.Load<Texture2D>($"Dice/avalancheD4"), new(360, 200, 64, 64), 4, $"Avalanche D4", $"Adds the values of all\n   dice to the left.");
            #endregion
            #region Danger
            dangerD20 = new(Dice.SpecialEffect.Danger, Content.Load<Texture2D>($"Dice/dangerD20"), new(80, 200, 64, 64), 20, $"Danger D20", $"    Triples your roll\n10% chance to negate it");
            dangerD12 = new(Dice.SpecialEffect.Danger, Content.Load<Texture2D>($"Dice/dangerD12"), new(150, 200, 64, 64), 12, $"Danger D12", $"    Triples your roll\n10% chance to negate it");
            dangerD8 = new(Dice.SpecialEffect.Danger, Content.Load<Texture2D>($"Dice/dangerD8"), new(220, 200, 64, 64), 8, $"Danger D8", $"    Triples your roll\n10% chance to negate it");
            dangerD6 = new(Dice.SpecialEffect.Danger, Content.Load<Texture2D>($"Dice/dangerD6"), new(290, 200, 64, 64), 6, $"Danger D6", $"    Triples your roll\n10% chance to negate it");
            dangerD4 = new(Dice.SpecialEffect.Danger, Content.Load<Texture2D>($"Dice/dangerD4"), new(360, 200, 64, 64), 4, $"Danger D4", $"    Triples your roll\n10% chance to negate it");
            #endregion
            #region Space
            spaceD20 = new(Dice.SpecialEffect.Space, Content.Load<Texture2D>($"Dice/spaceD20"), new(80, 200, 64, 64), 20, $"Space D20", $"Doubles your roll.");
            spaceD12 = new(Dice.SpecialEffect.Space, Content.Load<Texture2D>($"Dice/spaceD12"), new(150, 200, 64, 64), 12, $"Space D12", $"Doubles your roll.");
            spaceD8 = new(Dice.SpecialEffect.Space, Content.Load<Texture2D>($"Dice/spaceD8"), new(220, 200, 64, 64), 8, $"Space D8", $"Doubles your roll.");
            spaceD6 = new(Dice.SpecialEffect.Space, Content.Load<Texture2D>($"Dice/spaceD6"), new(290, 200, 64, 64), 6, $"Space D6", $"Doubles your roll.");
            spaceD4 = new(Dice.SpecialEffect.Space, Content.Load<Texture2D>($"Dice/spaceD4"), new(360, 200, 64, 64), 4, $"Space D4", $"Doubles your roll.");
            #endregion
            #region Galaxy
            galaxyD20 = new(Dice.SpecialEffect.Galaxy, Content.Load<Texture2D>($"Dice/galaxyD20"), new(80, 200, 64, 64), 20, $"Galaxy D20", $"Quadruples all dice.");
            galaxyD12 = new(Dice.SpecialEffect.Galaxy, Content.Load<Texture2D>($"Dice/galaxyD12"), new(150, 200, 64, 64), 12, $"Galaxy D12", $"Quadruples all dice.");
            galaxyD8 = new(Dice.SpecialEffect.Galaxy, Content.Load<Texture2D>($"Dice/galaxyD8"), new(220, 200, 64, 64), 8, $"Galaxy D8", $"Quadruples all dice.");
            galaxyD6 = new(Dice.SpecialEffect.Galaxy, Content.Load<Texture2D>($"Dice/galaxyD6"), new(290, 200, 64, 64), 6, $"Galaxy D6", $"Quadruples all dice.");
            galaxyD4 = new(Dice.SpecialEffect.Galaxy, Content.Load<Texture2D>($"Dice/galaxyD4"), new(360, 200, 64, 64), 4, $"Galaxy D4", $"Quadruples all dice.");
            #endregion
            #region Advantage
            advantageD20 = new(Dice.SpecialEffect.Advantage, Content.Load<Texture2D>($"Dice/advantageD20"), new(80, 200, 64, 64), 20, $"Advantage D20", $"Takes the better of\n      two rolls.");
            advantageD12 = new(Dice.SpecialEffect.Advantage, Content.Load<Texture2D>($"Dice/advantageD12"), new(150, 200, 64, 64), 12, $"Advantage D12", $"Takes the better of\n      two rolls.");
            advantageD8 = new(Dice.SpecialEffect.Advantage, Content.Load<Texture2D>($"Dice/advantageD8"), new(220, 200, 64, 64), 8, $"Advantage D8", $"Takes the better of\n      two rolls.");
            advantageD6 = new(Dice.SpecialEffect.Advantage, Content.Load<Texture2D>($"Dice/advantageD6"), new(290, 200, 64, 64), 6, $"Advantage D6", $"Takes the better of\n      two rolls.");
            advantageD4 = new(Dice.SpecialEffect.Advantage, Content.Load<Texture2D>($"Dice/advantageD4"), new(360, 200, 64, 64), 4, $"Advantage D4", $"Takes the better of\n      two rolls.");
            #endregion

            rollButton = new($"Roll!", largeText, new(30, 280), new(30, 280, 63, 40));
            skipButton = new($"Skip!", largeText, new(270, 290), new(270, 280, 74, 55));
            playButton = new($"Play!", largeText, new(500, 200), new(500, 200, 70, 45));
            quitButton = new($"Quit!", largeText, new(500, 270), new(500, 270, 70, 45));

            backgroundTrack = Content.Load<Song>($"spaaaace");
            for(int i = 0; i < 5; i++)
            {
                rollNoises[i] = Content.Load<SoundEffect>($"SoundEffects/diceRoll{i + 1}");
            }
            for(int i = 0; i < 4; i++)
            {
                goodLines[i] = Content.Load<SoundEffect>($"SoundEffects/Good{i + 1}");
                badLines[i] = Content.Load<SoundEffect>($"SoundEffects/Bad{i + 1}");
            }

            dice.Add(standardD20);
            dice.Add(standardD12);
            dice.Add(standardD8);
            dice.Add(standardD6);
            dice.Add(standardD4);

            bag.AddCommon(sprinkleD20, sprinkleD12, sprinkleD8, sprinkleD6, sprinkleD4, fickleD20, fickleD12, fickleD8, fickleD6, fickleD4, advantageD20, advantageD12, advantageD8, advantageD6, advantageD4);
            bag.AddUncommon(dangerD20, dangerD12, dangerD8, dangerD6, dangerD4, spaceD20, spaceD12, spaceD8, spaceD6, spaceD4);
            bag.AddRare(avalancheD20, avalancheD12, avalancheD8, avalancheD6, avalancheD4);
            bag.AddLegendary(galaxyD20, galaxyD12, galaxyD8, galaxyD6, galaxyD4);

            #region Music
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(backgroundTrack);
            MediaPlayer.IsRepeating = true;
            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit(); }
            ms = Mouse.GetState();
            mScaled = new((int)(ms.X * (640.0f / _graphics.PreferredBackBufferWidth)), (int)(ms.Y * (360.0f / _graphics.PreferredBackBufferHeight)));

            switch(gameState)
            {
                case GameState.Menu:

                    if(playButton.Update(ms, pms, mScaled))
                    {
                        gameState = GameState.Game;
                    }

                    if(quitButton.Update(ms, pms, mScaled))
                    {
                        Exit();
                    }

                    break;

                case GameState.Game:

                    if(rollButton.Update(ms, pms, mScaled) && !rolled)
                    {
                        rollNoises[rng.Next(0, 5)].Play(1.0f, 0.0f, 0.0f);

                        rolled = true;
                        rollTimer = 3.0;

                        foreach(Dice die in dice)
                        {
                            die.Roll();
                            
                            // Avalanche functionality.
                            if(die.Special == Dice.SpecialEffect.Avalanche)
                            {
                                die.Value += rollTotal;
                            }

                            // Galaxy functionality P1.
                            else if(die.Special == Dice.SpecialEffect.Galaxy)
                            {
                                galaxyTicker++;
                            }

                            rollTotal += die.Value;
                        }

                        // Galaxy functionality P2.
                        if(galaxyTicker > 0)
                        {
                            rollTotal = 0;
                            for(int i = 0; i < galaxyTicker; i++)
                            {
                                foreach(Dice die in dice)
                                {
                                    die.Value *= 4;
                                }
                            }
                            galaxyTicker = 0;
                            foreach(Dice die in dice)
                            {
                                rollTotal += die.Value;
                            }
                        }

                        // Double score on Nat 20.
                        if(dice[0].Max)
                        {
                            PlayVoiceLine(true);
                            rollTotal *= 2;
                        }

                        // Half score on Nat 1.
                        else if(dice[0].Min)
                        {
                            PlayVoiceLine(false);
                            rollTotal /= 2;
                        }

                        // Save highest roll this round.
                        if(rollTotal > highestRoundRoll)
                        {
                            highestRoundRoll = rollTotal;
                        }
                    }

                    if(rolled)
                    {
                        if(rollTimer <= 0)
                        {
                            rolled = false;

                            if(rng.Next(1, 6) == 1)
                            {
                                PlayVoiceLine(true);
                            }

                            if(rollTotal >= targetScore)
                            {
                                rollTotal = 0;
                                round++;
                                targetScore = (int)(targetScore * 1.25);

                                // Reset dice values.
                                foreach(Dice die in dice)
                                {
                                    die.Value = -1000;
                                }

                                left = bag.Pull();
                                right = bag.Pull();
                                gameState = GameState.ChooseDice;
                            }

                            // They lost.
                            else
                            {
                                PlayVoiceLine(false);
                                SaveData();
                                Reset();
                                gameState = GameState.Menu;
                            }
                        }

                        // Decrement timer.
                        else
                        {
                            rollTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                        }
                    }

                    break;

                case GameState.ChooseDice:

                    if(left.Clicked(leftOrigin, SingleClick(), mScaled))
                    {
                        left.Value = -1000;

                        // Assigns it to the right dice slot.
                        switch(left.MaxValue)
                        {
                            case 20:
                                dice[0].Value = -1000;
                                dice[0] = left;
                                break;

                            case 12:
                                dice[1].Value = -1000;
                                dice[1] = left;
                                break;

                            case 8:
                                dice[2].Value = -1000;
                                dice[2] = left;
                                break;

                            case 6:
                                dice[3].Value = -1000;
                                dice[3] = left;
                                break;

                            case 4:
                                dice[4].Value = -1000;
                                dice[4] = left;
                                break;
                        }

                        gameState = GameState.Game;
                    }

                    else if(right.Clicked(rightOrigin, SingleClick(), mScaled))
                    {
                        right.Value = -1000;

                        // Assigns it to the right dice slot.
                        switch(right.MaxValue)
                        {
                            case 20:
                                dice[0].Value = -1000;
                                dice[0] = right;
                                break;

                            case 12:
                                dice[1].Value = -1000;
                                dice[1] = right;
                                break;

                            case 8:
                                dice[2].Value = -1000;
                                dice[2] = right;
                                break;

                            case 6:
                                dice[3].Value = -1000;
                                dice[3] = right;
                                break;

                            case 4:
                                dice[4].Value = -1000;
                                dice[4] = right;
                                break;
                        }

                        gameState = GameState.Game;
                    }

                    // Allows player to skip choosing dice.
                    if(skipButton.Update(ms, pms, mScaled))
                    {
                        gameState = GameState.Game;
                    }

                    break;
            }

            pms = ms;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            #region Begin Draw
            GraphicsDevice.SetRenderTarget(_rt);
            GraphicsDevice.Clear(Color.Purple);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            #endregion


            switch(gameState)
            {
                case GameState.Menu:
                    // Backdrop!!!
                    _spriteBatch.Draw(tabletop, backdrop, Color.White);

                    _spriteBatch.Draw(hunkusPortrait, hunkusPortraitFrame, Color.White);
                    _spriteBatch.Draw(logo, logoframe, Color.White);

                    playButton.Draw(_spriteBatch);
                    quitButton.Draw(_spriteBatch);

                    _spriteBatch.DrawString(largeText, $"Highest Roll: {highestRoll}", highestRollDrawVec, Color.Gold);
                    _spriteBatch.DrawString(largeText, $"Furthest Round: {furthestRound}", furthestRoundDrawVec, Color.Gold);

                    break;

                case GameState.Game:

                    // Backdrop!!!
                    _spriteBatch.Draw(tabletop, backdrop, Color.White);

                    // Draw the dice.
                    foreach(Dice die in dice)
                    {
                        die.Draw(_spriteBatch, diceText, false);
                    }

                    // Draw the scores.
                    _spriteBatch.DrawString(largeText, $"Round: {round}", roundDrawVec, Color.LightPink);
                    _spriteBatch.DrawString(largeText, $"Total: {rollTotal}", totalDrawVec, Color.Gold);
                    _spriteBatch.DrawString(largeText, $"Target: {targetScore}", targetDrawVec, Color.Red);

                    _spriteBatch.Draw(hunkus, hunkusFrame, Color.White);

                    // Draw buttons.
                    if(!rolled)
                    {
                        rollButton.Draw(_spriteBatch);
                    }

                    break;

                case GameState.ChooseDice:

                    #region Grayed Out Backdrop
                    // Backdrop!!!
                    _spriteBatch.Draw(tabletop, backdrop, Color.Gray);

                    // Draw the dice.
                    foreach (Dice die in dice)
                    {
                        die.Draw(_spriteBatch, diceText, true);
                    }

                    // Draw the scores.
                    _spriteBatch.DrawString(largeText, $"Round: {round}", roundDrawVec, Color.Gray);
                    _spriteBatch.DrawString(largeText, $"Total: {rollTotal}", totalDrawVec, Color.Gray);
                    _spriteBatch.DrawString(largeText, $"Target: {targetScore}", targetDrawVec, Color.Gray);

                    _spriteBatch.Draw(hunkus, hunkusFrame, Color.Gray);
                    #endregion

                    left.DrawOption(_spriteBatch, diceText, leftOrigin);
                    right.DrawOption(_spriteBatch, diceText, rightOrigin);

                    skipButton.Draw(_spriteBatch);

                    break;
            }

            #region End Draw
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            base.Draw(gameTime);
            #endregion
            #region Exec Render Call
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_rt, screen, Color.White);
            _spriteBatch.End();
            #endregion
        }

        /// <summary>
        /// Resets the game.
        /// </summary>
        public void Reset()
        {
            rolled = false;
            targetScore = 15;
            rollTotal = 0;
            dice[0] = standardD20;
            dice[1] = standardD12;
            dice[2] = standardD8;
            dice[3] = standardD6;
            dice[4] = standardD4;
            round = 1;
            highestRoundRoll = 0;
            foreach(Dice die in dice)
            {
                die.Value = -1000;
            }
        }

        /// <summary>
        /// Checks to see if the player has clicked one time.
        /// </summary>
        /// <returns> Whether or not they've clicked one time. </returns>
        public bool SingleClick()
        {
            return ms.LeftButton == ButtonState.Pressed && pms.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Saves the player data.
        /// </summary>
        public void SaveData()
        {
            FileStream file = File.OpenWrite($"SaveData");
            BinaryWriter bw = new(file);
            try
            {
                // Highest Roll
                if(highestRoundRoll > highestRoll)
                {
                    highestRoll = highestRoundRoll;
                    bw.Write(highestRoundRoll);
                }
                else
                {
                    bw.Write(highestRoll);
                }

                // Round
                if(round > furthestRound)
                {
                    furthestRound = round;
                    bw.Write(round);
                }
                else
                {
                    bw.Write(furthestRound);
                }
            }
            catch
            {
                bw.Write(highestRoll);
                bw.Write(furthestRound);
            }
            finally
            {
                bw.Close();
            }
        }

        /// <summary>
        /// Plays a voice line.
        /// </summary>
        /// <param name="good"> Whether or not it's a good voice line. </param>
        public void PlayVoiceLine(bool good)
        {
            if(good)
            {
                goodLines[rng.Next(0, goodLines.Length)].Play(1.0f, 0.0f, 0.0f);
            }
            else
            {
                badLines[rng.Next(0, goodLines.Length)].Play(1.0f, 0.0f, 0.0f);
            }
        }
    }
}
