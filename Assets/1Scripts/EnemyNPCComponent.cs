using UnityEngine;



namespace NPCNavigationProJect
{


    public class EnemyNPCComponent:MonoBehaviour
    {
        protected EnemyNPC enemyNPC;

        protected virtual void Awake()
        {
            enemyNPC = GetComponentInParent<EnemyNPC>();
        }





    }
}

