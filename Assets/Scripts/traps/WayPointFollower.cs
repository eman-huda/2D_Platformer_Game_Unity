using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointFollower : MonoBehaviour
{
    public Transform[] wayPoints;
    public int currentWayPointIndex;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(wayPoints[currentWayPointIndex].position, transform.position) == 0f)
        {
            currentWayPointIndex++;
            if (currentWayPointIndex >= wayPoints.Length) {
                currentWayPointIndex = 0;

            }
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWayPointIndex].position, Time.deltaTime);

    }
}
