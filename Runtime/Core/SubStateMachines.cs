using System.Collections.Generic;

namespace AceLand.States.Core
{
    internal sealed class SubStateMachines
    {
        internal SubStateMachines() { }
        internal SubStateMachines(List<IStateMachine> machines) => _machines.AddRange(machines);

        private readonly List<IStateMachine> _machines = new();

        public void ToEntry()
        {
            foreach (var machine in _machines)
                machine?.ToEntryState();
        }

        public void ToExit()
        {
            foreach (var machine in _machines)
                machine?.ToExitState();
        }

        public void WithSubMachine(IStateMachine machine)
        {
            _machines.Add(machine);
        }

        public void WithOutSubMachine(IStateMachine machine)
        {
            if (!_machines.Contains(machine)) return;
            _machines.Remove(machine);
        }
    }
}