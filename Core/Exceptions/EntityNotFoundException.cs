using System;
namespace Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string EntityType { get; set; }
        public object EntityId { get; set; }

        public EntityNotFoundException(string entityType, object entityId, string message) : base(message)
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }
}
