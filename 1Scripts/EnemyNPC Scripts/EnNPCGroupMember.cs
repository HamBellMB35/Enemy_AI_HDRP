using UnityEngine;


namespace NPCNavigationProJect
{
    public class EnNPCGroupMember : EnemyNPCComponent
    {


        void Update()
        {
            if (enemyNPC.Group == null)
            {
                enemyNPC.Strolling = true;
                return;
            }

            if (enemyNPC.Group.IsGroupLeader(enemyNPC))
            {

                enemyNPC.Strolling = true;
            }

            else
            {
                Vector3 pos = enemyNPC.Group.GetPositionInGroup(enemyNPC);

                enemyNPC.Agent.SetDestination(pos); 
                enemyNPC.Agent.isStopped = false;

                enemyNPC.Strolling= false;
            }

        }
    }
}
