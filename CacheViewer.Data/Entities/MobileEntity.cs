namespace CacheViewer.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// </summary>
    public class MobileEntity
    {
        [Key]
        public int MobileEntityId { get; set; }
        public int ObjectId { get; set; }
        public string Name { get; set; }
        public string AiDescription { get; set; }
        public long MobToken { get; set; }
        public int Gender { get; set; }
        public int TrainingPowerBonus { get; set; }
        public int RuneType { get; set; }
        public int RuneCategory { get; set; }
        public int RuneStackRank { get; set; }
        public int RuneCost { get; set; }
        public int NumberOfSkillsRequired { get; set; }
        public int LevelRequired { get; set; }
        public int PowerId { get; set; }
        public int NameSize { get; set; }
        public int PetNameCount { get; set; }
        public byte ProhibitsRaceToggle { get; set; }
        public long SomeKindOfTypeHash { get; set; }
        public int RequiredGender { get; set; }
        public int MinRequiredLevel { get; set; }
        public int SomethingWithPets { get; set; }
        public int SecondNameSize { get; set; }
        public int IsPetOrRune { get; set; }
        public DateTime WolfpackCreateDate { get; set; }
    }
}