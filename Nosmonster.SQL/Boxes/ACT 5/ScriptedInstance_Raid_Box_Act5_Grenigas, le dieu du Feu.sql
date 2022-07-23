/* Info :
Rewards Vnum : 302  Name : Grenigas, le dieu du Feu
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 15
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4900', '0','1', '0', '2'),/* Épée incendiaire de Magmaros */
    (@BoxDesign, @BoxId, '0', '7', '4903', '0','1', '0', '2'),/* Aile du phénix */
    (@BoxDesign, @BoxId, '0', '7', '4906', '0','1', '0', '2'),/* Bâton du spectre de lave */
    (@BoxDesign, @BoxId, '0', '7', '4902', '0','1', '0', '2'),/* Épée du héros oublié */
    (@BoxDesign, @BoxId, '0', '7', '4905', '0','1', '0', '2'),/* Arc du héros oublié */
    (@BoxDesign, @BoxId, '0', '7', '4908', '0','1', '0', '2'),/* Baguette magique du héros oublié */
    (@BoxDesign, @BoxId, '0', '7', '4930', '0','1', '0', '2'),/* Casque du géant de flammes */
    (@BoxDesign, @BoxId, '0', '7', '4731', '0','1', '0', '2'),/* Griffe de Fenris */
    (@BoxDesign, @BoxId, '0', '7', '4732', '0','1', '0', '2'),/* Gant de golem de feu */
    (@BoxDesign, @BoxId, '0', '7', '4749', '0','1', '0', '2'),/* Armure du voyageur mystérieux */
    (@BoxDesign, @BoxId, '0', '7', '4750', '0','1', '0', '2'),/* Uniforme de Felice */
    (@BoxDesign, @BoxId, '0', '7', '5900', '0','1', '0', '2'),/* Parchemin de masque carbonisé */
    (@BoxDesign, @BoxId, '0', '7', '5901', '0','1', '0', '2'),/* Parchemin de fabrication d'accessoires de Grenigas */
    (@BoxDesign, @BoxId, '0', '7', '2500', '0','1', '0', '2'),/* Parchemin de fabrication d'armes de Grenigas */
    (@BoxDesign, @BoxId, '0', '7', '2510', '0','1', '0', '2'),/* Modèle du Graal */
    (@BoxDesign, @BoxId, '0', '7', '2507', '0','1', '0', '2'),/* Tablette de la civilisation antique */
    (@BoxDesign, @BoxId, '0', '7', '2506', '0','1', '0', '2'),/* Morceau de métal incandescent */
    (@BoxDesign, @BoxId, '0', '7', '2503', '0','3', '0', '2'),/* Élément de terre */
    (@BoxDesign, @BoxId, '0', '7', '2504', '0','1', '0', '2'),/* Eau de source */
    (@BoxDesign, @BoxId, '0', '7', '2900', '0','7', '0', '2'),/* Diamant brisé */
    (@BoxDesign, @BoxId, '0', '7', '2901', '0','2', '0', '2'),/* Diamant intact */
    (@BoxDesign, @BoxId, '0', '7', '2901', '0','5', '0', '2'),/* Diamant intact */
    (@BoxDesign, @BoxId, '0', '7', '2901', '0','10', '0', '2'),/* Diamant intact */
    (@BoxDesign, @BoxId, '0', '7', '9371', '0','1', '0', '2')/* Phénix */
