namespace OpenNos.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Br : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemInstance", "IsBattleRoyal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemInstance", "IsBattleRoyal");
        }
    }
}
