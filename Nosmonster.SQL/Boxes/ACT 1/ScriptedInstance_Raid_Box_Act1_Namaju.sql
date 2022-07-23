/* Info :
Rewards Vnum : 302  Name : Namaju
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 6
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '334', '0','1', '0', '2'),/* Perle scellée de Sakura */
    (@BoxDesign, @BoxId, '0', '7', '227', '0','1', '0', '2'),/* Masque X */
    (@BoxDesign, @BoxId, '0', '7', '182', '0','1', '0', '2'),/* Amulette du niveau d'attaque et de défense */
    (@BoxDesign, @BoxId, '0', '7', '1078', '0','1', '0', '2'),/* Potion de points de spécialiste */
    (@BoxDesign, @BoxId, '0', '7', '1092', '0','3', '0', '2'),/* Morceau d'espace-temps (groupe) */
    (@BoxDesign, @BoxId, '0', '7', '1095', '0','3', '0', '2'),/* Morceau d'espace-temps (chasse) */
    (@BoxDesign, @BoxId, '0', '7', '1012', '0','50', '0', '2'),/* Graine de pouvoir */
    (@BoxDesign, @BoxId, '0', '7', '9363', '0','1', '0', '2')/* Héros de Sakura */
