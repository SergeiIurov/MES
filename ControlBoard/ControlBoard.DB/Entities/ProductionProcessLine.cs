using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.DB.Entities
{
    /// <summary>
    /// Точка сканирования производственного процесса
    /// </summary>
    public class ProductionProcessLine
    {
        public int Id { get; set; }
        
        //Порядковый номер
        public string OrderNum { get; set; }
        
        //Пропуск точки сканирования
        public bool SkipTs { get; set; }
        
        //Многократное сканирование
        public bool MultiScanning { get; set; }
        
        //Проверка последовательности
        public bool CheckSequence { get; set; }

        public int ProductionProcessId { get; set; }
        
        public int ScanningPointId { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ProductionProcess ProductionProcess { get; set; }
        
        public virtual ScanningPoint ScanningPoint { get; set; }
    }
}
