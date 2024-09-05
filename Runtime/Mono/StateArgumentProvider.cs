using UnityEngine;

namespace AceLand.States.Mono
{
    [RequireComponent(typeof(MonoStateMachine))]
    public abstract class StateArgumentProvider : MonoBehaviour
    {
        // State Argument Methods
        // ---------------------------
        //      1. public static bool OnEventName() {}
        //      2. attach with State Machine
        //      3. no limited for other use
    }
}