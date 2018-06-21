namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.CacheIndexEntities",
                c => new
                    {
                        CacheIndexEntityId = c.Int(nullable: false, identity: true),
                        Offset = c.Int(nullable: false),
                        UnCompressedSize = c.Int(nullable: false),
                        CompressedSize = c.Int(nullable: false),
                        File = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CacheIndexEntityId);
            
            this.CreateTable(
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
            
            this.CreateTable(
                "dbo.RenderAndOffsets",
                c => new
                    {
                        RenderId = c.Int(nullable: false),
                        OffSet = c.Long(nullable: false),
                        CacheObjectEntity_CacheObjectEntityId = c.Int(),
                    })
                .PrimaryKey(t => t.RenderId)
                .ForeignKey("dbo.CacheObjectEntities", t => t.CacheObjectEntity_CacheObjectEntityId)
                .Index(t => t.CacheObjectEntity_CacheObjectEntityId);
            
            this.CreateTable(
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
            
            this.CreateTable(
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
            
            this.CreateTable(
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
            
            this.CreateTable(
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
            
            this.CreateTable(
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
                        TextureId = c.Int(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.RenderEntityId);
            
            this.CreateTable(
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
            
            this.CreateTable(
                "dbo.SkeletonEntities",
                c => new
                    {
                        SkeletonEntityId = c.Int(nullable: false, identity: true),
                        SkeletonText = c.String(),
                        MotionIdCounter = c.Int(nullable: false),
                        DistinctMotionCounter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SkeletonEntityId);
            
            this.CreateTable(
                "dbo.Textures",
                c => new
                    {
                        TextureId = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TextureId);
            
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.MotionEntities", "SkeletonEntity_SkeletonEntityId", "dbo.SkeletonEntities");
            this.DropForeignKey("dbo.RenderChildren", "RenderEntity_RenderEntityId", "dbo.RenderEntities");
            this.DropForeignKey("dbo.RenderAndOffsets", "CacheObjectEntity_CacheObjectEntityId", "dbo.CacheObjectEntities");
            this.DropIndex("dbo.RenderChildren", new[] { "RenderEntity_RenderEntityId" });
            this.DropIndex("dbo.MotionEntities", new[] { "SkeletonEntity_SkeletonEntityId" });
            this.DropIndex("dbo.RenderAndOffsets", new[] { "CacheObjectEntity_CacheObjectEntityId" });
            this.DropTable("dbo.Textures");
            this.DropTable("dbo.SkeletonEntities");
            this.DropTable("dbo.RenderChildren");
            this.DropTable("dbo.RenderEntities");
            this.DropTable("dbo.MotionEntities");
            this.DropTable("dbo.MobileEntities");
            this.DropTable("dbo.MeshEntities");
            this.DropTable("dbo.LogTable");
            this.DropTable("dbo.RenderAndOffsets");
            this.DropTable("dbo.CacheObjectEntities");
            this.DropTable("dbo.CacheIndexEntities");
        }
    }
}