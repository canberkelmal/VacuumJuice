using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSc : MonoBehaviour
{
    public float speed = 5f;


    private IdleManager idleManager;
    private GameObject machine, costumer;
    private float income = 0;
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
        //Debug.Log("worker hit to" + other.gameObject);
        if (gameObject.CompareTag("Busy") && goingMachine)
        {
            goingMachine = false;
            CancelInvoke("GoToMachine");
            income = machine.GetComponent<MachineSc>().TakeProduct();
            InvokeRepeating("GoToCostumer", 0, Time.fixedDeltaTime);
        }
    }

    public void ServeToCostumer(GameObject costumer, GameObject machine)
    {
        gameObject.tag = "Busy";
        idleManager.SetAvailableWorkers();
        this.costumer = costumer;
        this.machine = machine;
        StartDuty();
    }

    private void StartDuty()
    {
        if(machine.GetComponent<MachineSc>().status == 2)
        {
            goingMachine = true;
            InvokeRepeating("GoToMachine", 0, Time.fixedDeltaTime);
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
        costumer.GetComponent<CostumerSc>().TakeAndGo(true);
        idleManager.CashAnimation(costumer, income);
        idleManager.SetMoneyCount(income);
        income = 0;
        idleManager.SetAvailableWorkers();
    }


}
