
namespace BuldingBlock.Domain.Model
{
    public interface IAuditable
    {
        public DateTime? CreatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public long? LastModifiedBy { get; set; }
    }
}