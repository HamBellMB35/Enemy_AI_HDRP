using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;


namespace NPCNavigationProJect
{
    public class NPCGroupManager : MonoBehaviour
    {
        [SerializeField]    
        List<Group> Groups = new List<Group>();

        [ContextMenu("Create Enemy NPC Group")]

        public Group CreateNPCGroup()
        {
            GameObject go = new GameObject($"Group {Groups.Count + 1}");

            go.transform.parent = transform;

            Group group = go.AddComponent<Group>();

            Groups.Add(group);

            return group;
        }


        





    }
}

