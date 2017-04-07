namespace CodeFighter.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ActionValuesJSON = c.String(),
                        TargetSelfPart = c.Boolean(nullable: false),
                        TargetSelfShip = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        MaxHP = c.Int(nullable: false),
                        Mass = c.Double(nullable: false),
                        DR = c.Int(nullable: false),
                        DownAdjective = c.String(),
                        PenetrateVerb = c.String(),
                        Thrust = c.Double(nullable: false),
                        WeaponDamage = c.Int(nullable: false),
                        CritMultiplier = c.Int(nullable: false),
                        ReloadTime = c.Int(nullable: false),
                        DamageType = c.String(),
                        FiringType = c.String(),
                        Range = c.Double(nullable: false),
                        IsPointDefense = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShipDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShipHullDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShipHullDatas", t => t.ShipHullDataId, cascadeDelete: true)
                .Index(t => t.ShipHullDataId);
            
            CreateTable(
                "dbo.ScenarioShipDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartingPositionX = c.Int(nullable: false),
                        StartingPositionY = c.Int(nullable: false),
                        IsPlayer = c.Boolean(nullable: false),
                        ShipName = c.String(),
                        ScenarioDataId = c.Int(nullable: false),
                        ShipDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ScenarioDatas", t => t.ScenarioDataId, cascadeDelete: true)
                .ForeignKey("dbo.ShipDatas", t => t.ShipDataId, cascadeDelete: true)
                .Index(t => t.ScenarioDataId)
                .Index(t => t.ShipDataId);
            
            CreateTable(
                "dbo.ScenarioDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScenarioGUID = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        RoundLimit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScenarioFeatureDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PositionX = c.Int(nullable: false),
                        PositionY = c.Int(nullable: false),
                        ScenarioDataId = c.Int(nullable: false),
                        FeatureDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeatureDatas", t => t.FeatureDataId, cascadeDelete: true)
                .ForeignKey("dbo.ScenarioDatas", t => t.ScenarioDataId, cascadeDelete: true)
                .Index(t => t.ScenarioDataId)
                .Index(t => t.FeatureDataId);
            
            CreateTable(
                "dbo.FeatureDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        IsBlocking = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShipHullDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassName = c.String(),
                        MaxHP = c.Int(nullable: false),
                        HullSize = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayerDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerGUID = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayerScenarioDatas",
                c => new
                    {
                        PlayerDataId = c.Int(nullable: false),
                        ScenarioDataId = c.Int(nullable: false),
                        DateRun = c.DateTime(nullable: false),
                        FinalScore = c.Int(nullable: false),
                        AllActions = c.String(),
                    })
                .PrimaryKey(t => new { t.PlayerDataId, t.ScenarioDataId })
                .ForeignKey("dbo.PlayerDatas", t => t.PlayerDataId, cascadeDelete: true)
                .ForeignKey("dbo.ScenarioDatas", t => t.ScenarioDataId, cascadeDelete: true)
                .Index(t => t.PlayerDataId)
                .Index(t => t.ScenarioDataId);
            
            CreateTable(
                "dbo.PartDataActionDatas",
                c => new
                    {
                        PartData_Id = c.Int(nullable: false),
                        ActionData_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PartData_Id, t.ActionData_Id })
                .ForeignKey("dbo.PartDatas", t => t.PartData_Id, cascadeDelete: true)
                .ForeignKey("dbo.ActionDatas", t => t.ActionData_Id, cascadeDelete: true)
                .Index(t => t.PartData_Id)
                .Index(t => t.ActionData_Id);
            
            CreateTable(
                "dbo.ShipDataPartDatas",
                c => new
                    {
                        ShipData_Id = c.Int(nullable: false),
                        PartData_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShipData_Id, t.PartData_Id })
                .ForeignKey("dbo.ShipDatas", t => t.ShipData_Id, cascadeDelete: true)
                .ForeignKey("dbo.PartDatas", t => t.PartData_Id, cascadeDelete: true)
                .Index(t => t.ShipData_Id)
                .Index(t => t.PartData_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerScenarioDatas", "ScenarioDataId", "dbo.ScenarioDatas");
            DropForeignKey("dbo.PlayerScenarioDatas", "PlayerDataId", "dbo.PlayerDatas");
            DropForeignKey("dbo.ShipDatas", "ShipHullDataId", "dbo.ShipHullDatas");
            DropForeignKey("dbo.ScenarioShipDatas", "ShipDataId", "dbo.ShipDatas");
            DropForeignKey("dbo.ScenarioShipDatas", "ScenarioDataId", "dbo.ScenarioDatas");
            DropForeignKey("dbo.ScenarioFeatureDatas", "ScenarioDataId", "dbo.ScenarioDatas");
            DropForeignKey("dbo.ScenarioFeatureDatas", "FeatureDataId", "dbo.FeatureDatas");
            DropForeignKey("dbo.ShipDataPartDatas", "PartData_Id", "dbo.PartDatas");
            DropForeignKey("dbo.ShipDataPartDatas", "ShipData_Id", "dbo.ShipDatas");
            DropForeignKey("dbo.PartDataActionDatas", "ActionData_Id", "dbo.ActionDatas");
            DropForeignKey("dbo.PartDataActionDatas", "PartData_Id", "dbo.PartDatas");
            DropIndex("dbo.ShipDataPartDatas", new[] { "PartData_Id" });
            DropIndex("dbo.ShipDataPartDatas", new[] { "ShipData_Id" });
            DropIndex("dbo.PartDataActionDatas", new[] { "ActionData_Id" });
            DropIndex("dbo.PartDataActionDatas", new[] { "PartData_Id" });
            DropIndex("dbo.PlayerScenarioDatas", new[] { "ScenarioDataId" });
            DropIndex("dbo.PlayerScenarioDatas", new[] { "PlayerDataId" });
            DropIndex("dbo.ScenarioFeatureDatas", new[] { "FeatureDataId" });
            DropIndex("dbo.ScenarioFeatureDatas", new[] { "ScenarioDataId" });
            DropIndex("dbo.ScenarioShipDatas", new[] { "ShipDataId" });
            DropIndex("dbo.ScenarioShipDatas", new[] { "ScenarioDataId" });
            DropIndex("dbo.ShipDatas", new[] { "ShipHullDataId" });
            DropTable("dbo.ShipDataPartDatas");
            DropTable("dbo.PartDataActionDatas");
            DropTable("dbo.PlayerScenarioDatas");
            DropTable("dbo.PlayerDatas");
            DropTable("dbo.ShipHullDatas");
            DropTable("dbo.FeatureDatas");
            DropTable("dbo.ScenarioFeatureDatas");
            DropTable("dbo.ScenarioDatas");
            DropTable("dbo.ScenarioShipDatas");
            DropTable("dbo.ShipDatas");
            DropTable("dbo.PartDatas");
            DropTable("dbo.ActionDatas");
        }
    }
}
