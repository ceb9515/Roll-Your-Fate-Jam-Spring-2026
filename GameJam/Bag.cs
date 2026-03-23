using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam
{
    // Joshua Smith and Charlie Besgen
    // 03/22/2026
    //
    // Contains the dice not currently in use by the player.
    internal class Bag
    {
        private List<Dice> common;
        private List<Dice> uncommon;
        private List<Dice> rare;
        private List<Dice> legendary;

        private Random rng = new();

        public Bag()
        {
            common = [];
            uncommon = [];
            rare = [];
            legendary = [];
        }

        /// <summary>
        /// Pulls a dice from the bag with varying rarity.
        /// </summary>
        /// <returns> A dice from the bag. </returns>
        public Dice Pull()
        {
            int pullTemp = rng.Next(1, 201);

            // 70% for common dice.
            if(pullTemp <= 140)
            {
                return common[rng.Next(0, common.Count)];
            }

            // 22.5% for uncommon dice.
            else if(pullTemp <= 185)
            {
                return uncommon[rng.Next(0, uncommon.Count)];
            }

            // 7% for rare dice.
            else if (pullTemp <= 199)
            {
                return rare[rng.Next(0, rare.Count)];
            }

            // 0.5% for legendary dice.
            else
            {
                return legendary[rng.Next(0, legendary.Count)];
            }
        }

        public void AddCommon(params Dice[] dice)
        {
            foreach(Dice die in dice)
            {
                common.Add(die);
            }
        }

        public void AddUncommon(params Dice[] dice)
        {
            foreach (Dice die in dice)
            {
                uncommon.Add(die);
            }
        }

        public void AddRare(params Dice[] dice)
        {
            foreach (Dice die in dice)
            {
                rare.Add(die);
            }
        }

        public void AddLegendary(params Dice[] dice)
        {
            foreach (Dice die in dice)
            {
                legendary.Add(die);
            }
        }
    }
}
