

using UnityEngine;

namespace NPCNavigationProJect
{
    public abstract class State
    {


        protected GameObject go;

        protected StateMachine sm;
       

        public State (GameObject gameObject, StateMachine stateMachine)
        {
            go = gameObject;
            sm = stateMachine;
        }

        public virtual void Enter() { }

        public virtual void Update() { }

        public virtual void Exit() { }

    }

}


