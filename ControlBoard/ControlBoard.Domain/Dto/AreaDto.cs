using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.Domain.Dto
{
    public class AreaDto
    {
        public string Name { get; set; }
        public IEnumerable<StationDto> Stations { get; set; }
    }
}