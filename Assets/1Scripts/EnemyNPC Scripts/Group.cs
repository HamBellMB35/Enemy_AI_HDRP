using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;


namespace NPCNavigationProJect
{
    public class Group : MonoBehaviour
    {
        public bool DrawGizmos = false;

        [SerializeField]
        private List<EnemyNPC> groupMembers = new List<EnemyNPC>(); // 1 item is the leader, the resats are followers
        public List<EnemyNPC> GroupMembers => groupMembers;
        
        [SerializeField]
        public GroupFormation GroupFormation;
        
        public int GroupMemberCount { get { return groupMembers.Count; } }
        public int GroupFollowerCount { get { return Mathf.Max(0, GroupMemberCount - 1); } }
        public int GetGroupFollowerIndex(EnemyNPC enemyNpc) {  return groupMembers.IndexOf(enemyNpc) -1; }


        private void Start()
        {
            InitializeGroupMembers();
        }
        public bool IsGroupLeader(EnemyNPC enemyNpc)
        {

           int index = groupMembers.IndexOf(enemyNpc);

            return index == 0;

        }

        private void Update()
        {
            RemoveDeadGroupMembers();
        }

        public Vector3 GetPositionInGroup(EnemyNPC enemyNPC)
        {
            return GroupFormation.GetPosition(enemyNPC, this);
        }

        public EnemyNPC GetGroupLeader()
        {
            if(groupMembers.Count >= 1)
            {
                return groupMembers[0];
            }

            return null;
        }

        private void OnDrawGizmos()
        {
            if(GroupFormation == null || GroupMemberCount <= 0 || DrawGizmos == false)
            {
                return;
            }

            foreach(EnemyNPC member in GroupMembers)
            {

                Vector3 pos = GetPositionInGroup(member);

                Gizmos.color = Color.green;

                if (IsGroupLeader(member))
                {
                    Gizmos.color = Color.red;
                }
                
                Gizmos.DrawSphere(pos, 0.5f);

            }
        }

        public void RemoveDeadGroupMembers()
        {
            for (int i = groupMembers.Count -1; i >= 0; i--)
            {
                if(groupMembers[i].IsAlive == false)
                {
                    groupMembers[i].Group = null;

                    groupMembers.RemoveAt(i);
                }
            }
        }

        public void AddGroupMemeber(EnemyNPC member)
        {
            groupMembers.Add(member);

            member.Group = this;
        }

        public void RemoveGroupMember(EnemyNPC member)
        {
            groupMembers.Remove(member);
            
            member.Group = null;
        }

        public void InitializeGroupMembers()
        {
            foreach (EnemyNPC enemy in groupMembers)
            {
                enemy.Group = this;
            }
        }

    }
}

