namespace CacheViewer.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Archive;
    using CacheViewer.Data;
    using Exportable;
    using Extensions;
    using NLog;

    public class Mobile : AnimationObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Mobile" /> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        public Mobile(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data,
            int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
            this.StatArray = new List<uint>();
        }

        /// <summary>
        ///     Gets or sets the is pet or rune.
        /// </summary>
        /// <value>
        ///     The is pet or rune.
        /// </value>
        public uint IsPetOrRune { get; set; }

        /// <summary>
        ///     Gets or sets the object identifier.
        /// </summary>
        /// <value>
        ///     The object identifier.
        /// </value>
        public decimal ObjId { get; set; }

        /// <summary>
        ///     Gets or sets the size of the second name.
        /// </summary>
        /// <value>
        ///     The size of the second name.
        /// </value>
        public uint SecondNameSize { get; set; }

        /// <summary>
        ///     Gets or sets the ai description.
        /// </summary>
        /// <value>
        ///     The ai description.
        /// </value>
        public string AiDescription { get; set; }

        /// <summary>
        ///     Gets or sets the mob token.
        /// </summary>
        /// <value>
        ///     The mob token.
        /// </value>
        public uint MobToken { get; set; }

        /// <summary>
        ///     Gets or sets the z offset.
        /// </summary>
        /// <value>
        ///     The z offset.
        /// </value>
        public float ZOffset { get; set; }

        /// <summary>
        ///     Gets or sets the skills map.
        /// </summary>
        /// <value>
        ///     The skills map.
        /// </value>
        public Dictionary<string, List<uint>> SkillsMap { get; set; }

        /// <summary>
        ///     Gets or sets the stat array.
        /// </summary>
        /// <value>
        ///     The stat array.
        /// </value>
        public List<uint> StatArray { get; set; }

        /// <summary>
        ///     Gets or sets the gender.
        /// </summary>
        /// <value>
        ///     The gender.
        /// </value>
        public int Gender { get; set; }

        /// <summary>
        ///     Gets or sets the training power bonus.
        /// </summary>
        /// <value>
        ///     The training power bonus.
        /// </value>
        public int TrainingPowerBonus { get; set; }

        /// <summary>
        ///     Gets or sets the type of the rune.
        /// </summary>
        /// <value>
        ///     The type of the rune.
        /// </value>
        public int RuneType { get; set; }

        /// <summary>
        ///     Gets or sets the rune category.
        /// </summary>
        /// <value>
        ///     The rune category.
        /// </value>
        public uint RuneCategory { get; set; }

        /// <summary>
        ///     Gets or sets the rune stack rank.
        /// </summary>
        /// <value>
        ///     The rune stack rank.
        /// </value>
        public int RuneStackRank { get; set; }

        /// <summary>
        ///     Gets or sets the rune cost.
        /// </summary>
        /// <value>
        ///     The rune cost.
        /// </value>
        public int RuneCost { get; set; }

        /// <summary>
        ///     Gets or sets the number of skills required.
        /// </summary>
        /// <value>
        ///     The number of skills required.
        /// </value>
        public int NumberOfSkillsRequired { get; set; }

        /// <summary>
        ///     Gets or sets the level required.
        /// </summary>
        /// <value>
        ///     The level required.
        /// </value>
        public int LevelRequired { get; set; }

        /// <summary>
        ///     Gets or sets the power identifier.
        /// </summary>
        /// <value>
        ///     The power identifier.
        /// </value>
        public int PowerId { get; set; }

        /// <summary>
        ///     Gets or sets the exit message.
        /// </summary>
        /// <value>
        ///     The exit message.
        /// </value>
        public string ExitMessage { get; set; }

        /// <summary>
        ///     Gets or sets the size of the name.
        /// </summary>
        /// <value>
        ///     The size of the name.
        /// </value>
        public int NameSize { get; set; }

        /// <summary>
        ///     Gets or sets the pet name count.
        /// </summary>
        /// <value>
        ///     The pet name count.
        /// </value>
        public int PetNameCount { get; set; }

        /// <summary>
        ///     Gets or sets the prohibits race toggle.
        /// </summary>
        /// <value>
        ///     The prohibits race toggle.
        /// </value>
        public byte ProhibitsRaceToggle { get; set; }

        /// <summary>
        ///     Gets or sets some kind of type hash.
        /// </summary>
        /// <value>
        ///     Some kind of type hash.
        /// </value>
        public uint SomeKindOfTypeHash { get; set; }

        /// <summary>
        ///     Gets or sets the required gender.
        /// </summary>
        /// <value>
        ///     The required gender.
        /// </value>
        public int RequiredGender { get; set; }

        /// <summary>
        ///     Gets or sets the minimum required level.
        /// </summary>
        /// <value>
        ///     The minimum required level.
        /// </value>
        public int MinRequiredLevel { get; set; }

        /// <summary>
        ///     Gets or sets the prohibits disc toggle.
        /// </summary>
        /// <value>
        ///     The prohibits disc toggle.
        /// </value>
        public byte ProhibitsDiscToggle { get; set; }

        /// <summary>
        ///     Gets or sets the prohibits class toggle.
        /// </summary>
        /// <value>
        ///     The prohibits class toggle.
        /// </value>
        public byte ProhibitsClassToggle { get; set; }

        /// <summary>
        ///     Gets or sets the four int array.
        /// </summary>
        /// <value>
        ///     The four int array.
        /// </value>
        public int[] FourIntArray { get; set; }

        /// <summary>
        ///     Gets or sets the four thousand int.
        /// </summary>
        /// <value>
        ///     The four thousand int.
        /// </value>
        public int FourThousandInt { get; set; }

        /// <summary>
        ///     Gets or sets something with pets.
        /// </summary>
        /// <value>
        ///     Something with pets.
        /// </value>
        public int SomethingWithPets { get; set; }

        /// <summary>
        ///     Gets or sets the wolfpack create date.
        /// </summary>
        /// <value>
        ///     The wolfpack create date.
        /// </value>
        public DateTime WolfpackCreateDate { get; set; }

        /// <summary>
        ///     Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        // ReSharper disable once FunctionComplexityOverflow
        public override void Parse(ArraySegment<byte> data)
        {
            this.ObjId = this.CacheIndex.Identity;

            this.FourIntArray = new int[4];
            using (var reader = data.CreateBinaryReaderUtf32())
            {
                // TNLC
                reader.ReadInt32();
                reader.ReadInt32();

                // flag
                this.NameSize = reader.ReadInt32();
                reader.BaseStream.Position += this.NameSize * 2;

                // reader.BaseStream.Position = this.InnerOffset;
                var b1 = reader.ReadByte();

                //This block seems to be static for all Type 13 Objects
                // this should always be 00 00 80 3F
                var f1 = reader.ReadSingle();

                var time = reader.ReadUInt32();
                this.WolfpackCreateDate = ((double) time).FromUnixTimeStamp();

                var someScale = reader.ReadToVector3();
                var u1 = reader.ReadUInt32(); //0
                Debug.Assert(u1 == 0);

                //4000
                this.FourThousandInt = reader.ReadInt32();

                // should be all zeros
                for (var ed = 0; ed < 6; ed++)
                {
                    var u2 = reader.ReadUInt32();
                    Debug.Assert(u2 == 0);
                }

                // is this a boolean?
                var b2 = reader.ReadByte(); //Data block 00
                this.RuneCategory = reader.ReadUInt32(); // Rune Icon

                this.ZOffset = reader.ReadSingle(); // Z offset. Undead = 2.0, Bats = 4.0

                this.IsPetOrRune = reader.ReadUInt32(); // 4 for summoned pets? 2 for some runes.

                var u11 = reader.ReadUInt32();

                this.MobToken = reader.ReadUInt32();
                /*
                    if(mobToken == 612015249){
                        uint aiSize;
                        wchar_t ai[aiSize];
                        uint padding[4];
                    } else {
                        uint padding[3];
                    }
                 */
                if (this.MobToken == 612015249)
                {
                    // Determines Aggro Type
                    var strSize = reader.ReadUInt32();
                    this.AiDescription = reader.ReadAsciiString(strSize);

                    // this needs to be read here because its 4 if mobtoken is this
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                }
                else if (this.MobToken == 2085359803)
                {
                    reader.ReadByte();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                }
                else
                {
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                }

                var unknownFloat = reader.ReadSingle();
                var zeroUint = reader.ReadUInt32(); // All runes have this, set to 1.0.
                this.SomeKindOfTypeHash = reader.ReadUInt32(); //Some kind of type hash

                var petIndicator = reader.ReadUInt32();

                if (petIndicator == 0)
                {
                    for (var be = 0; be < 18; be++)
                    {
                        reader.ReadUInt32();
                    }

                    var thirtyTwo = reader.ReadUInt32();
                    Debug.Assert(thirtyTwo == 32);

                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();

                    // unknown short
                    reader.ReadUInt16();

                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();

                    if (this.MobToken == 3851523961)
                    {
                        // CSR
                        // skip two reads
                        reader.ReadUInt32();
                        reader.ReadUInt32();
                    }
                }
                else
                {
                    for (var pj = 0; pj < 5; pj++)
                    {
                        reader.ReadUInt32();
                    }

                    var petTextSize = reader.ReadUInt32();
                    var petText = reader.ReadAsciiString(petTextSize);
                    var something = reader.ReadUInt16();
                    for (var pj = 0; pj < 33; pj++)
                    {
                        reader.ReadUInt32();
                    }
                }

                this.SecondNameSize = reader.ReadUInt32();
                if (this.SecondNameSize == this.NameSize)
                {
                    var alsoName = reader.ReadAsciiString(this.SecondNameSize);
                }

                //reader.ReadUInt32(); //All them Pets = 34
                //reader.ReadUInt32(); //All 0s
                //reader.ReadUInt32(); //All them Pets = 231
                //reader.ReadUInt32(); //All 0s
                //reader.ReadUInt32(); //All them Pets = Really large

                //// Always ends up Pet
                //this.PetNameCount = reader.ReadInt32();

                //byte[] newString11 = new byte[PetNameCount];

                //for (int i = 0; i < PetNameCount; i++)
                //{
                //    newString11[i] = reader.ReadByte();
                //    reader.ReadByte(); //Discard due to Unicode
                //    return;
                //}

                //for (int i = 0; i < 4; i++)
                //{
                //    reader.ReadUInt32();
                //    //Debug.Assert(reader.ReadUInt32() == 0); //All 0s            
                //}

                ////reader.ReadUInt32(); //All 0s
                ////reader.ReadUInt32(); //All 0s
                ////reader.ReadUInt32(); //All 0s
                ////reader.ReadUInt32(); //All 0s
                //this.SomethingWithPets = reader.ReadInt32(); // Something with Pets

                //for (int i = 0; i < 8; i++)
                //{
                //    reader.ReadUInt32();
                //    //Debug.Assert(reader.ReadUInt32() == 0); //All 0s
                //}

                //if (reader.ReadUInt32() == 32)
                //{
                //    if (reader.ReadByte() == 32)
                //    {
                //        reader.ReadUInt32();
                //    }
                //    else
                //    {
                //        reader.ReadUInt32();
                //        reader.ReadByte();
                //        reader.ReadByte();
                //        reader.ReadByte(); //Resync
                //    }
                //}

                //this.FourIntArray[0] = reader.ReadInt32();
                //this.FourIntArray[1] = reader.ReadInt32();
                //this.FourIntArray[2] = reader.ReadInt32();
                //this.FourIntArray[3] = reader.ReadInt32();

                //reader.ReadByte();
                //reader.ReadByte();

                //this.NameSize = reader.ReadInt32();
                //if (this.NameSize != this.Name.Length)
                //{
                //    reader.ReadUInt32();
                //    reader.ReadUInt32();
                //    this.NameSize = reader.ReadInt32();
                //}

                //// pretty sure they didn't mean to do this.
                //if (this.NameSize != this.Name.Length)
                //{
                //    reader.ReadUInt32();
                //    reader.ReadUInt32();
                //    reader.ReadUInt32();
                //    this.NameSize = reader.ReadInt32();
                //}

                //// Mob Name... the syncing above makes me hate myself

                //if (this.NameSize > 500)
                //{
                //    this.ExitMessage = "this.NameSize exceeded 500 which is not possible.";
                //    // should never hit this.
                //    return;
                //}

                //byte[] newString = new byte[this.NameSize];

                //for (int x = 0; x < this.NameSize; x++)
                //{
                //    newString[x] = reader.ReadByte();
                //    reader.ReadByte(); //Discard due to Unicode
                //}
                /*
                for (int i = 0; i < 6; i++)
                {
                    Debug.Assert(reader.ReadUInt32() == 0); //All 0s
                }

                reader.ReadUInt32(); //All 298s
                reader.ReadUInt32(); //All 0s
                reader.ReadUInt32(); //All 4s

                //Debug.Assert(reader.ReadUInt32() == 298);
                //Debug.Assert(reader.ReadUInt32() == 0);
                //Debug.Assert(reader.ReadUInt32() == 4);

                for (int i = 0; i < 6; i++)
                {
                    Debug.Assert(reader.ReadUInt32() == 0); //All 0s
                }

                //reader.ReadUInt32(); //All 0s
                //reader.ReadUInt32(); //All 0s
                //reader.ReadUInt32(); //All 0s

                //reader.ReadUInt32(); //All 0s
                //reader.ReadUInt32(); //All 0s
                //reader.ReadUInt32(); //All 0s

                //Race Requirements
                uint numberOfRaces = reader.ReadUInt32();
                uint[] raceArray = new uint[numberOfRaces];
                this.ProhibitsRaceToggle = reader.ReadByte(); // 0 = requires, 1 = prohibits

                for (int ix = 0; ix < numberOfRaces; ix++)
                {
                    raceArray[ix] = reader.ReadUInt32();
                }

                // Class Requirements
                uint numberOfClasses = reader.ReadUInt32();

                uint[] classArray = new uint[numberOfClasses];

                this.ProhibitsClassToggle = reader.ReadByte(); // 0 = requires, 1 = prohibits

                for (int i = 0; i < numberOfClasses; i++)
                {
                    classArray[i] = reader.ReadUInt32();
                }

                //Disc Requirements
                uint numberOfDiscs = reader.ReadUInt32();
                uint[] discArray = new uint[numberOfDiscs];
                this.ProhibitsDiscToggle = reader.ReadByte(); // 0 = requires, 1 = prohibits
                for (int i = 0; i < numberOfDiscs; i++)
                {
                    discArray[i] = reader.ReadUInt32();
                }

                //Stat Requirements
                uint numberOfStats = reader.ReadUInt32();

                for (int ti = 0; ti < numberOfStats; ti++)
                {
                    this.StatArray.Add(reader.ReadUInt32()); //Stat
                    this.StatArray.Add(reader.ReadUInt32()); //Min Required
                }

                this.MinRequiredLevel = reader.ReadInt32();
                reader.ReadUInt32(); // All 0s
                this.RequiredGender = reader.ReadInt32();


                uint effectCount = reader.ReadUInt32();

                List<uint> effectArray = new List<uint>();

                for (int id = 0; id < effectCount; id++)
                {
                    effectArray.Add(reader.ReadUInt32()); //Effect Token
                    effectArray.Add(reader.ReadUInt32()); //Modifier
                    effectArray.Add(reader.ReadUInt32()); //Always 0?
                }

                uint numberOfPowers = reader.ReadUInt32();
                this.SkillsMap = new Dictionary<String, List<uint>>();
                if (this.objId < 3000)
                {

                }    //System.out.println(this.objName + "(Powers: " + numberOfPowers + ")");
                for (int ui = 0; ui < numberOfPowers; ui++)
                {
                    List<uint> tempArray = new List<uint>();

                    this.PowerId = reader.ReadInt32();
                    tempArray.Add((uint)this.PowerId); // PowerID

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("\t(Power ID: ");
                    sb.Append(tempArray[tempArray.Count - 1]);
                    sb.Append(") -> ");

                    this.LevelRequired = reader.ReadInt32();
                    tempArray.Add((uint)this.LevelRequired); //Level Required
                    sb.Append("[Required Level: ");
                    sb.Append(tempArray[tempArray.Count - 1]);
                    sb.Append("]");

                    uint temp = reader.ReadUInt32();

                    if (temp != 0)
                    {
                        this.ExitMessage = "Temp did not equal zero around line 325. Exiting.";
                        return;
                    }    //System.exit(0);

                    tempArray.Add(temp);

                    this.NumberOfSkillsRequired = reader.ReadInt32();

                    tempArray.Add((uint)this.NumberOfSkillsRequired);

                    sb.Append("[Required Skills(" + tempArray[tempArray.Count - 1] + "): ");

                    for (int z = 0; z < this.NumberOfSkillsRequired; z++)
                    {
                        tempArray.Add(reader.ReadUInt32()); //Required Skill
                        sb.Append(tempArray[tempArray.Count - 1] + " @ ");
                        tempArray.Add(reader.ReadUInt32()); //Level
                        sb.Append(tempArray[tempArray.Count - 1]);
                        if (z < this.NumberOfSkillsRequired - 1)
                        {
                            sb.Append(" | ");
                        }
                    }
                    sb.Append("]");
                    uint row4 = reader.ReadUInt32(); //Number of Powers Required
                    tempArray.Add(row4);
                    sb.Append("[Required Powers(" + tempArray[tempArray.Count - 1] + "): ");
                    for (int z = 0; z < row4; z++)
                    {
                        tempArray.Add(reader.ReadUInt32()); //Required Skill
                        sb.Append(tempArray[tempArray.Count - 1] + " @ ");

                        tempArray.Add(reader.ReadUInt32()); //Level
                        sb.Append(tempArray[tempArray.Count - 1]);
                        if (z < row4 - 1)
                        {
                            sb.Append(" | ");
                        }
                    }
                    sb.Append("]");

                    temp = reader.ReadUInt32();
                    if (temp != 0)
                    {
                        logger.Error("Mobile Parse failed for CacheIndex {0} when iterating powers.", this.CacheIndex.identity);
                        return;
                        //System.exit(0);
                    }

                    tempArray.Add(temp);
                    
                    this.SkillsMap.Add(this.Name + "_" + ui.ToString(CultureInfo.InvariantCulture), tempArray);
                    
                    if (this.CacheIndex.identity < 3000)
                    {
                        //String out = "\t(" + tempArray.get(0) + ") =>";
                        //for(int m = 1; m < tempArray.size(); m++)
                        //	out += "[" + tempArray.get(m)+ "]";
                        //System.out.println(out);
                        //System.out.println(outbound);
                    }
                }

                uint pwrLevelGrants = reader.ReadUInt32();
                Dictionary<string, List<uint>> pwrLevelGrantsMap = new Dictionary<string, List<uint>>();

                if (this.CacheIndex.identity < 3000)
                {
                    Console.WriteLine(this.Name + "(Skill Grants: " + pwrLevelGrants + ")");
                }

                for (int ig = 0; ig < pwrLevelGrants; ig++)
                {
                    List<uint> tempArray = new List<uint>();
                    tempArray.Add(reader.ReadUInt32()); //Skill ID
                    tempArray.Add(reader.ReadUInt32()); //Times this appears
                    for (int z = 0; z < tempArray[1]; z++)
                    {
                        tempArray.Add(reader.ReadUInt32());
                        tempArray.Add(reader.ReadUInt32());
                    }
                    string name = this.Name;
                    while (pwrLevelGrantsMap.ContainsKey(name))
                    {
                        name = name + "_1";
                    }
                    pwrLevelGrantsMap.Add(name, tempArray);

                    if (this.CacheIndex.identity < 3000)
                    {
                        //String out =
                        //"\t(Skill: " + tempArray[0] + ") =>";
                        //    //for(int m = 1; m < tempArray.size() - 1; m++)
                        //out +=
                        //"[Level Required: " + tempArray[1] + "]";
                        //out +=
                        //"[Bonus: " + tempArray[2] + "]";

                        if (tempArray.Count > 4)
                        {
                            return;
                            //System.exit(0);
                        }
                        //System.out.
                        //println(out );
                    }

                }

                reader.ReadUInt32(); //All 0s
                reader.ReadUInt32(); //All 0s
                reader.ReadUInt32(); //All 0s
                reader.ReadByte(); //All 0s
                reader.ReadByte(); //All 0s
                reader.ReadByte(); //All 0s

                this.RuneType = reader.ReadInt32(); // RuneType
                this.RuneCategory = reader.ReadUInt32(); // Rune Catagory

                reader.ReadByte(); //Some kind of boolean, not sure

                this.RuneCost = reader.ReadInt32(); // Rune Cost
                this.RuneStackRank = reader.ReadInt32(); // Rune Stack Rank
                this.TrainingPowerBonus = reader.ReadInt32(); // Training Power Bonus

                // 0, 10, 15 across the board. Bunch of 0s.
                // All Race and NPC runes are 10s. All Class runes are 15s.
                reader.ReadSingle();

                this.Gender = reader.ReadInt32(); //Gender. 1 for Male, 2 for Female.
                reader.ReadUInt32(); //All 0s

                // Unknown. Stat and Start runes are all 0. Race runes are 0. Class and Profession runes count upwards.
                reader.ReadUInt32();
                if (this.CacheIndex.identity < 3050)
                {
                    return;
                }

                if (this.CacheIndex.identity >= 15000)
                {
                } */

                /*

        //reader.ReadUInt32(); //HITPOINTS!
        //reader.ReadUInt32(); //MANA!
        //reader.ReadUInt32(); //STAMINA!
        try {
            ps.setFloat(6, reader.ReadSingle());
            ps.setFloat(7, reader.ReadSingle());
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        //reader.ReadSingle(); //Not sure - maybe aggro range? Draug is too big for that though.
        //reader.ReadSingle(); //Not sure - maybe aggro range? Draug is too big for that though.
        
        //reader.ReadUInt32(); //Attack Power?
        //reader.ReadUInt32(); //Defense Power?
        //reader.ReadUInt32(); //Level
        
        System.out.print("[Walk Speed=" + reader.ReadSingle() + "]"); //Walk Speed
        System.out.print("[Run Speed=" + reader.ReadSingle() + "]"); //Run Speed
        reader.ReadSingle(); //Combat Walk
        reader.ReadSingle(); //Combat Run
        reader.ReadSingle(); //Fly Walk
        reader.ReadSingle(); //Fly Run
        reader.ReadSingle(); //Swim Speed
        
        reader.ReadUInt32(); //Unknown - Some kind of Token
        reader.ReadByte();
        reader.ReadByte();
        
        int strSize = reader.ReadUInt32();
        byte[] flavorString = new byte[strSize];
        for(int i = 0; i < strSize; i++) {
            flavorString[i] = reader.ReadByte();
            reader.ReadByte(); //Discard due to Unicode
        }
        
        strSize = reader.ReadUInt32();
        byte[] descriptionString = new byte[strSize];
        for(int i = 0; i < strSize; i++) {
            descriptionString[i] = reader.ReadByte();
            reader.ReadByte(); //Discard due to Unicode
        }

        /*
        int t = reader.ReadUInt32();
        if(t != 0) {
            System.out.println("");
            System.out.print("(" + this.objID + ")" + new String(newString) + "=> (" + t + ") ");
            for(int i = 0; i < 16; i++)
                System.out.print("[" + reader.ReadByte() + "]");
        }
    
        if(this.objID == 14141)
            System.console();
        */

                // uint i = 0;

                /*
        int legitNameSize = reader.ReadUInt32();
        if(legitNameSize != n.length)
            System.out.println("BE SCURRED OF " + new String(n));
        
        byte[] finalName = new byte[legitNameSize];
        for(int i = 0; i < legitNameSize; i++) {
            finalName[i] = reader.ReadByte();
            reader.ReadByte(); //Discard due to Unicode
        }
        
        if(finalName.length != finalName.length)
            System.out.println(new String(finalName));
        
        System.out.println(new String(finalName));
        */

                //byte i = reader.ReadByte();
                //byte k = reader.ReadByte();
                //+ "(" + i + ")" + "(" + k + ")" 

                //System.out.println(counter2 + "/" + counter3);
            }
        }
    }
}