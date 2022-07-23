﻿using OpenNos.Data;

namespace OpenNos.GameObject
{
    public class FamilySkillMission : FamilySkillMissionDTO
    {

        #region Instantion
        public FamilySkillMission(FamilySkillMissionDTO input)
        {
            FamilyId = input.FamilyId;
            ItemVNum = input.ItemVNum;
            CurrentValue = input.CurrentValue;
            Date = input.Date;
            TotalValue = input.TotalValue;
        }
        #endregion

    }
}
