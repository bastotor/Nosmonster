/* Info :
Rewards Vnum : 882  Name : Coffre de maître Morcos
*/
DECLARE @BoxId SMALLINT = 882 
DECLARE @BoxDesign SMALLINT = 100
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
    (@BoxDesign, @BoxId, '0', '7', '350', '0','1', '0', '2'),/* Schiavona */
    (@BoxDesign, @BoxId, '0', '7', '354', '0','1', '0', '2'),/* Arc de la destruction */
    (@BoxDesign, @BoxId, '0', '7', '357', '0','1', '0', '2'),/* Baguette magique de Taracen */
    (@BoxDesign, @BoxId, '0', '7', '264', '0','1', '0', '2'),/* Épée élémentaire brisée */
    (@BoxDesign, @BoxId, '0', '7', '267', '0','1', '0', '2'),/* Arc majestueux */
    (@BoxDesign, @BoxId, '0', '7', '270', '0','1', '0', '2'),/* Baguette magique majestueuse */
    (@BoxDesign, @BoxId, '0', '7', '2514', '0','1', '0', '2'),/* Petit rubis de l'accomplissement */
    (@BoxDesign, @BoxId, '0', '7', '2436', '0','1', '0', '2'),/* Essence de Sellaim */
    (@BoxDesign, @BoxId, '0', '7', '4984', '0','1', '0', '2'),/* L'armure lourde du brave Share */
    (@BoxDesign, @BoxId, '0', '7', '9384', '0','1', '0', '2')/* Cracheur de feu */
