using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumerSc : MonoBehaviour
{
    public float movementSpeed = 1f;
    private Vector3 destination = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SendTo(Vector3 finalPoint)
    {
        destination = finalPoint;
        InvokeRepeating("GoToDestination", 0, Time.deltaTime);
    }

    private void GoToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
        if(transform.position == destination)
        {
            CancelInvoke("GoToDestination");
        }
    }
}
