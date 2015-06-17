
namespace CacheViewer.Domain.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using CacheViewer.Domain.Models;

    /// <summary>
    /// </summary>
    public class MobileEntity
    {
        /// <summary>
        /// </summary>
        [Key]
        public int MobileEntityId { get; set; }

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        public int ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the ai description.
        /// </summary>
        /// <value>
        ///   The ai description.
        /// </value>
        public string AiDescription { get; set; }

        /// <summary>
        ///   Gets or sets the mob token.
        /// </summary>
        /// <value>
        ///   The mob token.
        /// </value>
        public long MobToken { get; set; }

        /// <summary>
        ///   Gets or sets the gender.
        /// </summary>
        /// <value>
        ///   The gender.
        /// </value>
        public int Gender { get; set; }

        /// <summary>
        ///   Gets or sets the training power bonus.
        /// </summary>
        /// <value>
        ///   The training power bonus.
        /// </value>
        public int TrainingPowerBonus { get; set; }

        /// <summary>
        ///   Gets or sets the type of the rune.
        /// </summary>
        /// <value>
        ///   The type of the rune.
        /// </value>
        public int RuneType { get; set; }

        /// <summary>
        ///   Gets or sets the rune category.
        /// </summary>
        /// <value>
        ///   The rune category.
        /// </value>
        public int RuneCategory { get; set; }

        /// <summary>
        ///   Gets or sets the rune stack rank.
        /// </summary>
        /// <value>
        ///   The rune stack rank.
        /// </value>
        public int RuneStackRank { get; set; }

        /// <summary>
        ///   Gets or sets the rune cost.
        /// </summary>
        /// <value>
        ///   The rune cost.
        /// </value>
        public int RuneCost { get; set; }

        /// <summary>
        ///   Gets or sets the number of skills required.
        /// </summary>
        /// <value>
        ///   The number of skills required.
        /// </value>
        public int NumberOfSkillsRequired { get; set; }

        /// <summary>
        ///   Gets or sets the level required.
        /// </summary>
        /// <value>
        ///   The level required.
        /// </value>
        public int LevelRequired { get; set; }

        /// <summary>
        ///   Gets or sets the power identifier.
        /// </summary>
        /// <value>
        ///   The power identifier.
        /// </value>
        public int PowerId { get; set; }

        /// <summary>
        ///   Gets or sets the size of the name.
        /// </summary>
        /// <value>
        ///   The size of the name.
        /// </value>
        public int NameSize { get; set; }

        /// <summary>
        ///   Gets or sets the pet name count.
        /// </summary>
        /// <value>
        ///   The pet name count.
        /// </value>
        public int PetNameCount { get; set; }

        /// <summary>
        ///   Gets or sets the prohibits race toggle.
        /// </summary>
        /// <value>
        ///   The prohibits race toggle.
        /// </value>
        public byte ProhibitsRaceToggle { get; set; }

        /// <summary>
        ///   Gets or sets some kind of type hash.
        /// </summary>
        /// <value>
        ///   Some kind of type hash.
        /// </value>
        public long SomeKindOfTypeHash { get; set; }

        /// <summary>
        ///   Gets or sets the required gender.
        /// </summary>
        /// <value>
        ///   The required gender.
        /// </value>
        public int RequiredGender { get; set; }

        /// <summary>
        ///   Gets or sets the minimum required level.
        /// </summary>
        /// <value>
        ///   The minimum required level.
        /// </value>
        public int MinRequiredLevel { get; set; }
        

        /// <summary>
        ///   Gets or sets something with pets.
        /// </summary>
        /// <value>
        ///   Something with pets.
        /// </value>
        public int SomethingWithPets { get; set; }

        /// <summary>
        /// Gets or sets the size of the second name.
        /// </summary>
        /// <value>
        /// The size of the second name.
        /// </value>
        public int SecondNameSize { get; set; }

        /// <summary>
        /// Gets or sets the is pet or rune.
        /// </summary>
        /// <value>
        /// The is pet or rune.
        /// </value>
        public int IsPetOrRune { get; set; }

        /// <summary>
        ///   Gets or sets the wolfpack create date.
        /// </summary>
        /// <value>
        ///   The wolfpack create date.
        /// </value>
        public DateTime WolfpackCreateDate { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Mobile"/> to <see cref="MobileEntity"/>.
        /// </summary>
        /// <param name="mobile">The mobile.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator MobileEntity(Mobile mobile)
        {
            MobileEntity entity = new MobileEntity();

            entity.ObjectId = (int)mobile.ObjId;
            entity.AiDescription = mobile.AiDescription;
            entity.Gender = mobile.Gender;
            entity.LevelRequired = mobile.LevelRequired;
            entity.MinRequiredLevel = mobile.MinRequiredLevel;
            entity.MobToken = (long)mobile.MobToken;
            entity.NumberOfSkillsRequired = mobile.NumberOfSkillsRequired;
            entity.Name = mobile.Name;
            entity.SomeKindOfTypeHash = (long)mobile.SomeKindOfTypeHash;
            entity.RuneCategory = (int)mobile.RuneCategory;
            entity.WolfpackCreateDate = mobile.WolfpackCreateDate;
            entity.SecondNameSize = (int)mobile.SecondNameSize;
            entity.IsPetOrRune = (int)mobile.IsPetOrRune;
            return entity;
        }
    }
}