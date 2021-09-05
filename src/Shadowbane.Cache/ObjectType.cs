namespace Shadowbane.Cache
{
    public enum ObjectType
    {
        // Flag Info:
        // 0x01 = Sun
        // 0x03 = Basic objects,  Pillars, rocks, trees, monuments, static structures
        // 0x4 & 0x5 = Buildings and Interactive objects - don't know exact difference between 4 and 5.
        // 0x9 = Items, all weapons, armours, equipment, etc.
        // 0xD = All Runes / Creatures / NPCs / Characters
        // 0xF = All Deeds
        // 0x10 = Keys, Warrants, etc
        // 0x13 = Particles, Environment, Interface

        Sun = 1,
        Simple = 3,
        Structure = 4,
        Interactive = 5,
        Equipment = 9,

        Mobile = 13,
        Deed = 15,
        Warrant = 16,
        Unknown = 17,
        Particle = 19
    }
}