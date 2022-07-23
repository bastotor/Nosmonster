/* Info :
Rewards Vnum : 302  Name : Glacerus le Rude
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 17
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4497', '0','1', '0', '2'),/* Carte de spécialiste du Moine pugnace */
    (@BoxDesign, @BoxId, '0', '7', '4498', '0','1', '0', '2'),/* Carte de spécialiste de l'Éclaireur */
    (@BoxDesign, @BoxId, '0', '7', '4499', '0','1', '0', '2'),/* Carte de spécialiste de Sa Majesté des marées */
    (@BoxDesign, @BoxId, '0', '7', '4485', '0','1', '0', '2'),/* Carte de spécialiste d'Arts mystiques */
    (@BoxDesign, @BoxId, '0', '7', '2513', '0','1', '0', '2'),/* Coeur de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2282', '0','6', '0', '2'),/* Plume d'ange */
    (@BoxDesign, @BoxId, '0', '7', '1030', '0','3', '0', '2'),/* Cristal de pleine lune */
    (@BoxDesign, @BoxId, '0', '7', '1249', '0','1', '0', '2'),/* Potion d'augmentation de l'expérience */
    (@BoxDesign, @BoxId, '0', '7', '2349', '0','2', '0', '2'),/* Pierre précieuse brillante bleu ciel */
    (@BoxDesign, @BoxId, '0', '7', '2349', '0','3', '0', '2'),/* Pierre précieuse brillante bleu ciel */
    (@BoxDesign, @BoxId, '0', '7', '2515', '0','1', '0', '2'),/* Petit saphir de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2517', '0','1', '0', '2'),/* Petite topaze de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2519', '0','1', '0', '2'),/* Saphir de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '9373', '0','1', '0', '2')/* Glacial */
