namespace Shadowbane.CacheViewer.Code;

using System;
using System.ComponentModel;
using System.Windows.Forms;

public class InsideMover : Component
{
    private Container components;
    private int lastMouseX;
    private int lastMouseY;
    private bool moving;

    private Control parent;

    public InsideMover(IContainer container)
    {
        container.Add(this);
        this.InitializeComponent();
    }

    public InsideMover()
    {
        this.InitializeComponent();
    }

    public Control ControlToMove
    {
        set
        {
            if (this.parent != null)
            {
                this.parent.MouseDown -= this.ParentMouseDown;
                this.parent.MouseMove -= this.ParentMouseMove;
                this.parent.MouseUp -= this.ParentMouseUp;
                this.parent.DoubleClick -= this.ParentDoubleClick;
            }

            this.parent = value;
            if (value != null)
            {
                this.parent.MouseDown += this.ParentMouseDown;
                this.parent.MouseMove += this.ParentMouseMove;
                this.parent.MouseUp += this.ParentMouseUp;
                this.parent.DoubleClick += this.ParentDoubleClick;
            }
        }
        get => this.parent;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new Container();
    }
        
    public void StartMouseDown(MouseEventArgs e)
    {
        this.ParentMouseDown(null, e);
    }

    private void ParentMouseDown(object sender, MouseEventArgs e)
    {
        this.lastMouseX = e.X;
        this.lastMouseY = e.Y;
        this.moving = true;
    }

    private void ParentMouseMove(object sender, MouseEventArgs e)
    {
        if (this.moving)
        {
            var newLocation = this.parent.Location;

            if (!this.parent.Parent.Bounds.IntersectsWith(this.parent.Bounds))
            {
                newLocation.X += e.X - this.lastMouseX;
                newLocation.Y += e.Y - this.lastMouseY;
            }

            this.parent.Location = newLocation;
        }
    }

    private void ParentMouseUp(object sender, MouseEventArgs e)
    {
        this.moving = false;
    }

    private void ParentDoubleClick(object sender, EventArgs e)
    {
        if (this.parent is Form f)
        {
            f.WindowState = f.WindowState == FormWindowState.Normal ? FormWindowState.Maximized
                : FormWindowState.Normal;
        }
    }
}