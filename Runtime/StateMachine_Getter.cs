using System;
using System.Threading.Tasks;
using AceLand.States.Core;
using AceLand.TaskUtils;
using UnityEngine;

namespace AceLand.States
{
    public partial class StateMachine
    {
        public static Task<IStateMachine> Get(string id) => GetStateMachine(id);
        public static Task<IStateMachine> Get<T>(T id) where T : Enum => GetStateMachine(id.ToString());

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
