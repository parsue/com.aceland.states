using System.Collections.Generic;
using AceLand.Library.Optional;
using AceLand.PlayerLoopHack;
using AceLand.States.Core;
using AceLand.TasksUtils;
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

        protected override void BeforeDispose()
        {
            StopEngine();
            base.BeforeDispose();
        }

        private readonly PlayerLoopSystem _playerLoopSystem;
        private readonly PlayerLoopType _playerLoopType;
        private readonly int _index;

        public override IStateMachine StartEngine()
        {
            if (IsActive) return this;

            base.StartEngine();
            _playerLoopSystem.InsertSystem(_playerLoopType, _index);
            TaskHandler.AddApplicationQuitListener(StopEngine);
            return this;
        }

        public override void StopEngine()
        {
            if (!IsActive) return;
            
            base.StopEngine();
            _playerLoopSystem.RemoveSystem(_playerLoopType);
            TaskHandler.RemoveApplicationQuitListener(StopEngine);
        }

        public void SystemUpdate()
        {
            Update();
        }
    }
}