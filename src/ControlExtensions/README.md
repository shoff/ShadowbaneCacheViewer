# Control Extensions
Some extensions to System.Windows.Forms to allow easier use with x-threading and async methods calls.

Example:

  this.SelectOneLabel.SetVisibility(true);

Regex to generate the methods:

public (.+\s)(.+[^\r\n])


public static void Set$2(this DataGridView control, $1 value) {if (control.InvokeRequired){control.BeginInvoke(new MethodInvoker(() => control.Set$2(value)));}else{control.$2 = value;control.Refresh();}}
