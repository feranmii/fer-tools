using System;
using EventCallbacks;
using Fertools.StateMachine;
using UnityEngine;

namespace Fertools.StateMachine
{
    public class FStateMachine<T> : IStateMachine where T:  IComparable, IConvertible, IFormattable
    {
        public bool TriggerEvent { get; set; }

        public GameObject Target;
        public T CurrentState { get; protected set; }
        public T PreviousState { get; protected set; }

        public FStateMachine(GameObject target, bool triggerEvents)
        {
            Target = target;
            TriggerEvent = triggerEvents;
        }

        public virtual void ChangeState(T newState)
        {
            if (newState.Equals(CurrentState))
            {
                return;
            }

            PreviousState = CurrentState;
            CurrentState = newState;

            // if (TriggerEvent)
            // {
            //     StateChangeEvent ste = new StateChangeEvent(this);
            //     ste.FireEvent();
            // }
        }
    }
}

public interface IStateMachine
{ 
    bool TriggerEvent { get; set; }
}

public class StateChangeEvent<T> : Event<StateChangeEvent<T>> where T : Event<T>, IComparable, IConvertible, IFormattable
{
    public GameObject Target;
    public FStateMachine<T> TargetStateMachine;

    public T NewState;
    public T PreviousState;
    public StateChangeEvent(FStateMachine<T> targetStateMachine)
    {
        Target = targetStateMachine.Target;
        TargetStateMachine = targetStateMachine;
        NewState = targetStateMachine.CurrentState;
        PreviousState = targetStateMachine.PreviousState;
    }
    
}


