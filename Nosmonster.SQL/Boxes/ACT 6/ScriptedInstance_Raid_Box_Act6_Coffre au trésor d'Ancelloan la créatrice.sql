/* Info :
Rewards Vnum : 4717  Name : Coffre au trésor d'Ancelloan la créatrice
*/
DECLARE @BoxId SMALLINT = 4717 
DECLARE @BoxDesign SMALLINT = 25
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4981', '0','1', '0', '2'),/* La lame droite de Krem brisé */
    (@BoxDesign, @BoxId, '0', '7', '4982', '0','1', '0', '2'),/* Branche de lumière de Seli-Lusha */
    (@BoxDesign, @BoxId, '0', '7', '4840', '0','1', '0', '2'),/* Masque d'Azraël */
    (@BoxDesign, @BoxId, '0', '7', '4958', '0','1', '0', '2'),/* Épée céleste scellée */
    (@BoxDesign, @BoxId, '0', '7', '4960', '0','1', '0', '2'),/* Arc céleste scellé */
    (@BoxDesign, @BoxId, '0', '7', '4959', '0','1', '0', '2'),/* Bâton céleste scellé */
    (@BoxDesign, @BoxId, '0', '7', '4955', '0','1', '0', '2'),/* Arbalète céleste scellée */
    (@BoxDesign, @BoxId, '0', '7', '4957', '0','1', '0', '2'),/* Dague céleste sacrée */
    (@BoxDesign, @BoxId, '0', '7', '4956', '0','1', '0', '2'),/* Arme enchantée céleste scellée */
    (@BoxDesign, @BoxId, '0', '7', '4949', '0','1', '0', '2'),/* Armure céleste scellée lourde */
    (@BoxDesign, @BoxId, '0', '7', '4951', '0','1', '0', '2'),/* Armure céleste scellée en cuir */
    (@BoxDesign, @BoxId, '0', '7', '4950', '0','1', '0', '2'),/* Robe céleste scellée */
    (@BoxDesign, @BoxId, '0', '7', '2431', '0','1', '0', '2'),/* Oeuf de Fernon */
    (@BoxDesign, @BoxId, '0', '7', '1685', '0','1', '0', '2'),/* Ailes d'ange */
    (@BoxDesign, @BoxId, '0', '7', '5431', '0','1', '0', '2'),/* Ailes archangéliques */
    (@BoxDesign, @BoxId, '0', '7', '1025', '0','1', '0', '2'),/* Cellon (niveau 9) */
    (@BoxDesign, @BoxId, '0', '7', '2816', '0','1', '0', '2'),/* Morceau de métal inconnu */
    (@BoxDesign, @BoxId, '0', '7', '2814', '0','1', '0', '2'),/* Soie */
    (@BoxDesign, @BoxId, '0', '7', '2811', '0','1', '0', '2'),/* Orichalque abîmé */
    (@BoxDesign, @BoxId, '0', '7', '2805', '0','1', '0', '2'),/* Cristal de la balance */
    (@BoxDesign, @BoxId, '0', '7', '2819', '0','1', '0', '2'),/* Fil doré */
    (@BoxDesign, @BoxId, '0', '7', '4736', '0','1', '0', '2'),/* Poing de dieu */
    (@BoxDesign, @BoxId, '0', '7', '9380', '0','1', '0', '2')/* Messager d'Ancelloan */
