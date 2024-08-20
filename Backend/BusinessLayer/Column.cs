using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Column
    {
        public Dictionary<int,Task> Tasks { get; set; }
        public string ColumnName { get; set; }
        public int Limit { get; set; }

        public Column(string name, int limit) 
        {
            this.Limit = limit;
            this.ColumnName = name;
            this.Tasks = new Dictionary<int,Task>();
        }
    }
}
