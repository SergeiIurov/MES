using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoard.Domain.Dto
{
    public class SpecificationDto
    {
        public int Id { get; set; }
        public string SequenceNumber { get; set; }
        public string? SpecificationStr { get; set; }
    }
}
