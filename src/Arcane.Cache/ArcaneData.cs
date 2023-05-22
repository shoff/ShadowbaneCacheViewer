namespace Arcane.Cache;

using System.Text.Json;
using Arcane.Cache.Json.CObjects;
using Arcane.Cache.Json.Render;
using Arcane.Cache.Models;
using UnityEngine;
using Mesh = Json.Mesh.Mesh;

public sealed class ArcaneData
{
    private readonly List<string> renderAssets;
    private readonly List<string> meshAssets;

    public ArcaneData()
    {
        this.renderAssets = Directory.GetFiles(RENDER_PATH).ToList();
        this.meshAssets = Directory.GetFiles(MESH_PATH).ToList();
    }

    public SBRune? GetRune(uint id)
    {
        var assetFiles = Directory.GetFiles(Path.Combine(RUNE_PATH)).ToList();
        var file = assetFiles.FirstOrDefault(x => x.Contains($"{id}.json"));
        if (file == null)
        {
            return null;
        }

        var rune = JsonSerializer.Deserialize<Rune>(File.ReadAllText(file));
        rune!.RuneId = id;
        return new SBRune
        {
            Icon = rune.Icon,
            RuneId = rune.RuneId,
            RuneType = rune.RuneType,
            Renderable = GetById(rune.RenderObject),
            RenderObject = rune.RenderObject,
            RenderObjectLowDetail = rune.RenderObjectLowDetail,
            RuneAttack = rune.RuneAttack,
            RuneDefense = rune.RuneDefense,
            RuneAttrAdjs = rune.RuneAttrAdjs,
            RuneBeard = rune.RuneBeard,
            RuneBodyPartsArray = CreateBodyParts(rune.RuneBodyPartsArray),
            RuneCanFly = rune.RuneCanFly,
            RuneClassIcon = rune.RuneClassIcon,
            RuneCreationCost = rune.RuneCreationCost,
            RuneDeathEffect = rune.RuneDeathEffect,
            RuneDsc = rune.RuneDsc,
            RuneEnchantmentType = rune.RuneEnchantmentType,
            RuneEnemyGender = rune.RuneEnemyGender,
            RuneEnemyMonsterTypes = rune.RuneEnemyMonsterTypes,
            RuneExpReqToLevel = rune.RuneExpReqToLevel,
            RuneFxTxt = rune.RuneFxTxt,
            RuneGroup = rune.RuneGroup,
            RuneGroupeeMonsterTypes = rune.RuneGroupeeMonsterTypes,
            RuneGroupRoleSet = rune.RuneGroupRoleSet,
            RuneGroupTactics = rune.RuneGroupTactics,
            RuneHair = rune.RuneHair,
            RuneHealth = rune.RuneHealth,
            ArcHardpointList = rune.ArcHardpointList,
            CombatAttackResist = rune.CombatAttackResist,
            CombatPowers = rune.CombatPowers,
            CullDistance = rune.CullDistance,
            DefaultAlignment = rune.DefaultAlignment,
            DoubleFusion = rune.DoubleFusion,
            ForwardVector = rune.ForwardVector,
            Gravity = rune.Gravity,
            GravityF = rune.GravityF,
            HealthCurrent = rune.HealthCurrent,
            HealthFull = rune.HealthFull,
            ItemAttrReq = rune.ItemAttrReq,
            ItemBaneRank = rune.ItemBaneRank,
            ItemBaseName = rune.ItemBaseName,
            ItemBookArcana = rune.ItemBookArcana,
            ItemClassReq = rune.ItemClassReq,
            ItemDiscReq = rune.ItemDiscReq,
            ItemDsc = rune.ItemDsc,
            ItemEqSlotsAnd = rune.ItemEqSlotsAnd,
            ItemEqSlotsOr = rune.ItemEqSlotsOr,
            ItemEqSlotsType = rune.ItemEqSlotsType,
            ItemEqSlotsValue = rune.ItemEqSlotsValue,
            ItemFlags = rune.ItemFlags,
            ItemHasStub = rune.ItemHasStub,
            ItemHealthFull = rune.ItemHealthFull,
            ItemIgnoreSavedActions = rune.ItemIgnoreSavedActions,
            ItemInitialCharges = rune.ItemInitialCharges,
            ItemLevelReq = rune.ItemLevelReq,
            ItemOfferingAdjustments = rune.ItemOfferingAdjustments,
            ItemOfferingInfo = rune.ItemOfferingInfo,
            ItemParryAnimId = rune.ItemParryAnimId,
            ItemPassiveDefenseMod = rune.ItemPassiveDefenseMod,
            ItemPostItemId = rune.ItemPostItemId,
            ItemPowerAction = rune.ItemPowerAction,
            ItemPowerGrant = rune.ItemPowerGrant,
            ItemRaceReq = rune.ItemRaceReq,
            ItemRankReq = rune.ItemRankReq,
            ItemRenderObjectFemale = rune.ItemRenderObjectFemale,
            ItemResourceCosts = rune.ItemResourceCosts,
            ItemSexReq = rune.ItemSexReq,
            ItemSheathable = rune.ItemSheathable,
            ItemSkillMasteryUsed = rune.ItemSkillMasteryUsed,
            ItemSkillReq = rune.ItemSkillReq,
            ItemSkillUsed = rune.ItemSkillUsed,
            ItemTakeable = rune.ItemTakeable,
            ItemType = rune.ItemType,
            ItemUseFlags = rune.ItemUseFlags,
            ItemUserPowerActions = rune.ItemUserPowerActions,
            ItemValue = rune.ItemValue,
            ItemWT = rune.ItemWT,
            MaxTrackingDistance = rune.MaxTrackingDistance,
            ObjName = rune.ObjName,
            Pickable = rune.Pickable,
            RuneHelperMonsterTypes = rune.RuneHelperMonsterTypes,
            RuneInventoryContents = rune.RuneInventoryContents,
            RuneIsStandardCharacterCreation = rune.RuneIsStandardCharacterCreation,
            RuneLevel = rune.RuneLevel,
            RuneMana = rune.RuneMana,
            RuneMaxAttrAdjs = rune.RuneMaxAttrAdjs,
            RuneMaxDamage = rune.RuneMaxDamage,
            RuneMinDamage = rune.RuneMinDamage,
            RuneNaturalattacks = rune.RuneNaturalattacks,
            RuneNaturalPowerAttack = rune.RuneNaturalPowerAttack,
            RuneNotEnemyMonsterTypes = rune.RuneNotEnemyMonsterTypes,
            RunePracsPerLevel = rune.RunePracsPerLevel,
            RuneRank = rune.RuneRank,
            RuneRenderable = rune.RuneRenderable,
            RuneScaleFactor = rune.RuneScaleFactor,
            RuneSex = rune.RuneSex,
            RuneSkeleton = rune.RuneSkeleton,
            RuneSkillAdjs = rune.RuneSkillAdjs,
            RuneSkillGrants = rune.RuneSkillGrants,
            RuneSlopeHugger = rune.RuneSlopeHugger,
            RuneSparseData = rune.RuneSparseData,
            RuneSpeed = rune.RuneSpeed,
            RuneStamina = rune.RuneStamina,
            RuneSubType = rune.RuneSubType,
            RuneTombstoneId = rune.RuneTombstoneId,
            Scale = rune.Scale,
            SoundEvents = rune.SoundEvents,
            SoundTable = rune.SoundTable,
            SparseData = rune.SparseData,
            TrackingName = rune.TrackingName
        };
    }

    private SBRuneBodyParts[] CreateBodyParts(RuneBodyParts[] runeRuneBodyPartsArray)
    {
        // dm
        var bodyParts = new SBRuneBodyParts[runeRuneBodyPartsArray.Length];
        var assetFiles = Directory.GetFiles(Path.Combine(RENDER_PATH)).ToList();

        // for each body part, get the renderable and mesh
        for (int i = 0; i < runeRuneBodyPartsArray.Length; i++)
        {
            var runeBodyPart = runeRuneBodyPartsArray[i];
            var id = runeBodyPart.BodyPartRender;
            var sbBodyPart = new SBRuneBodyParts(runeBodyPart.BodyPartPosition, id)
            {
                BodyPart = GetById(id)
            };
            bodyParts[i] = sbBodyPart;
        }
        return bodyParts;
    }

    private Renderable? GetRenderableById(uint id)
    {
        var file = this.renderAssets.FirstOrDefault(x => x.Contains($"{id}.json"));
        if (file == null)
        {
            return null;
        }
        var renderable = JsonSerializer.Deserialize<Renderable>(File.ReadAllText(file));
        return renderable;
    }

    private SBMesh? GetMeshById(uint id)
    {
        var file = this.meshAssets.FirstOrDefault(x => x.Contains($"{id}.json"));
        
        if (file == null)
        {
            return null;
        }
        
        var mesh = JsonSerializer.Deserialize<Mesh>(File.ReadAllText(file));

        if (mesh == null)
        {
            return null;
        }

        return new SBMesh
        {
            MeshDistance = mesh.MeshDistance,
            MeshId = mesh.MeshId,
            MeshEndPoint = mesh.MeshEndPoint,
            MeshStartPoint = mesh.MeshStartPoint,
            MeshRefPoint = mesh.MeshRefPoint,
            MeshExtraIndices = SetExtraIndices(mesh.MeshExtraIndices),
            MeshIndices = mesh.MeshIndices,
            // MeshName = mesh.MeshName,
            MeshNormals = SetNormals(mesh.MeshNormals),
            MeshTangentVertices = mesh.MeshTangentVertices,
            MeshUv = SetUV(mesh.MeshUv),
            MeshUseFaceNormals = mesh.MeshUseFaceNormals,
            MeshUseTangentBasis = mesh.MeshUseTangentBasis,
            MeshVertices = SetVertices(mesh.MeshVertices)
        };
    }

    private List<Vector3> SetVertices(float[][] meshMeshVertices)
    {
        var vertices = new List<Vector3>();
        foreach (var vertex in meshMeshVertices)
        {
            vertices.Add(new Vector3(vertex[0], vertex[1], vertex[2]));
        }
        return vertices;
    }

    private List<Vector2> SetUV(float[][] meshMeshUv)
    {
        var uvs = new List<Vector2>();
        foreach (var uv in meshMeshUv)
        {
            uvs.Add(new Vector2(uv[0], uv[1]));
        }
        return uvs;
    }

    private List<Vector3> SetNormals(float[][] meshMeshNormals)
    {
        var normals = new List<Vector3>();
        foreach (var normal in meshMeshNormals)
        {
            normals.Add(new Vector3(normal[0], normal[1], normal[2]));
        }
        return normals;
    }

    private List<(int, int, int[])> SetExtraIndices(object[] meshExtraIndices)
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

    public SBRenderable GetById(uint id)
    {
        // first get the renderable
        var renderable = GetRenderableById(id);
        if (renderable == null)
        {
            throw new Exception($"Renderable not found for id {id}");
        }
        var sbRenderable = new SBRenderable
        {
            RederableId = id,
            RenderBumped = renderable.RenderBumped,
            RenderCalculateBoundingBox = renderable.RenderCalculateBoundingBox,
            RenderChildren = renderable.RenderChildren.Select(GetById).ToArray(),
            RenderCollides = renderable.RenderCollides,
            RenderGuildCrest = renderable.RenderGuildCrest,
            RenderHasLightEffects = renderable.RenderHasLightEffects,
            RenderHasLoc = renderable.RenderHasLoc,
            RenderHasTextureSet = renderable.RenderHasTextureSet,
            RenderLoc = renderable.RenderLoc,
            RenderNationCrest = renderable.RenderNationCrest,
            RenderScale = renderable.RenderScale,
            RenderTargetBone = renderable.RenderTargetBone,
            RenderTemplate = Convert(renderable.RenderTemplate),
            RenderTextureSets = renderable.RenderTextureSets.Select(Convert).ToArray(),
            RenderVpActive = renderable.RenderVpActive
        };
        return sbRenderable ??= new SBRenderable();
    }

    private SBRenderTextureSet Convert(RenderTextureSet ts)
    {
        var textureData = ts.TextureData;
        SBTextureData sbTextureData;

        if (textureData != null)
        {
            sbTextureData = new SBTextureData
            {
                TextureCompress = textureData.TextureCompress,
                TextureCreateMipMaps = textureData.TextureCreateMipMaps,
                TextureDetailNormalMap = textureData.TextureDetailNormalMap,
                TextureId = textureData.TextureId,
                TextureNormalMap = textureData.TextureNormalMap,
                TextureTransparent = textureData.TextureTransparent,
                TextureWrap = textureData.TextureWrap,
                TexturePath = Path.Combine(TEXTURE_PATH, $"{textureData.TextureId}.tga")
            };
        }
        else
        {
            // yuck 
            sbTextureData = new SBTextureData();
        }

        var textureSet = new SBRenderTextureSet
        {
            TextureData = sbTextureData,
            TextureType = ts.TextureType
        };

        return textureSet;
    }

    private SBRenderTemplate Convert(RenderTemplate renderTemplate)
    {
        var template = new SBRenderTemplate
        {
            TemplateBoneLength = renderTemplate.TemplateBoneLength,
            TemplateTracker = renderTemplate.TemplateTracker,
            TemplateObjectCanFade = renderTemplate.TemplateObjectCanFade,
            TemplateIlluminated = renderTemplate.TemplateIlluminated,
            TemplateClipMap = renderTemplate.TemplateClipMap,
            TemplateCullFace = renderTemplate.TemplateCullFace,
            TemplateLightTwoSide = renderTemplate.TemplateLightTwoSide,
            TemplateSpecularMap = renderTemplate.TemplateSpecularMap,
            TemplateShininess = renderTemplate.TemplateShininess,
            TemplateHasMesh = renderTemplate.TemplateHasMesh,
            TemplateMesh = Convert(renderTemplate.TemplateMesh)
        };
        return template;
    }

    private SBTemplateMesh Convert(TemplateMesh mesh)
    {
        var meshSet = new SBTemplateMesh();

        foreach (var set in mesh.MeshSet)
        {
            meshSet.MeshSet.Add(new SBMeshSet
            {
                Mesh = GetMeshById(set.PolymeshId)
            });
        }
        return meshSet;
    }
}

