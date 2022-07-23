namespace OpenNos.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class br2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Character", "LevelSaved", c => c.Byte(nullable: false));
            AddColumn("dbo.Character", "HeroLevelSaved", c => c.Byte(nullable: false));
            AddColumn("dbo.Character", "JobLevelSaved", c => c.Byte(nullable: false));
            AddColumn("dbo.Character", "PrestigeSaved", c => c.Int(nullable: false));
            AddColumn("dbo.Character", "IsBattleRoyalLevel", c => c.Boolean(nullable: false));
            AddColumn("dbo.Character", "IsAdventurerAfterBattle", c => c.Boolean(nullable: false));
            AddColumn("dbo.Character", "ReputationSaved", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Character", "ReputationSaved");
            DropColumn("dbo.Character", "IsAdventurerAfterBattle");
            DropColumn("dbo.Character", "IsBattleRoyalLevel");
            DropColumn("dbo.Character", "PrestigeSaved");
            DropColumn("dbo.Character", "JobLevelSaved");
            DropColumn("dbo.Character", "HeroLevelSaved");
            DropColumn("dbo.Character", "LevelSaved");
        }
    }
}
