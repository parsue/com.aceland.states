using AceLand.States.Core;
using AceLand.States.Mono;
using AceLand.States.ProjectSetting;
using AceLand.TasksUtils;
using AceLand.TasksUtils.Models;
using AceLand.TasksUtils.Promise;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AceLand.States.Handler
{
    public sealed class MonoStateMachineGetter : PromiseHandler<MonoStateMachineGetter, MonoStateMachine, ErrorMessage>
    {
        private static StatesSettings Settings => StatesHelper.Settings;

        internal MonoStateMachineGetter(string id)
        {
            GetStateMachine(id);
        }

        private void GetStateMachine(string id)
        {
            var targetTime = Time.realtimeSinceStartup + Settings.getterTimeout;
            var aliveToken = TaskHandler.ApplicationAliveToken;
            UniTask.Create(async () =>
            {
                var arg = false;
                MonoStateMachine stateMachine = default;
                while (Time.realtimeSinceStartup < targetTime)
                {
                    await UniTask.Yield(aliveToken);
                    if (aliveToken.IsCancellationRequested) return;
                    
                    arg = StateMachines.TryGetMonoMachine(id, out stateMachine);
                    
                    if (arg) break;
                }

                switch (arg)
                {
                    case true:
                        OnSuccess?.Invoke(stateMachine);
                        break;
                    case false:
                        var msg = ErrorMessage.Builder()
                            .WithMessage("GetStateMachine", $"id [{id}] not found")
                            .Build();
                        OnError?.Invoke(msg);
                        break;
                }
                
                OnFinal?.Invoke();
            });
        }
    }
}