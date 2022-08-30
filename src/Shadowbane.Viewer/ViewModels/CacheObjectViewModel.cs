namespace Shadowbane.Viewer.ViewModels;

using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using static Shadowbane.Cache.Constants;

public class CacheObjectViewModel : ViewModelBase
{
    private static readonly SolidColorBrush itemBrush = new(Color.FromRgb(240, 240, 240));
    private static readonly double mainItemFontSize = 18;
    public CacheObjectViewModel()
    {
        this.Items.Add(this.SimpleNode);
        this.Items.Add(this.StructureNode);
        this.Items.Add(this.InteractiveNode);
        this.Items.Add(this.EquipmentNode);
        this.Items.Add(this.MobileNode);
        this.Items.Add(this.DeedNode);
        this.Items.Add(this.UnknownNode);
        this.Items.Add(this.WarrantNode);
        this.Items.Add(this.ParticleNode);
    }

    public List<TreeViewItem> Items { get; set; } = new();

    public TreeViewItem SimpleNode { get; } = new() { Header = SIMPLE, Foreground = itemBrush, FontSize = mainItemFontSize };
    public TreeViewItem StructureNode { get; } = new() { Header = STRUCTURES, Foreground = itemBrush, FontSize = mainItemFontSize };
    public TreeViewItem InteractiveNode { get; } = new() { Header = INTERACTIVE, Foreground = itemBrush, FontSize = mainItemFontSize };
    public TreeViewItem EquipmentNode { get; } = new() { Header = EQUIPMENT, Foreground = itemBrush, FontSize = mainItemFontSize };
    public TreeViewItem MobileNode { get; } = new() { Header = MOBILES, Foreground = itemBrush, FontSize = mainItemFontSize };
    public TreeViewItem DeedNode { get; } = new() { Header = DEEDS, Foreground = itemBrush, FontSize = mainItemFontSize };
    public TreeViewItem UnknownNode { get; } = new() { Header = UNKNOWN, Foreground = itemBrush, FontSize = mainItemFontSize };
    public TreeViewItem WarrantNode { get; } = new() { Header = WARRANTS, Foreground = itemBrush, FontSize = mainItemFontSize };
    public TreeViewItem ParticleNode { get; } = new() { Header = PARTICLES, Foreground = itemBrush, FontSize = mainItemFontSize };

    public List<TreeViewItem> SimpleNodes { get; set; } = new();
    public List<TreeViewItem> StructureNodes { get; set; } = new();
    public List<TreeViewItem> InteractiveNodes { get; set; } = new();
    public List<TreeViewItem> EquipmentNodes { get; set; } = new();
    public List<TreeViewItem> MobileNodes { get; set; } = new();
    public List<TreeViewItem> DeedNodes { get; set; } = new();
    public List<TreeViewItem> WarrantNodes { get; set; } = new();
    public List<TreeViewItem> UnknownNodes { get; set; } = new();
    public List<TreeViewItem> ParticleNodes { get; set; } = new();
}