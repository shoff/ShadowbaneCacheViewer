namespace Arcane.Data.Mongo.Mappings;

using System.Numerics;
using System.Text.Json;
using Cache.Json.CObjects;
using Cache.Json.Mesh;
using Cache.Json.Render;
using Entities;

public static class EntityExtensions
{
    public static RuneEntity? GetRune(uint id)
    {
        var path = $"{RUNE_PATH}\\{id}.json";
        return GetRune(path, id);
    }

    public static RuneEntity? GetRune(string path, uint id)
    {
        var json = File.ReadAllText(path);
        var rune = JsonSerializer.Deserialize<Rune>(json);
        if (rune is null)
        {
            return null;
        }
        rune.RuneId = id;
        return CreateRuneEntity(rune);
    }

    private static RuneEntity CreateRuneEntity(Rune jsonRune)
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var rune = new RuneEntity();

        rune.Icon = jsonRune.Icon;
        rune.RuneId = jsonRune.RuneId;
        rune.RuneType = jsonRune.RuneType;
        rune.Renderable = CreateRenderable(jsonRune.RenderObject);
        rune.RenderObject = jsonRune.RenderObject;
        rune.RenderObjectLowDetail = jsonRune.RenderObjectLowDetail;
        rune.RuneAttack = jsonRune.RuneAttack;
        rune.RuneDefense = jsonRune.RuneDefense;
        // RuneAttrAdjs= jsonRune.RuneAttrAdjs;
        rune.RuneBeard = jsonRune.RuneBeard;
        rune.RuneBodyPartsArray = CreateBodyParts(jsonRune.RuneBodyPartsArray);
        rune.RuneCanFly = jsonRune.RuneCanFly;
        rune.RuneClassIcon = jsonRune.RuneClassIcon;
        rune.RuneCreationCost = jsonRune.RuneCreationCost;
        rune.RuneDeathEffect = jsonRune.RuneDeathEffect;
        rune.RuneDsc = jsonRune.RuneDsc;
        //RuneEnchantmentType= jsonRune.RuneEnchantmentType;
        // RuneEnemyGender= jsonRune.RuneEnemyGender;
        // RuneEnemyMonsterTypes= jsonRune.RuneEnemyMonsterTypes;
        rune.RuneExpReqToLevel = jsonRune.RuneExpReqToLevel;
        rune.RuneFxTxt = jsonRune.RuneFxTxt;
        rune.RuneGroup = CreateRuneGroup(jsonRune, rune);
        //RuneGroupeeMonsterTypes= jsonRune.RuneGroupeeMonsterTypes;
        rune.RuneGroupRoleSet = jsonRune.RuneGroupRoleSet;
        rune.RuneGroupTactics = jsonRune.RuneGroupTactics;
        rune.RuneHair = jsonRune.RuneHair;
        rune.RuneHealth = jsonRune.RuneHealth;
        //ArcHardpointList= jsonRune.ArcHardpointList;
        rune.CombatAttackResist = new CombatAttackResistEntity
        {
            AntiSiege = jsonRune.CombatAttackResist.AntiSiege,
            Bleeding = jsonRune.CombatAttackResist.Bleeding,
            Cold = jsonRune.CombatAttackResist.Cold,
            Crushing = jsonRune.CombatAttackResist.Crushing,
            Fire = jsonRune.CombatAttackResist.Fire,
            Healing = jsonRune.CombatAttackResist.Healing,
            Holy = jsonRune.CombatAttackResist.Holy,
            Lightning = jsonRune.CombatAttackResist.Lightning,
            Magic = jsonRune.CombatAttackResist.Magic,
            Mental = jsonRune.CombatAttackResist.Mental,
            Piercing = jsonRune.CombatAttackResist.Piercing,
            Poison = jsonRune.CombatAttackResist.Poison,
            Slashing = jsonRune.CombatAttackResist.Slashing,
            Unholy = jsonRune.CombatAttackResist.Unholy,
            Siege = jsonRune.CombatAttackResist.Siege,
            Unknown = jsonRune.CombatAttackResist.Unknown
        };
        // CombatPowers= jsonRune.CombatPowers;
        rune.CullDistance = jsonRune.CullDistance;
        rune.DefaultAlignment = jsonRune.DefaultAlignment;
        rune.DoubleFusion = jsonRune.DoubleFusion;
        rune.ForwardVector = jsonRune.ForwardVector;
        rune.Gravity = jsonRune.Gravity;
        rune.GravityF = jsonRune.GravityF;
        rune.HealthCurrent = jsonRune.HealthCurrent;
        rune.HealthFull = jsonRune.HealthFull;
        //ItemAttrReq= jsonRune.ItemAttrReq;
        rune.ItemBaneRank = jsonRune.ItemBaneRank;
        rune.ItemBaseName = jsonRune.ItemBaseName;
        rune.ItemBookArcana = jsonRune.ItemBookArcana;
        //ItemClassReq= jsonRune.ItemClassReq;
        //ItemDiscReq= jsonRune.ItemDiscReq;
        rune.ItemDsc = jsonRune.ItemDsc;
        rune.ItemEqSlotsAnd = jsonRune.ItemEqSlotsAnd;
        rune.ItemEqSlotsOr = jsonRune.ItemEqSlotsOr;
        rune.ItemEqSlotsType = jsonRune.ItemEqSlotsType;
        rune.ItemEqSlotsValue = jsonRune.ItemEqSlotsValue;
        // ItemFlags= jsonRune.ItemFlags;
        rune.ItemHasStub = jsonRune.ItemHasStub;
        rune.ItemHealthFull = jsonRune.ItemHealthFull;
        rune.ItemIgnoreSavedActions = jsonRune.ItemIgnoreSavedActions;
        rune.ItemInitialCharges = jsonRune.ItemInitialCharges;
        rune.ItemLevelReq = jsonRune.ItemLevelReq;
        //ItemOfferingAdjustments= jsonRune.ItemOfferingAdjustments;
        //ItemOfferingInfo= jsonRune.ItemOfferingInfo;
        rune.ItemParryAnimId = jsonRune.ItemParryAnimId;
        rune.ItemPassiveDefenseMod = jsonRune.ItemPassiveDefenseMod;
        rune.ItemPostItemId = jsonRune.ItemPostItemId;
        // ItemPowerAction= jsonRune.ItemPowerAction;
        //  ItemPowerGrant= jsonRune.ItemPowerGrant;
        //ItemRaceReq = new Data.Mongo.Entities.ItemRaceReq
        //{
        //    Races= jsonRune.ItemRaceReq.Races;
        //    Restrict= jsonRune.ItemRaceReq.Restrict
        //};
        rune.ItemRankReq = jsonRune.ItemRankReq;
        rune.ItemRenderObjectFemale = jsonRune.ItemRenderObjectFemale;
        //ItemResourceCosts= jsonRune.ItemResourceCosts;
        rune.ItemSexReq = jsonRune.ItemSexReq;
        rune.ItemSheathable = jsonRune.ItemSheathable;
        rune.ItemSkillMasteryUsed = jsonRune.ItemSkillMasteryUsed;
        //ItemSkillReq= jsonRune.ItemSkillReq;
        rune.ItemSkillUsed = jsonRune.ItemSkillUsed;
        rune.ItemTakeable = jsonRune.ItemTakeable;
        rune.ItemType = jsonRune.ItemType;
        //ItemUseFlags= jsonRune.ItemUseFlags;
        //ItemUserPowerActions= jsonRune.ItemUserPowerActions;
        rune.ItemValue = jsonRune.ItemValue;
        rune.ItemWT = jsonRune.ItemWT;
        rune.MaxTrackingDistance = jsonRune.MaxTrackingDistance;
        rune.ObjName = jsonRune.ObjName;
        rune.Pickable = jsonRune.Pickable;
        //RuneHelperMonsterTypes= jsonRune.RuneHelperMonsterTypes;
        // RuneInventoryContents= jsonRune.RuneInventoryContents;
        rune.RuneIsStandardCharacterCreation = jsonRune.RuneIsStandardCharacterCreation;
        rune.RuneLevel = jsonRune.RuneLevel;
        rune.RuneMana = jsonRune.RuneMana;
        // RuneMaxAttrAdjs= jsonRune.RuneMaxAttrAdjs;
        rune.RuneMaxDamage = jsonRune.RuneMaxDamage;
        rune.RuneMinDamage = jsonRune.RuneMinDamage;
        rune.RuneNaturalattacks = jsonRune.RuneNaturalattacks;
        rune.RuneNaturalPowerAttack = jsonRune.RuneNaturalPowerAttack;
        //RuneNotEnemyMonsterTypes= jsonRune.RuneNotEnemyMonsterTypes;
        rune.RunePracsPerLevel = jsonRune.RunePracsPerLevel;
        rune.RuneRank = jsonRune.RuneRank;
        rune.RuneRenderable = jsonRune.RuneRenderable;
        rune.RuneScaleFactor = jsonRune.RuneScaleFactor;
        rune.RuneSex = jsonRune.RuneSex;
        rune.RuneSkeleton = jsonRune.RuneSkeleton;
        //RuneSkillAdjs= jsonRune.RuneSkillAdjs;
        // RuneSkillGrants = null;
        rune.RuneSlopeHugger = jsonRune.RuneSlopeHugger;
        rune.RuneSpeed = CreateRuneSpeedEntity(jsonRune);
        rune.RuneStamina = jsonRune.RuneStamina;
        //RuneSubType= jsonRune.RuneSubType;
        rune.RuneTombstoneId = jsonRune.RuneTombstoneId;
        rune.Scale = jsonRune.Scale;
        //SoundEvents= jsonRune.SoundEvents,
        //SoundTable= jsonRune.SoundTable,
        rune.TrackingName = jsonRune.TrackingName;

        return rune;
    }

    private static RuneGroupEntity? CreateRuneGroup(Rune? jsonRune, RuneEntity rune)
    {
        if (jsonRune?.RuneGroup is null)
        {
            return null;
        }

        return new RuneGroupEntity
        {
            GroupIsFaction = (bool)rune.RuneGroup?.GroupIsFaction,
            GroupIsGuild = jsonRune.RuneGroup.GroupIsGuild,
            GroupType = jsonRune.RuneGroup.GroupType
        };
    }

    private static RuneSpeedEntity? CreateRuneSpeedEntity(Rune rune)
    {
        if (rune.RuneSpeed is null)
        {
            return null;
        }

        return new RuneSpeedEntity
        {
            Run = rune.RuneSpeed.Run,
            Walk = rune.RuneSpeed.Walk,
            CombatRun = rune.RuneSpeed.CombatRun,
            Combatwalk = rune.RuneSpeed.Combatwalk,
            FlyRun = rune.RuneSpeed.FlyRun,
            FlyWalk = rune.RuneSpeed.FlyWalk,
            Swim = rune.RuneSpeed.Swim
        };
    }

    private static RenderableEntity? CreateRenderable(uint renderId)
    {
        var path = Path.Combine(RENDER_PATH, $"{renderId}.json");
        var json = File.ReadAllText(path);
        Renderable? runeRenderObject = JsonSerializer.Deserialize<Renderable>(json);

        if (runeRenderObject is null)
        {
            return null;
        }

        runeRenderObject.RederableId = renderId;
        return CreateRenderable(runeRenderObject);
    }

    private static RenderableEntity? CreateRenderable(Renderable? runeRenderObject)
    {
        if (runeRenderObject == null)
        {
            return null;
        }

        return new RenderableEntity
        {
            RederableId = runeRenderObject.RederableId,
            RenderBumped = runeRenderObject.RenderBumped,
            RenderCalculateBoundingBox = runeRenderObject.RenderCalculateBoundingBox,
            RenderChildren = CreateRenderChildren(runeRenderObject.RenderChildren),
            RenderCollides = runeRenderObject.RenderCollides,
            RenderGuildCrest = runeRenderObject.RenderGuildCrest,
            RenderHasLightEffects = runeRenderObject.RenderHasLightEffects,
            RenderHasLoc = runeRenderObject.RenderHasLoc,
            RenderHasTextureSet = runeRenderObject.RenderHasTextureSet,
            RenderLoc = runeRenderObject.RenderLoc,
            RenderNationCrest = runeRenderObject.RenderNationCrest,
            RenderScale = runeRenderObject.RenderScale,
            RenderTargetBone = runeRenderObject.RenderTargetBone,
            RenderTemplate = CreateRenderTemplate(runeRenderObject.RenderTemplate),
            RenderTextureSets = CreateRenderTextureSets(runeRenderObject.RenderTextureSets),
            RenderVpActive = runeRenderObject.RenderVpActive
        };
    }

    private static RenderTemplateEntity? CreateRenderTemplate(RenderTemplate? renderTemplate)
    {
        if (renderTemplate is null)
        {
            return null;
        }

        return new RenderTemplateEntity
        {
            TemplateBoneLength = renderTemplate.TemplateBoneLength,
            TemplateClipMap = renderTemplate.TemplateClipMap,
            TemplateCullFace = renderTemplate.TemplateCullFace,
            TemplateHasMesh = renderTemplate.TemplateHasMesh,
            TemplateIlluminated = renderTemplate.TemplateIlluminated,
            TemplateLightTwoSide = renderTemplate.TemplateLightTwoSide,
            MeshSet = CreateMeshSet(renderTemplate.TemplateMesh?.MeshSet),
            TemplateObjectCanFade = renderTemplate.TemplateObjectCanFade,
            TemplateShininess = renderTemplate.TemplateShininess,
            TemplateSpecularMap = renderTemplate.TemplateSpecularMap,
            TemplateTracker = renderTemplate.TemplateTracker
        };
    }

    private static RenderableEntity[] CreateRenderChildren(uint[]? renderChildren)
    {
        if (renderChildren == null)
        {
            return Array.Empty<RenderableEntity>();
        }

        var children = new RenderableEntity[renderChildren.Length];

        for (int i = 0; i < renderChildren.Length; i++)
        {
            var child = CreateRenderable(renderChildren[i]);

            if (child is not null)
            {
                children[i] = child;
            }
        }
        return children;
    }

    private static RuneBodyPartsEntity[] CreateBodyParts(RuneBodyParts[] runeRuneBodyPartsArray)
    {
        // dm
        var bodyParts = new RuneBodyPartsEntity[runeRuneBodyPartsArray.Length];
        for (int i = 0; i < bodyParts.Length; i++)
        {
            var runeBodyPart = runeRuneBodyPartsArray[i];
            var id = runeBodyPart.BodyPartRender;
            var path = Path.Combine(RENDER_PATH, $"{id}.json");
            var json = File.ReadAllText(path);
            var renderable = JsonSerializer.Deserialize<Renderable>(json);

            if (renderable is null)
            {
                continue;
            }

            bodyParts[i] = new RuneBodyPartsEntity(runeBodyPart.BodyPartPosition, id)
            {

                BodyPart = new RenderableEntity
                {
                    RederableId = id,
                    RenderBumped = renderable.RenderBumped,
                    RenderCalculateBoundingBox = renderable.RenderCalculateBoundingBox,
                    RenderChildren = CreateRenderChildren(renderable.RenderChildren),
                    RenderCollides = renderable.RenderCollides,
                    RenderGuildCrest = renderable.RenderGuildCrest,
                    RenderHasLightEffects = renderable.RenderHasLightEffects,
                    RenderHasLoc = renderable.RenderHasLoc,
                    RenderHasTextureSet = renderable.RenderHasTextureSet,
                    RenderLoc = renderable.RenderLoc,
                    RenderNationCrest = renderable.RenderNationCrest,
                    RenderScale = renderable.RenderScale,
                    RenderTargetBone = renderable.RenderTargetBone,
                    RenderTemplate = CreateRenderTemplateEntity(renderable),
                    RenderTextureSets = CreateRenderTextureSets(renderable.RenderTextureSets),
                    RenderVpActive = renderable.RenderVpActive
                }
            };
        }
        return bodyParts;
    }

    private static RenderTemplateEntity? CreateRenderTemplateEntity(Renderable renderable)
    {
        if (renderable.RenderTemplate is null)
        {
            return null;
        }

        return new RenderTemplateEntity
        {
            TemplateBoneLength = renderable.RenderTemplate.TemplateBoneLength,
            TemplateClipMap = renderable.RenderTemplate.TemplateClipMap,
            TemplateCullFace = renderable.RenderTemplate.TemplateCullFace,
            TemplateHasMesh = renderable.RenderTemplate.TemplateHasMesh,
            TemplateIlluminated = renderable.RenderTemplate.TemplateIlluminated,
            TemplateLightTwoSide = renderable.RenderTemplate.TemplateLightTwoSide,
            MeshSet = CreateMeshSet(renderable.RenderTemplate.TemplateMesh?.MeshSet),
            TemplateObjectCanFade = renderable.RenderTemplate.TemplateObjectCanFade,
            TemplateShininess = renderable.RenderTemplate.TemplateShininess,
            TemplateSpecularMap = renderable.RenderTemplate.TemplateSpecularMap,
            TemplateTracker = renderable.RenderTemplate.TemplateTracker
        };
    }

    private static RenderTextureSetEntity[] CreateRenderTextureSets(RenderTextureSet[] bodyPartRenderTextureSets)
    {
        var textureSets = new RenderTextureSetEntity[bodyPartRenderTextureSets.Length];
        for (int i = 0; i < textureSets.Length; i++)
        {
            var textureSet = bodyPartRenderTextureSets[i];
            textureSets[i] = new RenderTextureSetEntity
            {
                TextureCompress = (bool)textureSet.TextureData?.TextureCompress,
                TextureCreateMipMaps = textureSet.TextureData.TextureCreateMipMaps,
                TextureDetailNormalMap = textureSet.TextureData.TextureDetailNormalMap,
                TextureId = textureSet.TextureData.TextureId,
                TextureNormalMap = textureSet.TextureData.TextureNormalMap,
                TexturePath = $"{TEXTURE_PATH}\\{textureSet.TextureData.TextureId}.tga",
                TextureTransparent = textureSet.TextureData.TextureTransparent
            };
        }
        return textureSets;
    }

    private static ICollection<MeshEntity> CreateMeshSet(ICollection<MeshSet>? templateMeshMeshSet)
    {
        var meshSets = new List<MeshEntity>();

        if (templateMeshMeshSet == null)
        {
            return meshSets;
        }

        foreach (var meshSet in templateMeshMeshSet.Where(t=>t.PolymeshId > 0))
        {
            var path = $"{MESH_PATH}\\{meshSet.PolymeshId}.json";
            var json = File.ReadAllText(path);
            var mesh = JsonSerializer.Deserialize<Mesh>(json);

            if (mesh is null)
            {
                continue;
            }

            meshSets.Add(new MeshEntity
            {
                MeshDistance = mesh.MeshDistance,
                PolymeshId = meshSet.PolymeshId,
                MeshEndPoint = mesh.MeshEndPoint,
                MeshExtraIndices = SetExtraIndices(mesh.MeshExtraIndices),
                MeshIndices = mesh.MeshIndices,
                MeshNormals = ToNumericList(mesh.MeshNormals),
                MeshStartPoint = mesh.MeshStartPoint,
                MeshVertices = ToNumericList(mesh.MeshVertices)

            });
        }
        return meshSets;
    }

    private static List<(int, int, int[])> SetExtraIndices(object[] meshExtraIndices)
    {
        // holy shit this is ugly
        var extraIndices = new List<(int, int, int[])>();

        foreach (JsonElement jsonElement in meshExtraIndices)
        {
            // there's only one I believe
            var x = jsonElement.GetRawText();
            var trimmed = x.Substring(1, x.Length - 2).Trim().Replace("\r\n", "").Replace(" ", ""); // remove [ and ] and trim

            // [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0], [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
            var y = trimmed.Split("],", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var s in y)
            {
                int one = int.Parse(s[0].ToString());
                int two = int.Parse(s[2].ToString());

                List<int> three = new();

                var temp =
                    s.Substring(5, s.Length - 6);
                var intArray = temp
                    .Split(',');
                for (int ia = 0; ia < intArray.Length; ia++)
                {
                    three.Add(int.Parse(intArray[ia]));
                }
                extraIndices.Add(new(one, two, three.ToArray()));
            }
        }
        return extraIndices;
    }

    public static List<Vector3> ToNumericList(float[][] v3s)
    {
        var result = new List<Vector3>();
        foreach (var v3 in v3s)
        {
            result.Add(ToNumericVector3(v3));
        }
        return result;
    }

    public static Vector3 ToNumericVector3(float[] v3)
    {
        return new Vector3(v3[0], v3[1], v3[2]);
    }

}