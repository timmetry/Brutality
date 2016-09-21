using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brutality
{
    public class Fighter
    {
        protected string name;  // name of fighter: usually randomized
        protected bool gender;  // (f)alse = (f)emale
        public string Name { get { return name; } }
        public string Gender { get { return gender ? "Male" : "Female"; } }

        protected int level;    // level of fighter: increases with experience
        protected int xp;       // current experience toward next level
        protected int nextXP;   // experience required for next level-up
        protected int rewardXP; // experience earned by opponent if defeated
        public int Level { get { return level; } }
        public int XP { get { return xp; } }
        public int NextXP { get { return nextXP; } }
        public int RewardXP { get { return rewardXP; } }

        protected StatSet baseStats;
        protected StatSet trueStats;

        protected int maxHP;    // maximum health: based on level and vitality
        protected int maxSP;    // maximum stamina: based on endurance
        protected int hp;       // health (life): consumed by damage
        protected int sp;       // stamina (energy): consumed by attacking
        public int MaxHP { get { return maxHP; } }
        public int MaxSP { get { return maxSP; } }
        public int HP { get { return hp; } }
        public int SP { get { return sp; } }
        protected bool fainted;
        public bool IsFainted { get { return fainted; } }

        protected int chanceToDrawWeapon;    // chance to draw a weapon when unarmed
        protected int chanceToThrowWeapon;   // chance to throw weapon at enemy
        protected int chanceToSwapTrinket;   // chance to swap trinkets before attacking
        protected int chanceToUseItem;       // chance to use an item this turn
        protected int chanceToUseSkill;      // chance to use a skill this turn
        protected int chanceToFeintAttack;   // chance to try a feint attack
        protected int chanceToCounterAttack; // chance to use a counter attack
        protected int chanceToComboAttack;   // chance to attack again this turn
        protected int chanceToDisarm;        // chance to disarm the enemy's weapon
        protected int chanceToDetrinket;     // chance to disarm the enemy's trinket
        protected int chanceToBlock;         // chance to block an enemy attack
        public int ChanceToDrawWeapon { get { return chanceToDrawWeapon; } }
        public int ChanceToThrowWeapon { get { return chanceToThrowWeapon; } }
        public int ChanceToSwapTrinket { get { return chanceToSwapTrinket; } }
        public int ChanceToUseItem { get { return chanceToUseItem; } }
        public int ChanceToUseSkill { get { return chanceToUseSkill; } }
        public int ChanceToFeintAttack { get { return chanceToFeintAttack; } }
        public int ChanceToCounterAttack { get { return chanceToCounterAttack; } }
        public int ChanceToComboAttack { get { return chanceToComboAttack; } }
        public int ChanceToDisarm { get { return chanceToDisarm; } }
        public int ChanceToDetrinket { get { return chanceToDetrinket; } }
        public int ChanceToBlock { get { return chanceToBlock; } }

        protected static Random RNG;
        public static void InitializeRNG(Random rng) { RNG = rng; }

        public Fighter()
        {
            // select gender at random
            if (RNG.Next(2) > 0)
            {
                // male = true
                gender = true;
                name = NameGenMale.GetRandomName();
            }
            else
            {
                // (f)emale = (f)alse
                gender = false;
                name = NameGenFemale.GetRandomName();
            }
            // start at level zero and level up once
            level = 0;
            baseStats = StatSet.TEN;
            LevelUp(false);
            RecalculateStats();
            FullRecovery();
        }
        public Fighter(int initialLevel): this()
        {
            LevelUp(false, initialLevel - 1);
            RecalculateStats();
            FullRecovery();
        }

        public int this[Stat s]
        { get { return trueStats[s]; } }

        public void RecalculateStats()
        {
            // calculate true stats, including modifiers (more to do later)
            trueStats = new StatSet(baseStats);

            // calculate max hp and sp
            maxHP = 50 + 5 * trueStats[Stat.VIT];
            maxSP = 50 + 5 * trueStats[Stat.END];
            if (hp > maxHP) hp = maxHP;
            if (sp > maxSP) sp = maxSP;
            /*
        protected int chanceToDrawWeapon;    // chance to draw a weapon when unarmed
        protected int chanceToThrowWeapon;   // chance to throw weapon at enemy
        protected int chanceToSwapTrinket;   // chance to swap trinkets before attacking
        protected int chanceToUseItem;       // chance to use an item this turn
        protected int chanceToUseSkill;      // chance to use a skill this turn
        protected int chanceToFeintAttack;   // chance to try a feint attack
        protected int chanceToCounterAttack; // chance to use a counter attack
        protected int chanceToComboAttack;   // chance to attack again this turn
        protected int chanceToDisarm;        // chance to disarm the enemy's weapon
        protected int chanceToDetrinket;     // chance to disarm the enemy's trinket
        protected int chanceToBlock;         // chance to block an enemy attack
             */

            // set base chances for battle effects
            chanceToDrawWeapon = 50;
            chanceToThrowWeapon = 10;
            chanceToSwapTrinket = 10;
            chanceToUseItem = 10;
            chanceToUseSkill = 10;
            chanceToFeintAttack = 10;
            chanceToCounterAttack = 10;
            chanceToComboAttack = 10;
            chanceToDisarm = 10;
            chanceToDetrinket = 10;
            chanceToBlock = 10;
        }

        public void HealHP(int health)
        {
            hp += health;
            if (hp > maxHP) hp = maxHP;
        }
        public void HealSP(int stamina)
        {
            sp += stamina;
            if (sp > maxSP) sp = maxSP;
        }
        public void FullRecovery()
        {
            hp = maxHP;
            sp = maxSP;
        }

        public void GainXP(int experience)
        {
            xp += experience;
            while (xp > nextXP)
                LevelUp(true);
        }

        // XP Curve Formulas:
        protected static int GetRequiredXP(int level) // xp required for level
        { return 6*level + 2*(level-1)*(level-2)/2; }
        protected static int GetRewardXP(int level) // xp rewarded for win
        { return 5 + 2*level + (int)(0.1*(level-1)*(level-2)/2 + 0.5); }

        public void LevelUp(bool playerPick)
        {
            level += 1;
            // wrap xp over to next level
            xp -= nextXP;
            if (xp < 0) xp = 0;
            // update xp counters
            nextXP = GetRequiredXP(level + 1);
            rewardXP = GetRewardXP(level);

            // first, increase base stat points by a few at random
            // at the beginning, get 3 stat points per level
            // every 10 levels get 1 more stat point per level
            StatSet bonusStats = StatSet.ZERO;
            bonusStats.BoostRandom(3 + level/10);
            baseStats += bonusStats;
            if (playerPick)
            {
                RecalculateStats();
                Console.Out.WriteLine(name + " has leveled up!");
                Console.Out.WriteLine("Base stat points awarded:");
                Console.Out.WriteLine(bonusStats.ToString());
                Console.Out.WriteLine("Resulting total stats:");
                Console.Out.WriteLine(trueStats.ToString());
            }
        }
        public void LevelUp(bool playerPick, int numLevels)
        {
            if (numLevels > 0)
            {
                LevelUp(playerPick);
                LevelUp(playerPick, numLevels);
            }
        }
    }
}
