using UnityEngine;

namespace NPCNavigationProJect
{
    [CreateAssetMenu(fileName = "LineFormation", menuName = "NPC_Nav_Project/LineFormation")]

    public class LineFormation : GroupFormation
    {

        [SerializeField]

        float NPC_Spacing = 3f;

        [SerializeField]

        float spacingVariant = 0.5f;

        public override Vector3 GetPosition(EnemyNPC enemyNPC, Group group)
        {
            if (group.IsGroupLeader(enemyNPC))
            {
                return enemyNPC.Position;
            }

            EnemyNPC groupLeader = group.GetGroupLeader();

            Vector3 groupLeaderRight = Vector3.Cross(Vector3.up, groupLeader.Direction).normalized;      // Calculate  Group Learder's right

            Vector3 pos = groupLeader.Position - groupLeader.Direction * NPC_Spacing;
            
            float groupFormationWidth = (group.GroupFollowerCount - 1) * NPC_Spacing;
            
            pos -= groupLeaderRight * groupFormationWidth * spacingVariant;

            pos += groupLeaderRight * NPC_Spacing * group.GetGroupFollowerIndex(enemyNPC);

            return AdjustPosition(pos, groupLeader.Position);

        }



    }
}
