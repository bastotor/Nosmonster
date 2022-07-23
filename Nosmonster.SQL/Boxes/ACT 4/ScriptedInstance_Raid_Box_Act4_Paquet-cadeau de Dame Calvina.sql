/* Info :
Rewards Vnum : 942  Name : Paquet-cadeau de Dame Calvina
*/
DECLARE @BoxId SMALLINT = 942 
DECLARE @BoxDesign SMALLINT = 102
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
    (@BoxDesign, @BoxId, '0', '7', '944', '0','1', '0', '2'),/* Épée houleuse de Calvina */
    (@BoxDesign, @BoxId, '0', '7', '945', '0','1', '0', '2'),/* Arc tsunamique de Calvina */
    (@BoxDesign, @BoxId, '0', '7', '946', '0','1', '0', '2'),/* Baguette magique hydrophyte de Calvina */
    (@BoxDesign, @BoxId, '0', '7', '947', '0','1', '0', '2'),/* Arbalète d'épine bleue de Calvina */
    (@BoxDesign, @BoxId, '0', '7', '948', '0','1', '0', '2'),/* Dague de canine bleue de Calvina */
    (@BoxDesign, @BoxId, '0', '7', '949', '0','1', '0', '2'),/* Arme enchantée de la corne bleue de Calvina */
    (@BoxDesign, @BoxId, '0', '7', '2515', '0','1', '0', '2'),/* Petit saphir de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2437', '0','1', '0', '2'),/* Essence de Woondine */
    (@BoxDesign, @BoxId, '0', '7', '4986', '0','1', '0', '2'),/* L'armure de cuir de l'âme argentée de la Couronne de glace */
    (@BoxDesign, @BoxId, '0', '7', '4762', '0','1', '0', '2'),/* Insigne d'argent avec deux chevaux */
    (@BoxDesign, @BoxId, '0', '7', '9386', '0','1', '0', '2')/* Maître de l'Eau */
