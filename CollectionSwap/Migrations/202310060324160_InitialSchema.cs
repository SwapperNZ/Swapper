namespace CollectionSwap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        FullName = c.String(nullable: false),
                        CompanyName = c.String(),
                        LineOne = c.String(nullable: false),
                        LineTwo = c.String(),
                        PostCode = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Collections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        ItemListJSON = c.String(),
                        Sponsor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sponsors", t => t.Sponsor_Id)
                .Index(t => t.Sponsor_Id);

            // Insert initial data
            Sql("INSERT INTO dbo.Collections (Name, ItemListJSON, Description) VALUES ('Feelings', '[\"1.png\",\"2.png\",\"3.png\",\"4.png\",\"5.png\",\"6.png\",\"7.png\",\"8.png\",\"9.png\",\"10.png\",\"11.png\",\"12.png\",\"13.png\",\"14.png\",\"15.png\"]', 'Explore the richness of human feelings')");
            Sql("INSERT INTO dbo.Collections (Name, ItemListJSON, Description) VALUES ('Women of History', '[\"1.png\",\"2.png\",\"3.png\",\"4.png\",\"5.png\",\"6.png\",\"7.png\",\"8.png\",\"9.png\",\"10.png\",\"11.png\",\"12.png\",\"13.png\",\"14.png\",\"15.png\",\"16.png\"]', 'Celebrate women who shaped history')");

            CreateTable(
                "dbo.Sponsors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CollectionId = c.Int(nullable: false),
                        Statement = c.String(),
                        Url = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SwapId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comments = c.String(),
                        DatePlaced = c.DateTimeOffset(nullable: false, precision: 7),
                        Receiver_Id = c.String(maxLength: 128),
                        Sender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Receiver_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BlockedUsers = c.String(),
                        ClosedAccount = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Address_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Address_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.HeldItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemListJSON = c.String(),
                        Swap_Id = c.Int(),
                        UserCollection_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Swaps", t => t.Swap_Id)
                .ForeignKey("dbo.UserCollections", t => t.UserCollection_Id)
                .Index(t => t.Swap_Id)
                .Index(t => t.UserCollection_Id);
            
            CreateTable(
                "dbo.Swaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderRequestedItems = c.String(nullable: false),
                        SenderConfirmSent = c.Boolean(nullable: false),
                        SenderConfirmReceived = c.Boolean(nullable: false),
                        SenderDisplaySwap = c.Boolean(nullable: false),
                        ReceiverRequestedItems = c.String(nullable: false),
                        ReceiverConfirmSent = c.Boolean(nullable: false),
                        ReceiverConfirmReceived = c.Boolean(nullable: false),
                        ReceiverDisplaySwap = c.Boolean(nullable: false),
                        SwapSize = c.Int(nullable: false),
                        Status = c.String(nullable: false),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        Collection_Id = c.Int(),
                        Receiver_Id = c.String(maxLength: 128),
                        ReceiverCollection_Id = c.Int(),
                        ReceiverFeedback_Id = c.Int(),
                        Sender_Id = c.String(maxLength: 128),
                        SenderCollection_Id = c.Int(),
                        SenderFeedback_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Collections", t => t.Collection_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Receiver_Id)
                .ForeignKey("dbo.UserCollections", t => t.ReceiverCollection_Id)
                .ForeignKey("dbo.Feedbacks", t => t.ReceiverFeedback_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id)
                .ForeignKey("dbo.UserCollections", t => t.SenderCollection_Id)
                .ForeignKey("dbo.Feedbacks", t => t.SenderFeedback_Id)
                .Index(t => t.Collection_Id)
                .Index(t => t.Receiver_Id)
                .Index(t => t.ReceiverCollection_Id)
                .Index(t => t.ReceiverFeedback_Id)
                .Index(t => t.Sender_Id)
                .Index(t => t.SenderCollection_Id)
                .Index(t => t.SenderFeedback_Id);
            
            CreateTable(
                "dbo.UserCollections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CollectionId = c.Int(nullable: false),
                        ItemCountJSON = c.String(),
                        Archived = c.Boolean(nullable: false),
                        Charity = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Collections", t => t.CollectionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CollectionId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.HeldItems", "UserCollection_Id", "dbo.UserCollections");
            DropForeignKey("dbo.HeldItems", "Swap_Id", "dbo.Swaps");
            DropForeignKey("dbo.Swaps", "SenderFeedback_Id", "dbo.Feedbacks");
            DropForeignKey("dbo.Swaps", "SenderCollection_Id", "dbo.UserCollections");
            DropForeignKey("dbo.Swaps", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Swaps", "ReceiverFeedback_Id", "dbo.Feedbacks");
            DropForeignKey("dbo.Swaps", "ReceiverCollection_Id", "dbo.UserCollections");
            DropForeignKey("dbo.UserCollections", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCollections", "CollectionId", "dbo.Collections");
            DropForeignKey("dbo.Swaps", "Receiver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Swaps", "Collection_Id", "dbo.Collections");
            DropForeignKey("dbo.Feedbacks", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feedbacks", "Receiver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Collections", "Sponsor_Id", "dbo.Sponsors");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.UserCollections", new[] { "CollectionId" });
            DropIndex("dbo.UserCollections", new[] { "UserId" });
            DropIndex("dbo.Swaps", new[] { "SenderFeedback_Id" });
            DropIndex("dbo.Swaps", new[] { "SenderCollection_Id" });
            DropIndex("dbo.Swaps", new[] { "Sender_Id" });
            DropIndex("dbo.Swaps", new[] { "ReceiverFeedback_Id" });
            DropIndex("dbo.Swaps", new[] { "ReceiverCollection_Id" });
            DropIndex("dbo.Swaps", new[] { "Receiver_Id" });
            DropIndex("dbo.Swaps", new[] { "Collection_Id" });
            DropIndex("dbo.HeldItems", new[] { "UserCollection_Id" });
            DropIndex("dbo.HeldItems", new[] { "Swap_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Address_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Feedbacks", new[] { "Sender_Id" });
            DropIndex("dbo.Feedbacks", new[] { "Receiver_Id" });
            DropIndex("dbo.Collections", new[] { "Sponsor_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.UserCollections");
            DropTable("dbo.Swaps");
            DropTable("dbo.HeldItems");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Sponsors");
            DropTable("dbo.Collections");
            DropTable("dbo.Addresses");
        }
    }
}
