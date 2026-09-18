using System.Collections.Generic;
using AceLand.Lifecycle;
using AceLand.Optional;
using AceLand.States.Core;

namespace AceLand.States
{
    [QuitOrder(-100)]
    public sealed class StateMachineSystem : StateMachine
    {
        internal StateMachineSystem(Option<string> id, IState[] states, IAnyState firstState,
            List<StateTransition> anyTransitions, List<StateTransition> transitions,
            PlayerLoopPoint playerLoopPoint) :
            base(id, states, firstState, anyTransitions, transitions)
        {
            _playerLoopPoint = playerLoopPoint;
        }

        public override void Dispose()
        {
            base.Dispose();
            Stop();
        }

        private readonly PlayerLoopPoint _playerLoopPoint;
        private IFrameHandle _systemHandle;

        public override IStateMachine Start()
        {
            if (IsActive) return this;

            base.Start();
            
            _systemHandle = LifecycleFrame.RunEveryFrame(Update, _playerLoopPoint);
            
            return this;
        }

        public override void Update()
        {
            // Update is replaced by SystemUpdate()
        }

        public override void Stop()
        {
            if (Disposed || !IsActive) return;
            
            base.Stop();
            
            _systemHandle?.Dispose();
        }

        public void SystemUpdate()
        {
            if (Disposed || !IsActive) return;
            
            StateTransition();
            StateUpdate();
        }
    }
}