using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumerSc : MonoBehaviour
{
    public string askFor = "apple";
    public float movementSpeed = 1f;
    public bool isWaiting = false;
    public bool isHandled = false;

    private IdleManager idleManager;
    private Vector3 destination = Vector3.zero;
    private RawImage statuUI;

    void Awake()
    {
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
        statuUI = transform.Find("Canvas").Find("Statu").GetComponent<RawImage>();
        statuUI.texture = idleManager.SetTexture(askFor);
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
        statuUI.texture = idleManager.SetTexture("happy");
        idleManager.SentCostumer(gameObject);
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
