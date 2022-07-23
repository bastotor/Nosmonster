/* Info :
Rewards Vnum : 302  Name : Slade géant
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 4
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '317', '0','1', '0', '2'),/* Gants divins */
    (@BoxDesign, @BoxId, '0', '7', '322', '0','1', '0', '2'),/* Chaussures de l'Obscurité */
    (@BoxDesign, @BoxId, '0', '7', '264', '0','1', '0', '2'),/* Épée élémentaire brisée */
    (@BoxDesign, @BoxId, '0', '7', '267', '0','1', '0', '2'),/* Arc majestueux */
    (@BoxDesign, @BoxId, '0', '7', '270', '0','1', '0', '2'),/* Baguette magique majestueuse */
    (@BoxDesign, @BoxId, '0', '7', '299', '0','1', '0', '2'),/* Surin */
    (@BoxDesign, @BoxId, '0', '7', '300', '0','1', '0', '2'),/* Arc de pouvoir */
    (@BoxDesign, @BoxId, '0', '7', '301', '0','1', '0', '2'),/* Baguette magique de Kai */
    (@BoxDesign, @BoxId, '0', '7', '9361', '0','1', '0', '2')/* Pierre ancestrale */
