using System;
#if BACKEND
using System.ComponentModel.DataAnnotations;
#endif

namespace Qurvey.Shared.Models
{
    public class LogEntry
    {
#if BACKEND
        [Key]
#endif
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        public LogEntry()
        {

        }

        public LogEntry(string Text)
            : this()
        {
            this.Created = DateTime.Now;
            this.Text = Text;
        }
    }
}
