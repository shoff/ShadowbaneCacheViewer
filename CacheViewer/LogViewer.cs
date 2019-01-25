

namespace CacheViewer
{
    using System.ComponentModel;
    using System.Windows.Forms;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using CacheViewer.Data;
    using CacheViewer.Data.Entities;
    using LinqKit;

    public partial class LogViewer : Form
    {
        private readonly SbCacheViewerContext sbCacheViewerContext;
        private readonly EntityRepository<Log, int> logRepository;

        public LogViewer()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.sbCacheViewerContext = new SbCacheViewerContext();
                this.sbCacheViewerContext.Configuration.AutoDetectChangesEnabled = false;
                this.sbCacheViewerContext.ValidateOnSave = false;
                this.logRepository = new EntityRepository<Log, int>(this.sbCacheViewerContext);
            }

            InitializeComponent();
        }

        private async void GetLogsButtonClick(object sender, EventArgs e)
        {
            var predicate = PredicateBuilder.False<Log>();

            if (this.DebugCheckBox.Checked)
            {
                predicate = predicate.Or(p => p.LogLevel == "Debug");
            }
            if (this.InfoCheckBox.Checked)
            {
                predicate = predicate.Or(x => x.LogLevel == "Info");
            }
            if (this.WarnCheckBox.Checked)
            {
                predicate = predicate.Or(x => x.LogLevel == "Warn");
            }
            if (this.ErrorCheckBox.Checked)
            {
                predicate = predicate.Or(x => x.LogLevel == "Error");
            }
            if (this.FatalCheckBox.Checked)
            {
                predicate = predicate.Or(x => x.LogLevel == "Fatal");
            }
            var logs = await this.Get(predicate);

            this.PopulateGrid(logs);
        }

        private void PopulateGrid(IEnumerable<Log> logs)
        {
            this.LogsGridView.Rows.Clear();
            var logList = logs.ToList().OrderByDescending(x => x.DateCreated);

            //await Task.Run(() =>
            //{
            foreach (var log in logList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(this.LogsGridView);
                row.Cells[0].Value = log.LogId;
                row.Cells[1].Value = log.DateCreated;
                row.Cells[2].Value = log.LogLevel;
                row.Cells[3].Value = log.Message;
                this.LogsGridView.Rows.Add(row);
            }
            // });
        }
        private void DeleteLogsButtonClick(object sender, EventArgs e)
        {
            this.logRepository.ExecuteInDatabaseByQuery(@"Delete from [LogTable]");
            this.logRepository.ExecuteInDatabaseByQuery(@"DBCC CHECKIDENT ('[LogTable]', RESEED, 0)");
            this.LogsGridView.Rows.Clear();
            this.LogsGridView.Refresh();
        }

        private async Task<IEnumerable<Log>> Get(Expression<Func<Log, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                IQueryable<Log> query = this.sbCacheViewerContext.SetEntity<Log>();

                if (predicate != null)
                {
                    query = query.AsExpandable().Where(predicate);
                }

                return query.ToList();
            });
        }


    }
}
