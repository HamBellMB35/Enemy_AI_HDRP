using UnityEngine;


namespace NPCNavigationProJect
{
    public class EnemyGeneator : MonoBehaviour
    {
        [SerializeField]
        EnemyNPC EnemyNPCPrefab;
        
        [SerializeField]
        DestinationArea DestinationArea;

        [SerializeField]
        int EnemyNPCsToSpawn = 20;

        private void Start()
        {
            GenerateEnemyNPCs();
        }

        private void GenerateEnemyNPCs()
        {
            for (int enemyNumber = 0; enemyNumber < EnemyNPCsToSpawn; enemyNumber++)
            {

                Vector3 enemyNPCPosition = DestinationArea.GetRandomDestinationPoint();

                Quaternion enemyNPCRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                EnemyNPC enemyNPC = Instantiate(EnemyNPCPrefab, enemyNPCPosition, enemyNPCRotation);

                enemyNPC.GetComponent<EnemyNPCStroll>().DestinationArea = DestinationArea;


            }
        }
    }
}

