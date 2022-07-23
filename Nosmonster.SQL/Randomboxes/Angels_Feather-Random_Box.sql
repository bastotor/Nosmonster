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

(0, 10, 2282, 0, 0, 0, 0, 5749, 50), /* 10x Angel's Feather */
(0, 20, 2282, 0, 0, 0, 0, 5749, 50), /* 20x Angel's Feather */
(0, 50, 2282, 0, 0, 0, 0, 5749, 30), /* 50x Angel's Feather */
(0, 100, 2282, 0, 0, 0, 0, 5749, 20), /* 100x Angel's Feather */
(0, 200, 2282, 0, 0, 0, 0, 5749, 10), /* 200x Angel's Feather */
(0, 999, 2282, 0, 0, 0, 0, 5749, 5); /* 999x Angel's Feather */

