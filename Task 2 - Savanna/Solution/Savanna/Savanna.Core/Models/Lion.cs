﻿using Savanna.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savanna.Core.Models
{
    public class Lion : IAnimal
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }
        public string? Symbol { get; set; }
        public int VisionRange { get; set; }
        public double Health { get; set; }
        public int RoundsNearOtherAnimal { get; set; }
        public bool IsPredator { get; set; }
        public Lion()
        {
            Symbol = "L";
            IsPredator = true;
            IsAlive = true;
            VisionRange = 1;
            Health = 15;
            RoundsNearOtherAnimal = 0;
        }
    }
}
