namespace Shadowbane.Viewer.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Controls;
using static Shadowbane.Cache.Constants;

public class CacheObjectViewModel : ViewModelBase
{

    private ObservableCollection<TreeViewItem> simpleNodes = new();
    private ObservableCollection<TreeViewItem> structureNodes = new();
    private ObservableCollection<TreeViewItem> interactiveNodes = new();
    private ObservableCollection<TreeViewItem> equipmentNodes = new();
    private ObservableCollection<TreeViewItem> mobileNodes = new();
    private ObservableCollection<TreeViewItem> deedNodes = new();
    private ObservableCollection<TreeViewItem> warrantNodes = new();
    private ObservableCollection<TreeViewItem> unknownNodes = new();
    private ObservableCollection<TreeViewItem> particleNodes = new();
    private ObservableCollection<TreeViewItem> items = new();

    public CacheObjectViewModel()
    {
        this.items.Add(this.SimpleNode);
        this.items.Add(this.StructureNode);
        this.items.Add(this.InteractiveNode);
        this.items.Add(this.EquipmentNode);
        this.items.Add(this.MobileNode);
        this.items.Add(this.DeedNode);
        this.items.Add(this.UnknownNode);
        this.items.Add(this.WarrantNode);
        this.items.Add(this.ParticleNode);
    }

    public ObservableCollection<TreeViewItem> Items
    {
        get => this.items;
        set => SetProperty(ref this.items, value);
    }

    public TreeViewItem SimpleNode { get; } = new() { Header = SIMPLE };
    public TreeViewItem StructureNode { get; } = new() { Header = STRUCTURES };
    public TreeViewItem InteractiveNode { get; } = new() { Header = INTERACTIVE };
    public TreeViewItem EquipmentNode { get; } = new() { Header = EQUIPMENT };
    public TreeViewItem MobileNode { get; } = new() { Header = MOBILES };
    public TreeViewItem DeedNode { get; } = new() { Header = DEEDS };
    public TreeViewItem UnknownNode { get; } = new() { Header = UNKNOWN };
    public TreeViewItem WarrantNode { get; } = new() { Header = WARRANTS };
    public TreeViewItem ParticleNode { get; } = new() { Header = PARTICLES };
    
    public ObservableCollection<TreeViewItem> SimpleNodes
    {
        get => simpleNodes;
        set => SetProperty(ref simpleNodes, value);
    }
    public ObservableCollection<TreeViewItem> StructureNodes
    {
        get => structureNodes;
        set => SetProperty(ref structureNodes, value);
    }
    public ObservableCollection<TreeViewItem> InteractiveNodes
    {
        get => interactiveNodes;
        set => SetProperty(ref interactiveNodes, value);
    }
    public ObservableCollection<TreeViewItem> EquipmentNodes
    {
        get => equipmentNodes;
        set => SetProperty(ref equipmentNodes, value);
    }
    public ObservableCollection<TreeViewItem> MobileNodes
    {
        get => mobileNodes;
        set => SetProperty(ref mobileNodes, value);
    }
    public ObservableCollection<TreeViewItem> DeedNodes
    {
        get => deedNodes;
        set => SetProperty(ref deedNodes, value);
    }
    public ObservableCollection<TreeViewItem> WarrantNodes
    {
        get => warrantNodes;
        set => SetProperty(ref warrantNodes, value);
    }
    public ObservableCollection<TreeViewItem> UnknownNodes
    {
        get => unknownNodes;
        set => SetProperty(ref unknownNodes, value);
    }
    public ObservableCollection<TreeViewItem> ParticleNodes
    {
        get => particleNodes;
        set => SetProperty(ref particleNodes, value);
    }
}