using AceLand.Library.Extensions;
using AceLand.States.Core;
using UnityEngine;

namespace AceLand.States.Mono
{
    public abstract class MonoStateBase : MonoBehaviour, IAnyState
    {
        [SerializeField] private string stateName;
        
        public string Name => stateName.IsNullOrEmptyOrWhiteSpace()
            ? GetType().Name
            : stateName;
        
        private protected readonly StateUpdater Updater = new();
        private protected readonly SubStateMachines SubStateMachines = new();

        public void StateEnter()
        {
            // noop
        }

        public void StateUpdate()
        {
            // noop
        }

        public void StateExit()
        {
            // noop
        }
    }
}