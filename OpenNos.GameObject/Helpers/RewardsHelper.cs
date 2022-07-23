using OpenNos.Core;
using OpenNos.DAL;
using OpenNos.Data;
using OpenNos.Domain;
using System;
using System.Linq;

namespace OpenNos.GameObject.Helpers
{
    public class RewardsHelper
    {
        #region Methods
        private static RewardsHelper _instance;

        public static RewardsHelper Instance => _instance ?? (_instance = new RewardsHelper());

        public void DailyReward(ClientSession session)
        {
            var isMartial = session.Character.Class.Equals(ClassType.MartialArtist);
            var count = DAOFactory.GeneralLogDAO.LoadByAccount(session.Account.AccountId)
                .Count(s => s.LogData == (isMartial ? "DAILY_REWARD" : "DAILY_REWARD") && s.Timestamp.Day >= DateTime.Now.Day);
            if (count != 0)
            {
                return;
            }
            session.Character.GiftAdd((short)(isMartial ? 5974 : 5974), (short)(isMartial ? 1 : 1));
            session.SendPacket(UserInterfaceHelper.GenerateInfo(string.Format(Language.Instance.GetMessageFromKey(isMartial ? "DAILY_REWARD" : "DAILY_REWARD"), 0)));

            session.Character.GeneralLogs.Add(new GeneralLogDTO
            {
                AccountId = session.Account.AccountId,
                CharacterId = session.Character.CharacterId,
                IpAddress = session.IpAddress,
                LogData = isMartial ? "DAILY_REWARD" : "DAILY_REWARD",
                LogType = "World",
                Timestamp = DateTime.Now
            });
        }
        public static int ArenaXpReward(byte characterLevel)
        {
            if (characterLevel <= 39)
            {
                // 25%
                return (int)(CharacterHelper.XPData[characterLevel] / 4);
            }

            if (characterLevel <= 55)
            {
                // 20%
                return (int)(CharacterHelper.XPData[characterLevel] / 5);
            }

            if (characterLevel <= 75)
            {
                // 10%
                return (int)(CharacterHelper.XPData[characterLevel] / 10);
            }

            if (characterLevel <= 79)
            {
                // 5%
                return (int)(CharacterHelper.XPData[characterLevel] / 20);
            }

            if (characterLevel <= 85)
            {
                // 2%
                return (int)(CharacterHelper.XPData[characterLevel] / 50);
            }

            if (characterLevel <= 90)
            {
                return (int)(CharacterHelper.XPData[characterLevel] / 80);
            }

            if (characterLevel <= 93)
            {
                return (int)(CharacterHelper.XPData[characterLevel] / 100);
            }

            if (characterLevel <= 99)
            {
                return (int)(CharacterHelper.XPData[characterLevel] / 1000);
            }

            return 0;
        }

        #endregion
    }
}