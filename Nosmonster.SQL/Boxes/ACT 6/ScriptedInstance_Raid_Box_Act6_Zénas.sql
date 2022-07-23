/* Info :
Rewards Vnum : 302  Name : Zénas
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 23
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4958', '0','1', '0', '2'),/* Épée céleste scellée */
    (@BoxDesign, @BoxId, '0', '7', '4960', '0','1', '0', '2'),/* Arc céleste scellé */
    (@BoxDesign, @BoxId, '0', '7', '4959', '0','1', '0', '2'),/* Bâton céleste scellé */
    (@BoxDesign, @BoxId, '0', '7', '4955', '0','1', '0', '2'),/* Arbalète céleste scellée */
    (@BoxDesign, @BoxId, '0', '7', '4957', '0','1', '0', '2'),/* Dague céleste sacrée */
    (@BoxDesign, @BoxId, '0', '7', '4956', '0','1', '0', '2'),/* Arme enchantée céleste scellée */
    (@BoxDesign, @BoxId, '0', '7', '4949', '0','1', '0', '2'),/* Armure céleste scellée lourde */
    (@BoxDesign, @BoxId, '0', '7', '4951', '0','1', '0', '2'),/* Armure céleste scellée en cuir */
    (@BoxDesign, @BoxId, '0', '7', '4950', '0','1', '0', '2'),/* Robe céleste scellée */
    (@BoxDesign, @BoxId, '0', '7', '4941', '0','1', '0', '2'),/* Casque céleste scellé */
    (@BoxDesign, @BoxId, '0', '7', '4971', '0','1', '0', '2'),/* Chapeau céleste scellé en cuir */
    (@BoxDesign, @BoxId, '0', '7', '4973', '0','1', '0', '2'),/* Serre-tête céleste scellé */
    (@BoxDesign, @BoxId, '0', '7', '4976', '0','1', '0', '2'),/* Talons hauts luxueux de Zénas */
    (@BoxDesign, @BoxId, '0', '7', '4967', '0','1', '0', '2'),/* Gants célestes scellés */
    (@BoxDesign, @BoxId, '0', '7', '4968', '0','1', '0', '2'),/* Chaussures célestes scellées */
    (@BoxDesign, @BoxId, '0', '7', '1685', '0','1', '0', '2'),/* Ailes d'ange */
    (@BoxDesign, @BoxId, '0', '7', '5431', '0','1', '0', '2'),/* Ailes archangéliques */
    (@BoxDesign, @BoxId, '0', '7', '2814', '0','3', '0', '2'),/* Soie */
    (@BoxDesign, @BoxId, '0', '7', '2805', '0','3', '0', '2'),/* Cristal de la balance */
    (@BoxDesign, @BoxId, '0', '7', '2812', '0','3', '0', '2'),/* Orichalque intact */
    (@BoxDesign, @BoxId, '0', '7', '2517', '0','3', '0', '2'),/* Petite topaze de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2521', '0','3', '0', '2'),/* Topaze de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2513', '0','2', '0', '2'),/* Coeur de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2285', '0','3', '0', '2'),/* Â2me étincelante bleue */
    (@BoxDesign, @BoxId, '0', '7', '2282', '0','10', '0', '2'),/* Plume d'ange */
    (@BoxDesign, @BoxId, '0', '7', '5886', '0','1', '0', '2'),/* Parchemin de fabrication d'armes secondaires d'Ancelloan */
    (@BoxDesign, @BoxId, '0', '7', '5887', '0','1', '0', '2'),/* Parchemin de fabrication d'armures d'Ancelloan */
    (@BoxDesign, @BoxId, '0', '7', '1025', '0','1', '0', '2'),/* Cellon (niveau 9) */
    (@BoxDesign, @BoxId, '0', '7', '2427', '0','1', '0', '2'),/* Oeuf de Zénas */
    (@BoxDesign, @BoxId, '0', '7', '4755', '0','1', '0', '2'),/* Anneau céleste de Zénas */
    (@BoxDesign, @BoxId, '0', '7', '4767', '0','1', '0', '2'),/* Insigne du dragon bleu magique */
    (@BoxDesign, @BoxId, '0', '7', '9378', '0','1', '0', '2')/* Angelot */
