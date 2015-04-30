using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qurvey.Backend
{
    class SurveyContext : DbContext
    {
        public SurveyContext() : base("QurveyContext") { }
        public DbSet<Survey> Surveys { get; set; }
    }
}
