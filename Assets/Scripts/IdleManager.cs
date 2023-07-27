using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleManager : MonoBehaviour
{
    public GameObject machinesParent;
    public GameObject workersParent;
    public GameObject costumersParent;
    public GameObject costumer;
    public Transform costumerSpawnPoint;
    public Transform costumerLastPoint;
    public Transform costumerExitPoint;

    private int costumerCount = 0;
    private GameObject[] machines, appleMachines;
    private GameObject[] workers = new GameObject[0];
    private GameObject[] costumers = new GameObject[0];
    // Start is called before the first frame update
    void Awake()
    {
        FillMachinesArray();
        FillWorkersArray();
        costumerCount = costumersParent.transform.childCount;
    }

    private void Update()
    {

        // Restart the game when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    private void FillMachinesArray()
    {
        machines = new GameObject[machinesParent.transform.childCount];

        for(int i = 0; i< machines.Length; i++)
        {
            machines[i] = machinesParent.transform.GetChild(i).gameObject;

            if (machines[i].CompareTag("appleMachine"))
            {
                appleMachines = AddToCustomArray(appleMachines, machines[i]);
            }
        }
    }
    private void FillWorkersArray()
    {
        workers = new GameObject[workersParent.transform.childCount];

        for (int i = 0; i < workers.Length; i++)
        {
            Debug.Log(workersParent.transform.GetChild(i).gameObject + " is added.");
            workers[i] = workersParent.transform.GetChild(i).gameObject;
        }
    }

    public void SpawnCostumer()
    {
        costumerCount++;
        GameObject newCostumer = Instantiate(costumer, costumerSpawnPoint.position, Quaternion.identity, costumersParent.transform);
        newCostumer.GetComponent<CostumerSc>().SendTo(costumerLastPoint.position - (Vector3.right * 1.5f * (costumerCount-1) ));
        costumers = AddToCustomArray(costumers, newCostumer);
    }

    public void CostumerAsksFor(string productName, GameObject costumer)
    {
        switch (productName)
        {
            case "apple":
                AvailableWorker().GetComponent<WorkerSc>().ServeToCostumer(costumer, AvailableMachine(productName));
                break;
        }
    }

    private GameObject AvailableWorker()
    {
        GameObject availableWorker = new GameObject();
        
        foreach (GameObject worker in workers)
        {
            if (worker.CompareTag("NotBusy"))
            {
                availableWorker = worker;
            }
            else
            {
                Debug.Log("No available worker.");
            }
        }

        return availableWorker;
    }
    private GameObject AvailableMachine(string machineProduct)
    {
        GameObject availableMachine = new GameObject();

        foreach (GameObject machine in machines)
        {
            if (machine.CompareTag(machineProduct + "Machine") && machine.GetComponent<MachineSc>().status == 2)
            {
                availableMachine = machine;
                return availableMachine;
            }
            else
            {
                Debug.Log("Next machine.");
            }
        }

        return availableMachine;
    }

    private void DeleteFromCostumersArray(GameObject deletedObj)
    {
        //X array inden deletedObj isimli objeyi bulup sil
    }

    private GameObject[] AddToCustomArray(GameObject[] originalArray, GameObject addedObj)
    {
        if (addedObj == null)
        {
            Debug.Log("Null obj is tried to add to the array");
            return originalArray;
        }

        GameObject[] newArray = new GameObject[originalArray.Length + 1];
        for (int i = 0; i < originalArray.Length; i++)
        {
            newArray[i] = originalArray[i];
        }

        newArray[originalArray.Length] = addedObj;

        Debug.Log("Object is added to the array");

        return newArray;
    }

    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
