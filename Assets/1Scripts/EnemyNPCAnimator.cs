using System.Collections;
using System.Collections.Generic;
using NPCNavigationProJect;
using UnityEngine;

namespace NPCNavigationProJect
{
    public class EnemyNPCAnimator : EnemyNPCComponent
    {
        private void Update()
        {
            enemyNPC.Animator.SetFloat("Speed", enemyNPC.CurrentEnemyNPCSpeed);         // Change strign to Hash??
        }





    }

}
