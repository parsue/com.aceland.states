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
            StatesHelper.Settings = Resources.Load<StatesSettings>(nameof(StatesSettings));
            StatesHelper.AnyState = AnyState.Builder().WithName("Any State").Build();
            StatesHelper.EntryState = AnyState.Builder().WithName("Entry State").Build();
            StatesHelper.ExitState = AnyState.Builder().WithName("Exit State").Build();
        }
    }
}