using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Collections.Generic;

namespace GameJam
{
    // Joshua Smith and Charlie Besgen
    // 03/21/2026
    //
    // Our team was not able to make it, but we stay absolutely dialed and we're gonna make a game anyways!!!
    public class Game1 : Game
    {
        // CURRENT VERSION INFO
        private readonly int currentVersion = 64203320;
        private readonly string versionDisplay = $"1.2";
        // CURRENT VERSION INFO

        #region Utils
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _rt;
        private Random rng;
        #endregion

        #region Input

        Point mScaled;
        MouseState ms;
        MouseState pms;
        KeyboardState kb;
        KeyboardState pkb;

        #endregion

        enum GameState
        {
            Menu,
            Game,
            ChooseDice,
            GameOver,
        }
        GameState gameState;

        enum VoiceLine
        {
            WonARound,
            Lose,
            NaturalTwenty,
            NaturalOne,
            EasterEgg1,
            EasterEgg2,
        }

        #region Draw Locations
        private Rectangle screen;
        private Rectangle backdrop;
        private Rectangle hunkusFrame;
        private Rectangle hunkusPortraitFrame;
        private Rectangle logoframe;
        private Vector2 totalDrawVec;
        private Vector2 skipButtonFlavorVec;
        private Vector2 targetDrawVec;
        private Vector2 roundDrawVec;
        private DiceChoice leftChoice;
        private DiceChoice centerChoice;
        private DiceChoice rightChoice;
        private Vector2 highestRollDrawVec;
        private Vector2 furthestRoundDrawVec;
        private Vector2 versionDrawVec;
        private Vector2 minMaxFlavorVec;
        #endregion

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
        private double rollTimer;
        private int galaxyTicker;
        private long highestRoundRoll;
        private long highestRoll;
        private int furthestRound;
        private int round;
        private bool multiplying;
        private bool frozen;
        private int versionCode;

        #region Assets
        private Color backDarken;
        private SpriteFont mediumText;
        private SpriteFont largeText;
        private SpriteFont smallText;
        private Texture2D tabletop;
        private Texture2D hunkus;
        // private Texture2D hunkusPortrait;
        // private Texture2D logo;
        private Texture2D diceDisplayFrame;
        private Texture2D hunkTitle;
        #region Dice
        #region V0
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
        #endregion
        #region V1
        #region Custom
        Dice golden;
        Dice fibonacci;
        Dice octopus;
        #endregion
        #region Rainbow
        private Dice rainbowD20;
        private Dice rainbowD12;
        private Dice rainbowD8;
        private Dice rainbowD6;
        private Dice rainbowD4;
        #endregion
        #region Flowered
        private Dice floweredD20;
        private Dice floweredD12;
        private Dice floweredD8;
        private Dice floweredD6;
        private Dice floweredD4;
        #endregion
        #region Yin Yang
        private Dice yinyangD20;
        private Dice yinyangD12;
        private Dice yinyangD8;
        private Dice yinyangD6;
        private Dice yinyangD4;
        #endregion
        #region Chalkboard
        private Dice chalkboardD20;
        private Dice chalkboardD12;
        private Dice chalkboardD8;
        private Dice chalkboardD6;
        private Dice chalkboardD4;
        #endregion
        #region Frozen
        private Dice frozenD20;
        private Dice frozenD12;
        private Dice frozenD8;
        private Dice frozenD6;
        private Dice frozenD4;
        #endregion
        #region Explosive
        private Dice explosiveD20;
        private Dice explosiveD12;
        private Dice explosiveD8;
        private Dice explosiveD6;
        private Dice explosiveD4;
        #endregion
        #region Rounder
        private Dice rounderD20;
        private Dice rounderD12;
        private Dice rounderD8;
        private Dice rounderD6;
        private Dice rounderD4;
        #endregion
        #region Weighted
        private Dice weightedD20;
        private Dice weightedD12;
        private Dice weightedD8;
        private Dice weightedD6;
        private Dice weightedD4;
        #endregion
        #region Trio
        private Dice trioD20;
        private Dice trioD12;
        private Dice trioD8;
        private Dice trioD6;
        private Dice trioD4;
        #endregion
        #endregion
        #endregion
        private Song backgroundTrack;
        private SoundEffect[] rollNoises;
        private SoundEffect[] goodLines;
        private SoundEffect[] badLines;
        #endregion

        /// <summary>
        /// The only constructor for a game.
        /// </summary>
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
            multiplying = false;
            frozen = false;
            rollNoises = new SoundEffect[5];
            goodLines = new SoundEffect[4];
            badLines = new SoundEffect[4];
            backDarken = new(50, 50, 50);

            // Load saved data.
            if(File.Exists($"SaveData"))
            {
                FileStream fs = File.OpenRead($"SaveData");
                BinaryReader br = new(fs);
                try
                {
                    versionCode = br.ReadInt32();
                    if(versionCode == currentVersion)
                    {
                        highestRoll = br.ReadInt64();
                        furthestRound = br.ReadInt32();
                    }
                    else
                    {
                        versionCode = currentVersion;
                        highestRoll = 0;
                        furthestRound = 1;
                    }
                }
                catch
                {
                    versionCode = currentVersion;
                    highestRoll = 0;
                    furthestRound = 1;
                }
                finally
                {
                    br.Close();
                }
            }

            // No save data, create file.
            else
            {
                FileStream fs = File.Create($"SaveData");
                BinaryWriter bw = new(fs);
                highestRoll = 0;
                furthestRound = 1;
                bw.Write(currentVersion);
                bw.Write(0);
                bw.Write(1);
                bw.Close();
            }

            #region Draw Locations
            screen = new(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            backdrop = new(0, 0, 640, 360);
            hunkusFrame = new(512, 0, 128, 128);
            // hunkusPortraitFrame = new(0, 100, 192, 248);
            // logoframe = new(180, 30, 360, 180);
            skipButtonFlavorVec = new(420, 290);
            totalDrawVec = new(435, 210);
            targetDrawVec = new(355, 25);
            roundDrawVec = new(355, 0);
            highestRollDrawVec = new(5, 320);
            furthestRoundDrawVec = new(5, 335);
            versionDrawVec = new(570, 342);
            minMaxFlavorVec = new(112, 274);
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Assets
            mediumText = Content.Load<SpriteFont>($"MediumJersey10");
            largeText = Content.Load<SpriteFont>($"LargeJersey10");
            smallText = Content.Load<SpriteFont>($"SmallJersey10");
            tabletop = Content.Load<Texture2D>($"Tabletop");
            hunkus = Content.Load<Texture2D>($"Hunkus");
            // hunkusPortrait = Content.Load<Texture2D>($"HunkusPortrait");
            // logo = Content.Load<Texture2D>($"logo");
            diceDisplayFrame = Content.Load<Texture2D>($"DiceDisplayFrame");
            hunkTitle = Content.Load<Texture2D>($"hunkTitle");
            leftChoice = new(new(67, 86), diceDisplayFrame, smallText);
            centerChoice = new(new(258, 86), diceDisplayFrame, smallText);
            rightChoice = new(new(449, 86), diceDisplayFrame, smallText);
            #endregion

            #region Dice
            #region V0
            #region Standard
            standardD20 = new(Dice.SpecialEffect.None, Content.Load<Texture2D>($"Dice/standardD20"), new(80, 200, 64, 64), 20, $"Standard D20", $"Just a regular die!");
            standardD12 = new(Dice.SpecialEffect.None, Content.Load<Texture2D>($"Dice/standardD12"), new(150, 200, 64, 64), 12, $"Standard D12", $"Just a regular die!");
            standardD8 = new(Dice.SpecialEffect.None, Content.Load<Texture2D>($"Dice/standardD8"), new(220, 200, 64, 64), 8, $"Standard D8", $"Just a regular die!");
            standardD6 = new(Dice.SpecialEffect.None, Content.Load<Texture2D>($"Dice/standardD6"), new(290, 200, 64, 64), 6, $"Standard D6", $"Just a regular die!");
            standardD4 = new(Dice.SpecialEffect.None, Content.Load<Texture2D>($"Dice/standardD4"), new(360, 200, 64, 64), 4, $"Standard D4", $"Just a regular die!");
            #endregion
            #region Sprinkle
            sprinkleD20 = new(Dice.SpecialEffect.Sprinkle, Content.Load<Texture2D>($"Dice/sprinkleD20"), new(80, 200, 64, 64), 20, $"Sprinkle D20", $"20% chance to add\n    15 to your roll!");
            sprinkleD12 =new(Dice.SpecialEffect.Sprinkle, Content.Load<Texture2D>($"Dice/sprinkleD12"), new(150, 200, 64, 64), 12, $"Sprinkle D12", $"20% chance to add\n    15 to your roll!");
            sprinkleD8 =   new(Dice.SpecialEffect.Sprinkle, Content.Load<Texture2D>($"Dice/sprinkleD8"), new(220, 200, 64, 64), 8, $"Sprinkle D8", $"20% chance to add\n    15 to your roll!");
            sprinkleD6 =   new(Dice.SpecialEffect.Sprinkle, Content.Load<Texture2D>($"Dice/sprinkleD6"), new(290, 200, 64, 64), 6, $"Sprinkle D6", $"20% chance to add\n    15 to your roll!");
            sprinkleD4 =   new(Dice.SpecialEffect.Sprinkle, Content.Load<Texture2D>($"Dice/sprinkleD4"), new(360, 200, 64, 64), 4, $"Sprinkle D4", $"20% chance to add\n    15 to your roll!");
            #endregion
            #region Fickle
            fickleD20 = new(Dice.SpecialEffect.Fickle, Content.Load<Texture2D>($"Dice/fickleD20"), new(80, 200, 64, 64), 20, $"Fickle D20", $"Even numbers are doubled,\n   everything else is zero.");
            fickleD12 = new(Dice.SpecialEffect.Fickle, Content.Load<Texture2D>($"Dice/fickleD12"), new(150, 200, 64, 64), 12, $"Fickle D12", $"Even numbers are doubled,\n   everything else is zero.");
            fickleD8 =  new(Dice.SpecialEffect.Fickle, Content.Load<Texture2D>($"Dice/fickleD8"), new(220, 200, 64, 64), 8, $"Fickle D8", $"Even numbers are doubled,\n   everything else is zero.");
            fickleD6 =  new(Dice.SpecialEffect.Fickle, Content.Load<Texture2D>($"Dice/fickleD6"), new(290, 200, 64, 64), 6, $"Fickle D6", $"Even numbers are doubled,\n   everything else is zero.");
            fickleD4 =  new(Dice.SpecialEffect.Fickle, Content.Load<Texture2D>($"Dice/fickleD4"), new(360, 200, 64, 64), 4, $"Fickle D4", $"Even numbers are doubled,\n   everything else is zero.");
            #endregion
            #region Avalanche
            avalancheD20 = new(Dice.SpecialEffect.Avalanche, Content.Load<Texture2D>($"Dice/avalancheD20"), new(80, 200, 64, 64), 20, $"Avalanche D20", $"Adds the values of all\n     dice to the left.");
            avalancheD12 = new(Dice.SpecialEffect.Avalanche, Content.Load<Texture2D>($"Dice/avalancheD12"), new(150, 200, 64, 64), 12, $"Avalanche D12", $"Adds the values of all\n     dice to the left.");
            avalancheD8 = new(Dice.SpecialEffect.Avalanche, Content.Load<Texture2D>($"Dice/avalancheD8"), new(220, 200, 64, 64), 8, $"Avalanche D8", $"Adds the values of all\n     dice to the left.");
            avalancheD6 = new(Dice.SpecialEffect.Avalanche, Content.Load<Texture2D>($"Dice/avalancheD6"), new(290, 200, 64, 64), 6, $"Avalanche D6", $"Adds the values of all\n     dice to the left.");
            avalancheD4 = new(Dice.SpecialEffect.Avalanche, Content.Load<Texture2D>($"Dice/avalancheD4"), new(360, 200, 64, 64), 4, $"Avalanche D4", $"Adds the values of all\n     dice to the left.");
            #endregion
            #region Danger
            dangerD20 = new(Dice.SpecialEffect.Danger, Content.Load<Texture2D>($"Dice/dangerD20"), new(80, 200, 64, 64), 20, $"Danger D20", $"      Triples your roll\n10% chance to negate it");
            dangerD12 = new(Dice.SpecialEffect.Danger, Content.Load<Texture2D>($"Dice/dangerD12"), new(150, 200, 64, 64), 12, $"Danger D12", $"      Triples your roll\n10% chance to negate it");
            dangerD8 = new(Dice.SpecialEffect.Danger, Content.Load<Texture2D>($"Dice/dangerD8"), new(220, 200, 64, 64), 8, $"Danger D8", $"      Triples your roll\n10% chance to negate it");
            dangerD6 = new(Dice.SpecialEffect.Danger, Content.Load<Texture2D>($"Dice/dangerD6"), new(290, 200, 64, 64), 6, $"Danger D6", $"      Triples your roll\n10% chance to negate it");
            dangerD4 = new(Dice.SpecialEffect.Danger, Content.Load<Texture2D>($"Dice/dangerD4"), new(360, 200, 64, 64), 4, $"Danger D4", $"      Triples your roll\n10% chance to negate it");
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
            advantageD20 = new(Dice.SpecialEffect.Advantage, Content.Load<Texture2D>($"Dice/advantageD20"), new(80, 200, 64, 64), 20, $"Advantage D20", $"Takes the better of\n        two rolls.");
            advantageD12 = new(Dice.SpecialEffect.Advantage, Content.Load<Texture2D>($"Dice/advantageD12"), new(150, 200, 64, 64), 12, $"Advantage D12", $"Takes the better of\n        two rolls.");
            advantageD8 = new(Dice.SpecialEffect.Advantage, Content.Load<Texture2D>($"Dice/advantageD8"), new(220, 200, 64, 64), 8, $"Advantage D8", $"Takes the better of\n        two rolls.");
            advantageD6 = new(Dice.SpecialEffect.Advantage, Content.Load<Texture2D>($"Dice/advantageD6"), new(290, 200, 64, 64), 6, $"Advantage D6", $"Takes the better of\n        two rolls.");
            advantageD4 = new(Dice.SpecialEffect.Advantage, Content.Load<Texture2D>($"Dice/advantageD4"), new(360, 200, 64, 64), 4, $"Advantage D4", $"Takes the better of\n        two rolls.");
            #endregion
            #endregion
            #region V1
            #region Custom
            golden = new(Dice.SpecialEffect.Golden, Content.Load<Texture2D>($"Dice/Custom/golden"), new(80, 200, 64, 64), 20, $"Golden D20", $"Nat 20s now multiply your score by 20!!!\n      Also, 1s are no longer penalized!");
            fibonacci = new(Dice.SpecialEffect.Fibonacci, Content.Load<Texture2D>($"Dice/Custom/fibonacci"), new(80, 200, 64, 64), 20, $"Fibonacci D20", $"Rolls with the Fibonacci Sequence");
            octopus = new(Dice.SpecialEffect.Octopus, Content.Load<Texture2D>($"Dice/Custom/octopus"), new(220, 200, 64, 64), 8, $"Octopus D8", $"Octuples all dice.");
            #endregion
            #region Rainbow
            rainbowD20 = new(Dice.SpecialEffect.Rainbow, Content.Load<Texture2D>($"Dice/rainbowD20"), new(80, 200, 64, 64), 20, $"Rainbow D20", $"Multiplies all dice to\nthe right into itself.");
            rainbowD12 = new(Dice.SpecialEffect.Rainbow, Content.Load<Texture2D>($"Dice/rainbowD12"), new(150, 200, 64, 64), 12, $"Rainbow D12", $"Multiplies all dice to\nthe right into itself.");
            rainbowD8 = new(Dice.SpecialEffect.Rainbow, Content.Load<Texture2D>($"Dice/rainbowD8"), new(220, 200, 64, 64), 8, $"Rainbow D8", $"Multiplies all dice to\nthe right into itself.");
            rainbowD6 = new(Dice.SpecialEffect.Rainbow, Content.Load<Texture2D>($"Dice/rainbowD6"), new(290, 200, 64, 64), 6, $"Rainbow D6", $"Multiplies all dice to\nthe right into itself.");
            rainbowD4 = new(Dice.SpecialEffect.Rainbow, Content.Load<Texture2D>($"Dice/rainbowD4"), new(360, 200, 64, 64), 4, $"Rainbow D4", $"Multiplies all dice to\nthe right into itself.");
            #endregion
            #region Flowered
            floweredD20 = new(Dice.SpecialEffect.Flowered, Content.Load<Texture2D>($"Dice/floweredD20"), new(80, 200, 64, 64), 20, $"Flowered D20", $"Squares itself.");
            floweredD12 = new(Dice.SpecialEffect.Flowered, Content.Load<Texture2D>($"Dice/floweredD12"), new(150, 200, 64, 64), 12, $"Flowered D12", $"Squares itself.");
            floweredD8 = new(Dice.SpecialEffect.Flowered, Content.Load<Texture2D>($"Dice/floweredD8"), new(220, 200, 64, 64), 8, $"Flowered D8", $"Squares itself.");
            floweredD6 = new(Dice.SpecialEffect.Flowered, Content.Load<Texture2D>($"Dice/floweredD6"), new(290, 200, 64, 64), 6, $"Flowered D6", $"Squares itself.");
            floweredD4 = new(Dice.SpecialEffect.Flowered, Content.Load<Texture2D>($"Dice/floweredD4"), new(360, 200, 64, 64), 4, $"Flowered D4", $"Squares itself.");
            #endregion
            #region Yin Yang
            yinyangD20 = new(Dice.SpecialEffect.YinYang, Content.Load<Texture2D>($"Dice/yyD20"), new(80, 200, 64, 64), 20, $"Yin Yang D20", $"Can only roll a minimum or\n        maximum value.");
            yinyangD12 = new(Dice.SpecialEffect.YinYang, Content.Load<Texture2D>($"Dice/yyD12"), new(150, 200, 64, 64), 12, $"Yin Yang D12", $"Can only roll a minimum or\n        maximum value.");
            yinyangD8 = new(Dice.SpecialEffect.YinYang, Content.Load<Texture2D>($"Dice/yyD8"), new(220, 200, 64, 64), 8, $"Yin Yang D8", $"Can only roll a minimum or\n        maximum value.");
            yinyangD6 = new(Dice.SpecialEffect.YinYang, Content.Load<Texture2D>($"Dice/yyD6"), new(290, 200, 64, 64), 6, $"Yin Yang D6", $"Can only roll a minimum or\n        maximum value.");
            yinyangD4 = new(Dice.SpecialEffect.YinYang, Content.Load<Texture2D>($"Dice/yyD4"), new(360, 200, 64, 64), 4, $"Yin Yang D4", $"Can only roll a minimum or\n        maximum value.");
            #endregion
            #region Chalkboard
            chalkboardD20 = new(Dice.SpecialEffect.Chalkboard, Content.Load<Texture2D>($"Dice/chalkboardD20"), new(80, 200, 64, 64), 20,  $"Chalkboard D20", $"Rolls are additive.");
            chalkboardD12 = new(Dice.SpecialEffect.Chalkboard, Content.Load<Texture2D>($"Dice/chalkboardD12"), new(150, 200, 64, 64), 12, $"Chalkboard D12", $"Rolls are additive.");
            chalkboardD8 =  new(Dice.SpecialEffect.Chalkboard, Content.Load<Texture2D>($"Dice/chalkboardD8"), new(220, 200, 64, 64), 8,   $"Chalkboard D8",  $"Rolls are additive.");
            chalkboardD6 =  new(Dice.SpecialEffect.Chalkboard, Content.Load<Texture2D>($"Dice/chalkboardD6"), new(290, 200, 64, 64), 6,   $"Chalkboard D6",  $"Rolls are additive.");
            chalkboardD4 =  new(Dice.SpecialEffect.Chalkboard, Content.Load<Texture2D>($"Dice/chalkboardD4"), new(360, 200, 64, 64), 4,   $"Chalkboard D4",  $"Rolls are additive.");
            #endregion
            #region Frozen
            frozenD20 = new(Dice.SpecialEffect.Frozen, Content.Load<Texture2D>($"Dice/frozenD20"), new(80, 200, 64, 64), 20,  $"Frozen D20", $"Dice to the right are prevented from \n       rolling lower than this dice.");
            frozenD12 = new(Dice.SpecialEffect.Frozen, Content.Load<Texture2D>($"Dice/frozenD12"), new(150, 200, 64, 64), 12, $"Frozen D12", $"Dice to the right are prevented from \n       rolling lower than this dice.");
            frozenD8 =  new(Dice.SpecialEffect.Frozen, Content.Load<Texture2D>($"Dice/frozenD8"), new(220, 200, 64, 64), 8,   $"Frozen D8",  $"Dice to the right are prevented from \n       rolling lower than this dice.");
            frozenD6 =  new(Dice.SpecialEffect.Frozen, Content.Load<Texture2D>($"Dice/frozenD6"), new(290, 200, 64, 64), 6,   $"Frozen D6",  $"Dice to the right are prevented from \n       rolling lower than this dice.");
            frozenD4 =  new(Dice.SpecialEffect.Frozen, Content.Load<Texture2D>($"Dice/frozenD4"), new(360, 200, 64, 64), 4,   $"Frozen D4",  $"Dice to the right are prevented from \n       rolling lower than this dice.");
            #endregion
            #region Explosive
            explosiveD20 = new(Dice.SpecialEffect.Explosive, Content.Load<Texture2D>($"Dice/explosiveD20"), new(80, 200, 64, 64), 20,  $"Explosive D20", $"Every three rounds,\n  quadruples itself.");
            explosiveD12 = new(Dice.SpecialEffect.Explosive, Content.Load<Texture2D>($"Dice/explosiveD12"), new(150, 200, 64, 64), 12, $"Explosive D12", $"Every three rounds,\n  quadruples itself.");
            explosiveD8 = new(Dice.SpecialEffect.Explosive, Content.Load<Texture2D>($"Dice/explosiveD8"), new(220, 200, 64, 64), 8,    $"Explosive D8",  $"Every three rounds,\n  quadruples itself.");
            explosiveD6 = new(Dice.SpecialEffect.Explosive, Content.Load<Texture2D>($"Dice/explosiveD6"), new(290, 200, 64, 64), 6,    $"Explosive D6",  $"Every three rounds,\n  quadruples itself.");
            explosiveD4 = new(Dice.SpecialEffect.Explosive, Content.Load<Texture2D>($"Dice/explosiveD4"), new(360, 200, 64, 64), 4,    $"Explosive D4",  $"Every three rounds,\n  quadruples itself.");
            #endregion
            #region Rounder
            rounderD20 = new(Dice.SpecialEffect.RoundTotaler, Content.Load<Texture2D>($"Dice/rounderD20"), new(80, 200, 64, 64), 20, $"Rounder D20", $" Adds half of the current\nround number to its total.");
            rounderD12 = new(Dice.SpecialEffect.RoundTotaler, Content.Load<Texture2D>($"Dice/rounderD12"), new(150, 200, 64, 64), 12, $"Rounder D12", $" Adds half of the current\nround number to its total.");
            rounderD8 = new(Dice.SpecialEffect.RoundTotaler, Content.Load<Texture2D>($"Dice/rounderD8"), new(220, 200, 64, 64), 8, $"Rounder D8", $" Adds half of the current\nround number to its total.");
            rounderD6 = new(Dice.SpecialEffect.RoundTotaler, Content.Load<Texture2D>($"Dice/rounderD6"), new(290, 200, 64, 64), 6, $"Rounder D6", $" Adds half of the current\nround number to its total.");
            rounderD4 = new(Dice.SpecialEffect.RoundTotaler, Content.Load<Texture2D>($"Dice/rounderD4"), new(360, 200, 64, 64), 4, $"Rounder D4", $" Adds half of the current\nround number to its total.");
            #endregion
            #region Weighted
            weightedD20 = new(Dice.SpecialEffect.Weighted, Content.Load<Texture2D>($"Dice/weightedD20"), new(80, 200, 64, 64), 20, $"Weighted D20", $"More likely to roll a 20.");
            weightedD12 = new(Dice.SpecialEffect.Weighted, Content.Load<Texture2D>($"Dice/weightedD12"), new(150, 200, 64, 64), 12, $"Weighted D12", $"More likely to roll a 12.");
            weightedD8 = new(Dice.SpecialEffect.Weighted, Content.Load<Texture2D>($"Dice/weightedD8"), new(220, 200, 64, 64), 8, $"Weighted D8", $"More likely to roll an 8.");
            weightedD6 = new(Dice.SpecialEffect.Weighted, Content.Load<Texture2D>($"Dice/weightedD6"), new(290, 200, 64, 64), 6, $"Weighted D6", $"More likely to roll a 6.");
            weightedD4 = new(Dice.SpecialEffect.Weighted, Content.Load<Texture2D>($"Dice/weightedD4"), new(360, 200, 64, 64), 4, $"Weighted D4", $"More likely to roll a 4.");
            #endregion
            #region Trio
            trioD20 = new(Dice.SpecialEffect.Trio, Content.Load<Texture2D>($"Dice/trioD20"), new(80, 200, 64, 64), 20, $"Trio D20", $"If the roll is divisible by 3, triples it.\n           If not, subtracts 3.");
            trioD12 = new(Dice.SpecialEffect.Trio, Content.Load<Texture2D>($"Dice/trioD12"), new(150, 200, 64, 64), 12, $"Trio D12", $"If the roll is divisible by 3, triples it.\n           If not, subtracts 3.");
            trioD8 = new(Dice.SpecialEffect.Trio, Content.Load<Texture2D>($"Dice/trioD8"), new(220, 200, 64, 64), 8, $"Trio D8", $"If the roll is divisible by 3, triples it.\n           If not, subtracts 3.");
            trioD6 = new(Dice.SpecialEffect.Trio, Content.Load<Texture2D>($"Dice/trioD6"), new(290, 200, 64, 64), 6, $"Trio D6", $"If the roll is divisible by 3, triples it.\n           If not, subtracts 3.");
            trioD4 = new(Dice.SpecialEffect.Trio, Content.Load<Texture2D>($"Dice/trioD4"), new(360, 200, 64, 64), 4, $"Trio D4", $"If the roll is divisible by 3, triples it.\n           If not, subtracts 3.");
            #endregion
            #endregion
            #endregion

            rollButton = new($"Roll!", largeText, new(30, 280), Color.White);
            skipButton = new($"Skip!", largeText, new(500, 260), Color.Red);
            playButton = new($"Play!", largeText, new(500, 200), Color.White);
            quitButton = new($"Quit!", largeText, new(500, 270), Color.White);

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

            bag.AddCommon(fickleD20, fickleD12, fickleD8, fickleD6, fickleD4, advantageD20, advantageD12, advantageD8, advantageD6, advantageD4, trioD20, trioD12, trioD8, trioD6, trioD4, weightedD20, weightedD12, weightedD8, weightedD6, weightedD4, rounderD20, rounderD12, rounderD8, rounderD6, rounderD4);
            bag.AddUncommon(dangerD20, dangerD12, dangerD8, dangerD6, dangerD4, spaceD20, spaceD12, spaceD8, spaceD6, spaceD4, sprinkleD20, sprinkleD12, sprinkleD8, sprinkleD6, sprinkleD4, yinyangD20, yinyangD12, yinyangD8, yinyangD6, yinyangD4, frozenD20, frozenD12, frozenD8, frozenD6, frozenD4, explosiveD20, explosiveD12, explosiveD8, explosiveD6, explosiveD4);
            bag.AddRare(avalancheD20, avalancheD12, avalancheD8, avalancheD6, avalancheD4, floweredD20, floweredD12, floweredD8, floweredD6, floweredD4, chalkboardD20, chalkboardD12, chalkboardD8, chalkboardD6, chalkboardD4);
            bag.AddLegendary(galaxyD20, galaxyD12, galaxyD8, galaxyD6, galaxyD4, golden, fibonacci, octopus, rainbowD20, rainbowD12, rainbowD8, rainbowD6, rainbowD4);

            #region Music
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(backgroundTrack);
            MediaPlayer.IsRepeating = true;
            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            ms = Mouse.GetState();
            kb = Keyboard.GetState();
            mScaled = new((int)(ms.X * (640.0f / _graphics.PreferredBackBufferWidth)), (int)(ms.Y * (360.0f / _graphics.PreferredBackBufferHeight)));

            // Make mouse invisible if they're using keyboard, and visible if it moves again.
            if(kb.GetPressedKeys().Length != 0)
            {
                IsMouseVisible = false;
            }
            else if(ms.Position != pms.Position)
            {
                IsMouseVisible = true;
            }

            switch (gameState)
            {
                case GameState.Menu:

                    if(playButton.Update(ms, pms, mScaled) || SingleKeyPress(Keys.Enter))
                    {
                        gameState = GameState.Game;
                    }

                    if(quitButton.Update(ms, pms, mScaled) || SingleKeyPress(Keys.Escape))
                    {
                        Exit();
                    }

                    break;

                case GameState.Game:

                    // ROLL THE DICE AND CALCULATE THEIR TOTALS!!!
                    if((rollButton.Update(ms, pms, mScaled) || SingleKeyPress(Keys.Enter)) && !rolled)
                    {
                        // SWEET dice rolling noises :)
                        rollNoises[rng.Next(0, 5)].Play(1.0f, 0.0f, 0.0f);

                        rolled = true;
                        rollTimer = 3.0;

                        // Roll each dice, and calculate rollTotal BEFORE multiplying.
                        foreach (Dice die in dice)
                            {
                                die.Roll();

                                // Round totaler functionality.
                                if(die.Special == Dice.SpecialEffect.RoundTotaler)
                                {
                                    die.Value += round / 2;
                                }

                                // Frozen functionality P1.
                                else if(die.Special == Dice.SpecialEffect.Frozen)
                                {
                                    frozen = true;
                                }

                                // Rainbow functionality P1.
                                else if (die.Special == Dice.SpecialEffect.Rainbow)
                                {
                                    multiplying = true;
                                }

                                // Avalanche functionality.
                                else if (die.Special == Dice.SpecialEffect.Avalanche)
                                {
                                    die.Value += rollTotal;
                                }

                                // Galaxy functionality P1.
                                else if (die.Special == Dice.SpecialEffect.Galaxy)
                                {
                                    multiplying = true;
                                    galaxyTicker++;
                                }

                                rollTotal += die.Value;
                            }

                        // Octopus functionality P1.
                        if(dice[2].Special == Dice.SpecialEffect.Octopus)
                            {
                                multiplying = true;
                            }

                        // Handle special multiplication cases.
                        if(multiplying)
                            {
                                // Reset before multiplying.
                                rollTotal = 0;

                                // Rainbow dice functionality.
                                for (int i = 0; i < 5; i++)
                                {
                                    if (dice[i].Special == Dice.SpecialEffect.Rainbow)
                                    {
                                        for (int j = 4; j > i; j--)
                                        {
                                            dice[i].Value *= dice[j].Value;
                                        }
                                    }
                                }

                                // Galaxy functionality P2.
                                if (galaxyTicker > 0)
                                {
                                    // Resolve all Galaxy dice.
                                    for (int i = 0; i < galaxyTicker; i++)
                                    {
                                        foreach (Dice die in dice)
                                        {
                                            die.Value *= 4;
                                        }
                                    }
                                    galaxyTicker = 0;
                                }

                                // Octopus functionality P2.
                                if (dice[2].Special == Dice.SpecialEffect.Octopus)
                                {
                                    // Octuple all dice.
                                    foreach (Dice die in dice)
                                    {
                                        die.Value *= 8;
                                    }
                                }

                                // Re-calculate rollTotal.
                                foreach (Dice die in dice)
                                {
                                    rollTotal += die.Value;
                                }

                                multiplying = false;
                            }

                        // Frozen functionality P2.
                        if(frozen)
                            {
                                rollTotal = 0;
                                for (int i = 0; i < dice.Count; i++)
                                {
                                    if (dice[i].Special == Dice.SpecialEffect.Frozen)
                                    {
                                        for (int j = i + 1; j < dice.Count; j++)
                                        {
                                            if (dice[j].Value < dice[i].Value)
                                            {
                                                dice[j].Value = dice[i].Value;
                                            }
                                        }
                                    }
                                }
                                // Recalculate again.
                                foreach (Dice die in dice)
                                {
                                    rollTotal += die.Value;
                                }
                            }

                        #region Nat 20s and 1s
                            // Double score on Nat 20.
                            if (dice[0].Max)
                            {
                                PlayVoiceLine(true);

                                // Golden Dice functionality.
                                if (dice[0].Special == Dice.SpecialEffect.Golden)
                                {
                                    rollTotal *= 20;
                                }

                                // Natural 20.
                                else
                                {
                                    rollTotal *= 2;
                                }
                            }

                            // Half score on Nat 1.
                            else if (dice[0].Min && dice[0].Special != Dice.SpecialEffect.Golden)
                            {
                                PlayVoiceLine(false);
                                rollTotal /= 2;
                            }
                            #endregion

                        // Save highest roll this round.
                        if (rollTotal > highestRoundRoll)
                            {
                                highestRoundRoll = rollTotal;
                            }
                    }

                    if(rolled)
                    {
                        if (rollTimer <= 0)
                        {
                            rolled = false;

                            if (rollTotal >= targetScore)
                            {
                                rollTotal = 0;
                                round++;
                                targetScore = (int)(targetScore * 1.25);

                                // 20% chance to play a voice line when they win a round.
                                if(rng.Next(1, 6) == 1)
                                {
                                    PlayVoiceLine(true);
                                }

                                // Reset dice values.
                                foreach(Dice die in dice)
                                {
                                    die.Value = -1000;
                                    die.Max = false;
                                    die.Min = false;
                                }

                                leftChoice.Dice = bag.Pull(dice[0], dice[1], dice[2], dice[3], dice[4]);
                                centerChoice.Dice = bag.Pull(dice[0], dice[1], dice[2], dice[3], dice[4], leftChoice.Dice);
                                rightChoice.Dice = bag.Pull(dice[0], dice[1], dice[2], dice[3], dice[4], leftChoice.Dice, centerChoice.Dice);
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

                    if (leftChoice.CheckInteraction(mScaled, ms, pms) || SingleKeyPress(Keys.D1))
                        {
                            leftChoice.Dice.Value = -1000;

                            // Assigns it to the right dice slot.
                            switch (leftChoice.Dice.MaxValue)
                            {
                                case 20:
                                    dice[0].Value = -1000;
                                    dice[0] = leftChoice.Dice;
                                    break;

                                case 12:
                                    dice[1].Value = -1000;
                                    dice[1] = leftChoice.Dice;
                                    break;

                                case 8:
                                    dice[2].Value = -1000;
                                    dice[2] = leftChoice.Dice;
                                    break;

                                case 6:
                                    dice[3].Value = -1000;
                                    dice[3] = leftChoice.Dice;
                                    break;

                                case 4:
                                    dice[4].Value = -1000;
                                    dice[4] = leftChoice.Dice;
                                    break;
                            }

                            gameState = GameState.Game;
                        }

                    else if (centerChoice.CheckInteraction(mScaled, ms, pms) || SingleKeyPress(Keys.D2))
                        {
                            centerChoice.Dice.Value = -1000;

                            // Assigns it to the right dice slot.
                            switch (centerChoice.Dice.MaxValue)
                            {
                                case 20:
                                    dice[0].Value = -1000;
                                    dice[0] = centerChoice.Dice;
                                    break;

                                case 12:
                                    dice[1].Value = -1000;
                                    dice[1] = centerChoice.Dice;
                                    break;

                                case 8:
                                    dice[2].Value = -1000;
                                    dice[2] = centerChoice.Dice;
                                    break;

                                case 6:
                                    dice[3].Value = -1000;
                                    dice[3] = centerChoice.Dice;
                                    break;

                                case 4:
                                    dice[4].Value = -1000;
                                    dice[4] = centerChoice.Dice;
                                    break;
                            }

                            gameState = GameState.Game;
                        }

                    else if (rightChoice.CheckInteraction(mScaled, ms, pms) || SingleKeyPress(Keys.D3))
                        {
                            rightChoice.Dice.Value = -1000;

                            // Assigns it to the right dice slot.
                            switch (rightChoice.Dice.MaxValue)
                            {
                                case 20:
                                    dice[0].Value = -1000;
                                    dice[0] = rightChoice.Dice;
                                    break;

                                case 12:
                                    dice[1].Value = -1000;
                                    dice[1] = rightChoice.Dice;
                                    break;

                                case 8:
                                    dice[2].Value = -1000;
                                    dice[2] = rightChoice.Dice;
                                    break;

                                case 6:
                                    dice[3].Value = -1000;
                                    dice[3] = rightChoice.Dice;
                                    break;

                                case 4:
                                    dice[4].Value = -1000;
                                    dice[4] = rightChoice.Dice;
                                    break;
                            }

                            gameState = GameState.Game;
                        }

                    // Allows player to skip choosing dice.
                    if (skipButton.Update(ms, pms, mScaled) || SingleKeyPress(Keys.RightShift))
                        {
                            gameState = GameState.Game;
                        }

                    break;
            }

            pms = ms;
            pkb = kb;
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
                    _spriteBatch.Draw(hunkTitle, backdrop, Color.White);

                    playButton.Draw(_spriteBatch);
                    quitButton.Draw(_spriteBatch);

                    _spriteBatch.DrawString(smallText, $"Highest Roll: {highestRoll}", highestRollDrawVec, Color.Gold);
                    _spriteBatch.DrawString(smallText, $"Furthest Round: {furthestRound}", furthestRoundDrawVec, Color.Gold);
                    _spriteBatch.DrawString(smallText, $"Version {versionDisplay}", versionDrawVec, Color.SkyBlue);

                    break;

                case GameState.Game:

                    // Backdrop!!!
                    _spriteBatch.Draw(tabletop, backdrop, Color.White);

                    // Draw the dice.
                    foreach(Dice die in dice)
                    {
                        die.Draw(_spriteBatch, smallText, false);
                    }

                    // Flavor text when they roll a Nat 20 or Nat 1.
                    if(dice[0].Max)
                    {
                        Vector2 centerTemp = smallText.MeasureString($"Nat 20!");
                        _spriteBatch.DrawString(smallText, $"Nat 20!", new(minMaxFlavorVec.X - (centerTemp.X / 2), minMaxFlavorVec.Y - (centerTemp.Y / 2)), Color.LimeGreen);
                    }
                    else if(dice[0].Min)
                    {
                        Vector2 centerTemp = smallText.MeasureString($"Nat 1!");
                        _spriteBatch.DrawString(smallText, $"Nat 1!", new(minMaxFlavorVec.X - (centerTemp.X / 2), minMaxFlavorVec.Y - (centerTemp.Y / 2)), Color.Red);
                    }

                    // Draw the scores.
                    _spriteBatch.DrawString(mediumText, $"Round: {round}", roundDrawVec, Color.LightPink);
                    _spriteBatch.DrawString(mediumText, $"Total: {rollTotal}", totalDrawVec, Color.Gold);
                    _spriteBatch.DrawString(mediumText, $"Target: {targetScore}", targetDrawVec, Color.Firebrick);

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
                    _spriteBatch.Draw(tabletop, backdrop, backDarken);

                    // Draw the dice.
                    foreach (Dice die in dice)
                    {
                        die.Draw(_spriteBatch, mediumText, true);
                    }

                    // Draw the scores.
                    _spriteBatch.DrawString(mediumText, $"Round: {round}", roundDrawVec, Color.LightPink * backDarken);
                    _spriteBatch.DrawString(mediumText, $"Total: {rollTotal}", totalDrawVec, Color.Gold * backDarken);
                    _spriteBatch.DrawString(mediumText, $"Target: {targetScore}", targetDrawVec, Color.Firebrick * backDarken);

                    _spriteBatch.Draw(hunkus, hunkusFrame, backDarken);
                    #endregion

                    leftChoice.Draw(_spriteBatch);
                    centerChoice.Draw(_spriteBatch);
                    rightChoice.Draw(_spriteBatch);

                    _spriteBatch.DrawString(smallText, $"or, you can", skipButtonFlavorVec, Color.White);
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
        /// Checks to see if the player pressed a specific key.
        /// </summary>
        /// <param name="key"> The key we're checking. </param>
        /// <returns> Whether or not it was pushed. <returns>
        public bool SingleKeyPress(Keys key)
        {
            return kb.IsKeyDown(key) && pkb.IsKeyUp(key);
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
                bw.Write(currentVersion);
                // Highest Roll
                if(highestRoundRoll > highestRoll)
                {
                    highestRoll = highestRoundRoll;
                    bw.Write(highestRoll);
                }
                else
                {
                    bw.Write(highestRoll);
                }

                // Round
                if(round > furthestRound)
                {
                    furthestRound = round;
                    bw.Write(furthestRound);
                }
                else
                {
                    bw.Write(furthestRound);
                }
            }
            catch
            {
                bw.Write(currentVersion);
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
