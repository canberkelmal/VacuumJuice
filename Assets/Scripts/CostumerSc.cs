using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumerSc : MonoBehaviour
{
    public string askFor = "apple";
    public float movementSpeed = 1f;
    public bool isWaiting = false;
    public bool isHandled = false;
    

    private IdleManager idleManager;
    private Vector3 destination = Vector3.zero;

    void Awake()
    {
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
    }

    public void SendTo(Vector3 finalPoint)
    {
        destination = finalPoint;
        InvokeRepeating("GoToDestination", 0, Time.fixedDeltaTime);
    }

    private void GoToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
        if(transform.position == destination)
        {
            Debug.Log("Reached to destination.");
            idleManager.CostumerAsksFor(askFor, gameObject);
            CancelInvoke("GoToDestination");
        }
    }

    public void TakeAndGo()
    {
        InvokeRepeating("GoToExit", 0, Time.fixedDeltaTime);
    }

    private void GoToExit()
    {
        transform.position = Vector3.MoveTowards(transform.position, idleManager.costumerExitPoint.position, movementSpeed * Time.deltaTime);
        if (transform.position == idleManager.costumerExitPoint.position)
        {
            Debug.Log("Costumer went.");
            CancelInvoke("GoToExit");
        }
    }
}
