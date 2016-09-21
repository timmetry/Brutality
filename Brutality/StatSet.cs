using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brutality
{
    public enum Stat
    {
        VIT,    // Vitality  (VIT) boosts Max HP
        END,    // Endurance (END) boosts Max SP
        STR,    // Strength  (STR) boosts attack damage
        DEX,    // Dexterity (DEX) boosts acrobatics success rate
        INT,    // Intellect (INT) boosts tactics success rate
        FRT,    // Fortitude (FRT) resists damage taken
        SPD,    // Speed     (SPD) boosts attack rate
        MAX
    }

    public class StatSet
    {

        protected static Random RNG;
        public static void InitializeRNG(Random rng) { RNG = rng; }

        protected int[] stats;

        public override string ToString()
        {
            string s = "";
            if (this[Stat.VIT] > 0)
                s += "Vitality  : " + this[Stat.VIT] + '\n';
            if (this[Stat.END] > 0)
                s += "Endurance : " + this[Stat.END] + '\n';
            if (this[Stat.STR] > 0)
                s += "Strength  : " + this[Stat.STR] + '\n';
            if (this[Stat.DEX] > 0)
                s += "Dexterity : " + this[Stat.DEX] + '\n';
            if (this[Stat.INT] > 0)
                s += "Intellect : " + this[Stat.INT] + '\n';
            if (this[Stat.FRT] > 0)
                s += "Fortitude : " + this[Stat.FRT] + '\n';
            if (this[Stat.SPD] > 0)
                s += "Speed     : " + this[Stat.SPD] + '\n';
            return s;
        }

        public StatSet()
        {
            stats = new int[(int)Stat.MAX];
        }
        public StatSet(StatSet b): this()
        {
            for (int i = 0; i < (int)Stat.MAX; ++i)
                stats[i] = b.stats[i];
        }
        public StatSet(int initial): this()
        {
            for (int i = 0; i < (int)Stat.MAX; ++i)
                stats[i] = initial;
        }

        public int this[Stat s]
        {
            get { return stats[(int)s]; }
            set { stats[(int)s] = value; }
        }

        public Stat BoostRandom()
        {
            Stat stat = (Stat)RNG.Next((int)Stat.MAX);
            this[stat] += 1;
            return stat;
        }
        public void BoostRandom(int num)
        {
            if (num > 0)
            {
                BoostRandom();
                BoostRandom(num - 1);
            }
        }

        public static StatSet operator +(StatSet a, StatSet b)
        {
            StatSet r = new StatSet(a);
            for (int i = 0; i < (int)Stat.MAX; ++i)
                r.stats[i] += b.stats[i];
            return r;
        }

        public static StatSet ZERO { get { return new StatSet(0); } }
        public static StatSet ONE { get { return new StatSet(1); } }
        public static StatSet FIVE { get { return new StatSet(5); } }
        public static StatSet EIGHT { get { return new StatSet(8); } }
        public static StatSet TEN { get { return new StatSet(10); } }

        public static StatSet RANDOM_START_FIVE
        { get {
            StatSet s = FIVE;
            s.BoostRandom(5 * (int)Stat.MAX);
            return s;
        }
        }

        public static StatSet RANDOM_START_EIGHT
        {
            get
            {
                StatSet s = EIGHT;
                s.BoostRandom(2 * (int)Stat.MAX);
                return s;
            }
        }
    }
}
