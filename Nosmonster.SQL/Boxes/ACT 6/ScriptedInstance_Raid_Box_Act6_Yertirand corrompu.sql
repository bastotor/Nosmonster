/* Info :
Rewards Vnum : 302  Name : Yertirand corrompu
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 27
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4012', '0','1', '0', '2'),/* Armure en or de la bande des voleurs */
    (@BoxDesign, @BoxId, '0', '7', '4015', '0','1', '0', '2'),/* Tunique en or de la bande des voleurs */
    (@BoxDesign, @BoxId, '0', '7', '4018', '0','1', '0', '2'),/* Robe en or de la bande des voleurs */
    (@BoxDesign, @BoxId, '0', '7', '4000', '0','1', '0', '2'),/* Épée longue des voleurs du désert */
    (@BoxDesign, @BoxId, '0', '7', '4002', '0','1', '0', '2'),/* Arc des voleurs du désert */
    (@BoxDesign, @BoxId, '0', '7', '4004', '0','1', '0', '2'),/* Canne des voleurs du désert */
    (@BoxDesign, @BoxId, '0', '7', '4327', '0','1', '0', '2'),/* Baguette magique de Yertirand */
    (@BoxDesign, @BoxId, '0', '7', '4328', '0','1', '0', '2'),/* Baguette magique de Yertirand corrompu */
    (@BoxDesign, @BoxId, '0', '7', '2282', '0','10', '0', '2'),/* Plume d'ange */
    (@BoxDesign, @BoxId, '0', '7', '2513', '0','1', '0', '2'),/* Coeur de dragon */
    (@BoxDesign, @BoxId, '0', '7', '2395', '0','1', '0', '2'),/* Pierre précieuse bleue */
    (@BoxDesign, @BoxId, '0', '7', '4308', '0','1', '0', '2'),/* Ancien costume de Viking (7 jours) */
    (@BoxDesign, @BoxId, '0', '7', '4304', '0','1', '0', '2'),/* Clé de Frigg */
    (@BoxDesign, @BoxId, '0', '7', '4305', '0','1', '0', '2'),/* Clé de Ragnar */
    (@BoxDesign, @BoxId, '0', '7', '4306', '0','1', '0', '2'),/* Clé d'Erdimine */
    (@BoxDesign, @BoxId, '0', '7', '4315', '0','1', '0', '2'),/* Clé de Jennifer */
    (@BoxDesign, @BoxId, '0', '7', '4330', '0','1', '0', '2'),/* Clé de Yertirand */
    (@BoxDesign, @BoxId, '0', '7', '4326', '0','1', '0', '2'),/* Carte de spécialiste pour partenaire du guerrier squelettique Ragnar */
    (@BoxDesign, @BoxId, '0', '7', '5997', '0','1', '0', '2'),/* Dragon squelettique magique */
    (@BoxDesign, @BoxId, '0', '7', '9382', '0','1', '0', '2')/* Corrompu */
