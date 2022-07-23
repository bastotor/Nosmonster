/* Info :
Rewards Vnum : 4718  Name : Coffre au trésor de Fernon la destructrice
*/
DECLARE @BoxId SMALLINT = 4718 
DECLARE @BoxDesign SMALLINT = 25
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4980', '0','1', '0', '2'),/* La lame gauche de Krem brisé */
    (@BoxDesign, @BoxId, '0', '7', '4983', '0','1', '0', '2'),/* Ailes d'Azraël */
    (@BoxDesign, @BoxId, '0', '7', '4839', '0','1', '0', '2'),/* Chaussures de Fernon */
    (@BoxDesign, @BoxId, '0', '7', '4964', '0','1', '0', '2'),/* Épée infernale scellée */
    (@BoxDesign, @BoxId, '0', '7', '4966', '0','1', '0', '2'),/* Arc infernal scellé */
    (@BoxDesign, @BoxId, '0', '7', '4965', '0','1', '0', '2'),/* Bâton infernal scellé */
    (@BoxDesign, @BoxId, '0', '7', '4961', '0','1', '0', '2'),/* Arbalète infernale scellée */
    (@BoxDesign, @BoxId, '0', '7', '4963', '0','1', '0', '2'),/* Dague infernale scellée */
    (@BoxDesign, @BoxId, '0', '7', '4962', '0','1', '0', '2'),/* Arme enchantée infernale scellée */
    (@BoxDesign, @BoxId, '0', '7', '4952', '0','1', '0', '2'),/* Armure infernale scellée lourde */
    (@BoxDesign, @BoxId, '0', '7', '4954', '0','1', '0', '2'),/* Armure infernale scellée en cuir */
    (@BoxDesign, @BoxId, '0', '7', '4953', '0','1', '0', '2'),/* Robe infernale scellée */
    (@BoxDesign, @BoxId, '0', '7', '2431', '0','1', '0', '2'),/* Oeuf de Fernon */
    (@BoxDesign, @BoxId, '0', '7', '1686', '0','1', '0', '2'),/* Ailes de démon */
    (@BoxDesign, @BoxId, '0', '7', '5432', '0','1', '0', '2'),/* Ailes archidémoniaques */
    (@BoxDesign, @BoxId, '0', '7', '1025', '0','1', '0', '2'),/* Cellon (niveau 9) */
    (@BoxDesign, @BoxId, '0', '7', '2816', '0','1', '0', '2'),/* Morceau de métal inconnu */
    (@BoxDesign, @BoxId, '0', '7', '2814', '0','1', '0', '2'),/* Soie */
    (@BoxDesign, @BoxId, '0', '7', '2811', '0','1', '0', '2'),/* Orichalque abîmé */
    (@BoxDesign, @BoxId, '0', '7', '2805', '0','1', '0', '2'),/* Cristal de la balance */
    (@BoxDesign, @BoxId, '0', '7', '2819', '0','1', '0', '2'),/* Fil doré */
    (@BoxDesign, @BoxId, '0', '7', '4754', '0','1', '0', '2'),/* Armure du champion */
    (@BoxDesign, @BoxId, '0', '7', '9380', '0','1', '0', '2')/* Messager d'Ancelloan */
