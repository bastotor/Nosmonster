﻿using OpenNos.DAL.EF;
using OpenNos.GameObject.Helpers;
using System.Linq;

namespace OpenNos.GameObject.Extension
{
    public static class FamilySystemExtensions
    {
        #region Methods

        public static string GenerateFmi(this Character c)
        {
            if (c.Family == null) return $"fmi ";

            var prepData = FamilySystemHelper.GetFmiPrepSortedData1();
            string packet = string.Empty;
            byte i = 0;
            byte state = 0;

            do
            {
                foreach (var m in prepData)
                {
                    if (c.Family.FamilySkillMissions.FirstOrDefault(mission => mission.ItemVNum.Equals(m.Key)) is FamilySkillMission mission && mission != null)
                    {
                        state = c.Family.CheckFsmStatus(m.Value, m.Key);

                        packet +=
                            $"{i}|" +
                            $"{9000 + m.Value[3]}|" +
                            $"{state}|" +
                            $"{(state == 1 ? mission.Date.Day + mission.Date.Month * 100 + mission.Date.Year * 10000 : mission.CurrentValue)}|" +
                            $"{mission.TotalValue} ";
                    }
                    else
                    {
                        state = c.Family.CheckFsmStatus(m.Value, m.Key);
                        packet += $"{i}|{9000 + m.Value[3]}|{state}|0|0 ";
                    }
                }

                if (i == 0) prepData = FamilySystemHelper.GetFmiPrepSortedData2();
                i++;
            }
            while (i < 2);

            return $"fmi {packet}";
        }

        public static string GenerateFmp(this Character c)
        {
            if (c.Family == null) return $"fmp 0|0";

            var prepData = FamilySystemHelper.GetFmpPrepSortedData();
            string packet = string.Empty;
            byte typeSort = 0;

            foreach (var check in prepData)
            {
                if (check.Value[2] != 0 && typeSort == check.Value[2])
                    continue;

                if (c.Family.FamilySkillMissions.Any(s => s.ItemVNum == check.Key))
                {
                    packet += $"{check.Key}|{(check.Value[0] == 0 ? c.Family.CheckBuff(check.Key) ? 1 : 2 : 0)} ";  //Edit this 0 into used/ready to use for buffs/skills
                    typeSort = (byte)check.Value[2];
                }
            }
            return $"fmp {packet}";
        }



        #endregion
    }
}
