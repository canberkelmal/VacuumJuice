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
    private GameObject[] machines;
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
        AddToCostumersArray(newCostumer);
    }

    public void CostumerAsksFor(string productName, GameObject costumer)
    {
        switch (productName)
        {
            case "apple":
                AvailableWorker().GetComponent<WorkerSc>().ServeToCostumer(costumer, machines[0]);
                break;
        }
    }

    private GameObject AvailableWorker()
    {
        GameObject availableWorker = new GameObject();
        Debug.Log("AvailableWorker method 1");
        
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

    private void DeleteFromCostumersArray(GameObject deletedObj)
    {
        //X array inden deletedObj isimli objeyi bulup sil
    }
    private void AddToCostumersArray(GameObject addedObj)
    {
        if (addedObj == null)
        {
            Debug.Log("Null obj is tried to add costumers array");
            return;
        }

        GameObject[] newArray = new GameObject[costumers.Length + 1];
        for (int i = 0; i < costumers.Length; i++)
        {
            newArray[i] = costumers[i];
        }

        newArray[costumers.Length] = addedObj;
        costumers = newArray;

        Debug.Log("Costumers is added to costumers array");
    }

    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
