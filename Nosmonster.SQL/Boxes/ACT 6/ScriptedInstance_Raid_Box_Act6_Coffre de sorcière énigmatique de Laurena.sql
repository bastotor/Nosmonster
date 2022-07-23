/* Info :
Rewards Vnum : 5988  Name : Coffre de sorcière énigmatique de Laurena
*/
DECLARE @BoxId SMALLINT = 5988 
DECLARE @BoxDesign SMALLINT = 20
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4489', '0','1', '0', '2'),/* Carte de spécialiste du Renégat */
    (@BoxDesign, @BoxId, '0', '7', '4488', '0','1', '0', '2'),/* Carte de spécialiste de l'Ange vengeur */
    (@BoxDesign, @BoxId, '0', '7', '4487', '0','1', '0', '2'),/* Carte de spécialiste de l'Archimage */
    (@BoxDesign, @BoxId, '0', '7', '4813', '0','1', '0', '2'),/* Carte de spécialiste pour partenaire de Laurena */
    (@BoxDesign, @BoxId, '0', '7', '4812', '0','1', '0', '2'),/* Carte de spécialiste pour partenaire de l'archange Lucifer */
    (@BoxDesign, @BoxId, '0', '7', '4699', '0','1', '0', '2'),/* Chapeau de sorcière de Laurena */
    (@BoxDesign, @BoxId, '0', '7', '2282', '0','5', '0', '2'),/* Plume d'ange */
    (@BoxDesign, @BoxId, '0', '7', '2285', '0','1', '0', '2'),/* Â2me étincelante bleue */
    (@BoxDesign, @BoxId, '0', '7', '2397', '0','1', '0', '2'),/* Pierre précieuse jaune */
    (@BoxDesign, @BoxId, '0', '7', '2396', '0','1', '0', '2'),/* Pierre précieuse verte */
    (@BoxDesign, @BoxId, '0', '7', '2514', '0','1', '0', '2'),/* Petit rubis de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2515', '0','1', '0', '2'),/* Petit saphir de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2516', '0','1', '0', '2'),/* Petite obsidienne de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2517', '0','1', '0', '2'),/* Petite topaze de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2518', '0','1', '0', '2'),/* Rubis de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2519', '0','1', '0', '2'),/* Saphir de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2520', '0','1', '0', '2'),/* Obsidienne de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2521', '0','1', '0', '2'),/* Topaze de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2434', '0','1', '0', '2'),/* Essence du crépuscule */
    (@BoxDesign, @BoxId, '0', '7', '4835', '0','1', '0', '2'),/* Collier de Laurena */
    (@BoxDesign, @BoxId, '0', '7', '4836', '0','1', '0', '2'),/* Anneau de Laurena */
    (@BoxDesign, @BoxId, '0', '7', '4837', '0','1', '0', '2'),/* Bracelet de Laurena */
    (@BoxDesign, @BoxId, '0', '7', '9375', '0','1', '0', '2')/* Petite sorcière */
