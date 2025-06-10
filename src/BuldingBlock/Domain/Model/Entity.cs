using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuldingBlock.Domain.Model
{
    public abstract class Auditable : IAuditable
    {
        public DateTime? CreatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public long? LastModifiedBy { get; set; }
    }

}