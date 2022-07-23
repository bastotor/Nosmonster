/* Info :
Rewards Vnum : 302  Name : Erenia
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 24
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4964', '0','1', '0', '2'),/* Épée infernale scellée */
    (@BoxDesign, @BoxId, '0', '7', '4966', '0','1', '0', '2'),/* Arc infernal scellé */
    (@BoxDesign, @BoxId, '0', '7', '4965', '0','1', '0', '2'),/* Bâton infernal scellé */
    (@BoxDesign, @BoxId, '0', '7', '4961', '0','1', '0', '2'),/* Arbalète infernale scellée */
    (@BoxDesign, @BoxId, '0', '7', '4963', '0','1', '0', '2'),/* Dague infernale scellée */
    (@BoxDesign, @BoxId, '0', '7', '4962', '0','1', '0', '2'),/* Arme enchantée infernale scellée */
    (@BoxDesign, @BoxId, '0', '7', '4952', '0','1', '0', '2'),/* Armure infernale scellée lourde */
    (@BoxDesign, @BoxId, '0', '7', '4954', '0','1', '0', '2'),/* Armure infernale scellée en cuir */
    (@BoxDesign, @BoxId, '0', '7', '4953', '0','1', '0', '2'),/* Robe infernale scellée */
    (@BoxDesign, @BoxId, '0', '7', '4945', '0','1', '0', '2'),/* Casque démoniaque scellé */
    (@BoxDesign, @BoxId, '0', '7', '4972', '0','1', '0', '2'),/* Chapeau infernal scellé en cuir */
    (@BoxDesign, @BoxId, '0', '7', '4974', '0','1', '0', '2'),/* Serre-tête infernal scellé */
    (@BoxDesign, @BoxId, '0', '7', '4975', '0','1', '0', '2'),/* Corne d'Érénia artisanale */
    (@BoxDesign, @BoxId, '0', '7', '4969', '0','1', '0', '2'),/* Gants infernaux scellés */
    (@BoxDesign, @BoxId, '0', '7', '4970', '0','1', '0', '2'),/* Chaussures infernales scellées */
    (@BoxDesign, @BoxId, '0', '7', '1686', '0','1', '0', '2'),/* Ailes de démon */
    (@BoxDesign, @BoxId, '0', '7', '5432', '0','1', '0', '2'),/* Ailes archidémoniaques */
    (@BoxDesign, @BoxId, '0', '7', '2814', '0','3', '0', '2'),/* Soie */
    (@BoxDesign, @BoxId, '0', '7', '2805', '0','3', '0', '2'),/* Cristal de la balance */
    (@BoxDesign, @BoxId, '0', '7', '2812', '0','3', '0', '2'),/* Orichalque intact */
    (@BoxDesign, @BoxId, '0', '7', '2516', '0','3', '0', '2'),/* Petite obsidienne de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2520', '0','3', '0', '2'),/* Obsidienne de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2513', '0','2', '0', '2'),/* Coeur de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2285', '0','3', '0', '2'),/* Â2me étincelante bleue */
    (@BoxDesign, @BoxId, '0', '7', '2282', '0','10', '0', '2'),/* Plume d'ange */
    (@BoxDesign, @BoxId, '0', '7', '5885', '0','1', '0', '2'),/* Parchemin de fabrication d'armes d'Ancelloan */
    (@BoxDesign, @BoxId, '0', '7', '5884', '0','1', '0', '2'),/* Parchemin de fabrication d'accessoires d'Ancelloan */
    (@BoxDesign, @BoxId, '0', '7', '1025', '0','1', '0', '2'),/* Cellon (niveau 9) */
    (@BoxDesign, @BoxId, '0', '7', '2429', '0','1', '0', '2'),/* Oeuf d'Érénia */
    (@BoxDesign, @BoxId, '0', '7', '4770', '0','1', '0', '2'),/* Insigne de la tortue noire magique */
    (@BoxDesign, @BoxId, '0', '7', '9379', '0','1', '0', '2')/* Diablotin */
