using System.Collections.Generic;

namespace AceLand.States.Core
{
    internal sealed class SubStateMachines
    {
        internal SubStateMachines() { }
        internal SubStateMachines(List<IStateMachine> machines) => _machines.AddRange(machines);

        private readonly List<IStateMachine> _machines = new();

        public void StartMachine()
        {
            foreach (var machine in _machines)
                machine?.StartMachine();
        }

        public void StopMachine()
        {
            foreach (var machine in _machines)
                machine?.StopMachine();
        }

        public void InjectSubMachine(IStateMachine machine)
        {
            _machines.Add(machine);
        }

        public void RemoveSubMachine(IStateMachine machine)
        {
            if (!_machines.Contains(machine)) return;
            _machines.Remove(machine);
        }
    }
}