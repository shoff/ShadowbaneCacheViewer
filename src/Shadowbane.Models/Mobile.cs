// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Shadowbane.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Cache;
using Geometry;

public record Mobile : AnimationObject
{
    public Mobile(uint identity, string name, uint offset, ReadOnlyMemory<byte> data,
        uint innerOffset)
        : base(identity, ObjectType.Mobile, name, offset, data, innerOffset)
    {
    }

    public uint IsPetOrRune { get; set; }
    public decimal ObjId { get; set; }
    public uint SecondNameSize { get; set; }
    public string? AiDescription { get; set; }
    public uint MobToken { get; set; }
    public float ZOffset { get; set; }
    public Dictionary<string,List<uint>> SkillsMap { get; set; } = new();
    public List<uint> StatArray { get; } = new();
    public int Gender { get; set; }
    public int TrainingPowerBonus { get; set; }
    public int RuneType { get; set; }
    public uint RuneCategory { get; set; }
    public int RuneStackRank { get; set; }
    public int RuneCost { get; set; }
    public int NumberOfSkillsRequired { get; set; }
    public int LevelRequired { get; set; }
    public int PowerId { get; set; }
    public string? ExitMessage { get; set; }
    public int NameSize { get; set; }
    public int PetNameCount { get; set; }
    public byte ProhibitsRaceToggle { get; set; }
    public uint SomeKindOfTypeHash { get; set; }
    public int RequiredGender { get; set; }
    public int MinRequiredLevel { get; set; }
    public byte ProhibitsDiscToggle { get; set; }
    public byte ProhibitsClassToggle { get; set; }
    public int[] FourIntArray { get; set; } = new int[4];
    public int FourThousandInt { get; set; }
    public int SomethingWithPets { get; set; }
    public DateTime WolfpackCreateDate { get; set; }

    // ReSharper disable once CognitiveComplexity
    public override ICacheObject Parse()
    {
        this.ObjId = this.Identity; // huh?

        this.FourIntArray = new int[4];
        using var reader = this.Data.CreateBinaryReaderUtf32();
        // TNLC
        reader.ReadInt32();
        reader.ReadInt32();

        // flag
        this.NameSize = reader.ReadInt32();
        reader.BaseStream.Position += this.NameSize * 2;

        // reader.BaseStream.Position = this.InnerOffset;
        _ = reader.ReadByte();

        //This block seems to be static for all Type 13 Objects
        // this should always be 00 00 80 3F
        _ = reader.ReadSingle();

        var time = reader.ReadUInt32();
        this.WolfpackCreateDate = DateTime.UnixEpoch.AddSeconds(time);

        _ = reader.ToVector3();
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
        _ = reader.ReadByte(); //Data block 00
        this.RuneCategory = reader.ReadUInt32(); // Rune Icon
        this.ZOffset = reader.ReadSingle(); // Z offset. Undead = 2.0, Bats = 4.0
        this.IsPetOrRune = reader.ReadUInt32(); // 4 for summoned pets? 2 for some runes.
        var sanity = reader.ReadUInt32();
        Debug.Assert(sanity <= 0);

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
            this.AiDescription = reader.AsciiString(strSize);

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

        _ = reader.ReadSingle();
        _ = reader.ReadUInt32(); // All runes have this, set to 1.0.
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
            _ = reader.AsciiString(petTextSize);
            _ = reader.ReadUInt16();
            for (var pj = 0; pj < 33; pj++)
            {
                reader.ReadUInt32();
            }
        }

        this.SecondNameSize = reader.ReadUInt32();
        if (this.SecondNameSize == this.NameSize)
        {
            _ = reader.AsciiString(this.SecondNameSize);
        }

        //StructureValidationResult validationResult = null;
        MobileRenderFinder? finder = null;

        while (reader.CanRead(20) && this.RenderIds.Count == 0)
        {
            finder = this.GetFinder(reader);

            if (finder.IsValid)
            {
                this.RenderIds.Add(finder.RenderId);
                reader.BaseStream.Position -= 28;
                this.RenderCount = reader.ReadUInt32();
                reader.BaseStream.Position += 24;
            }
            else
            {
                reader.BaseStream.Position = (finder.InitialOffset + 1);
            }
        }

        while (reader.CanRead(20) && finder?.NullTerminator == 0)
        {
            finder = GetFinder(reader);
            if (finder.IsValid)
            {
                this.RenderIds.Add(finder.RenderId);
            }
        }

        // see  if there is a counter to another group of ids
        if (finder?.NullTerminator > 0)
        {
            for (int i = 0; i < finder.NullTerminator; i++)
            {
                if (reader.CanRead(8))
                {
                    reader.ReadInt32();
                    var id = reader.ReadUInt32();
                    this.RenderIds.Add(id);
                }
            }
        }

        // another counter?
        int renderCounter = 0;
        if (reader.CanRead(4))
        {
            renderCounter = reader.ReadInt32();
        }

        for (uint i = 0; i < renderCounter; i++)
        {
            if (reader.CanRead(8))
            {
                reader.ReadInt32();
                var id = reader.ReadUInt32();
                this.RenderIds.Add(id);
            }
        }

        #region old shit
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
        #endregion
        return this;
    }
        
    public MobileRenderFinder? GetFinder(BinaryReader reader)
    {
        var finder = new MobileRenderFinder
        {
            InitialOffset = reader.BaseStream.Position,
            RenderId = reader.ReadUInt32(),
            Identity = reader.ToVector3(),
            NullTerminator = reader.ReadUInt32(),
            IsValid = true
        };

        // TODO verify that these should be the same for all mobs
        //if (ArchiveLoader.RenderArchive[finder.RenderId] == null)
        //{
        //    finder.IsValid = false;
        //    return finder;
        //}

        if (finder.Identity != MobileRenderFinder.V1)
        {
            finder.IsValid = false;
            return finder;
        }

        return finder;
    }

    //public void ParseAndAssemble(ReadOnlyMemory<byte> assetItem1)
    //{
    //    this.Parse();
    //    foreach (var render in this.RenderIds)
    //    {
    //        // TODO this doesn't handle duplicate ids
    //        var cacheIndex = ArchiveLoader.RenderArchive[render];
    //        var renderInformation = RenderableObjectBuilder.Create(cacheIndex.CacheIndex);
    //        this.Renders.Add(renderInformation);
    //    }
    //}
}

public class MobileRenderFinder
{
    public static readonly Vector3 V1 = new(1, 1, 1);
    // size of 20

    // pattern renderId float 1, float 1, float 1 null
    public long InitialOffset { get; set; }
    public uint RenderId { get; set; }  // 4
    public Vector3 Identity { get; set; } // 12
    public uint NullTerminator { get; set; } // 4
    public bool IsValid { get; set; }
}