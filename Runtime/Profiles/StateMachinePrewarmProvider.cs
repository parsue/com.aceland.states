using UnityEngine;

namespace AceLand.States.Profiles
{
    public abstract class StateMachinePrewarmProvider : ScriptableObject
    {
        public abstract void PrewarmStateMachine();
        public abstract void Dispose();
    }
}