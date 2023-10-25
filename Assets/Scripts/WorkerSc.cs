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
<<<<<<< HEAD
    private float income = 0; 
=======
    private float income = 0;
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    private bool waitingCostumer = true;
    private bool waitingForMachine = false;
    private bool serving = false;
    private bool goingMachine = false;
    void Awake()
    {
<<<<<<< HEAD
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>(); 
=======
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
<<<<<<< HEAD
        transform.position = Vector3.MoveTowards(transform.position, machine.transform.Find("TakeProductPoint").position, speed * Time.fixedDeltaTime);
=======
        transform.position = Vector3.MoveTowards(transform.position, machine.transform.Find("TakeProductPoint").position, speed * Time.deltaTime);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
<<<<<<< HEAD
        transform.position = Vector3.MoveTowards(transform.position, costumer.transform.position - Vector3.forward*3f, speed * Time.fixedDeltaTime);
        if (transform.position == costumer.transform.position - Vector3.forward * 3f)
=======
        transform.position = Vector3.MoveTowards(transform.position, costumer.transform.position - Vector3.forward*3.5f, speed * Time.deltaTime);
        if (transform.position == costumer.transform.position - Vector3.forward * 3.5f)
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
