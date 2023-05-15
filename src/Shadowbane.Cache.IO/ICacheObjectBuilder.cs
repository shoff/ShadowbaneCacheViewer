namespace Shadowbane.Cache.IO;

public interface ICacheRecordBuilder
{
    ICacheRecord? NameOnly(uint identity);
    ICacheRecord? CreateAndParse(uint identity);
}