using AceLand.States.Core;
using UnityEngine;

namespace AceLand.States
{
    internal static class StatesBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialization()
        {
            StatesUtils.AnyState = AnyState.Builder().WithName("Any State").Build();
            StatesUtils.EntryState = AnyState.Builder().WithName("Entry State").Build();
            StatesUtils.ExitState = AnyState.Builder().WithName("Exit State").Build();
        }
    }
}