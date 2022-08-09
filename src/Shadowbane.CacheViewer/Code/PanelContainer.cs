namespace Shadowbane.CacheViewer.Code;

using System.ComponentModel;
using System.Windows.Forms;

public class PanelContainer : Panel, IContainer
{
    private ComponentCollection components;

    public void Add(IComponent component)
    {
        if (this.components == null)
        {
            this.components = new ComponentCollection(new[] {component});
        }
        else
        {
            var array = new IComponent[this.components.Count+1];
            this.components.CopyTo(array, 0);
            array[array.Length - 1] = component;
            this.components = new ComponentCollection(array);
        }
    }

    public void Add(IComponent component, string name)
    {
        this.Add(component);
    }

    public void Remove(IComponent component)
    {
        var array = new IComponent[this.components.Count];
        this.components.CopyTo(array, 0);
        var array1 = new IComponent[this.components.Count - 1];
        for (int i = 0; i < array.Length; i++)
        {
            if (!array[i].Equals(component))
            {
                array1[i] = array[i];
            }
        }

        this.components = new ComponentCollection(array1);
    }

    public void RemoveAll()
    {
        this.components = new ComponentCollection(new IComponent[0]);
    }

    public ComponentCollection Components
    {
        get { return this.components ?? new ComponentCollection(new IComponent[0]); }
    }
}