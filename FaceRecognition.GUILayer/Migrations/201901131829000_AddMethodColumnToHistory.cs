namespace FaceRecognition.GUILayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMethodColumnToHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("fr.History", "Method", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("fr.History", "Method");
        }
    }
}
