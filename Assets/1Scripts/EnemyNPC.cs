using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace NPCNavigationProJect
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]

    public class EnemyNPC : MonoBehaviour
    {
        [HideInInspector]
        public NavMeshAgent Agent;

        [HideInInspector]
        public Animator Animator;

        public float CurrentEnemyNPCSpeed => GetNPCSpeed();

        //public float CurrentEnemyNPCSpeed { get { return Agent.velocity.magnitude; } }

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
        }

        public float GetNPCSpeed()
        {
            return Agent.velocity.magnitude;
        }
    }


}
