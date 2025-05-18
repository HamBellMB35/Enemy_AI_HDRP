using NPCNavigationProJect;
using UnityEngine;


namespace NPCNavigationProJect
{
    [CreateAssetMenu(fileName = "ColumnFormation", menuName = "NPC_Nav_Project/ColumnFormation")]    

    public class ColumnFormation : GroupFormation
    {

        [SerializeField]
   
        float NPC_Spacing = 3f;


        public override Vector3 GetPosition(EnemyNPC enemyNPC, Group group)
        {
            if(group.IsGroupLeader(enemyNPC))
            {
                return enemyNPC.Position;
            }

            EnemyNPC groupLeader = group.GetGroupLeader();

            Vector3 pos = groupLeader.Position - groupLeader.Direction * NPC_Spacing * group.GroupMembers.IndexOf(enemyNPC);  // Followers will be in a line behind the leader

            return AdjustPosition(pos,groupLeader.Position );
        }



    }
}

