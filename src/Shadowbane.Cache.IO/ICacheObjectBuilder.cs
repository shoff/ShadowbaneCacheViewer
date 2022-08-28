namespace Shadowbane.Cache.IO;

public interface ICacheObjectBuilder
{
    ICacheObject? NameOnly(uint identity);
    ICacheObject? CreateAndParse(uint identity);
}