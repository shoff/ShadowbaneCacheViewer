namespace Arcane.Cache.Json;

public enum Sparse : int
{
    SPARSE_VAL_LONG = 0,  // dword
    SPARSE_VAL_FLOAT = 1,  // float
    SPARSE_VAL_BOOL = 2,  // bool
    SPARSE_UID = 3, // dword
    SPARSE_REF_VECTOR3 = 4, // float (3)
    SPARSE_REF_ANIM_TYPE = 5,  // NONE (initialize)
    SPARSE_REF_ARC_STRING = 6,  // string
    SPARSE_REF_PROJECTILE_IMPACT_INFO = 7, // NONE (initialize)
    SPARSE_REF_PET_DATA = 8, // NONE (initialize)
    SPARSE_REF_MERCHANT_DATA = 9, // dword (3)
    SPARSE_REF_ARC_CACHE_ID = 10,  // qword
    SPARSE_PTR_ANIM_INFO = 11,  // NONE (initialize)
    SPARSE_PTR_CLIENT_ALLIANCE_MASTER = 12,  // NONE (initialize)
    SPARSE_PTR_ACTION_RESPONSE = 13, // dword ???
    SPARSE_OWNED_PTR_REF_LONG = 14,  // NONE (initialize)
    SPARSE_LINKED_PTR_ARC_SPELL_EFFECT = 15,  // NONE (initialize)
    SPARSE_LINKED_PTR_ARC_OBJECT = 16, // NONE (initialize)
    SPARSE_LINKED_PTR_ARC_CHARACTER = 17, // NONE (initialize)
    SPARSE_ENUM_ITEM_TYPE = 18  // dword
}