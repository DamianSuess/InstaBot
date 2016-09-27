using ServiceStack.DataAnnotations;

namespace InstaBot.Core.Domain
{
    public class EntityBase<TKey>
    {
        public EntityBase(TKey id)
        {
            Id = id;
        }

        [PrimaryKey]
        public TKey Id { get; set; }
    }
}