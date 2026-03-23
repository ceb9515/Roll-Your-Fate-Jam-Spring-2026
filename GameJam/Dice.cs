using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam
{
    // Joshua Smith and Charlie Besgen
    // 03/21/2026
    //
    // The dice! It does pretty much exactly what a dice in real life does, which is pretty cool.
    internal class Dice
    {
        private Texture2D texture;
        private readonly Random rng;
        private readonly Rectangle frame;
        private readonly Vector2 droffset;
        private readonly string name;
        private readonly string desc;

        /// <summary>
        /// The value rolled by this dice.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// The highest value of this dice before abilities.
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// Used to see if they get a NAT 20.
        /// </summary>
        public bool Max { get; set; }

        /// <summary>
        /// Used to see if they get a NAT 1.
        /// </summary>
        public bool Min { get; set; }

        /// <summary>
        /// The special effect of the dice.
        /// </summary>
        public enum SpecialEffect
        {
            None,
            Sprinkle,
            Fickle,
            Avalanche,
            Danger,
            Space,
            Galaxy,
            Advantage,
        }
        public SpecialEffect Special { get; set; }

        public Dice(SpecialEffect special, Texture2D texture, Rectangle frame, int maxValue, string name, string desc)
        {
            this.texture = texture;
            this.frame = frame;
            this.MaxValue = maxValue;
            this.name = name;
            this.desc = desc;
            rng = new();
            Special = special;
            Value = -1000;

            // Sets the correct offset. Will change later once we have more functionality.
            if(maxValue == 20)
            {
                droffset = new(frame.X + 23, frame.Y + 19);
            }
            else if(maxValue == 12)
            {
                droffset = new(frame.X + 25, frame.Y + 18);
            }
            else if(maxValue == 8)
            {
                droffset = new(frame.X + 27, frame.Y + 16);
            }
            else if(maxValue == 6)
            {
                droffset = new(frame.X + 27, frame.Y + 18);
            }
            else if(maxValue == 4)
            {
                droffset = new(frame.X + 27, frame.Y + 23);
            }
        }

        /// <summary>
        /// Rolls the dice.
        /// </summary>
        public void Roll()
        {
            Value = rng.Next(1, MaxValue + 1);
            Max = Value == MaxValue;
            Min = Value == 1;
            ApplySpecial();
        }

        public void ApplySpecial()
        {
            switch(Special)
            {
                case SpecialEffect.Sprinkle:
                    if(rng.Next(1, 6) == 5)
                    {
                        Value += 15;
                    }
                    break;

                case SpecialEffect.Fickle:
                    if(Value % 2 == 0)
                    {
                        Value -= 8;
                    }
                    else
                    {
                        Value += 8;
                    }
                    break;

                case SpecialEffect.Danger:
                    Value *= 3;
                    if(rng.Next(1, 11) == 1)
                    {
                        Value *= -1;
                    }
                    break;

                case SpecialEffect.Space:
                    Value *= 2;
                    break;

                case SpecialEffect.Advantage:
                    int temp = rng.Next(1, MaxValue + 1);
                    if(temp > Value)
                    {
                        Value = temp;
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws the dice to their respective locations.
        /// </summary>
        /// <param name="sb"> The SpriteBatch we're drawing with. </param>
        /// <param name="font"> The font used to display the value of the dice roll. </param>
        public void Draw(SpriteBatch sb, SpriteFont font, bool tinted)
        {
            // Draws it normal if not tinted, and grayed out if tinted.
            if(tinted)
            {
                sb.Draw(texture, frame, Color.Gray);
            }
            else
            {
                sb.Draw(texture, frame, Color.White);
            }

            if(Value > -500)
            {
                if(Special == SpecialEffect.Avalanche)
                {
                    sb.DrawString(font, $"{Value}", droffset, Color.Black);
                }
                else
                {
                    sb.DrawString(font, $"{Value}", droffset, Color.White);
                }
            }
        }

        /// <summary>
        /// Checks to see if the player clicked on the dice.
        /// </summary>
        /// <param name="clicked"> Whether or not the player has clicked. </param>
        /// <param name="mScaled"> The location of the player's mouse. </param>
        /// <returns> Whether or not they clicked the dice. </returns>
        public bool Clicked(Vector2 origin, bool clicked, Point mScaled)
        {
            Rectangle drawTemp = new((int)(origin.X + 30), (int)(origin.Y + 30), 64, 64);
            return clicked && drawTemp.Contains(mScaled);
        }

        public void DrawOption(SpriteBatch sb, SpriteFont font, Vector2 origin)
        {
            sb.DrawString(font, name, new(origin.X + 5, origin.Y + 5), Color.White);
            sb.Draw(texture, new Rectangle((int)(origin.X + 30), (int)(origin.Y + 30), 64, 64), Color.White);
            sb.DrawString(font, desc, new(origin.X - 20, origin.Y + 95), Color.White);
        }
    }
}
