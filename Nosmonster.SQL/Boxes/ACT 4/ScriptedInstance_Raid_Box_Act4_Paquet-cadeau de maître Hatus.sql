/* Info :
Rewards Vnum : 185  Name : Paquet-cadeau de maître Hatus
*/
DECLARE @BoxId SMALLINT = 185 
DECLARE @BoxDesign SMALLINT = 101
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '565', '0','1', '0', '2'),/* Rune complète (arme) */
    (@BoxDesign, @BoxId, '0', '7', '568', '0','1', '0', '2'),/* Rune spéciale (arme) */
    (@BoxDesign, @BoxId, '0', '7', '571', '0','1', '0', '2'),/* Rune PvP (arme) */
    (@BoxDesign, @BoxId, '0', '7', '574', '0','1', '0', '2'),/* Rune parfaite (arme) */
    (@BoxDesign, @BoxId, '0', '7', '577', '0','1', '0', '2'),/* Rune complète (armure) */
    (@BoxDesign, @BoxId, '0', '7', '580', '0','1', '0', '2'),/* Rune spéciale (armure) */
    (@BoxDesign, @BoxId, '0', '7', '583', '0','1', '0', '2'),/* Rune PvP (armure) */
    (@BoxDesign, @BoxId, '0', '7', '586', '0','1', '0', '2'),/* Rune parfaite (armure) */
    (@BoxDesign, @BoxId, '0', '7', '1430', '0','1', '0', '2'),/* Effaceur magique */
    (@BoxDesign, @BoxId, '0', '7', '1429', '0','5', '0', '2'),/* Perle d'arc-en-ciel */
    (@BoxDesign, @BoxId, '0', '7', '199', '0','1', '0', '2'),/* Lance obscure d'Hatus */
    (@BoxDesign, @BoxId, '0', '7', '940', '0','1', '0', '2'),/* Arc du précipice d'Hatus */
    (@BoxDesign, @BoxId, '0', '7', '941', '0','1', '0', '2'),/* Baguette infernale de Hatus */
    (@BoxDesign, @BoxId, '0', '7', '2516', '0','1', '0', '2'),/* Petite obsidienne de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2439', '0','1', '0', '2'),/* Essence de Turik */
    (@BoxDesign, @BoxId, '0', '7', '4979', '0','1', '0', '2'),/* Arme enchantée du Lorten souterrain */
    (@BoxDesign, @BoxId, '0', '7', '9385', '0','1', '0', '2')/* Coeur de l'Obscurité */
