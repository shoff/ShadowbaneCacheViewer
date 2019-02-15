namespace CacheViewer.Domain.Models
{
    public class StructureValidationResult
    {
        public const int NOT_READ = -1;
        public int Id { get; set; }
        public long InitialOffset { get; set; }
        public long EndingOffset { get; set; }
        public bool NullTerminatorRead { get; set; }
        public int NullTerminator { get; set; }
        public uint FirstInt { get; set; }
        public uint SecondInt { get; set; }
        public uint ThirdInt { get; set; }
        public int Range { get; set; }
        public bool IsValidRenderId { get; set; }
        public int BytesLeftInObject { get; set; }
        public bool IsValid { get; set; }
        public bool PaddingIsValid =>
            (FirstInt < 51) &&
            (SecondInt == 0 || SecondInt == 1 || SecondInt == 2 || SecondInt == 3 || SecondInt == 4);

    }


}