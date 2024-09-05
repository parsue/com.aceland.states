using AceLand.States.Core;
using AceLand.States.ProjectSetting;
using AceLand.TasksUtils;
using AceLand.TasksUtils.Models;
using AceLand.TasksUtils.Promise;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AceLand.States.Handler
{
    public sealed class StateMachineGetter : PromiseHandler<StateMachineGetter, IStateMachine, ErrorMessage>
    {
        private static StatesSettings Settings => StatesHelper.Settings;

        internal StateMachineGetter(string id)
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
                IStateMachine stateMachine = default;
                while (Time.realtimeSinceStartup < targetTime)
                {
                    await UniTask.Yield(aliveToken);
                    if (aliveToken.IsCancellationRequested) return;
                    
                    arg = StateMachines.TryGetMachine(id, out stateMachine);
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
    
    public sealed class StateMachineSystemGetter : PromiseHandler<StateMachineSystemGetter, IStateMachine, ErrorMessage>
    {
        private static StatesSettings Settings => StatesHelper.Settings;

        internal StateMachineSystemGetter(string id)
        {
            GetStateMachine(id);
        }

        private void GetStateMachine(string id)
        {
            var targetTime = Time.realtimeSinceStartup + Settings.getterTimeout;
            var aliveToken = TaskHandler.ApplicationAliveToken;
            UniTask.Create(async () =>
            {
                var arg = 0;
                IStateMachine stateMachine = default;
                while (Time.realtimeSinceStartup < targetTime)
                {
                    await UniTask.Yield(aliveToken);
                    if (aliveToken.IsCancellationRequested) return;
                    
                    arg = StateMachines.TryGetMachineSystem(id, out stateMachine);
                    if (arg is 0 or 2) break;
                }

                switch (arg)
                {
                    case 0:
                        OnSuccess?.Invoke(stateMachine);
                        break;
                    case 1:
                        var msg = ErrorMessage.Builder()
                            .WithMessage("GetStateMachine", $"id [{id}] not found")
                            .Build();
                        OnError?.Invoke(msg);
                        break;
                    case 2:
                        msg = ErrorMessage.Builder()
                            .WithMessage("GetStateMachine", "incorrect Player Loop System type")
                            .Build();
                        OnError?.Invoke(msg);
                        break;
                }
                
                OnFinal?.Invoke();
            });
        }
    }
}