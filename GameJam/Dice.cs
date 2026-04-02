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
        private readonly Texture2D texture;
        private readonly Random rng;
        private readonly Rectangle frame;
        private readonly string name;
        private readonly string desc;
        private int roundCounter;
        private int totalValue;

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
        /// Used to print the rarity out to the player.
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// The special effect of the dice.
        /// </summary>
        public enum SpecialEffect
        {
            #region V0
            None,
            Sprinkle,
            Fickle,
            Avalanche,
            Danger,
            Space,
            Galaxy,
            Advantage,
            #endregion
            #region V1
            Golden,
            Octopus,
            Fibonacci,
            Rainbow,
            Flowered,
            Chalkboard,
            YinYang,
            Frozen,
            Explosive,
            Trio,
            Weighted,
            RoundTotaler,
            #endregion
        }
        public SpecialEffect Special { get; set; }

        public Dice(SpecialEffect special, Texture2D texture, Rectangle frame, int maxValue, string name, string desc)
        {
            this.texture = texture;
            this.frame = frame;
            MaxValue = maxValue;
            this.name = name;
            this.desc = desc;
            rng = new();
            Special = special;
            Value = -1000;
            roundCounter = 0;
            totalValue = 0;
        }

        /// <summary>
        /// Rolls the dice.
        /// </summary>
        public void Roll()
        {
            // Higher chance to roll biggest value.
            if(Special == SpecialEffect.Weighted)
            {
                Value = rng.Next(1, MaxValue + 3);
                if(Value > MaxValue)
                {
                    Value = MaxValue;
                }
            }

            // Roll like normal.
            else
            {
                Value = rng.Next(1, MaxValue + 1);
            }

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
                        Value *= 2;
                    }
                    else
                    {
                        Value = 0;
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
                    Max = Value == MaxValue;
                    Min = Value == 1;
                    break;

                case SpecialEffect.Fibonacci:
                    switch(Value)
                    {
                        case 2:
                            Value = 1;
                            break;
                        case 3:
                            Value = 2;
                            break;
                        case 4:
                            Value = 3;
                            break;
                        case 6:
                            Value = 8;
                            break;
                        case 7:
                            Value = 13;
                            break;
                        case 8:
                            Value = 21;
                            break;
                        case 9:
                            Value = 34;
                            break;
                        case 10:
                            Value = 55;
                            break;
                        case 11:
                            Value = 89;
                            break;
                        case 12:
                            Value = 144;
                            break;
                        case 13:
                            Value = 233;
                            break;
                        case 14:
                            Value = 377;
                            break;
                        case 15:
                            Value = 610;
                            break;
                        case 16:
                            Value = 987;
                            break;
                        case 17:
                            Value = 1597;
                            break;
                        case 18:
                            Value = 2584;
                            break;
                        case 19:
                            Value = 4181;
                            break;
                        case 20:
                            Value = 6765;
                            break;
                    }
                    break;

                case SpecialEffect.Flowered:
                    Value *= Value;
                    break;

                case SpecialEffect.Chalkboard:
                    totalValue += Value;
                    Value = totalValue;
                    break;

                case SpecialEffect.YinYang:
                    if(rng.Next(1, 3) == 2)
                    {
                        Value = MaxValue;
                        Max = true;
                    }
                    else
                    {
                        Value = 1;
                        Min = true;
                    }
                    break;

                case SpecialEffect.Explosive:
                    roundCounter++;
                    if(roundCounter == 3)
                    {
                        roundCounter = 0;
                        Value *= 4;
                    }
                    break;

                case SpecialEffect.Trio:
                    if(Value % 3 == 0)
                    {
                        Value *= 3;
                    }
                    else
                    {
                        Value -= 3;
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
                sb.Draw(texture, frame, new(43, 43, 43));
            }
            else
            {
                sb.Draw(texture, frame, Color.White);
            }

            Vector2 valueMeasurements = font.MeasureString($"{Value}");

            if(Value > -500)
            {
                if(Special == SpecialEffect.Avalanche || Special == SpecialEffect.Trio || Special == SpecialEffect.Frozen)
                {
                    sb.DrawString(font, $"{Value}", new(frame.Center.X - (valueMeasurements.X / 2), frame.Center.Y - (valueMeasurements.Y / 2)), Color.Black);
                }
                else if(Special == SpecialEffect.YinYang)
                {
                    sb.DrawString(font, $"{Value}", new(frame.Center.X - (valueMeasurements.X / 2), frame.Center.Y - (valueMeasurements.Y / 2)), Color.Gray);
                }
                else
                {
                    sb.DrawString(font, $"{Value}", new(frame.Center.X - (valueMeasurements.X / 2), frame.Center.Y - (valueMeasurements.Y / 2)), Color.White);
                }
            }
        }

        /// <summary>
        /// Resets the inner tally for all individually rolled values. Used when you pull chalk.
        /// </summary>
        public void SpecialReset()
        {
            totalValue = 0;
            roundCounter = 0;
        }

        /// <summary>
        /// Draws the dice's information.
        /// </summary>
        /// <param name="sb"> The SpriteBatch being used to draw. </param>
        /// <param name="font"> The font being used to write. </param>
        /// <param name="origin"> Where to calculate positions from. </param>
        /// <param name="hovering"> Whether or not the dice is being hovered over. </param>
        /// <param name="held"> Whether or not the player is holding down on the dice. </param>
        public void DrawOption(SpriteBatch sb, SpriteFont font, Vector2 origin, bool hovering, bool held)
        {
            Vector2 nameMeasurements = font.MeasureString(name);
            Vector2 rarityMeasurements = font.MeasureString(Rarity);

            if(hovering)
            {
                // Draw dark gray.
                if(held)
                {
                    sb.DrawString(font, name, new(origin.X + 62 - (nameMeasurements.X / 2), origin.Y + 5), Color.Gray);
                    sb.Draw(texture, new Rectangle((int)(origin.X + 30), (int)(origin.Y + 30), 64, 64), Color.Gray);
                    switch (Rarity)
                    {
                        case "Common":
                            sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.LightGray * Color.Gray);
                            break;

                        case "Uncommon":
                            sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.LightGreen * Color.Gray);
                            break;

                        case "Rare":
                            sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.Blue * Color.Gray);
                            break;

                        case "Legendary":
                            sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.Violet * Color.Gray);
                            break;
                    }
                }

                // Draw gray.
                else
                {
                    sb.DrawString(font, name, new(origin.X + 62 - (nameMeasurements.X / 2), origin.Y + 5), Color.DarkGray);
                    sb.Draw(texture, new Rectangle((int)(origin.X + 30), (int)(origin.Y + 30), 64, 64), Color.DarkGray);
                    switch (Rarity)
                    {
                        case "Common":
                            sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.LightGray * Color.DarkGray);
                            break;

                        case "Uncommon":
                            sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.LightGreen * Color.DarkGray);
                            break;

                        case "Rare":
                            sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.Blue * Color.DarkGray);
                            break;

                        case "Legendary":
                            sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.Violet * Color.DarkGray);
                            break;
                    }
                }
            }

            // Draw like normal.
            else
            {
                sb.DrawString(font, name, new(origin.X + 62 - (nameMeasurements.X / 2), origin.Y + 5), Color.White);
                sb.Draw(texture, new Rectangle((int)(origin.X + 30), (int)(origin.Y + 30), 64, 64), Color.White);
                switch (Rarity)
                {
                    case "Common":
                        sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.LightGray);
                        break;

                    case "Uncommon":
                        sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.LightGreen);
                        break;

                    case "Rare":
                        sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.Blue);
                        break;

                    case "Legendary":
                        sb.DrawString(font, Rarity, new(origin.X + 62 - (rarityMeasurements.X / 2), origin.Y + 95), Color.Violet);
                        break;
                }
            }
        }

        /// <summary>
        /// Writes the description of the dice.
        /// </summary>
        /// <param name="sb"> The SpriteBatch we're using to draw. </param>
        /// <param name="font"> The font we're writing with. </param>
        /// <param name="location"> The location it's being centered on. </param>
        public void WriteDesc(SpriteBatch sb, SpriteFont font, Vector2 location)
        {
            Vector2 descMeasurements = font.MeasureString(desc);
            sb.DrawString(font, desc, new(location.X - descMeasurements.X / 2, location.Y - descMeasurements.Y / 2), Color.White);
        }
    }
}
