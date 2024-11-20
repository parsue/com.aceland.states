using System.Collections.Generic;

namespace AceLand.States.Core
{
    internal sealed class SubStateMachines
    {
        internal SubStateMachines() { }
        internal SubStateMachines(List<IStateMachine> machines) => _machines.AddRange(machines);

        private readonly List<IStateMachine> _machines = new();
        private bool _isEntered;

        public void Start()
        {
            foreach (var machine in _machines)
                machine?.Start();
            
            _isEntered = true;
        }

        public void Update()
        {
            foreach (var machine in _machines)
                machine?.Update();
        }

        public void Stop()
        {
            foreach (var machine in _machines)
                machine?.Stop();
            
            _isEntered = false;
        }

        public void InjectSubMachine(IStateMachine machine)
        {
            _machines.Add(machine);
            if (StatesUtils.Settings.InvokeEnterOnLateInject && _isEntered)
                machine.Start();
        }

        public void RemoveSubMachine(IStateMachine machine)
        {
            if (!_machines.Contains(machine)) return;
            
            machine.Stop();
            _machines.Remove(machine);
        }
    }
}
