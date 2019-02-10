namespace CacheViewer.Code
{
    using System.Windows.Forms;
    using Controls;

    public static class PanelContainerExtensions
    {
        public static void InvokeObjectControl(this CObjectControl objectControl)
        {
        }

        public static void SetControlAdd(this PanelContainer panel, CObjectControl objectControl)
        {
            if (panel.InvokeRequired)
            {
                panel.BeginInvoke(new MethodInvoker(() => panel.SetControlAdd( objectControl)));
            }
            else
            {
                if (objectControl.InvokeRequired)
                {
                    objectControl.BeginInvoke(new MethodInvoker(objectControl.InvokeObjectControl));
                }

                panel.Controls.Add(objectControl);
                panel.Refresh();
            }
        }



        public static void SetAdd(this PanelContainer panel, Control control)
        {
            if (panel.InvokeRequired)
            {
                panel.BeginInvoke(new MethodInvoker(() => panel.SetAdd(control)));
            }
            else
            {
                panel.Add(control);
                panel.Refresh();
            }
        }

        public static void SetClear(this PanelContainer control)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(control.SetClear));
            }
            else
            {
                control.Controls.Clear();
                control.Refresh();
            }
        }
    }
}