﻿using System.Collections.Generic;

namespace Game.Configuration.Item
{
    public struct UpgradeRuneConfiguration
    {
        #region Properties

        // Just a Simple configuration
        public int[] GoldPrice { get; set; }

        public List<RequiredItem>[] Item { get; set; }

        public double[] PercentBreaked { get; set; }

        public double[] PercentFail { get; set; }

        public double[] PercentSucess { get; set; }

        #endregion Properties
    }
}