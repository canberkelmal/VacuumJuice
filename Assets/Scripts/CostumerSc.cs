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
    public bool onQueue = false;
    public bool noResource = false;

    private Transform productPlace;
    private IdleManager idleManager;
    private Vector3 destination = Vector3.zero;
    private RawImage statuUI;

    void Awake()
    {
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
        switch (idleManager.currentLevel)
        {
            case 1:
                askFor = Random.Range(1, 100) < 50 ? "apple" : "orange";
                break;
            case 2:
                int a = Random.Range(1, 150);
                if (a <= 50)
                    askFor = "apple";
                else if (a > 50 && a <= 100)
                    askFor = "orange";
                else
                    askFor = "frozen";

                break;
        }
        statuUI = transform.Find("Canvas").Find("Statu").GetComponent<RawImage>();
        statuUI.texture = idleManager.SetTexture(askFor);
        productPlace = transform.Find("ProductPlace");
    }

    public void SendTo(Vector3 finalPoint)
    {
        destination = finalPoint;
        transform.LookAt(destination);
        transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
        transform.Find("CostumerObj").GetComponent<Animator>().SetBool("Walk", true);
        InvokeRepeating("GoToDestination", 0, Time.fixedDeltaTime);
    }

    private void GoToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
        if (transform.position == destination)
        {
            if(!onQueue)
            {
                idleManager.AddToQueue(gameObject);
                onQueue = true;
                //Debug.Log("Reached to destination.");
                idleManager.CostumerAsksFor(askFor, gameObject);
            }
            transform.Find("CostumerObj").GetComponent<Animator>().SetBool("Walk", false);
            transform.rotation= Quaternion.Euler(0f, 180f, 0f);
            transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
            CancelInvoke("GoToDestination");
        }
    }


    public void TakeAndGo(bool taken, GameObject handledProduct)
    {
        Destroy(Instantiate(idleManager.takeProductParticle, transform.position + Vector3.up, Quaternion.Euler(Vector3.right * -90)), 1);
        if (taken)
        {
            statuUI.texture = idleManager.SetTexture("happy");
        }
        else
        { 
            noResource = true;
            statuUI.texture = idleManager.SetTexture("unHappy");
        }
        if(handledProduct != null)
        {
            handledProduct.transform.parent = productPlace;
            handledProduct.transform.localPosition = Vector3.zero;
        }
        idleManager.SetCostumerPlaceAvailable(transform.position);
        idleManager.SentCostumer(gameObject, taken);
        transform.LookAt(idleManager.costumerExitPoint.position);
        transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
        transform.Find("CostumerObj").GetComponent<Animator>().SetBool("Walk", true);
        InvokeRepeating("GoToExit", 0, Time.fixedDeltaTime); 
    }

    private void GoToExit()
    {
        CancelInvoke("GoToDestination");
        transform.position = Vector3.MoveTowards(transform.position, idleManager.costumerExitPoint.position, movementSpeed * Time.deltaTime);
        if (transform.position == idleManager.costumerExitPoint.position)
        {
            //Debug.Log("Costumer went.");
            CancelInvoke("GoToExit");
            Destroy(gameObject);
        }
    }
}
