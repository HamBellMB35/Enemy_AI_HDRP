using NPCNavigationProJect;
using UnityEngine;
using UnityEngine.AI;


namespace NPCNavigationProJect
{
    public class EnemyNPCStroll : EnemyNPCComponent
    {
        public DestinationArea DestinationArea;

        private void Start()
        {
            SelectRandomDestination();
        }


        private void Update()
        {
            if (HasArrived())
            {
                SelectRandomDestination();
            }
        }

        void SelectRandomDestination()
        {
            enemyNPC.Agent.SetDestination(DestinationArea.GetRandomDestinationPoint());
        }

        bool HasArrived()
        {
            return enemyNPC.Agent.remainingDistance <= enemyNPC.Agent.stoppingDistance;

        }




    }
}

