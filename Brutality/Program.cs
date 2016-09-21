using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brutality
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rng = new Random();
            StatSet.InitializeRNG(rng);
            Fighter.InitializeRNG(rng);
            NameGenFemale.InitializeNameGenerator(rng);
            NameGenMale.InitializeNameGenerator(rng);


        }
    }
}
