namespace Arcane.Data.Mongo.Entities;

public class RuneBodyPartsEntity
{
    public RuneBodyPartsEntity(float[] bodyPartPosition, uint renderId)
    {
        this.BodyPartRender = renderId;
        this.BodyPartPosition = bodyPartPosition;
    }

    public uint BodyPartRender { get; }
    public float[] BodyPartPosition { get; }
    public RenderableEntity? BodyPart { get; set; }
}