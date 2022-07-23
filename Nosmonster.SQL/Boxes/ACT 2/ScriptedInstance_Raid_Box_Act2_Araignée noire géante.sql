/* Info :
Rewards Vnum : 302  Name : Araignée noire géante
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 3
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '316', '0','1', '0', '2'),/* Gant de la tempête */
    (@BoxDesign, @BoxId, '0', '7', '319', '0','1', '0', '2'),/* Chaussures du feu */
    (@BoxDesign, @BoxId, '0', '7', '141', '0','1', '0', '2'),/* Épée assassine */
    (@BoxDesign, @BoxId, '0', '7', '148', '0','1', '0', '2'),/* Arc de siège */
    (@BoxDesign, @BoxId, '0', '7', '155', '0','1', '0', '2'),/* Baguette magique de l'âme */
    (@BoxDesign, @BoxId, '0', '7', '292', '0','1', '0', '2'),/* Arbalète Balenty */
    (@BoxDesign, @BoxId, '0', '7', '290', '0','1', '0', '2'),/* Kriss */
    (@BoxDesign, @BoxId, '0', '7', '294', '0','1', '0', '2'),/* Arme enchantée de la Lumière */
    (@BoxDesign, @BoxId, '0', '7', '298', '0','1', '0', '2'),/* Défenseur splendide */
    (@BoxDesign, @BoxId, '0', '7', '296', '0','1', '0', '2'),/* Robe de lumière */
    (@BoxDesign, @BoxId, '0', '7', '272', '0','1', '0', '2'),/* Robe d'essai */
    (@BoxDesign, @BoxId, '0', '7', '9360', '0','1', '0', '2')/* Poltron */
