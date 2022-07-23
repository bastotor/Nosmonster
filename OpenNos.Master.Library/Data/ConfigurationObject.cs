using System;

namespace OpenNos.Master.Library.Data
{
    [Serializable]
    public class ConfigurationObject
    {
        public bool IsAntiCheatEnabled { get; set; }

        public string AntiCheatClientKey { get; set; }

        public string AntiCheatServerKey { get; set; }

        public string Act4IP { get; set; }

        public int Act4Port { get; set; }

        public int SessionLimit { get; set; }

        public bool SceneOnCreate { get; set; }

        public bool WorldInformation { get; set; }

        public int RateXP { get; set; }

        public int RateHeroicXP { get; set; }

        public int RateGold { get; set; }

        public int RateGoldDrop { get; set; }

        public int RateReputation { get; set; }

        public long MaxGold { get; set; }

        public int RateDrop { get; set; }

        public byte MaxLevel { get; set; }

        public byte MaxJobLevel { get; set; }

        public byte MaxHeroLevel { get; set; }


        public byte MaxSPLevel { get; set; }

        public int RateFairyXP { get; set; }

        public long PartnerSpXp { get; set; }

        public byte MaxUpgrade { get; set; }

        public string MallBaseURL { get; set; }

        public string MallAPIKey { get; set; }

        public int QuestDropRate { get; set; }

        public bool UseLogService { get; set; }

        public bool HalloweenEvent { get; set; }

        public bool EasterEvent {get; set; }

        public bool ChristmasEvent { get; set; }

        public bool EstivalEvent { get; set; }

        public bool ValentineEvent { get; set; }

        public bool LunarNewYearEvent { get; set; }

        public bool LockSystem { get; set; }

        public bool AutoLootEnable { get; set; }

        public int DoubleSpUp { get; set; }

        public int DoubleEqUp { get; set; }

        public bool BCardsInArenaTalent { get; set; }

        public bool EnableTimeSpaceQuest { get; set; }

        public bool FamilyExpBuff { get; set; } = false;

        public bool FamilyGoldBuff { get; set; } = false;

        public DateTime TimeExpBuff { get; set; } = DateTime.Now.AddHours(-2);

        public DateTime TimeGoldBuff { get; set; } = DateTime.Now.AddHours(-2);
        public bool DoubleExpe { get; set; }

        public bool DoubleFairyXP { get; set; }

        public bool DoubleRaidBox { get; set; }

        public bool DobleSpUp { get; set; }

        public bool DobleEqUp { get; set; }

        public bool dobleapuestaUp { get; set; }

        public bool DoblePerfectionUp { get; set; }

        public bool DobleExpeFamily { get; set; }

        public bool DobleGold { get; set; }

        public bool DobleReput { get; set; }

        public bool DobleDrop { get; set; }

        public bool MultiEvent { get; set; }

        public byte HeroicStartLevel { get; set; }
    }
}
