namespace NOSTALE.CONFIG.Config
{
    public struct RateItem
    {
        #region RatesItem Config

        public byte[] RareRate { get; set; }

        public byte[] BuyCraftRareRate { get; set; }

        public byte[] RarifyRate { get; set; }

        public byte[] SpUpFailRate { get; set; }

        public byte[] SpDestroyRate { get; set; }

        public byte[] RareItemUpgradeFixRateRate { get; set; }

        public byte[] ItemUpgradeFixRate { get; set; }

        public byte[] ItemUpgradeFailRate { get; set; }

        public byte[] R8ItemUpgradeFixRate { get; set; }

        public byte[] R8ItemUpgradeFailRate { get; set; }


        #endregion

        #region Rates Event

        public byte[] SPUpLuckRateEvent { get; set; }

        public byte[] ItemUpgradeFailRateEvent { get; set; }

        public byte[] R8ItemUpgradeFailRateEvent { get; set; }

        public byte[] RarifyRateEvent { get; set; }

        #endregion
    }
}
