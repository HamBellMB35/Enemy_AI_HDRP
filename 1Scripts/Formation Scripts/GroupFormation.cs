using UnityEngine;
using UnityEngine.AI;


namespace NPCNavigationProJect
{
    [CreateAssetMenu(fileName = "GroupFormation", menuName = "Scriptable Objects/GroupFormation")]
    public abstract class GroupFormation : ScriptableObject
    {
        public abstract Vector3 GetPosition(EnemyNPC enemyNPC, Group group);

        protected Vector3 AdjustPosition( Vector3 position, Vector3 groupLeaderPosition)
        {
            if(NavMesh.Raycast(groupLeaderPosition, position, out NavMeshHit hit, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return position;
        }
    }
}

