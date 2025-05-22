using UnityEngine;

namespace NPCNavigationProJect
{
    [CreateAssetMenu(fileName = "MatrixFormation", menuName = "NPC_Nav_Project/MatrixFormation")] 

    public class MatrixFormation : GroupFormation 
    {
        [SerializeField]
        float NPC_Spacing = 3f;

        [SerializeField]
        float rowSpacing = 3f; // Spacing between subsequent rows in the matrix

        [SerializeField]
        int charactersWide = 4; // Defines the width of the matrix

        [SerializeField]
        float leaderBufferZone = 5f; // New field: Additional spacing behind the leader

        public override Vector3 GetPosition(EnemyNPC enemyNPC, Group group)
        {
            if (group.IsGroupLeader(enemyNPC))
            {
                return enemyNPC.Position;
            }

            EnemyNPC groupLeader = group.GetGroupLeader();

            // Calculate Group Leader's right and forward directions
            // Assuming groupLeader.Direction is a Vector3 representing the leader's forward direction
            Vector3 groupLeaderRight = Vector3.Cross(Vector3.up, groupLeader.Direction).normalized;
            Vector3 groupLeaderForward = groupLeader.Direction.normalized;

            int followerIndex = group.GetGroupFollowerIndex(enemyNPC);

            // Calculate row and column
            int column = followerIndex % charactersWide;
            int row = followerIndex / charactersWide;

            // Calculate the offset for the current character
            float xOffset = (column - (charactersWide - 1) / 2f) * NPC_Spacing; // Center the formation horizontally

            // Modified zOffset to include the leaderBufferZone
            // The first row (row = 0) will be at -leaderBufferZone - (0 * rowSpacing)
            // The second row (row = 1) will be at -leaderBufferZone - (1 * rowSpacing)
            float zOffset = -leaderBufferZone - (row * rowSpacing); // Move characters backward in rows

            // Apply offsets relative to the group leader's position and orientation
            Vector3 pos = groupLeader.Position + groupLeaderRight * xOffset + groupLeaderForward * zOffset;

            return AdjustPosition(pos, groupLeader.Position);
        }
    }   
}
