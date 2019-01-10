namespace FaceRecognition.GUILayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("fr.Repository", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("fr.Repository", "Description");
        }
    }
}
