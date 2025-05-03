using NPCNavigationProJect;
using UnityEngine;
using UnityEngine.AI;


namespace NPCNavigationProJect
{
    public class EnemyNPCStroll : EnemyNPCComponent
    {
        enum EnemyState
        {
            Strolling,
            Waiting
        }
        
        [Header("Debugging")]
        [SerializeField]
        EnemyState state = EnemyState.Strolling;

        [SerializeField]
        float maxWaitime = 3f;

        [SerializeField]
        float maxRandomWaitTime = 5f;

        float currentMaxWaitTime = 3f;

        [SerializeField]
        private float waitTime = 0f;

        public DestinationArea DestinationArea;

        private void Start()
        {
           // SelectRandomDestination();

            if (Random.Range(0f, 100.0f) > 50f)
            {
                ChangeState(EnemyState.Strolling);
            }

            else
            {
                ChangeState(EnemyState.Waiting);
            }
        }


        private void Update()
        {
            //if (HasArrived())
            //{
            //    SelectRandomDestination();
            //}

            if (state == EnemyState.Waiting)
            {
                waitTime -= Time.deltaTime;

                if(waitTime <= 0f)
                {
                    ChangeState(EnemyState.Strolling);
                }
            }

            else if (state == EnemyState.Strolling)
            {
                if (HasArrived())
                {
                    ChangeState(EnemyState.Waiting);
                }
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

        void ChangeState(EnemyState newstate)
        {
            state = newstate;

            if (state == EnemyState.Strolling)
            {
                enemyNPC.Agent.isStopped = false;

                SelectRandomDestination();
            }

            else if (state == EnemyState.Waiting)
            {
                enemyNPC.Agent.isStopped = true;

                waitTime = maxWaitime + Random.Range(0f, maxRandomWaitTime);

               //waitTime = maxWaitime;

            }
        }




    }
}

