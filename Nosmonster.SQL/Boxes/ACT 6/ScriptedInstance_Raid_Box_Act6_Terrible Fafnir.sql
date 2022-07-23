/* Info :
Rewards Vnum : 302  Name : Terrible Fafnir
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 26
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '2048', '0','1', '0', '2'),/* Cuir supérieur */
    (@BoxDesign, @BoxId, '0', '7', '2044', '0','1', '0', '2'),/* Tissu supérieur */
    (@BoxDesign, @BoxId, '0', '7', '2604', '0','3', '0', '2'),/* Essence du feu à l'état pur */
    (@BoxDesign, @BoxId, '0', '7', '2605', '0','1', '0', '2'),/* Énergie de Fafnir */
    (@BoxDesign, @BoxId, '0', '7', '2606', '0','1', '0', '2'),/* Corne de Fafnir */
    (@BoxDesign, @BoxId, '0', '7', '2607', '0','1', '0', '2'),/* Coeur de Fafnir */
    (@BoxDesign, @BoxId, '0', '7', '2608', '0','1', '0', '2'),/* Molaire de Fafnir */
    (@BoxDesign, @BoxId, '0', '7', '2609', '0','1', '0', '2'),/* Écaille de Fafnir */
    (@BoxDesign, @BoxId, '0', '7', '2200', '0','1', '0', '2'),/* Aiguille */
    (@BoxDesign, @BoxId, '0', '7', '2208', '0','1', '0', '2'),/* Bobine */
    (@BoxDesign, @BoxId, '0', '7', '2511', '0','1', '0', '2'),/* Peau de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2512', '0','1', '0', '2'),/* Sang de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2513', '0','1', '0', '2'),/* Coeur de dragon */
    (@BoxDesign, @BoxId, '0', '7', '1011', '0','10', '0', '2'),/* Potion de récupération géante */
    (@BoxDesign, @BoxId, '0', '7', '5903', '0','1', '0', '2'),/* Schéma du cor de chasse de Fafnir */
    (@BoxDesign, @BoxId, '0', '7', '4129', '0','1', '0', '2'),/* Elkaim */
    (@BoxDesign, @BoxId, '0', '7', '4130', '0','1', '0', '2'),/* Ladine */
    (@BoxDesign, @BoxId, '0', '7', '4131', '0','1', '0', '2'),/* Rumial */
    (@BoxDesign, @BoxId, '0', '7', '4132', '0','1', '0', '2'),/* Varik */
    (@BoxDesign, @BoxId, '0', '7', '4307', '0','1', '0', '2'),/* Ornement d'ailes de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2611', '0','1', '0', '2'),/* Cor de chasse de Fafnir */
    (@BoxDesign, @BoxId, '0', '7', '9381', '0','1', '0', '2')/* Chasseur de trésors */
