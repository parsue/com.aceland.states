using System;

namespace AceLand.States.Exceptions
{
    public class StateMachineNotFoundException : Exception
    {
        public StateMachineNotFoundException(string message) : base(message) { }
    }
}