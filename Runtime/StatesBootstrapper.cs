using AceLand.States.Core;
using AceLand.States.ProjectSetting;
using UnityEngine;

namespace AceLand.States
{
    internal static class StatesBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialization()
        {
            StatesUtils.Settings = Resources.Load<StatesSettings>(nameof(StatesSettings));
            StatesUtils.AnyState = AnyState.Builder().WithName("Any State").Build();
            StatesUtils.EntryState = AnyState.Builder().WithName("Entry State").Build();
            StatesUtils.ExitState = AnyState.Builder().WithName("Exit State").Build();
        }
    }
}