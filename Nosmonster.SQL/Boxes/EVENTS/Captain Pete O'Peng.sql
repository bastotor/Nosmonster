/* Info :
Rewards Vnum : 302  Name : Ginseng
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 12
INSERT INTO [dbo].[RollGeneratedI2tem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '5141', '0','1', '0', '2'),/* Piece of the Pirate SP (Event) */
    (@BoxDesign, @BoxId, '0', '7', '5144', '0','1', '0', '2'),/* O'Peng's Treasure Map (Event) */
    (@BoxDesign, @BoxId, '0', '7', '5145', '0','1', '0', '2'),/*  Lime Juice (Event) */
    (@BoxDesign, @BoxId, '0', '7', '5146', '0','1', '0', '2'),/* Pirate Flag Signpost */
    (@BoxDesign, @BoxId, '0', '7', '4166', '0','1', '0', '2'),/*  Leona */
    (@BoxDesign, @BoxId, '0', '7', '271', '0','1', '0', '2'),/* Tenue bleue du sage */
    (@BoxDesign, @BoxId, '0', '7', '310', '0','1', '0', '2'),/* Bague en cristal antique */
    (@BoxDesign, @BoxId, '0', '7', '309', '0','1', '0', '2'),/* Collier en cristal antique */
    (@BoxDesign, @BoxId, '0', '7', '311', '0','1', '0', '2'),/* Bracelet en cristal antique */
    (@BoxDesign, @BoxId, '0', '7', '9358', '0','1', '0', '2')/* Pouce vert */
