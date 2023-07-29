using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorkerSc : MonoBehaviour
{
    public float speed = 5f;


    private IdleManager idleManager;
    private GameObject machine, costumer;

    private bool waitingCostumer = true;
    private bool waitingForMachine = false;
    private bool serving = false;
    private bool goingMachine = false;
    void Awake()
    {
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == machine)
        {
            CancelInvoke("GoToMachine");
            machine.GetComponent<MachineSc>().TakeProduct();
            InvokeRepeating("GoToCostumer", 0, Time.deltaTime);
        }
    }

    public void ServeToCostumer(GameObject costumer, GameObject machine)
    {
        gameObject.tag = "Busy";
        this.costumer = costumer;
        this.machine = machine;
        StartDuty();
    }

    private void StartDuty()
    {
        if(machine.GetComponent<MachineSc>().status == 2)
        {
            InvokeRepeating("GoToMachine", 0, Time.deltaTime);
        }
    }

    private void GoToMachine()
    {
        transform.position = Vector3.MoveTowards(transform.position, machine.transform.Find("TakeProductPoint").position, speed * Time.deltaTime);
    }

    private void GoToCostumer()
    {
        transform.position = Vector3.MoveTowards(transform.position, costumer.transform.position - Vector3.forward*3.5f, speed * Time.deltaTime);
        if (transform.position == costumer.transform.position - Vector3.forward * 3.5f)
        {
            DeliverToCostumer();
        }
    }

    private void DeliverToCostumer()
    {
        CancelInvoke("GoToCostumer");
        gameObject.tag = "NotBusy";
        costumer.GetComponent<CostumerSc>().TakeAndGo();
    }


}
