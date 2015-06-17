namespace CacheViewer.Domain.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LogTable")]
    public class Log
    {
        [Key]
        public int LogId { get; set; }

        // [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateCreated { get; set; }

        [MaxLength(10)]
        public string LogLevel { get; set; }

        [MaxLength(128)]
        public string Logger { get; set; }

        [MaxLength(8000)]
        public string Message { get; set; }

        public string MessageId { get; set; }

        [MaxLength(256)]
        public string WindowsUserName { get; set; }

        [MaxLength(256)]
        public string CallSite { get; set; }

        [MaxLength(128)]
        public string ThreadId { get; set; }

        public string Exception { get; set; }
        public string StackTrace { get; set; }
    }
}