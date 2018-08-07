using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class CollectorBM
    {
        [Required]
        public int CollectorId { get; set; }

        [Required]
        public string CollectorName { get; set; }

        public int Ordinance { get; set; }

        public List<CollectorAssignmentBM> CollectorAssignments { get; set; }
    }

    public class CollectorAssignmentBM
    {
        public int? CollectorAssignmentId { get; set; }

        public string LetterName { get; set; }
    }

    public class UnAssignedCustomerPrefix
    {
        [Required]
        public int CollectorAssignmentId { get; set; }

        public string LetterName { get; set; }

        public int? CollectorId { get; set; }
    }
}
