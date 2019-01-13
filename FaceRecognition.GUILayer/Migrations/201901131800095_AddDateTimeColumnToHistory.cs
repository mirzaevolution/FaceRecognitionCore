namespace FaceRecognition.GUILayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimeColumnToHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("fr.History", "DateTime", c => c.DateTime(nullable: false));
            AddColumn("fr.History", "DetectionTime", c => c.String());
            AddColumn("fr.History", "Distance", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("fr.History", "Distance");
            DropColumn("fr.History", "DetectionTime");
            DropColumn("fr.History", "DateTime");
        }
    }
}
