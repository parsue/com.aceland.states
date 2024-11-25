using System;

namespace AceLand.States.Core
{
    public abstract partial class StateMachineBase
    {
        public bool Equals(IAnyStateMachine x, IAnyStateMachine y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }
        public bool Equals(IAnyStateMachine other)
        {
            if (other == null) return false;
            return Id == other.Id;
        }

        // Override Object.Equals
        public override bool Equals(object obj)
        {
            if (obj is IAnyStateMachine other)
                return Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // IComparable<MyClass>
        public int CompareTo(IAnyStateMachine other)
        {
            if (other == null) return 1;
            return string.Compare(Id, other.Id); 
        }

        // IComparable
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is IAnyStateMachine other)
                return CompareTo(other);

            throw new ArgumentException("Object is not a State Machine");
        }
    }
}
