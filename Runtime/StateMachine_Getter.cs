using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AceLand.Library.Optional;
using AceLand.PlayerLoopHack;
using AceLand.States.Core;
using AceLand.TaskUtils;
using AceLand.TaskUtils.PromiseAwaiter;
using UnityEngine;

namespace AceLand.States
{
    public partial class StateMachine
    {
        public static Promise<IStateMachine> Get(string id) => GetStateMachine(id);
        public static Promise<IStateMachine> Get<T>(T id) where T : Enum => GetStateMachine(id.ToString());

        private static async Task<IStateMachine> GetStateMachine(string id)
        {
            var aliveToken = TaskHelper.ApplicationAliveToken;
            var targetTime = Time.realtimeSinceStartup + Settings.getterTimeout;

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
