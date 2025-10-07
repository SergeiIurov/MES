using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.DB.Entities
{
    /// <summary>
    /// Производственный процесс
    /// </summary>
    public class ProductionProcess
    {
        public int Id { get; set; }
        
        //Номер линии
        public int LineNumber { get; set; }
        
        //Тип продукта
        public string ProductType { get; set; }
        
        //Комментарий
        public string Comment { get; set; }
        public DateTime Created { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool IsDeleted { get; set; }

        public virtual List<ProductionProcessLine> ProcessLines { get; set; } = new List<ProductionProcessLine>();
    }
}
