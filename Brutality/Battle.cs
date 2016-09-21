using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brutality
{
    public class Battle
    {
        private bool started;
        private int numberOfFighters;
        private int numberOfActiveFighters;
        public int NumberOfFighters { get { return numberOfFighters; } }
        public int NumberOfActiveFighters { get { return numberOfActiveFighters; } }

        private Fighter[] fighters;
        private int[] fighterTeams;

        public Battle(int numberOfFighters)
        {
            started = false;
            this.numberOfFighters = numberOfFighters;
            numberOfActiveFighters = 0;

            fighters = new Fighter[numberOfFighters];
            fighterTeams = new int[numberOfFighters];
        }

        public void AddFighter(Fighter fighter, int teamNumber)
        {
            if (started) throw new Exception(
                "You cannot add fighters to a battle that has already started!");
            if (numberOfActiveFighters >= numberOfFighters) throw new Exception(
                "The battle is already full, you cannot add any more fighters!");

            fighters[numberOfActiveFighters] = fighter;
            fighterTeams[numberOfActiveFighters] = teamNumber;
            numberOfActiveFighters += 1;
        }

        public void BeginBattle()
        {
            if (started) throw new Exception(
                "This battle has already started, you can't start over now!");
            if (numberOfActiveFighters < numberOfFighters) throw new Exception(
                "You cannot start the battle yet, not all the fighters have been added!");

            started = true;
        }
    }
}
