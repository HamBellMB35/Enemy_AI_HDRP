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

        [Header("Waiting Settings")]

        [SerializeField]
        private float waitTime = 0f;
        
        [SerializeField]
        float maxWaitime = 3f;

        [SerializeField]
        float maxRandomWaitTime = 5f;
        //float currentMaxWaitTime = 3f;


        [Space(15f)]
        [Header("Strolling Settings")]
        
        [SerializeField]
        float strollingTime = 0f;
        
        [SerializeField]
        float maxStrollingTime = 5f;





       

        public DestinationArea DestinationArea;

        private void Start()
        {
            // SelectRandomDestination();

            if (enemyNPC.Strolling)
            {
                if (Random.Range(0f, 100.0f) > 50f)
                {
                    ChangeState(EnemyState.Strolling);
                }

                else
                {
                    ChangeState(EnemyState.Waiting);
                }
            }

           
        }


        private void Update()
        {
            //if (HasArrived())
            //{
            //    SelectRandomDestination();
            //}

            if (enemyNPC.Strolling == false)
            {
                return;
            }

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
                strollingTime -= Time.deltaTime;
                
                if (HasArrived() || strollingTime <= 0f )
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

                strollingTime = maxStrollingTime;
            }

            else if (state == EnemyState.Waiting)
            {
                waitTime = maxWaitime;

                enemyNPC.Agent.isStopped = true;

                waitTime = maxWaitime + Random.Range(0f, maxRandomWaitTime);


            }
        }




    }
}

