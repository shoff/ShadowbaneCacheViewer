namespace CacheViewer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CacheObjectEntities",
                c => new
                    {
                        CacheObjectEntityId = c.Int(nullable: false, identity: true),
                        CacheIndexIdentity = c.Int(nullable: false),
                        CompressedSize = c.Int(nullable: false),
                        UncompressedSize = c.Int(nullable: false),
                        FileOffset = c.Int(nullable: false),
                        RenderKey = c.Int(nullable: false),
                        Name = c.String(),
                        ObjectType = c.Int(nullable: false),
                        ObjectTypeDescription = c.String(maxLength: 11),
                    })
                .PrimaryKey(t => t.CacheObjectEntityId);
            
            CreateTable(
                "dbo.RenderAndOffsets",
                c => new
                    {
                        RenderAndOffsetId = c.Int(nullable: false, identity: true),
                        RenderId = c.Int(nullable: false),
                        OffSet = c.Long(nullable: false),
                        CacheIndexId = c.Int(nullable: false),
                        CacheObjectEntity_CacheObjectEntityId = c.Int(),
                    })
                .PrimaryKey(t => t.RenderAndOffsetId)
                .ForeignKey("dbo.CacheObjectEntities", t => t.CacheObjectEntity_CacheObjectEntityId)
                .Index(t => t.CacheObjectEntity_CacheObjectEntityId);
            
            CreateTable(
                "dbo.RenderEntities",
                c => new
                    {
                        RenderEntityId = c.Int(nullable: false, identity: true),
                        CacheIndexIdentity = c.Int(nullable: false),
                        ByteCount = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        HasMesh = c.Boolean(nullable: false),
                        MeshId = c.Int(),
                        JointName = c.String(),
                        Scale = c.String(maxLength: 64),
                        Position = c.String(maxLength: 64),
                        RenderCount = c.Int(nullable: false),
                        CompressedSize = c.Int(nullable: false),
                        UncompressedSize = c.Int(nullable: false),
                        FileOffset = c.Int(nullable: false),
                        HasTexture = c.Boolean(nullable: false),
                        TextureId = c.Int(),
                        Notes = c.String(),
                        InvalidData = c.Boolean(nullable: false),
                        RenderEntity_RenderEntityId = c.Int(),
                    })
                .PrimaryKey(t => t.RenderEntityId)
                .ForeignKey("dbo.RenderEntities", t => t.RenderEntity_RenderEntityId)
                .Index(t => t.RenderEntity_RenderEntityId);
            
            CreateTable(
                "dbo.RenderChildren",
                c => new
                    {
                        DatabaseRenderId = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        ChildRenderId = c.Int(nullable: false),
                        RenderEntity_RenderEntityId = c.Int(),
                    })
                .PrimaryKey(t => t.DatabaseRenderId)
                .ForeignKey("dbo.RenderEntities", t => t.ChildRenderId)
                .ForeignKey("dbo.RenderEntities", t => t.ParentId)
                .ForeignKey("dbo.RenderEntities", t => t.RenderEntity_RenderEntityId)
                .Index(t => t.ParentId)
                .Index(t => t.ChildRenderId)
                .Index(t => t.RenderEntity_RenderEntityId);
            
            CreateTable(
                "dbo.TextureEntities",
                c => new
                    {
                        TextureEntityId = c.Int(nullable: false, identity: true),
                        TextureId = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        RenderEntity_RenderEntityId = c.Int(),
                    })
                .PrimaryKey(t => t.TextureEntityId)
                .ForeignKey("dbo.RenderEntities", t => t.RenderEntity_RenderEntityId)
                .Index(t => t.RenderEntity_RenderEntityId);
            
            CreateTable(
                "dbo.MeshEntities",
                c => new
                    {
                        MeshEntityId = c.Int(nullable: false, identity: true),
                        CacheIndexIdentity = c.Int(nullable: false),
                        CompressedSize = c.Int(nullable: false),
                        UncompressedSize = c.Int(nullable: false),
                        FileOffset = c.Int(nullable: false),
                        VertexCount = c.Int(nullable: false),
                        NormalsCount = c.Int(nullable: false),
                        TexturesCount = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Vertices = c.String(),
                        Normals = c.String(),
                        TextureVectors = c.String(),
                    })
                .PrimaryKey(t => t.MeshEntityId);
            
            CreateTable(
                "dbo.RenderTextures",
                c => new
                    {
                        RenderTextureId = c.Int(nullable: false, identity: true),
                        RenderId = c.Int(nullable: false),
                        Offset = c.Long(nullable: false),
                        TextureId = c.Int(nullable: false),
                        MeshEntity_MeshEntityId = c.Int(),
                    })
                .PrimaryKey(t => t.RenderTextureId)
                .ForeignKey("dbo.MeshEntities", t => t.MeshEntity_MeshEntityId)
                .Index(t => t.MeshEntity_MeshEntityId);
            
            CreateTable(
                "dbo.InvalidValues",
                c => new
                    {
                        InvalidValueId = c.Int(nullable: false, identity: true),
                        RenderId = c.Int(nullable: false),
                        OffSet = c.Long(nullable: false),
                        CacheIndexId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvalidValueId);
            
            CreateTable(
                "dbo.LogTable",
                c => new
                    {
                        LogId = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(),
                        LogLevel = c.String(maxLength: 10),
                        Logger = c.String(maxLength: 128),
                        Message = c.String(),
                        MessageId = c.String(),
                        WindowsUserName = c.String(maxLength: 256),
                        CallSite = c.String(maxLength: 256),
                        ThreadId = c.String(maxLength: 128),
                        Exception = c.String(),
                        StackTrace = c.String(),
                    })
                .PrimaryKey(t => t.LogId);
            
            CreateTable(
                "dbo.MobileEntities",
                c => new
                    {
                        MobileEntityId = c.Int(nullable: false, identity: true),
                        ObjectId = c.Int(nullable: false),
                        Name = c.String(),
                        AiDescription = c.String(),
                        MobToken = c.Long(nullable: false),
                        Gender = c.Int(nullable: false),
                        TrainingPowerBonus = c.Int(nullable: false),
                        RuneType = c.Int(nullable: false),
                        RuneCategory = c.Int(nullable: false),
                        RuneStackRank = c.Int(nullable: false),
                        RuneCost = c.Int(nullable: false),
                        NumberOfSkillsRequired = c.Int(nullable: false),
                        LevelRequired = c.Int(nullable: false),
                        PowerId = c.Int(nullable: false),
                        NameSize = c.Int(nullable: false),
                        PetNameCount = c.Int(nullable: false),
                        ProhibitsRaceToggle = c.Byte(nullable: false),
                        SomeKindOfTypeHash = c.Long(nullable: false),
                        RequiredGender = c.Int(nullable: false),
                        MinRequiredLevel = c.Int(nullable: false),
                        SomethingWithPets = c.Int(nullable: false),
                        SecondNameSize = c.Int(nullable: false),
                        IsPetOrRune = c.Int(nullable: false),
                        WolfpackCreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MobileEntityId);
            
            CreateTable(
                "dbo.MotionEntities",
                c => new
                    {
                        MotionEntityId = c.Int(nullable: false, identity: true),
                        CacheIdentity = c.Long(nullable: false),
                        SkeletonEntity_SkeletonEntityId = c.Int(),
                    })
                .PrimaryKey(t => t.MotionEntityId)
                .ForeignKey("dbo.SkeletonEntities", t => t.SkeletonEntity_SkeletonEntityId)
                .Index(t => t.SkeletonEntity_SkeletonEntityId);
            
            CreateTable(
                "dbo.ParseErrors",
                c => new
                    {
                        ParseErrorId = c.Long(nullable: false, identity: true),
                        CursorOffset = c.Int(nullable: false),
                        ObjectType = c.Int(nullable: false),
                        Name = c.String(),
                        Data = c.Binary(),
                        InnerOffset = c.Int(nullable: false),
                        RenderId = c.Int(nullable: false),
                        CacheIndexIdentity = c.Int(nullable: false),
                        CacheIndexOffset = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParseErrorId);
            
            CreateTable(
                "dbo.SkeletonEntities",
                c => new
                    {
                        SkeletonEntityId = c.Int(nullable: false, identity: true),
                        SkeletonText = c.String(),
                        MotionIdCounter = c.Int(nullable: false),
                        DistinctMotionCounter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SkeletonEntityId);
            
            CreateTable(
                "dbo.RenderEntityCacheObjectEntities",
                c => new
                    {
                        RenderEntity_RenderEntityId = c.Int(nullable: false),
                        CacheObjectEntity_CacheObjectEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RenderEntity_RenderEntityId, t.CacheObjectEntity_CacheObjectEntityId })
                .ForeignKey("dbo.RenderEntities", t => t.RenderEntity_RenderEntityId, cascadeDelete: true)
                .ForeignKey("dbo.CacheObjectEntities", t => t.CacheObjectEntity_CacheObjectEntityId, cascadeDelete: true)
                .Index(t => t.RenderEntity_RenderEntityId)
                .Index(t => t.CacheObjectEntity_CacheObjectEntityId);
            
            CreateTable(
                "dbo.MeshEntityTextureEntities",
                c => new
                    {
                        MeshEntity_MeshEntityId = c.Int(nullable: false),
                        TextureEntity_TextureEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MeshEntity_MeshEntityId, t.TextureEntity_TextureEntityId })
                .ForeignKey("dbo.MeshEntities", t => t.MeshEntity_MeshEntityId, cascadeDelete: true)
                .ForeignKey("dbo.TextureEntities", t => t.TextureEntity_TextureEntityId, cascadeDelete: true)
                .Index(t => t.MeshEntity_MeshEntityId)
                .Index(t => t.TextureEntity_TextureEntityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MotionEntities", "SkeletonEntity_SkeletonEntityId", "dbo.SkeletonEntities");
            DropForeignKey("dbo.TextureEntities", "RenderEntity_RenderEntityId", "dbo.RenderEntities");
            DropForeignKey("dbo.MeshEntityTextureEntities", "TextureEntity_TextureEntityId", "dbo.TextureEntities");
            DropForeignKey("dbo.MeshEntityTextureEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            DropForeignKey("dbo.RenderTextures", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            DropForeignKey("dbo.RenderEntities", "RenderEntity_RenderEntityId", "dbo.RenderEntities");
            DropForeignKey("dbo.RenderChildren", "RenderEntity_RenderEntityId", "dbo.RenderEntities");
            DropForeignKey("dbo.RenderChildren", "ParentId", "dbo.RenderEntities");
            DropForeignKey("dbo.RenderChildren", "ChildRenderId", "dbo.RenderEntities");
            DropForeignKey("dbo.RenderEntityCacheObjectEntities", "CacheObjectEntity_CacheObjectEntityId", "dbo.CacheObjectEntities");
            DropForeignKey("dbo.RenderEntityCacheObjectEntities", "RenderEntity_RenderEntityId", "dbo.RenderEntities");
            DropForeignKey("dbo.RenderAndOffsets", "CacheObjectEntity_CacheObjectEntityId", "dbo.CacheObjectEntities");
            DropIndex("dbo.MeshEntityTextureEntities", new[] { "TextureEntity_TextureEntityId" });
            DropIndex("dbo.MeshEntityTextureEntities", new[] { "MeshEntity_MeshEntityId" });
            DropIndex("dbo.RenderEntityCacheObjectEntities", new[] { "CacheObjectEntity_CacheObjectEntityId" });
            DropIndex("dbo.RenderEntityCacheObjectEntities", new[] { "RenderEntity_RenderEntityId" });
            DropIndex("dbo.MotionEntities", new[] { "SkeletonEntity_SkeletonEntityId" });
            DropIndex("dbo.RenderTextures", new[] { "MeshEntity_MeshEntityId" });
            DropIndex("dbo.TextureEntities", new[] { "RenderEntity_RenderEntityId" });
            DropIndex("dbo.RenderChildren", new[] { "RenderEntity_RenderEntityId" });
            DropIndex("dbo.RenderChildren", new[] { "ChildRenderId" });
            DropIndex("dbo.RenderChildren", new[] { "ParentId" });
            DropIndex("dbo.RenderEntities", new[] { "RenderEntity_RenderEntityId" });
            DropIndex("dbo.RenderAndOffsets", new[] { "CacheObjectEntity_CacheObjectEntityId" });
            DropTable("dbo.MeshEntityTextureEntities");
            DropTable("dbo.RenderEntityCacheObjectEntities");
            DropTable("dbo.SkeletonEntities");
            DropTable("dbo.ParseErrors");
            DropTable("dbo.MotionEntities");
            DropTable("dbo.MobileEntities");
            DropTable("dbo.LogTable");
            DropTable("dbo.InvalidValues");
            DropTable("dbo.RenderTextures");
            DropTable("dbo.MeshEntities");
            DropTable("dbo.TextureEntities");
            DropTable("dbo.RenderChildren");
            DropTable("dbo.RenderEntities");
            DropTable("dbo.RenderAndOffsets");
            DropTable("dbo.CacheObjectEntities");
        }
    }
}
