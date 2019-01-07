namespace FaceRecognition.GUILayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "fr.History",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        CapturedImage = c.Binary(),
                        UserID = c.Int(nullable: false),
                        RepositoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("fr.User", t => t.UserID, cascadeDelete: true)
                .ForeignKey("fr.Repository", t => t.ID)
                .Index(t => t.ID)
                .Index(t => t.UserID);
            
            CreateTable(
                "fr.Repository",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SampleImage = c.Binary(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("fr.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "fr.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256, unicode: false),
                        FullName = c.String(nullable: false, maxLength: 256, unicode: false),
                        PasswordHash = c.String(nullable: false, maxLength: 1000),
                        Email = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("fr.History", "ID", "fr.Repository");
            DropForeignKey("fr.Repository", "UserID", "fr.User");
            DropForeignKey("fr.History", "UserID", "fr.User");
            DropIndex("fr.Repository", new[] { "UserID" });
            DropIndex("fr.History", new[] { "UserID" });
            DropIndex("fr.History", new[] { "ID" });
            DropTable("fr.User");
            DropTable("fr.Repository");
            DropTable("fr.History");
        }
    }
}
