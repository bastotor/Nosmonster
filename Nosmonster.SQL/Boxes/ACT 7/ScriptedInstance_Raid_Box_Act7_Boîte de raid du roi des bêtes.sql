/* Info :
Rewards Vnum : 4461  Name : Boîte de raid du roi des bêtes
*/
DECLARE @BoxId SMALLINT = 4461 
DECLARE @BoxDesign SMALLINT = 31
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4507', '0','1', '0', '2'),/* Heaume des bêtes antiques */
    (@BoxDesign, @BoxId, '0', '7', '4508', '0','1', '0', '2'),/* Heaume cérémoniel */
    (@BoxDesign, @BoxId, '0', '7', '4509', '0','1', '0', '2'),/* Gants des bêtes antiques */
    (@BoxDesign, @BoxId, '0', '7', '4511', '0','1', '0', '2'),/* Chaussures des bêtes antiques */
    (@BoxDesign, @BoxId, '0', '7', '5763', '0','10', '0', '2'),/* Jus de mousse orc */
    (@BoxDesign, @BoxId, '0', '7', '2466', '0','10', '0', '2'),/* Plume de pikanya */
    (@BoxDesign, @BoxId, '0', '7', '2410', '0','8', '0', '2'),/* Pierre de jade */
    (@BoxDesign, @BoxId, '0', '7', '2464', '0','5', '0', '2'),/* Crinière de hibours */
    (@BoxDesign, @BoxId, '0', '7', '2459', '0','5', '0', '2'),/* Cuir animal de Mourice */
    (@BoxDesign, @BoxId, '0', '7', '2411', '0','4', '0', '2'),/* Poudre runique loa */
    (@BoxDesign, @BoxId, '0', '7', '2416', '0','4', '0', '2'),/* Obsidienne de Mourice */
    (@BoxDesign, @BoxId, '0', '7', '2413', '0','1', '0', '2'),/* Fragment de corne du roi des bêtes */
    (@BoxDesign, @BoxId, '0', '7', '1026', '0','1', '0', '2'),/* Cellon (niveau 10) */
    (@BoxDesign, @BoxId, '0', '7', '4447', '0','1', '0', '2'),/* Épée de chef de guerre orc */
    (@BoxDesign, @BoxId, '0', '7', '4450', '0','1', '0', '2'),/* Arc du maître-archer orc */
    (@BoxDesign, @BoxId, '0', '7', '4453', '0','1', '0', '2'),/* Bâton divin du maître sorcier */
    (@BoxDesign, @BoxId, '0', '7', '4456', '0','1', '0', '2'),/* Gants de plaques orcs en sekrass */
    (@BoxDesign, @BoxId, '0', '7', '4459', '0','1', '0', '2'),/* Arbalète de chef de guerre orc */
    (@BoxDesign, @BoxId, '0', '7', '4466', '0','1', '0', '2'),/* Dague d'assassin orc */
    (@BoxDesign, @BoxId, '0', '7', '4469', '0','1', '0', '2'),/* Arme enchantée orc */
    (@BoxDesign, @BoxId, '0', '7', '4472', '0','1', '0', '2'),/* Marque de jade en sekrass */
    (@BoxDesign, @BoxId, '0', '7', '4475', '0','1', '0', '2'),/* Armure de pierre sekrass */
    (@BoxDesign, @BoxId, '0', '7', '4478', '0','1', '0', '2'),/* Armure en cuir du maître-chasseur orc */
    (@BoxDesign, @BoxId, '0', '7', '4481', '0','1', '0', '2'),/* Monture de combat du sorcier orc */
    (@BoxDesign, @BoxId, '0', '7', '4484', '0','1', '0', '2'),/* Armure en sekrass légère */
    (@BoxDesign, @BoxId, '0', '7', '5752', '0','1', '0', '2'),/* Parchemin de fabrication d'armes secondaires loas orcs */
    (@BoxDesign, @BoxId, '0', '7', '5753', '0','1', '0', '2')/* Parchemin de fabrication d'armures loas orcs */
