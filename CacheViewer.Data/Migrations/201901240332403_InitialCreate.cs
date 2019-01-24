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
                        MeshId = c.Int(nullable: false),
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
                        CacheObjectEntity_CacheObjectEntityId = c.Int(),
                    })
                .PrimaryKey(t => t.RenderEntityId)
                .ForeignKey("dbo.CacheObjectEntities", t => t.CacheObjectEntity_CacheObjectEntityId)
                .Index(t => t.CacheObjectEntity_CacheObjectEntityId);
            
            CreateTable(
                "dbo.RenderChildren",
                c => new
                    {
                        RenderChildId = c.Int(nullable: false, identity: true),
                        RenderId = c.Int(nullable: false),
                        RenderEntity_RenderEntityId = c.Int(),
                    })
                .PrimaryKey(t => t.RenderChildId)
                .ForeignKey("dbo.RenderEntities", t => t.RenderEntity_RenderEntityId)
                .Index(t => t.RenderEntity_RenderEntityId);
            
            CreateTable(
                "dbo.CObjects",
                c => new
                    {
                        CObjectsId = c.Int(nullable: false, identity: true),
                        Identity = c.Int(nullable: false),
                        Junk1 = c.Int(nullable: false),
                        Offset = c.Int(nullable: false),
                        UnCompressedSize = c.Int(nullable: false),
                        CompressedSize = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Name = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.CObjectsId);
            
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
                "dbo.TextureEntities",
                c => new
                    {
                        TextureEntityId = c.Int(nullable: false, identity: true),
                        TextureId = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TextureEntityId);
            
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
                "dbo.RenderRaws",
                c => new
                    {
                        RenderRawId = c.Int(nullable: false, identity: true),
                        Identity = c.Int(nullable: false),
                        Junk1 = c.Int(nullable: false),
                        Offset = c.Int(nullable: false),
                        UnCompressedSize = c.Int(nullable: false),
                        CompressedSize = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Name = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.RenderRawId);
            
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
                "dbo.TextureEntityMeshEntities",
                c => new
                    {
                        TextureEntity_TextureEntityId = c.Int(nullable: false),
                        MeshEntity_MeshEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TextureEntity_TextureEntityId, t.MeshEntity_MeshEntityId })
                .ForeignKey("dbo.TextureEntities", t => t.TextureEntity_TextureEntityId, cascadeDelete: true)
                .ForeignKey("dbo.MeshEntities", t => t.MeshEntity_MeshEntityId, cascadeDelete: true)
                .Index(t => t.TextureEntity_TextureEntityId)
                .Index(t => t.MeshEntity_MeshEntityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MotionEntities", "SkeletonEntity_SkeletonEntityId", "dbo.SkeletonEntities");
            DropForeignKey("dbo.TextureEntityMeshEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            DropForeignKey("dbo.TextureEntityMeshEntities", "TextureEntity_TextureEntityId", "dbo.TextureEntities");
            DropForeignKey("dbo.RenderTextures", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            DropForeignKey("dbo.RenderEntities", "CacheObjectEntity_CacheObjectEntityId", "dbo.CacheObjectEntities");
            DropForeignKey("dbo.RenderChildren", "RenderEntity_RenderEntityId", "dbo.RenderEntities");
            DropForeignKey("dbo.RenderAndOffsets", "CacheObjectEntity_CacheObjectEntityId", "dbo.CacheObjectEntities");
            DropIndex("dbo.TextureEntityMeshEntities", new[] { "MeshEntity_MeshEntityId" });
            DropIndex("dbo.TextureEntityMeshEntities", new[] { "TextureEntity_TextureEntityId" });
            DropIndex("dbo.MotionEntities", new[] { "SkeletonEntity_SkeletonEntityId" });
            DropIndex("dbo.RenderTextures", new[] { "MeshEntity_MeshEntityId" });
            DropIndex("dbo.RenderChildren", new[] { "RenderEntity_RenderEntityId" });
            DropIndex("dbo.RenderEntities", new[] { "CacheObjectEntity_CacheObjectEntityId" });
            DropIndex("dbo.RenderAndOffsets", new[] { "CacheObjectEntity_CacheObjectEntityId" });
            DropTable("dbo.TextureEntityMeshEntities");
            DropTable("dbo.SkeletonEntities");
            DropTable("dbo.RenderRaws");
            DropTable("dbo.ParseErrors");
            DropTable("dbo.MotionEntities");
            DropTable("dbo.MobileEntities");
            DropTable("dbo.TextureEntities");
            DropTable("dbo.RenderTextures");
            DropTable("dbo.MeshEntities");
            DropTable("dbo.LogTable");
            DropTable("dbo.InvalidValues");
            DropTable("dbo.CObjects");
            DropTable("dbo.RenderChildren");
            DropTable("dbo.RenderEntities");
            DropTable("dbo.RenderAndOffsets");
            DropTable("dbo.CacheObjectEntities");
        }
    }
}
