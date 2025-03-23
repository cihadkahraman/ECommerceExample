using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Domain.Common
{
    public abstract class Entity
    {
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public int Id { get; protected set; }

        public bool IsTransient() => Id == default;

        public override bool Equals(object? obj)
        {
            if (obj is null || GetType() != obj.GetType())
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            Entity other = (Entity)obj;

            if (IsTransient() || other.IsTransient())
                return false;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetType(), Id);
        }

        public static bool operator ==(Entity? left, Entity? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;

            return left.Equals(right);
        }

        public static bool operator !=(Entity? left, Entity? right) => !(left == right);
    }
}
