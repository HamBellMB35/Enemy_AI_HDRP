using UnityEngine;
using UnityEngine.AI;


namespace NPCNavigationProJect
{
    public class DestinationArea : MonoBehaviour
    {
        public float Radius = 18f;


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }

        public Vector3 GetRandomDestinationPoint()
        {
            Vector3 randomDirection = Random.insideUnitSphere * Radius;
            randomDirection.y = 0;

            Vector3 randomPoint = transform.position + randomDirection;

            NavMeshHit Hit;

            Vector3 finalPosition = transform.position;

            if ( NavMesh.SamplePosition(randomPoint, out Hit, 2f, 1))
            {
                finalPosition = Hit.position;
            } 
            
            return finalPosition;


        }


    }

}

