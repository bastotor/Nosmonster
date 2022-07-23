using NOSTALE.CONFIG.Config;

namespace NOSTALE.CONFIG.ApplyConfig
{
    //S1-Nosmonster
    public class ServerConfigurationS1
    {
        #region Singleton

        private static ServerConfigurationS1 _instance;

        public static ServerConfigurationS1 Instance => _instance ?? (_instance = new ServerConfigurationS1());

        #endregion

        #region Properties

        #region RatesItem Config

        public RateServer RateServer { get; set; } = new RateServer
        {
            RateXP = 50,
            RateHeroicXP = 50,
            RateGold = 30,
            RateReputation = 3,
            RateGoldDrop = 200,
            MaxGold = int.MaxValue,
            RateDrop = 1,
            MaxLevel = 99,
            MaxJobLevel = 100,
            HeroicStartLevel = 99,
            MaxSPLevel = 255,
            MaxHeroLevel = 70,
            RateFairyXP = 20,
            PartnerSpXp = 10000,
            MaxUpgrade = 10
        };

        public RateItem RateItem { get; set; } = new RateItem
        {
            RareRate = new byte[] { 100, 100, 90, 80, 60, 40, 30, 30, 100, 100 },
            BuyCraftRareRate = new byte[] { 100, 100, 63, 48, 35, 24, 14, 6 },
            RarifyRate = new byte[] { 100, 100, 100, 90, 80, 70, 60, 50, 40, 30, 40 },
            SpUpFailRate = new byte[] { 0, 0, 20, 50, 50, 60, 65, 70, 75, 80, 90, 93, 95, 97, 99 },
            SpDestroyRate = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            ItemUpgradeFixRate = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            ItemUpgradeFailRate = new byte[] { 0, 0, 0, 10, 20, 50, 70, 75, 80, 95 },
            R8ItemUpgradeFixRate = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            R8ItemUpgradeFailRate = new byte[] { 0, 0, 0, 0, 20, 50, 50, 50, 80, 90 },
            SPUpLuckRateEvent = new byte[] { 100, 100, 100, 90, 75, 60, 51, 45, 47, 30, 15, 10, 7, 4, 2 },
            ItemUpgradeFailRateEvent = new byte[] { 0, 0, 0, 0, 0, 0, 20, 40, 60, 70 },
            R8ItemUpgradeFailRateEvent = new byte[] { 0, 0, 20, 0, 20, 40, 50, 54, 76, 78 },
            RarifyRateEvent = new byte[] { 100, 100, 90, 60, 45, 30, 20, 10, 6, 4, 2 },
            // wtf ??
            RareItemUpgradeFixRateRate = new byte[] { }
        };

        #endregion

        #endregion
    }
}