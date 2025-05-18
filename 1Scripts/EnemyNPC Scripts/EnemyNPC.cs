using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;


namespace NPCNavigationProJect
{
    [SelectionBase]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]

    public class EnemyNPC : MonoBehaviour
    {
        [HideInInspector]
        public NavMeshAgent Agent;

        [HideInInspector]
        public Animator Animator;

        public Vector3 Direction {  get; private set; } = Vector3.forward;
        public Vector3 Position => transform.position;

        public Group Group = null;

        public bool Strolling = false;
        public bool IsAlive = true;

        public float CurrentEnemyNPCSpeed => GetNPCSpeed();

        //public float CurrentEnemyNPCSpeed { get { return Agent.velocity.magnitude; } }

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(CurrentEnemyNPCSpeed > 0.1f)
            {
                Direction = Agent.velocity.normalized;
            }
        }

        public float GetNPCSpeed()
        {
            return Agent.velocity.magnitude;
        }
    }


}
