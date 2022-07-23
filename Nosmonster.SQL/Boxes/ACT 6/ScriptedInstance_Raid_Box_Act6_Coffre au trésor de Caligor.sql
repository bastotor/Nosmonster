/* Info :
Rewards Vnum : 5958  Name : Coffre au trésor de Caligor
*/
DECLARE @BoxId SMALLINT = 5958 
DECLARE @BoxDesign SMALLINT = 99
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4493', '0','1', '0', '2'),/* Carte de spécialiste du Mortifère */
    (@BoxDesign, @BoxId, '0', '7', '4492', '0','1', '0', '2'),/* Carte de spécialiste du Chasseur de démons */
    (@BoxDesign, @BoxId, '0', '7', '4491', '0','1', '0', '2'),/* Carte de spécialiste du Devin */
    (@BoxDesign, @BoxId, '0', '7', '4490', '0','1', '0', '2'),/* Corne dorée de Caligor */
    (@BoxDesign, @BoxId, '0', '7', '2513', '0','1', '0', '2'),/* Coeur de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2282', '0','30', '0', '2'),/* Plume d'ange */
    (@BoxDesign, @BoxId, '0', '7', '2285', '0','1', '0', '2'),/* Â2me étincelante bleue */
    (@BoxDesign, @BoxId, '0', '7', '2514', '0','1', '0', '2'),/* Petit rubis de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2515', '0','1', '0', '2'),/* Petit saphir de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2516', '0','1', '0', '2'),/* Petite obsidienne de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2517', '0','1', '0', '2'),/* Petite topaze de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2518', '0','1', '0', '2'),/* Rubis de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2519', '0','1', '0', '2'),/* Saphir de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '9388', '0','1', '0', '2')/* Servile */
