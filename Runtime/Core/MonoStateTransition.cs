using System;
using AceLand.States.Mono;
using UnityEngine;

namespace AceLand.States.Core
{
    [Serializable]
    internal class MonoStateTransition
    {
        [HideInInspector] public string name; 
        [SerializeField] private bool isAny;
        [SerializeField] private MonoStateBase from;
        [SerializeField] private MonoStateBase to;
        [SerializeField] private string funcName;
        [SerializeField] private bool preventToSelf = true;
        
        public bool IsAny => isAny;
        public IAnyState From => isAny ? StatesHelper.AnyState : from;
        public IAnyState To => to;
        public string FuncName => funcName;
        public bool PreventToSelf => preventToSelf;
        
        private bool GetArgument(bool value) => value;
    }
}