namespace Nogales.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsRestrictedModuleAccess", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsRestrictedCategoryAccess", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsRestrictedCategoryAccess");
            DropColumn("dbo.AspNetUsers", "IsRestrictedModuleAccess");
        }
    }
}
