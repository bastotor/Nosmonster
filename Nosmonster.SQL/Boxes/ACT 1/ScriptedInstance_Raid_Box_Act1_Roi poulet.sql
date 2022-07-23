/* Info :
Rewards Vnum : 302  Name : Roi poulet
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 5
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '907', '0','1', '0', '2'),/* Carte de spécialiste de la poule */
    (@BoxDesign, @BoxId, '0', '7', '2181', '0','20', '0', '2'),/* Oeuf magique */
    (@BoxDesign, @BoxId, '0', '7', '2027', '0','50', '0', '2'),/* Poulet rôti */
    (@BoxDesign, @BoxId, '0', '7', '254', '0','1', '0', '2'),/* Poussinet caracolant */
    (@BoxDesign, @BoxId, '0', '7', '255', '0','1', '0', '2'),/* Poussinet danseur */
    (@BoxDesign, @BoxId, '0', '7', '256', '0','1', '0', '2'),/* Poussinet furibond */
    (@BoxDesign, @BoxId, '0', '7', '9362', '0','1', '0', '2')/* Pépite */
