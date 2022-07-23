/* Info :
Rewards Vnum : 4463  Name : Boîte de raid de Bélial
*/
DECLARE @BoxId SMALLINT = 4463 
DECLARE @BoxDesign SMALLINT = 32
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '5763', '0','10', '0', '2'),/* Jus de mousse orc */
    (@BoxDesign, @BoxId, '0', '7', '2410', '0','8', '0', '2'),/* Pierre de jade */
    (@BoxDesign, @BoxId, '0', '7', '2410', '0','16', '0', '2'),/* Pierre de jade */
    (@BoxDesign, @BoxId, '0', '7', '2411', '0','4', '0', '2'),/* Poudre runique loa */
    (@BoxDesign, @BoxId, '0', '7', '2411', '0','8', '0', '2'),/* Poudre runique loa */
    (@BoxDesign, @BoxId, '0', '7', '2416', '0','4', '0', '2'),/* Obsidienne de Mourice */
    (@BoxDesign, @BoxId, '0', '7', '2416', '0','8', '0', '2'),/* Obsidienne de Mourice */
    (@BoxDesign, @BoxId, '0', '7', '2462', '0','1', '0', '2'),/* Pierre runique de Bélial */
    (@BoxDesign, @BoxId, '0', '7', '2432', '0','1', '0', '2'),/* Épée magique éclatée de Bélial */
    (@BoxDesign, @BoxId, '0', '7', '1026', '0','1', '0', '2'),/* Cellon (niveau 10) */
    (@BoxDesign, @BoxId, '0', '7', '4447', '0','1', '0', '2'),/* Épée de chef de guerre orc */
    (@BoxDesign, @BoxId, '0', '7', '4448', '0','1', '0', '2'),/* Épée au loa lion */
    (@BoxDesign, @BoxId, '0', '7', '4450', '0','1', '0', '2'),/* Arc du maître-archer orc */
    (@BoxDesign, @BoxId, '0', '7', '4451', '0','1', '0', '2'),/* Arc au loa aigle */
    (@BoxDesign, @BoxId, '0', '7', '4453', '0','1', '0', '2'),/* Bâton divin du maître sorcier */
    (@BoxDesign, @BoxId, '0', '7', '4454', '0','1', '0', '2'),/* Bâton au loa serpent */
    (@BoxDesign, @BoxId, '0', '7', '4456', '0','1', '0', '2'),/* Gants de plaques orcs en sekrass */
    (@BoxDesign, @BoxId, '0', '7', '4457', '0','1', '0', '2'),/* Gants de plaques au loa ours */
    (@BoxDesign, @BoxId, '0', '7', '4460', '0','1', '0', '2'),/* Arbalète au loa lion */
    (@BoxDesign, @BoxId, '0', '7', '4467', '0','1', '0', '2'),/* Dague au loa chauve-souris */
    (@BoxDesign, @BoxId, '0', '7', '4470', '0','1', '0', '2'),/* Arme enchantée au loa serpent */
    (@BoxDesign, @BoxId, '0', '7', '4473', '0','1', '0', '2'),/* Marque de jade en sekrass bénie */
    (@BoxDesign, @BoxId, '0', '7', '4476', '0','1', '0', '2'),/* Armure de pierre sekrass au loa lion */
    (@BoxDesign, @BoxId, '0', '7', '4479', '0','1', '0', '2'),/* Armure en cuir au loa aigle */
    (@BoxDesign, @BoxId, '0', '7', '4482', '0','1', '0', '2'),/* Monture de combat au loa serpent */
    (@BoxDesign, @BoxId, '0', '7', '4505', '0','1', '0', '2'),/* Armure légère au loa ours */
    (@BoxDesign, @BoxId, '0', '7', '4521', '0','1', '0', '2'),/* Collier occulte */
    (@BoxDesign, @BoxId, '0', '7', '4517', '0','1', '0', '2'),/* Anneau de jade des esprits */
    (@BoxDesign, @BoxId, '0', '7', '4513', '0','1', '0', '2'),/* Bracelet au loa serpent */
    (@BoxDesign, @BoxId, '0', '7', '5750', '0','1', '0', '2'),/* Parchemin de fabrication d'accessoires loas orcs */
    (@BoxDesign, @BoxId, '0', '7', '5751', '0','1', '0', '2'),/* Parchemin de fabrication d'armes loas orcs */
    (@BoxDesign, @BoxId, '0', '7', '5752', '0','1', '0', '2'),/* Parchemin de fabrication d'armes secondaires loas orcs */
    (@BoxDesign, @BoxId, '0', '7', '5753', '0','1', '0', '2')/* Parchemin de fabrication d'armures loas orcs */
