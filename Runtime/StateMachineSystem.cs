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
        internal StateMachineSystem(Option<string> id, IState[] states, IAnyState firstState,
            List<StateTransition> anyTransitions, List<StateTransition> transitions,
            PlayerLoopState playerLoopState, int index) :
            base(id, states, firstState, anyTransitions, transitions)
        {
            _playerLoopState = playerLoopState;
            _index = index;
            _playerLoopSystem = this.CreatePlayerLoopSystem();
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            Stop();
        }

        private readonly PlayerLoopSystem _playerLoopSystem;
        private readonly PlayerLoopState _playerLoopState;
        private readonly int _index;

        public override IStateMachine Start()
        {
            if (IsActive) return this;

            base.Start();
            _playerLoopSystem.InjectSystem(_playerLoopState, _index);
            Promise.AddApplicationQuitListener(Stop);
            return this;
        }

        public override void Update()
        {
            // Update is replaced by SystemUpdate()
        }

        public override void Stop()
        {
            if (!IsActive) return;
            
            base.Stop();
            _playerLoopSystem.RemoveSystem(_playerLoopState);
            Promise.RemoveApplicationQuitListener(Stop);
        }

        public void SystemUpdate()
        {
            StateTransition();
            StateUpdate();
        }
    }
}