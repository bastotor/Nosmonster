/* Info :
Rewards Vnum : 1877  Name : Sac d'Ibrahim
*/
DECLARE @BoxId SMALLINT = 1877 
DECLARE @BoxDesign SMALLINT = 9
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '1876', '0','1', '0', '2'),/* Clé en or d'Ibrahim */
    (@BoxDesign, @BoxId, '0', '7', '1872', '0','1', '0', '2'),/* Pièce d'or de la bande des voleurs */
    (@BoxDesign, @BoxId, '0', '7', '2349', '0','1', '0', '2'),/* Pierre précieuse brillante bleu ciel */
    (@BoxDesign, @BoxId, '0', '7', '9366', '0','1', '0', '2')/* Voleur expert */
