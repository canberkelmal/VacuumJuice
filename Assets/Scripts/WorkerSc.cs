using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSc : MonoBehaviour
{
    public float speed = 5f;

    private Transform productPlace;
    private IdleManager idleManager;
    private GameObject machine, costumer;
    private GameObject handledProduct;
    private float income = 0;
    private bool waitingCostumer = true;
    private bool waitingForMachine = false;
    private bool serving = false;
    private bool goingMachine = false;
    void Awake()
    {
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
        productPlace = transform.Find("ProductPlace");
    }

    /*private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("worker hit to" + other.gameObject);
        if (gameObject.CompareTag("Busy") && goingMachine && other.gameObject == machine)
        {
            goingMachine = false;
            CancelInvoke("GoToMachine");
            income = machine.GetComponent<MachineSc>().TakeProduct();
            InvokeRepeating("GoToCostumer", 0, Time.fixedDeltaTime);
        }
    }*/

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
        if(machine != null && machine.GetComponent<MachineSc>().status == 2)
        {
            goingMachine = true;
            transform.LookAt(machine.transform.Find("TakeProductPoint").position);
            transform.Find("WorkerObj").GetComponent<Animator>().SetBool("Walk", true);
            InvokeRepeating("GoToMachine", 0, Time.fixedDeltaTime);
        }
        else
        {
            CancelInvoke("GoToCostumer");
            transform.Find("WorkerObj").GetComponent<Animator>().SetBool("Walk", false);
            machine = null;
            gameObject.tag = "NotBusy";
            income = 0;
            idleManager.SetAvailableWorkers();
        }
    }

    private void GoToMachine()
    {
        transform.position = Vector3.MoveTowards(transform.position, machine.transform.Find("TakeProductPoint").position, speed * Time.deltaTime);
        if (transform.position == machine.transform.Find("TakeProductPoint").position)
        {
            goingMachine = false;
            CancelInvoke("GoToMachine");
            income = machine.GetComponent<MachineSc>().TakeProduct();
            handledProduct = Instantiate(machine.GetComponent<MachineSc>().HandleProduct(), productPlace);
            handledProduct.transform.localPosition = Vector3.zero;
            transform.LookAt(costumer.transform.position);
            InvokeRepeating("GoToCostumer", 0, Time.fixedDeltaTime);
        }
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
        transform.Find("WorkerObj").GetComponent<Animator>().SetBool("Walk", false);
        CancelInvoke("GoToCostumer");
        machine = null;
        gameObject.tag = "NotBusy";
        costumer.GetComponent<CostumerSc>().TakeAndGo(true, handledProduct);
        idleManager.CashAnimation(costumer, income);
        idleManager.SetMoneyCount(income);
        income = 0;
        idleManager.SetAvailableWorkers();
    }


}
