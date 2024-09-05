using System;
using AceLand.Library.Attribute;
using AceLand.Library.Extensions;
using AceLand.States.Core;
using UnityEngine;

namespace AceLand.States.Mono
{
    public abstract class LateStateAction : MonoBehaviour
    {
        [Serializable] public enum SearchMode { ById, ByRef, }

        [Header("Ref Settings")]
        [SerializeField] private SearchMode searchMode;
        
        [ConditionalShow("searchMode", SearchMode.ById)]
        [SerializeField] private string machineId;
        [ConditionalShow("searchMode", SearchMode.ById)]
        [SerializeField] private string stateName;
        [ConditionalShow("searchMode", SearchMode.ByRef)]
        [SerializeField] private MonoStateBase state;

        public void Awake()
        {
            if (searchMode is SearchMode.ById) SearchById();
            else AddStateActionToState();
        }

        private void SearchById()
        {
            if (machineId.IsNullOrEmptyOrWhiteSpace() || stateName.IsNullOrEmptyOrWhiteSpace())
            {
                Debug.LogError("State Machine ID or State Name cannot be empty", this);
                throw new NullReferenceException("State Machine ID or State Name is null");
            }

            StateMachine.GetMono(machineId)
                .Then(machine =>
                {
                    machine.GetState(stateName)
                        .WithActions(StateEnter, StateUpdate, StateExit);
                });
        }

        private void AddStateActionToState()
        {
            if (state == null)
            {
                Debug.LogError("State Machine or State cannot be empty", this);
                throw new NullReferenceException("MonoStateMachine or MonoStateBase is null");
            }
            
            if (state is IIdleState idle)
                idle.WithActions(StateEnter, StateExit);
            if (state is IState monoState)
                monoState.WithActions(StateEnter, StateUpdate, StateExit);
        }

        protected virtual void StateEnter()
        {
            // noop
        }

        protected virtual void StateUpdate()
        {
            // noop
        }

        protected virtual void StateExit()
        {
            // noop
        }
    }
}