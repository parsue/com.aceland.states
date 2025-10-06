using System;

namespace AceLand.States.Exceptions
{
    public class StateNotFoundException : Exception
    {
        public StateNotFoundException(string name) : base(name) { }
    }
}