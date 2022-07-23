/* Info :
Rewards Vnum : 302  Name : Castra obscur
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 2
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '312', '0','1', '0', '2'),/* Collier à diamant antique */
    (@BoxDesign, @BoxId, '0', '7', '313', '0','1', '0', '2'),/* Anneau en diamant antique */
    (@BoxDesign, @BoxId, '0', '7', '314', '0','1', '0', '2'),/* Bracelet à diamant antique */
    (@BoxDesign, @BoxId, '0', '7', '263', '0','1', '0', '2'),/* Sabre glorieux */
    (@BoxDesign, @BoxId, '0', '7', '266', '0','1', '0', '2'),/* Arc de la paix */
    (@BoxDesign, @BoxId, '0', '7', '269', '0','1', '0', '2'),/* Baguette magique du fantôme */
    (@BoxDesign, @BoxId, '0', '7', '227', '0','1', '0', '2'),/* Masque X */
    (@BoxDesign, @BoxId, '0', '7', '318', '0','1', '0', '2'),/* Gants funestes */
    (@BoxDesign, @BoxId, '0', '7', '320', '0','1', '0', '2'),/* Chaussures des vagues */
    (@BoxDesign, @BoxId, '0', '7', '9359', '0','1', '0', '2')/* Ritualiste */
