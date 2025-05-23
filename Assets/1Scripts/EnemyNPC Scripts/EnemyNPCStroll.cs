using System.Collections;
using NPCNavigationProJect;
using UnityEngine;
using UnityEngine.AI;


namespace NPCNavigationProJect
{
    // This script controls the strolling and waiting behavior of an Enemy NPC.
    public class EnemyNPCStroll : EnemyNPCComponent
    {
        // Defines the possible states for the NPC: Strolling or Waiting.
        enum EnemyState
        {
            Strolling,
            Waiting
        }

        [Header("Debugging")]
        [SerializeField]
        // Current state of the NPC.
        EnemyState state = EnemyState.Strolling;

        [Header("Waiting Settings")]
        [SerializeField]
        // Current time remaining in the waiting state.
        private float waitTime = 0f;

        [SerializeField]
        // Base maximum time the NPC will wait.
        float maxWaitime = 3f;

        [SerializeField]
        // Additional random time to add to the wait time.
        float maxRandomWaitTime = 5f;

        [Space(15f)]
        [Header("Strolling Settings")]
        [SerializeField]
        // Current time remaining in the strolling state.
        float strollingTime = 0f;

        [SerializeField]
        // Maximum time the NPC will stroll before potentially waiting.
        float maxStrollingTime = 5f;

        // Reference to the area where the NPC can find new destinations.
        public DestinationArea DestinationArea;

        
        private void Start()
        {
            // If the NPC is set to stroll, randomly choose to start in Strolling or Waiting state.
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
            // If NPC is not set to stroll, do nothing.
            if (enemyNPC.Strolling == false)
            {
                return;
            }

            // Handle logic based on the current state.
            if (state == EnemyState.Waiting)
            {
                // Decrease wait time.
                waitTime -= Time.deltaTime;

                // If wait time is over, change to Strolling state.
                if (waitTime <= 0f)
                {
                    ChangeState(EnemyState.Strolling);
                }
            }
            else if (state == EnemyState.Strolling)
            {
                // Decrease strolling time.
                strollingTime -= Time.deltaTime;

                // If arrived at destination or strolling time is over, change to Waiting state.
                if (HasArrived() || strollingTime <= 0f)
                {
                    ChangeState(EnemyState.Waiting);
                }
            }
        }

        
        void SelectRandomDestination()
        {
            enemyNPC.Agent.SetDestination(DestinationArea.GetRandomDestinationPoint());
        }

        // Checks if the NPC has arrived at its current destination.
        bool HasArrived()
        {
            return enemyNPC.Agent.remainingDistance <= enemyNPC.Agent.stoppingDistance;
        }

        // Changes the NPC's state and performs actions based on the new state.
        void ChangeState(EnemyState newstate)
        {
            state = newstate;

            if (state == EnemyState.Strolling)
            {
                // Enable agent movement, select new destination, and reset strolling time.
                enemyNPC.Agent.isStopped = false;
                SelectRandomDestination();
                strollingTime = maxStrollingTime;
            }
            else if (state == EnemyState.Waiting)
            {
                // Disable agent movement and set a new random wait time.
                enemyNPC.Agent.isStopped = true;
                waitTime = maxWaitime + Random.Range(0f, maxRandomWaitTime);
            }
        }
    }
}

