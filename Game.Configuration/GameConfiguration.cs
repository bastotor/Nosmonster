using Game.Configuration.Item;
using System.Collections.Generic;

namespace Game.Configuration
{
    public class GameConfiguration
    {
        public static RemoveRuneConfiguration RRemove { get; set; } = new RemoveRuneConfiguration
        {
            ItemVnum = 5812
        };

        public static UpgradeRuneConfiguration RUpgrade { get; set; } = new UpgradeRuneConfiguration
        {
            GoldPrice = new[]
     {
            3000, 16000, 99000, 55000, 110000,
            280000, 220000, 310000, 500000, 450000,
            560000, 790000, 700000, 880000, 1100000,
            1320000, 1584000, 1916640, 1742400, 1916640,
            2108304
        },
            PercentSucess = new[]
 {
            100, 90, 79, 65, 45,
            25, 20, 15, 7, 7,
            5, 1, 3, 2, 0.5,
            2, 1, 0.3, 1.5, 1,
            0.1
        },
            PercentFail = new[]
 {
            0, 10, 17, 30, 49,
            68, 72, 76, 83, 81,
            81, 84, 79, 78, 74.5,
            77, 77, 74.7, 75.5, 75,
            74.9
        },
            PercentBreaked = new double[]
 {
            0, 0, 4, 5, 6,
            7, 8, 9, 10, 12,
            14, 15, 18, 20, 25,
            21, 22, 25, 23, 24,
            25
 },

            // 2416 2411 2430 2475 2413 2462
            Item = new[]
     {
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 10
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 5
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 12
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 7
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 16
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 11
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 10
                    },
                    new RequiredItem
                    {
                        Id = 2475,
                        Quantity = 1
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 14
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 9
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 16
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 11
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 20
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 15
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 15
                    },
                    new RequiredItem
                    {
                        Id = 2475,
                        Quantity = 1
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 18
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 13
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 20
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 15
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 30
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 30
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 20
                    },
                    new RequiredItem
                    {
                        Id = 2413,
                        Quantity = 1
                    },
                    new RequiredItem
                    {
                        Id = 2475,
                        Quantity = 1
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 27
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 26
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 29
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 29
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 36
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 38
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 25
                    },
                    new RequiredItem
                    {
                        Id = 2413,
                        Quantity = 1
                    },
                    new RequiredItem
                    {
                        Id = 2462,
                        Quantity = 1
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 31
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 32
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 34
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 35
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 48
                    },
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 45
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 30
                    },
                    new RequiredItem
                    {
                        Id = 2413,
                        Quantity = 1
                    },
                    new RequiredItem
                    {
                        Id = 2462,
                        Quantity = 1
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2483,
                        Quantity = 40
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 24
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2483,
                        Quantity = 42
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 25
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2483,
                        Quantity = 66
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 35
                    },
                    new RequiredItem
                    {
                        Id = 2484,
                        Quantity = 1
                    },
                    new RequiredItem
                    {
                        Id = 2482,
                        Quantity = 1
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2483,
                        Quantity = 45
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 27
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2483,
                        Quantity = 48
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 29
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2483,
                        Quantity = 80
                    },
                    new RequiredItem
                    {
                        Id = 2430,
                        Quantity = 40
                    },
                    new RequiredItem
                    {
                        Id = 2484,
                        Quantity = 2
                    },
                    new RequiredItem
                    {
                        Id = 2482,
                        Quantity = 2
                    }
                },
            }
        };

        public static RemoveTattooConfiguration TRemove { get; set; } = new RemoveTattooConfiguration
        {
            ItemVnum = 5799
        };

        public static UpgradeTattooConfiguration TUpgrade { get; set; } = new UpgradeTattooConfiguration
        {
            GoldPrice = new[] { 30000, 67000, 140000, 230000, 380000, 540000, 770000, 960000, 1200000 },
            PercentSucess = new[] { 80, 60, 50, 35, 20, 10, 5, 2, 1 },
            PercentFail = new[] { 20, 30, 35, 40, 50, 55, 45, 28, 9 },
            PercentDestroyed = new[] { 0, 10, 15, 25, 30, 35, 50, 70, 90 },
            Item = new[]
            {
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 17
                    },
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 7
                    },
                    new RequiredItem
                    {
                        Id = 2408,
                        Quantity = 6
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 19
                    },
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 10
                    },
                    new RequiredItem
                    {
                        Id = 2408,
                        Quantity = 7
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 21
                    },
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 13
                    },
                    new RequiredItem
                    {
                        Id = 2408,
                        Quantity = 8
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 25
                    },
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 16
                    },
                    new RequiredItem
                    {
                        Id = 2408,
                        Quantity = 9
                    },
                    new RequiredItem
                    {
                        Id = 2410,
                        Quantity = 15
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 30
                    },
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 20
                    },
                    new RequiredItem
                    {
                        Id = 2408,
                        Quantity = 10
                    },
                    new RequiredItem
                    {
                        Id = 2410,
                        Quantity = 20
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 35
                    },
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 25
                    },
                    new RequiredItem
                    {
                        Id = 2408,
                        Quantity = 12
                    },
                    new RequiredItem
                    {
                        Id = 2410,
                        Quantity = 25
                    },
                    new RequiredItem
                    {
                        Id = 2474,
                        Quantity = 3
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 60
                    },
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 30
                    },
                    new RequiredItem
                    {
                        Id = 2408,
                        Quantity = 20
                    },
                    new RequiredItem
                    {
                        Id = 2410,
                        Quantity = 20
                    },
                    new RequiredItem
                    {
                        Id = 2474,
                        Quantity = 4
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 80
                    },
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 40
                    },
                    new RequiredItem
                    {
                        Id = 2408,
                        Quantity = 30
                    },
                    new RequiredItem
                    {
                        Id = 2410,
                        Quantity = 25
                    },
                    new RequiredItem
                    {
                        Id = 2412,
                        Quantity = 3
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 2411,
                        Quantity = 100
                    },
                    new RequiredItem
                    {
                        Id = 2416,
                        Quantity = 80
                    },
                    new RequiredItem
                    {
                        Id = 2408,
                        Quantity = 40
                    },
                    new RequiredItem
                    {
                        Id = 2410,
                        Quantity = 40
                    },
                    new RequiredItem
                    {
                        Id = 2412,
                        Quantity = 4
                    }
                }
            }
        };

        public static CraftA6FairyConfiguration Fairy { get; set; } = new CraftA6FairyConfiguration
        {
            FairyVnum = new[] { 4129, 4130, 4131, 4132 }, // 4129, 4130, 4131, 4132
            GoldPrice = new[] { 2000000, 2000000, 50000000 }, //2000000, 2000000, 50000000
            PercentSucess = new[] { 10, 10, 100 }, //10, 10, 100
            SuccesVnumFairy = new[] { 4705, 4709, 4713 }, //4705, 4709, 4713
            Item = new[]
            {
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 5875,
                        Quantity = 1
                    },
                    new RequiredItem
                    {
                        Id = 2434,
                        Quantity = 3
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 5876,
                        Quantity = 1
                    },
                    new RequiredItem
                    {
                        Id = 2435,
                        Quantity = 2
                    }
                },
                new List<RequiredItem>
                {
                    new RequiredItem
                    {
                        Id = 5877,
                        Quantity = 1
                    },
                    new RequiredItem
                    {
                        Id = 2444,
                        Quantity = 1
                    }
                }
            }
        };
    }
}