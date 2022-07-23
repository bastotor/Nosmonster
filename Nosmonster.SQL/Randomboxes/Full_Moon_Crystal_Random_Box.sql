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

(0, 10, 1030, 0, 0, 0, 0, 5825, 50), /* 10x Full Moon Crystal */
(0, 20, 1030, 0, 0, 0, 0, 5825, 50), /* 20x Full Moon Crystal */
(0, 50, 1030, 0, 0, 0, 0, 5825, 30), /* 50x Full Moon Crystal */
(0, 100, 1030, 0, 0, 0, 0, 5825, 20), /* 100x Full Moon Crystal */
(0, 200, 1030, 0, 0, 0, 0, 5825, 10), /* 200x Full Moon Crystal */
(0, 999, 1030, 0, 0, 0, 0, 5825, 5); /* 999x Full Moon Crystal */

