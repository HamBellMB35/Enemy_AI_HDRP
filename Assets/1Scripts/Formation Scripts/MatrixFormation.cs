using UnityEngine;

namespace NPCNavigationProJect
{
    [CreateAssetMenu(fileName = "MatrixFormation", menuName = "NPC_Nav_Project/MatrixFormation")] // Changed menu name

    public class MatrixFormation : GroupFormation // Renamed class to MatrixFormation
    {
        [SerializeField]
        float NPC_Spacing = 3f;

        [SerializeField]
        float rowSpacing = 3f; // New field for spacing between rows

        [SerializeField]
        int charactersWide = 4; // Defines the width of the matrix

        public override Vector3 GetPosition(EnemyNPC enemyNPC, Group group)
        {
            if (group.IsGroupLeader(enemyNPC))
            {
                return enemyNPC.Position;
            }

            EnemyNPC groupLeader = group.GetGroupLeader();

            // Calculate Group Leader's right and forward directions
            Vector3 groupLeaderRight = Vector3.Cross(Vector3.up, groupLeader.Direction).normalized;
            Vector3 groupLeaderForward = groupLeader.Direction.normalized;

            int followerIndex = group.GetGroupFollowerIndex(enemyNPC);

            // Calculate row and column
            int column = followerIndex % charactersWide;
            int row = followerIndex / charactersWide;

            // Calculate the offset for the current character
            float xOffset = (column - (charactersWide - 1) / 2f) * NPC_Spacing; // Center the formation horizontally
            float zOffset = -row * rowSpacing; // Move characters backward in rows

            // Apply offsets relative to the group leader's position and orientation
            Vector3 pos = groupLeader.Position + groupLeaderRight * xOffset + groupLeaderForward * zOffset;

            return AdjustPosition(pos, groupLeader.Position);
        }
    }
}
