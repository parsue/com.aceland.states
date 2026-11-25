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
#if UNITY_EDITOR
            // No-domain-reload play mode keeps the static machine registry alive between play
            // sessions. Clear it on (re)initialization so no stale machine leaks across sessions.
            // Editor-only; player builds get a fresh static state per process launch.
            StateMachines.ClearAll();
#endif
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