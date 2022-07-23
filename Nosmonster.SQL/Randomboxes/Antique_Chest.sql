INSERT INTO [main_ventus].[dbo].[RollGeneratedItem] (
	[IsRareRandom],
	[ItemGeneratedAmount],
	[ItemGeneratedVNum],
	[ItemGeneratedDesign],
	[MaximumOriginalItemRare],
	[MinimumOriginalItemRare],
	[OriginalItemDesign],
	[OriginalItemVNum],
	[Probability]
	
)
VALUES
/*Schau es dir von oben ab, es ist in genau der Reihenfolge*/

(0, 10, 5895, 0, 0, 0, 0, 1464, 50), /* Excellent Medal of Honour */
(0, 200, 1076, 0, 0, 0, 0, 1464, 100), /* Giant recovery potion */
(0, 10, 1363, 0, 0, 0, 0, 1464, 25), /* Niedrige SP Schutzrolle */
(0, 10, 1364, 0, 0, 0, 0, 1464, 25), /* Hohe SP Schutzrolle */
(0, 5, 1362, 0, 0, 0, 0, 1464, 50), /* Soulstone Blessing */
(0, 5, 1452, 0, 0, 0, 0, 1464, 50), /* Ancelloan Blessing */
(0, 25, 1246, 0, 0, 0, 0, 1464, 100), /* Attack Potion */
(0, 25, 1247, 0, 0, 0, 0, 1464, 100), /* Defence Potion */
(0, 25, 1248, 0, 0, 0, 0, 1464, 100), /* Energy Potion */
(0, 2, 1249, 0, 0, 0, 0, 1464, 100), /* Experience Potion */
(0, 5, 1296, 0, 0, 0, 0, 1464, 50); /* Fairy Booster */
