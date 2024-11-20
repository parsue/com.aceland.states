using System;
using System.Threading.Tasks;
using AceLand.States.Core;
using AceLand.TaskUtils;
using UnityEngine;

namespace AceLand.States
{
    public partial class StateMachine
    {
        public static Task<IStateMachine> GetAsync(string id) => GetStateMachine(id);
        public static Task<IStateMachine> GetAsync<TEnum>(TEnum id) where TEnum : Enum => GetStateMachine(id.ToString());

        public static IStateMachine Get(string id) =>
            StateMachines.TryGetMachine(id, out var stateMachine) ? stateMachine : null;
        public static IStateMachine Get<TEnum>(TEnum id) =>
            Get(id.ToString());

        private static async Task<IStateMachine> GetStateMachine(string id)
        {
            var aliveToken = Promise.ApplicationAliveToken;
            var targetTime = Time.realtimeSinceStartup + Settings.GetterTimeout;

            while (!aliveToken.IsCancellationRequested && Time.realtimeSinceStartup < targetTime)
            {
                await Task.Yield();
                
                var arg = StateMachines.TryGetMachine(id, out var stateMachine);
                
                if (arg) return stateMachine;
            }

            throw new Exception($"State Machine [{id}] is not found");
        }
    }
}
