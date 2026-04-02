using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJam
{
    // Joshua Smith and Charlie Besgen
    // 03/24/2026
    //
    // This is the object that contains the dice a player can choose from between rounds. It'll have expanded button
    // functionality, to add polish to the game and make the interface less ugly.
    internal class DiceChoice(Vector2 origin, Texture2D frame, SpriteFont font)
    {
        /// <summary>
        /// The dice available in this dice choice.
        /// </summary>
        public Dice Dice { get; set; }

        private readonly Vector2 origin = origin; // Where the choice is being drawn.
        private readonly Rectangle hitbox = new((int)origin.X, (int)origin.Y, 124, 124); // The hitbox of the choice.
        private readonly Texture2D frame = frame; // The frame drawn behind the dice.
        private readonly SpriteFont font = font; // The font we're using to draw everything.
        private readonly Vector2 descOrigin = new(170, 310); // Where we write the description of this dice.

        private bool hovering; // If the player is hovering over this dice.
        private bool held; // If the player is holding on this dice.

        /// <summary>
        /// Checks how the player is interacting with the choice.
        /// </summary>
        /// <param name="mScaled"> The adjusted location of the mouse. </param>
        /// <param name="ms"> The current state of the mouse. </param>
        /// <param name="pms"> The previous state of the mouse. </param>
        /// <returns> Whether or not the player chose this option. </returns>
        public bool CheckInteraction(Point mScaled, MouseState ms, MouseState pms)
        {
            hovering = hitbox.Contains(mScaled); // Check if the player is hovering over this.
            held = ms.LeftButton == ButtonState.Pressed; //Check to see if the player is holding on this choice.
            return hovering && !held && pms.LeftButton == ButtonState.Pressed;
        }

        public void Draw(SpriteBatch sb)
        {
            if(hovering)
            {
                if(held)
                {
                    sb.Draw(frame, hitbox, Color.Gray);
                }
                else
                {
                    sb.Draw(frame, hitbox, Color.DarkGray);
                }

                Dice.WriteDesc(sb, font, descOrigin);
            }
            else
            {
                sb.Draw(frame, hitbox, Color.White);
            }

            Dice.DrawOption(sb, font, origin, hovering, held);
        }
    }
}
