namespace Shadowbane.Cache.IO;

public interface ICacheObjectBuilder
{
    ICacheObject? CreateAndParse(uint identity);
}