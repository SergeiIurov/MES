using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.DB.Entities
{
    public class HistoryInfo
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string JsonInfo { get; set; }
        public bool IsDeleted { get; set; }
    }
}
