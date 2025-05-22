using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace NPCNavigationProJect
{
    [RequireComponent(typeof(NPCGroupManager))]

    public class NPCGroupGenerator : MonoBehaviour
    {

        [SerializeField]

        DestinationArea[] DestinationAreas;
        
        [SerializeField]
        GroupFormation[] NPCGroupFormations;

        [Header("Prefabs Needed")]
        [SerializeField]
        EnemyNPC GroupLeader;

        [SerializeField]
        EnemyNPC GroupMember;

        [Header("Group Settings")]
        [SerializeField]
        int MinimumGroupSize = 3;

        [SerializeField]
        int MaximumGroupSize = 12;

        [SerializeField]
        int GroupsCount = 6;

        NPCGroupManager npcGroupManager;


        private void Start()
        {
            
            npcGroupManager = GetComponent<NPCGroupManager>();

            for (int i = 0; i < GroupsCount; i++)
            {
                GenerateRandomNPCGroup();
            }



        }

        private void GenerateRandomNPCGroup()
        {
            Group group = npcGroupManager.CreateNPCGroup();

            DestinationArea destinationArea = GetRandomDestinationArea();

            Vector3 groupLeaderPosition = destinationArea.GetRandomDestinationPoint();

            GroupFormation groupFormation = GetRandomGroupFormation();

            Quaternion groupLeaderRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            EnemyNPC groupLeader = Instantiate(GroupLeader, groupLeaderPosition, groupLeaderRotation);

            GroupLeader.GetComponent<EnemyNPCStroll>().DestinationArea = destinationArea;

            group.AddGroupMemeber(groupLeader);
            
            group.GroupFormation = groupFormation;

            int npcFollowerCount = Random.Range(MinimumGroupSize, MaximumGroupSize);

            for (int i = 0;i < npcFollowerCount; i++)
            {
                EnemyNPC follower = Instantiate(GroupMember, groupLeaderPosition, groupLeaderRotation);

                group.AddGroupMemeber(follower);

                Vector3 groupMemberPosition = group.GetPositionInGroup(follower);

                follower.transform.position = groupMemberPosition;

            }






        }

        DestinationArea GetRandomDestinationArea()
        {
            return DestinationAreas[Random.Range(0, DestinationAreas.Length)];
        }

        GroupFormation GetRandomGroupFormation()
        {
            return NPCGroupFormations[Random.Range(0, NPCGroupFormations.Length)];
        }

        

        


    }

}