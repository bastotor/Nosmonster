using NOSTALE.CONFIG.Config;

namespace NOSTALE.CONFIG.ApplyConfig
{
    //S3-Nosmonster
    public class ServerConfigurationS3
    {
        #region Singleton

        private static ServerConfigurationS3 _instance;

        public static ServerConfigurationS3 Instance => _instance ?? (_instance = new ServerConfigurationS3());

        #endregion

        #region Properties

        #region RatesItem Config

        public RateServer RateServer { get; set; } = new RateServer
        {
            RateXP = 10,
            RateHeroicXP = 10,
            RateGold = 3,
            RateReputation = 2,
            RateGoldDrop = 5,
            MaxGold = int.MaxValue,
            RateDrop = 1,
            MaxLevel = 99,
            MaxJobLevel = 99,
            HeroicStartLevel = 80,
            MaxSPLevel = 99,
            MaxHeroLevel = 60,
            RateFairyXP = 15,
            PartnerSpXp = 10,
            MaxUpgrade = 10
        };

        public RateItem RateItem { get; set; } = new RateItem
        {
            RareRate = new byte[] { 100, 80, 70, 50, 30, 15, 5, 1 },
            BuyCraftRareRate = new byte[] { 100, 100, 63, 48, 35, 24, 14, 6 },
            RarifyRate = new byte[] { 80, 70, 60, 40, 30, 15, 10, 5, 3, 2, 1 },
            SpUpFailRate = new byte[] { 0, 0, 20, 50, 50, 60, 65, 70, 75, 80, 90, 93, 95, 97, 99 },
            SpDestroyRate = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            ItemUpgradeFixRate = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            ItemUpgradeFailRate = new byte[] { 0, 0, 0, 10, 20, 50, 70, 75, 80, 95 },
            R8ItemUpgradeFixRate = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            R8ItemUpgradeFailRate = new byte[] { 0, 0, 0, 0, 20, 50, 50, 50, 80, 95 },
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