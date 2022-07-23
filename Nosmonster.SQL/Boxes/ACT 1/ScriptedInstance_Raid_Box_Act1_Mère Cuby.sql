/* Info :
Rewards Vnum : 302  Name : Mère Cuby
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 0
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '1906', '0','1', '0', '2'),/* Magic Scooter*/
    (@BoxDesign, @BoxId, '0', '7', '4102', '0','1', '0', '2'),/* Wingless^Amora*/
    (@BoxDesign, @BoxId, '0', '7', '1904', '0','1', '0', '2'),/* Tarot Card Game */
    (@BoxDesign, @BoxId, '0', '7', '4144', '0','1', '0', '2'),/* Necromancer Title */
    (@BoxDesign, @BoxId, '0', '7', '544', '0','1', '0', '2'),/* Tired Pink Jelly */
