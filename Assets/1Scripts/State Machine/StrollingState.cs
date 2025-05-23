using UnityEngine;


namespace NPCNavigationProJect
{
    public class StrollingState : State
    {
        EnemyNPCStroll Stroll;
        EnemyNPC EnemyNPC{ get; set; }

        DestinationArea DestinationArea { get; set; }


        public StrollingState(GameObject go, StateMachine sm) : base(go, sm) { }


        public override void Enter() { 
        
            base.Enter();             
        
        }
        public override void Update() { 

            base.Update();

            EnemyNPC.Agent.isStopped = false;

            SelectRandomDestination();

        }

        public override void Exit() { base.Exit(); }

        void SelectRandomDestination()
        {
            EnemyNPC.Agent.SetDestination(DestinationArea.GetRandomDestinationPoint());
           
        }




    }
}

