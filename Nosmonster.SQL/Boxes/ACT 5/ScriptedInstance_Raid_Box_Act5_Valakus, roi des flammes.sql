/* Info :
Rewards Vnum : 302  Name : Valakus, roi des flammes
*/
DECLARE @BoxId SMALLINT = 302 
DECLARE @BoxDesign SMALLINT = 14
INSERT INTO [dbo].[RollGeneratedItem]
(
    [OriginalItemDesign], [OriginalItemVNum],
    [MinimumOriginalItemRare], [MaximumOriginalItemRare], [ItemGeneratedVNum],
    [ItemGeneratedDesign], [ItemGeneratedAmount], [IsRareRandom],
    [Probability]
)
VALUES
    (@BoxDesign, @BoxId, '0', '7', '4918', '0','1', '0', '2'),/* Armure blindée du géant de flammes */
    (@BoxDesign, @BoxId, '0', '7', '4921', '0','1', '0', '2'),/* Armure en cuir de chien de braise */
    (@BoxDesign, @BoxId, '0', '7', '4924', '0','1', '0', '2'),/* Tunique du phénix flamboyant */
    (@BoxDesign, @BoxId, '0', '7', '4920', '0','1', '0', '2'),/* Armure du héros oublié */
    (@BoxDesign, @BoxId, '0', '7', '4923', '0','1', '0', '2'),/* Armure en cuir du héros oublié */
    (@BoxDesign, @BoxId, '0', '7', '4926', '0','1', '0', '2'),/* Tunique du héros oublié */
    (@BoxDesign, @BoxId, '0', '7', '4932', '0','1', '0', '2'),/* Gant de Valakus */
    (@BoxDesign, @BoxId, '0', '7', '4730', '0','1', '0', '2'),/* Larme de Ladine */
    (@BoxDesign, @BoxId, '0', '7', '4748', '0','1', '0', '2'),/* Armure d'Akamur */
    (@BoxDesign, @BoxId, '0', '7', '5900', '0','1', '0', '2'),/* Parchemin de masque carbonisé */
    (@BoxDesign, @BoxId, '0', '7', '5901', '0','1', '0', '2'),/* Parchemin de fabrication d'accessoires de Grenigas */
    (@BoxDesign, @BoxId, '0', '7', '2502', '0','1', '0', '2'),/* Parchemin de fabrication d'armures de Grenigas */
    (@BoxDesign, @BoxId, '0', '7', '2509', '0','1', '0', '2'),/* Modèle de Katol */
    (@BoxDesign, @BoxId, '0', '7', '2507', '0','1', '0', '2'),/* Tablette de la civilisation antique */
    (@BoxDesign, @BoxId, '0', '7', '2506', '0','1', '0', '2'),/* Morceau de métal incandescent */
    (@BoxDesign, @BoxId, '0', '7', '2503', '0','3', '0', '2'),/* Élément de terre */
    (@BoxDesign, @BoxId, '0', '7', '2504', '0','1', '0', '2'),/* Eau de source */
    (@BoxDesign, @BoxId, '0', '7', '2900', '0','7', '0', '2'),/* Diamant brisé */
    (@BoxDesign, @BoxId, '0', '7', '2901', '0','2', '0', '2'),/* Diamant intact */
    (@BoxDesign, @BoxId, '0', '7', '2901', '0','5', '0', '2'),/* Diamant intact */
    (@BoxDesign, @BoxId, '0', '7', '2901', '0','10', '0', '2'),/* Diamant intact */
    (@BoxDesign, @BoxId, '0', '7', '5918', '0','1', '0', '2'),/* Partie droite du sceau de raid de Grenigas */
    (@BoxDesign, @BoxId, '0', '7', '9370', '0','1', '0', '2')/* Lance-flammes */
