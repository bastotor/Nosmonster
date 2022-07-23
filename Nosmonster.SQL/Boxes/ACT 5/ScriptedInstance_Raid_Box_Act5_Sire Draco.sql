/* Info :
Rewards Vnum : 302  Name : Sire Draco
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 16
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4500', '0','1', '0', '2'),/* Carte de spécialiste du Gladiateur */
    (@BoxDesign, @BoxId, '0', '7', '4501', '0','1', '0', '2'),/* Carte de spécialiste du Canonnier de feu */
    (@BoxDesign, @BoxId, '0', '7', '4502', '0','1', '0', '2'),/* Carte de spécialiste du Volcanor */
    (@BoxDesign, @BoxId, '0', '7', '4486', '0','1', '0', '2'),/* Carte de spécialiste de Drakenfer */
    (@BoxDesign, @BoxId, '0', '7', '2511', '0','3', '0', '2'),/* Peau de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2512', '0','2', '0', '2'),/* Sang de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2513', '0','1', '0', '2'),/* Coeur de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2514', '0','1', '0', '2'),/* Petit rubis de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2516', '0','1', '0', '2'),/* Petite obsidienne de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2518', '0','1', '0', '2'),/* Rubis de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2282', '0','6', '0', '2'),/* Plume d'ange */
    (@BoxDesign, @BoxId, '0', '7', '1030', '0','3', '0', '2'),/* Cristal de pleine lune */
    (@BoxDesign, @BoxId, '0', '7', '1249', '0','1', '0', '2'),/* Potion d'augmentation de l'expérience */
    (@BoxDesign, @BoxId, '0', '7', '9372', '0','1', '0', '2')/* Pourfendeur de dragons */
