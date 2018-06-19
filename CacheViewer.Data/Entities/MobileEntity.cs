namespace CacheViewer.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// </summary>
    public class MobileEntity
    {
        /// <summary>
        /// </summary>
        [Key]
        public int MobileEntityId { get; set; }

        /// <summary>
        ///     Gets or sets the object identifier.
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the ai description.
        /// </summary>
        public string AiDescription { get; set; }

        /// <summary>
        ///     Gets or sets the mob token.
        /// </summary>
        public long MobToken { get; set; }

        /// <summary>
        ///     Gets or sets the gender.
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        ///     Gets or sets the training power bonus.
        /// </summary>
        public int TrainingPowerBonus { get; set; }

        /// <summary>
        ///     Gets or sets the type of the rune.
        /// </summary>
        public int RuneType { get; set; }

        /// <summary>
        ///     Gets or sets the rune category.
        /// </summary>
        public int RuneCategory { get; set; }

        /// <summary>
        ///     Gets or sets the rune stack rank.
        /// </summary>
        public int RuneStackRank { get; set; }

        /// <summary>
        ///     Gets or sets the rune cost.
        /// </summary>
        public int RuneCost { get; set; }

        /// <summary>
        ///     Gets or sets the number of skills required.
        /// </summary>
        public int NumberOfSkillsRequired { get; set; }

        /// <summary>
        ///     Gets or sets the level required.
        /// </summary>
        public int LevelRequired { get; set; }

        /// <summary>
        ///     Gets or sets the power identifier.
        /// </summary>
        public int PowerId { get; set; }

        /// <summary>
        ///     Gets or sets the size of the name.
        /// </summary>
        public int NameSize { get; set; }

        /// <summary>
        ///     Gets or sets the pet name count.
        /// </summary>
        public int PetNameCount { get; set; }

        /// <summary>
        ///     Gets or sets the prohibits race toggle.
        /// </summary>
        public byte ProhibitsRaceToggle { get; set; }

        /// <summary>
        ///     Gets or sets some kind of type hash.
        /// </summary>
        public long SomeKindOfTypeHash { get; set; }

        /// <summary>
        ///     Gets or sets the required gender.
        /// </summary>
        public int RequiredGender { get; set; }

        /// <summary>
        ///     Gets or sets the minimum required level.
        /// </summary>
        public int MinRequiredLevel { get; set; }

        /// <summary>
        ///     Gets or sets something with pets.
        /// </summary>
        public int SomethingWithPets { get; set; }

        /// <summary>
        ///     Gets or sets the size of the second name.
        /// </summary>
        public int SecondNameSize { get; set; }

        /// <summary>
        ///     Gets or sets the is pet or rune.
        /// </summary>
        public int IsPetOrRune { get; set; }

        /// <summary>
        ///     Gets or sets the wolfpack create date.
        /// </summary>
        public DateTime WolfpackCreateDate { get; set; }
    }
}