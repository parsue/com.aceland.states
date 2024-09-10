using System.Collections.Generic;
using AceLand.Library.Optional;
using AceLand.PlayerLoopHack;
using AceLand.States.Core;
using AceLand.TaskUtils;
using UnityEngine.LowLevel;

namespace AceLand.States
{
    public sealed class StateMachineSystem : StateMachine, IPlayerLoopSystem
    {
        internal StateMachineSystem(Option<string> id, IState[] states, IAnyState entryState,
            List<StateTransition> anyTransitions, List<StateTransition> transitions,
            PlayerLoopType playerLoopType, int index) :
            base(id, states, entryState, anyTransitions, transitions)
        {
            _playerLoopType = playerLoopType;
            _index = index;
            _playerLoopSystem = this.CreatePlayerLoopSystem();
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            StopMachine();
        }

        private readonly PlayerLoopSystem _playerLoopSystem;
        private readonly PlayerLoopType _playerLoopType;
        private readonly int _index;

        public override IStateMachine StartMachine()
        {
            if (IsActive) return this;

            base.StartMachine();
            _playerLoopSystem.InsertSystem(_playerLoopType, _index);
            TaskHandler.AddApplicationQuitListener(StopMachine);
            return this;
        }

        public override void StopMachine()
        {
            if (!IsActive) return;
            
            base.StopMachine();
            _playerLoopSystem.RemoveSystem(_playerLoopType);
            TaskHandler.RemoveApplicationQuitListener(StopMachine);
        }

        public void SystemUpdate()
        {
            Update();
        }
    }
}