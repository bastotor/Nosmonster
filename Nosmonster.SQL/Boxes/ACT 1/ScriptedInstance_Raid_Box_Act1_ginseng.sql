/* Info :
Rewards Vnum : 302  Name : Ginseng
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 1
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '291', '0','1', '0', '2'),/* Arbalète Winslet */
    (@BoxDesign, @BoxId, '0', '7', '289', '0','1', '0', '2'),/* Tranchoir */
    (@BoxDesign, @BoxId, '0', '7', '293', '0','1', '0', '2'),/* Arme enchantée paff */
    (@BoxDesign, @BoxId, '0', '7', '297', '0','1', '0', '2'),/* Défenseur brave */
    (@BoxDesign, @BoxId, '0', '7', '295', '0','1', '0', '2'),/* Regard du gardien */
    (@BoxDesign, @BoxId, '0', '7', '271', '0','1', '0', '2'),/* Tenue bleue du sage */
    (@BoxDesign, @BoxId, '0', '7', '310', '0','1', '0', '2'),/* Bague en cristal antique */
    (@BoxDesign, @BoxId, '0', '7', '309', '0','1', '0', '2'),/* Collier en cristal antique */
    (@BoxDesign, @BoxId, '0', '7', '311', '0','1', '0', '2'),/* Bracelet en cristal antique */
    (@BoxDesign, @BoxId, '0', '7', '9358', '0','1', '0', '2')/* Pouce vert */
