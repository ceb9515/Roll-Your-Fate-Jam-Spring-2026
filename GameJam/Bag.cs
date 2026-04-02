using System;
using System.Collections.Generic;

namespace GameJam
{
    // Joshua Smith and Charlie Besgen
    // 03/22/2026
    //
    // Contains the dice not currently in use by the player.
    internal class Bag
    {
        // Different rarities of dice.
        private readonly List<Dice> common;
        private readonly List<Dice> uncommon;
        private readonly List<Dice> rare;
        private readonly List<Dice> legendary;

        private readonly Random rng = new();

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
        public Dice Pull(params Dice[] owned)
        {
            int pullTemp = rng.Next(1, 201);
            bool same;
            List<Dice> rarity;

            // 70% for common dice.
            if(pullTemp <= 140)
            {
                rarity = common;
            }

            // 22.5% for uncommon dice.
            else if(pullTemp <= 185)
            {
                rarity = uncommon;
            }

            // 7% for rare dice.
            else if (pullTemp <= 199)
            {
                rarity = rare;
            }

            // 0.5% for legendary dice.
            else
            {
                rarity = legendary;
            }

            // Make sure it's a unique dice before returning.
            while(true)
            {
                // Generate random dice.
                pullTemp = rng.Next(0, rarity.Count);
                same = false;

                // Check if this dice has already been pulled, or the player already has it.
                foreach(Dice die in owned)
                {
                    if(rarity[pullTemp] == die)
                    {
                        same = true;
                        break;
                    }
                }

                // If it is, pull a new dice.
                if(same)
                {
                    continue;
                }

                // If it's a unique dice, but it's a special dice, reset it.
                if(rarity[pullTemp].Special == Dice.SpecialEffect.Chalkboard || rarity[pullTemp].Special == Dice.SpecialEffect.Explosive)
                {
                    rarity[pullTemp].SpecialReset();
                }

                return rarity[pullTemp];
            }
        }

        public void AddCommon(params Dice[] dice)
        {
            foreach(Dice die in dice)
            {
                die.Rarity = "Common";
                common.Add(die);
            }
        }

        public void AddUncommon(params Dice[] dice)
        {
            foreach (Dice die in dice)
            {
                die.Rarity = "Uncommon";
                uncommon.Add(die);
            }
        }

        public void AddRare(params Dice[] dice)
        {
            foreach (Dice die in dice)
            {
                die.Rarity = "Rare";
                rare.Add(die);
            }
        }

        public void AddLegendary(params Dice[] dice)
        {
            foreach (Dice die in dice)
            {
                die.Rarity = "Legendary";
                legendary.Add(die);
            }
        }
    }
}
