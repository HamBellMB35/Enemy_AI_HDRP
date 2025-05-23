using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();                                               // For demo only, remove after video

    public int wayPointIndex;                                                                               // For demo only, remove after video
    public int moveSpeed = 8;                                                                               // For demo only, remove after video
    public WayPointManager waypointManger;
    private void Start()
    {
        wayPointIndex = 0;                                                                                   // For demo only, remove after video               
    }
    // Update is called once per frame
    
}
