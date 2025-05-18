using UnityEngine;


namespace NPCNavigationProJect
{
    public class StateMachine
    {
        private State currentState;


        public State CurrentState
        {
            get => currentState;
            set
            {
                if (currentState != null) { currentState.Exit(); }

                currentState = value;

                if (currentState != null) { currentState.Enter(); }

            }

        }



    }
}

