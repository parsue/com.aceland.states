using AceLand.Lifecycle;
using AceLand.States.Core;
using UnityEngine;

namespace AceLand.States
{
    [LifecycleModule(ModulePhase.Core, AllowParallel = true)]
    internal class StatesBootstrapper : ModuleBase
    {
        public override void Initialize()
        {
            StatesUtils.AnyState = AnyState.Builder().WithName("Any State").Build();
            StatesUtils.EntryState = AnyState.Builder().WithName("Entry State").Build();
            StatesUtils.ExitState = AnyState.Builder().WithName("Exit State").Build();

            var settings = StatesUtils.Settings;
            foreach (var provider in settings.PrewarmProviders)
                provider.PrewarmStateMachine();
            
            Debug.Log($"States Bootstrapper: {settings.PrewarmProviders.Length} Prewarm Machines");
            Debug.Log("States Bootstrapper Initialized");
        }
    }
}