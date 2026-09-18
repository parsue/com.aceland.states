using System;
using System.Threading.Tasks;
using AceLand.Lifecycle;
using AceLand.States.Core;
using AceLand.States.Exceptions;
using UnityEngine;

namespace AceLand.States
{
    public partial class StateMachine
    {
        public static Task<IStateMachine> GetAsync(string id) => GetStateMachineAsync(id);
        public static Task<IStateMachine> GetAsync<TEnum>(TEnum id) where TEnum : Enum => GetStateMachineAsync(id.ToString());

        public static IStateMachine Get(string id) =>
            StateMachines.TryGetMachine(id, out var stateMachine) ? stateMachine : null;
        public static IStateMachine Get<TEnum>(TEnum id) =>
            Get(id.ToString());

        private static async Task<IStateMachine> GetStateMachineAsync(string id)
        {
            var aliveToken = LifecycleToken.ApplicationAlive;
            var targetTime = Time.realtimeSinceStartup + Settings.GetterTimeout;

            while (!aliveToken.IsCancellationRequested && Time.realtimeSinceStartup < targetTime)
            {
                if (StateMachines.TryGetMachine(id, out var stateMachine))
                    return stateMachine;
                
                await Task.Yield();
            }

            throw new StateMachineNotFoundException($"State Machine [{id}] is not found");
        }
    }
}
