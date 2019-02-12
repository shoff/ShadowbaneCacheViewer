namespace CacheViewer
{
    using System;
    using System.Data.Entity;
    using System.Windows.Forms;
    using Data;

    /// <summary>
    /// </summary>
    internal static class Program
    {
        // TODO look into http://arraysegments.codeplex.com/

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SbCacheViewerContext>());
            Application.Run(new SBCacheObjectForm());
        }
    }
}