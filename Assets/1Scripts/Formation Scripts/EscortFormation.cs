using Unity.VisualScripting;
using UnityEngine;


namespace NPCNavigationProJect
{

    [CreateAssetMenu(fileName = "EscortFormation", menuName = "NPC_Nav_Project/EscortFormation")]
    public class EscortFormation : GroupFormation
    {
        [SerializeField]
        float Spacing = 3.5f;

        public override Vector3 GetPosition(EnemyNPC enemyNPC, Group group)
        {
            if(group.IsGroupLeader(enemyNPC))
            {
                return enemyNPC.Position;
            }

            int groupMemeberIndex = group.GroupMembers.IndexOf(enemyNPC);

            EnemyNPC groupLeader = group.GroupMembers[groupMemeberIndex - 1];                       // The group Leader is the npc in front

            float distanceToGroupLeader = Vector3.Distance(enemyNPC.Position, groupLeader.Position);            

            if(distanceToGroupLeader < Spacing)
            {
                return enemyNPC.Position;
            }

            else
            {
                Vector3 directionToEnemyNPC = (enemyNPC.Position  - groupLeader.Position).normalized;

                Vector3 targetPosition = groupLeader.Position + directionToEnemyNPC * Spacing;

                return AdjustPosition(targetPosition, groupLeader.Position);
            }

          
        }

    }

}
