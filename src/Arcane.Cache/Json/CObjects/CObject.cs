﻿namespace Arcane.Cache.Json.CObjects;

public class CObject
{
    public string obj_name { get; set; } = string.Empty;
    public bool obj_pickable { get; set; }
    public float obj_gravity { get; set; }
    public float obj_cull_distance { get; set; }
    public float[] obj_scale { get; set; } = Array.Empty<float>();
    public int obj_render_object { get; set; }
    public bool obj_double_fusion { get; set; }
    public float[] obj_forward_vector { get; set; } = Array.Empty<float>();
    public string obj_tracking_name { get; set; } = string.Empty;
    public float obj_max_tracking_distance { get; set; }
    public int obj_icon { get; set; }
    public float obj_gravity_f { get; set; }
    public object[] obj_sound_events { get; set; } = Array.Empty<object>();
    public object[] obj_arc_hardpoint_list { get; set; } = Array.Empty<object>();
    public ObjSparseData? obj_sparse_data { get; set; }
    public int obj_render_object_low_detail { get; set; }
    public float[] obj_default_alignment { get; set; } = Array.Empty<float>();
    public int obj_sound_table { get; set; }
}
