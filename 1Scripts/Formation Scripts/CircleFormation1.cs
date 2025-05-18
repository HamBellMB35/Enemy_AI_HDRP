using System;
using UnityEngine;



namespace NPCNavigationProJect
{
    [CreateAssetMenu(fileName = "CircleFormation", menuName = "NPC_Nav_Project/CircleFormation")]

    public class CircleFormation : GroupFormation
    {
        [SerializeField]

        float FormationRadius = 3f;

        public override Vector3 GetPosition(EnemyNPC enemyNPC, Group group)
        {
            if( group.IsGroupLeader(enemyNPC) )
            {
                return enemyNPC.Position;
            }

            EnemyNPC groupLeader = group.GetGroupLeader();

            float angle = group.GetGroupFollowerIndex(enemyNPC) * 360f / group.GroupFollowerCount;

            Vector3 pos = groupLeader.Position + Quaternion.Euler(0f, angle, 0f) * groupLeader.Direction * FormationRadius;

            return AdjustPosition(pos, groupLeader.Position);

        }




    }
}


