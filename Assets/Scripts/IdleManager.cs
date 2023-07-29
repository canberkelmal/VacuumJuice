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
    public GameObject[] machines = new GameObject[0];
    public GameObject[] readyMachines = new GameObject[0];
    private GameObject[] availableWorkers = new GameObject[0];
    private GameObject[] appleMachines = new GameObject[0];
    private GameObject[] workers = new GameObject[0];
    private GameObject[] costumers = new GameObject[0];
    private GameObject[] handledCostumers = new GameObject[0];
    private GameObject[] waitingCostumers = new GameObject[0];
    // Start is called before the first frame update
    void Awake()
    {
        FillMachinesArray();
        FillWorkersArray();
        costumerCount = costumersParent.transform.childCount;
        SetAvailableWorkers();
    }

    private void Update()
    {

        // Restart the game when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
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
        // Ready machine && available worker
        if(CheckForReadyMachine(productName) && availableWorkers.Length > 0)
        {
            switch (productName)
            {
                case "apple":
                    costumer.GetComponent<CostumerSc>().isHandled = true;
                    handledCostumers = AddToCustomArray(handledCostumers, costumer);

                    AvailableWorker().GetComponent<WorkerSc>().ServeToCostumer(costumer, AvailableMachine(productName));
                    break;
            }
        }
        // Ready machine && NO available worker
        else if (CheckForReadyMachine(productName) && !(availableWorkers.Length > 0))
        {
            waitingCostumers = AddToCustomArray(waitingCostumers, costumer);
        }
        // else if -> !readyMachine && availableWorker
        // else if -> ! && !
    }

    private bool CheckForReadyMachine(string obj)
    {
        foreach (GameObject machine in readyMachines)
        {
            if (machine.CompareTag(obj + "Machine"))
                return true;
        }
        return false;
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

        foreach (GameObject machine in readyMachines)
        {
            if (machine.CompareTag(machineProduct + "Machine"))
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

    public void SetReadyMachines()
    {
        Debug.Log("SetReadyMachines triged");
        readyMachines = new GameObject[0];
        foreach (GameObject machine in machines)
        {
            Debug.Log("SetReadyMachines forlooping");
            if (machine.GetComponent<MachineSc>().status == 2)
            {
                Debug.Log(machine + "is ready");
                readyMachines = AddToCustomArray(readyMachines, machine);
            }
        }
    }
    public void SetAvailableWorkers()
    {
        availableWorkers = new GameObject[0];
        foreach (GameObject worker in workers)
        {
            if (worker.CompareTag("NotBusy"))
            {
                availableWorkers = AddToCustomArray(availableWorkers, worker);
            }
        }
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
    private GameObject[] RemoveFromCustomArray(GameObject[] originalArray, GameObject removedObj)
    {
        if (removedObj == null)
        {
            Debug.Log("Null obj is tried to remove from the array");
            return originalArray;
        }

        int removedIndex = -1;

        // Find the index of the object to be removed
        for (int i = 0; i < originalArray.Length; i++)
        {
            if (originalArray[i] == removedObj)
            {
                removedIndex = i;
                break;
            }
        }

        if (removedIndex == -1)
        {
            Debug.Log("Object not found in the array");
            return originalArray;
        }

        GameObject[] newArray = new GameObject[originalArray.Length - 1];

        // Copy elements before the removed object
        for (int i = 0; i < removedIndex; i++)
        {
            newArray[i] = originalArray[i];
        }

        // Skip the removed object and copy the rest of the elements
        for (int i = removedIndex + 1; i < originalArray.Length; i++)
        {
            newArray[i - 1] = originalArray[i];
        }

        Debug.Log("Object is removed from the array");

        return newArray;
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

    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
