using OpenNos.Domain;
using System.Collections.Generic;

namespace OpenNos.GameObject.Helpers
{
    public class ItemHelper
    {
        #region Properties

        public static readonly byte[] RareRate = new byte[] { 100, 100, 90, 80, 60, 40, 30, 30, 100, 100 }; //<=== ici 

        public static readonly byte[] BuyCraftRareRate = new byte[] { 100, 100, 63, 48, 35, 24, 14, 6 };

        public static readonly byte[] RarifyRate = new byte[] { 100, 100, 100, 90, 80, 70, 60, 50, 40, 30, 40 }; // <==== ou ici

        public static readonly byte[] SpUpFailRate = new byte[] { 20, 25, 30, 40, 50, 60, 65, 70, 75, 80, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, };

        public static readonly byte[] SpDestroyRate = new byte[] { 0, 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 70, 0, 0, 0, 0, 0, };

        public static readonly byte[] ItemUpgradeFixRate = new byte[] { 0, 0, 10, 15, 20, 20, 20, 20, 15, 14 };

        public static readonly byte[] ItemUpgradeFailRate = new byte[] { 0, 0, 0, 5, 20, 40, 60, 70, 80, 85 }; 

        public static readonly byte[] R8ItemUpgradeFixRate = new byte[] { 50, 40, 50, 55, 60, 70, 75, 77, 78, 79 }; // <=== ici aussi il me semble

        public static readonly byte[] R8ItemUpgradeFailRate = new byte[] { 50, 40, 60, 50, 60, 70, 75, 77, 83, 89 }; 

        #region Rates Event

        public static readonly byte[] ItemUpgradeFailRateEvent = new byte[] { 0, 0, 0, 5, 20, 40, 60, 70, 80, 85 };

        public static readonly byte[] R8ItemUpgradeFailRateEvent = new byte[] { 0, 0, 0, 0, 30, 40, 45, 55, 55, 60 };

        public static readonly byte[] SPUpLuckRateEvent = new byte[] { 100, 100, 100, 90, 75, 60, 51, 45, 47, 30, 15, 10, 7, 4, 2 };

        #endregion Rates Event

        #endregion

        #region Singleton

        private static PassiveSkillHelper _instance;

        public static PassiveSkillHelper Instance => _instance ?? (_instance = new PassiveSkillHelper());

        #endregion
    }
}