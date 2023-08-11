using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IdleManager : MonoBehaviour
{
    public int startLevel = 0;
    public GameObject machinesParent;
    public GameObject workersParent;
    public GameObject costumersParent;
    public GameObject costumer;
    public GameObject machinePanel;
    public Transform costumerSpawnPoint;
    public Transform costumerLastPoint;
    public Transform costumerExitPoint;
    public LayerMask machinesLayerMask;
    public Texture appleIcon, orangeIcon, warningIcon, happyIcon;
    public GameObject[] levels;
    public int resourceCount = 0;
    public int[] maxMachineLevels = new int[0];
    public int currentMaxMachineLevel = -1;
    public Material appleMachineMat, orangeMachineMat, defMachineMat;
    public Dictionary<Vector3, bool> costumerPlaces = new Dictionary<Vector3, bool>();

    private bool anyAvailableWorker = true;
    private int costumerCount = 0;
    public GameObject[] machines = new GameObject[0];
    public GameObject[] readyMachines = new GameObject[0];
    public GameObject[] availableWorkers = new GameObject[0];
    public GameObject[] onQueueCostumers = new GameObject[0];
    private GameObject[] appleMachines = new GameObject[0];
    private GameObject[] orangeMachines = new GameObject[0];
    private GameObject[] workers = new GameObject[0];
    private GameObject[] costumers = new GameObject[0];
    private GameObject[] handledCostumers = new GameObject[0];
    public GameObject[] waitingCostumers = new GameObject[0];

    // Start is called before the first frame update
    void Awake()
    {
        SetCostumerPlaces();
        resourceCount = PlayerPrefs.GetInt("cupCount", 0);
        InitIdleScene();
        FillMachinesArray();
        FillWorkersArray();
        costumerCount = costumersParent.transform.childCount;
        SetAvailableWorkers();
        InvokeRepeating("SpawnCostumer", 1, 1);

    }

    void InitIdleScene()
    {
        PlayerPrefs.SetInt("IdleLevel", startLevel);
        currentMaxMachineLevel = maxMachineLevels[PlayerPrefs.GetInt("IdleLevel", startLevel)];
        GameObject scene = Instantiate(levels[PlayerPrefs.GetInt("IdleLevel", 0)], GameObject.Find("Levels").transform);
        machinesParent = scene.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        InputController();
    } 

    void InputController()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Open machine panel
            if (Physics.Raycast(ray, out hit, 100, machinesLayerMask))
            {
                hit.collider.gameObject.GetComponent<MachineSc>().OpenMachinePanel(true);
            }
        }

        // Restart the game when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void SetCostumerPlaces()
    {
        for(int i = 0; i < 6; i++)
        {
            costumerPlaces.Add(costumerLastPoint.position - (Vector3.right * 1.5f * i), false);
        }
    }

    public Vector3 AvailableCostumerPlace()
    {
        Vector3 sendVector = Vector3.zero;
        foreach(var place in costumerPlaces)
        {
            if (!place.Value)
            {
                sendVector = place.Key;
                costumerPlaces[sendVector] = true;
                break;
            }
        }
        return sendVector;
    }

    public void SetCostumerPlaceAvailable(Vector3 emptyPlace)
    {
        foreach (var place in costumerPlaces)
        {
            if (place.Key == emptyPlace)
            {
                costumerPlaces[emptyPlace] = false;
                break;
            }
        }
    }

    public void SetCupCount(int count)
    {
        resourceCount += count;
        PlayerPrefs.SetInt("cupCount", resourceCount);
    }

    public void CloseMachinePanel()
    {
        Button lvButton = machinePanel.transform.Find("LevelButton").GetComponent<Button>();
        lvButton.onClick.RemoveAllListeners();
        machinePanel.SetActive(false);
    }
    public void SpawnCostumer()
    {
        if(costumerCount < 6 && resourceCount > 0)
        {
            costumerCount++;
            GameObject newCostumer = Instantiate(costumer, costumerSpawnPoint.position, Quaternion.identity, costumersParent.transform);
            //newCostumer.GetComponent<CostumerSc>().SendTo(costumerLastPoint.position - (Vector3.right * 1.5f * (costumerCount - 1)));
            newCostumer.GetComponent<CostumerSc>().SendTo(AvailableCostumerPlace());
            costumers = AddToCustomArray(costumers, newCostumer);
        }
        else if(resourceCount <= 0)
        {
            CancelInvoke("SpawnCostumer");
        }
    }

    public void SentCostumer(GameObject sentCostumer, bool withProduct)
    {
        costumerCount--;
        sentCostumer.transform.parent = null;
        costumers = RemoveFromCustomArray(costumers, sentCostumer);
        RemoveFromQueue(sentCostumer);
        if (withProduct)
        {
            EditOrder();
        }
    }

    public void EditOrder()
    {
        int i = 0;
        foreach(GameObject obj in onQueueCostumers)
        {
            if (!obj.GetComponent<CostumerSc>().noResource)
            {
                //obj.GetComponent<CostumerSc>().SendTo(costumerLastPoint.position - (Vector3.right * 1.5f * i));
            }
            i++;
        }
    }

    public void AddToQueue(GameObject addedCostumer)
    {
        onQueueCostumers = AddToCustomArray(onQueueCostumers, addedCostumer);
    }
    public void RemoveFromQueue(GameObject addedCostumer)
    {
        onQueueCostumers = RemoveFromCustomArray(onQueueCostumers, addedCostumer);
    }

    public void CostumerAsksFor(string productName, GameObject costumer)
    {
        if(!CheckForNoResourceMachine(productName))
        {
            // Ready machine && available worker
            if (CheckForReadyMachine(productName) && anyAvailableWorker)
            {
                CostumerHandled(costumer);

                AvailableWorker().GetComponent<WorkerSc>().ServeToCostumer(costumer, AvailableMachine(productName));

                /*
                switch (productName)
                {
                    case "apple":
                        CostumerHandled(costumer);

                        AvailableWorker().GetComponent<WorkerSc>().ServeToCostumer(costumer, AvailableMachine(productName));
                        break;

                    case "orange":
                        CostumerHandled(costumer);

                        AvailableWorker().GetComponent<WorkerSc>().ServeToCostumer(costumer, AvailableMachine(productName));
                        break;
                }*/
            }
            else
            {
                if (!costumer.GetComponent<CostumerSc>().isWaiting)
                {
                    costumer.GetComponent<CostumerSc>().isWaiting = true;
                    waitingCostumers = AddToCustomArray(waitingCostumers, costumer);
                }
            }
            /*
            // Ready machine && NO available worker
            else if (CheckForReadyMachine(productName) && !(availableWorkers.Length > 0))
            {
                Debug.Log(costumer + " is waiting for worker.");

                if (costumer.GetComponent<CostumerSc>().isWaiting == false)
                {
                    costumer.GetComponent<CostumerSc>().isWaiting = true;
                    waitingCostumers = AddToCustomArray(waitingCostumers, costumer);
                }
            }
            // NO ready machine && available worker
            else if (!CheckForReadyMachine(productName) && (availableWorkers.Length > 0))
            {
                Debug.Log(costumer + " is waiting for machine.");

                if (costumer.GetComponent<CostumerSc>().isWaiting == false)
                {
                    costumer.GetComponent<CostumerSc>().isWaiting = true;
                    waitingCostumers = AddToCustomArray(waitingCostumers, costumer);
                }
            }
            // NO ready machine && NO available worker
            else if (!CheckForReadyMachine(productName) && !(availableWorkers.Length > 0))
            {

            }
            */
        }
        else
        {
            if (!costumer.GetComponent<CostumerSc>().isWaiting)
            {
                costumer.GetComponent<CostumerSc>().isWaiting = true;
                waitingCostumers = AddToCustomArray(waitingCostumers, costumer);
            }
            NoResource(productName);
        }
    }

    private void CostumerHandled(GameObject handled)
    {
        handled.GetComponent<CostumerSc>().isWaiting = false;
        handled.GetComponent<CostumerSc>().isHandled = true;
        waitingCostumers = RemoveFromCustomArray(waitingCostumers, handled);
        handledCostumers = AddToCustomArray(handledCostumers, handled);
    }

    private void NoResource(string productName)
    {
        foreach (var waiting in waitingCostumers)
        {
            if (waiting.GetComponent<CostumerSc>().askFor == productName)
            {
                waitingCostumers = RemoveFromCustomArray(waitingCostumers, waiting);
                waiting.GetComponent<CostumerSc>().TakeAndGo(false);
            }
        }
        /*switch (productName)
        {
            case "apple":
                foreach (var waiting in waitingCostumers)
                {
                    if (waiting.GetComponent<CostumerSc>().askFor == productName)
                    {
                        waitingCostumers = RemoveFromCustomArray(waitingCostumers, waiting);
                        waiting.GetComponent<CostumerSc>().TakeAndGo(false);
                    }
                }
                break;
        }*/
        EditOrder();
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
    private bool CheckForNoResourceMachine(string obj)
    {
        foreach (GameObject machine in machines)
        {
            if (machine.CompareTag(obj + "Machine") && machine.GetComponent<MachineSc>().status > 0)
                return false;
        }
        return true;
    }

    private GameObject AvailableWorker()
    {
        //GameObject availableWorker = new GameObject();
        
        foreach (GameObject worker in workers)
        {
            if (worker.CompareTag("NotBusy"))
            {
                //availableWorker = worker;
                return worker;
            }
            else
            {
                //Debug.Log("____No available worker.___");
            }
        }

        return null;
    }
    private GameObject AvailableMachine(string machineProduct)
    {
        //GameObject availableMachine = new GameObject();

        foreach (GameObject machine in readyMachines)
        {
            if (machine.CompareTag(machineProduct + "Machine"))
            {
                //availableMachine = machine;
                return machine;
            }
            else
            {
                //Debug.Log("Next machine.");
            }
        }

        return null;
    }

    public void SetReadyMachines()
    {
        readyMachines = new GameObject[0];
        foreach (GameObject machine in machines)
        {
            if (machine.GetComponent<MachineSc>().status == 2)
            {
                readyMachines = AddToCustomArray(readyMachines, machine);
            }
        }
        if (waitingCostumers.Length > 0 && readyMachines.Length > 0)
        {
            CostumerAsksFor(waitingCostumers[0].GetComponent<CostumerSc>().askFor, waitingCostumers[0]);
        }
    }
    public void SetAvailableWorkers()
    {
        bool av = false;
        foreach (GameObject worker in workers)
        {
            if (worker.CompareTag("NotBusy"))
            {
                availableWorkers = AddToCustomArray(availableWorkers, worker);
                av = true;
            }
        }
        anyAvailableWorker = av;
        if (waitingCostumers.Length > 0 && anyAvailableWorker)
        {
            CostumerAsksFor(waitingCostumers[0].GetComponent<CostumerSc>().askFor, waitingCostumers[0]);
        }
    }
    private GameObject[] AddToCustomArray(GameObject[] originalArray, GameObject addedObj)
    {
        if (addedObj == null)
        {
            //Debug.Log("Null obj is tried to add to the array");
            return originalArray;
        }

        // Do not add if addedObj is member of original array
        foreach (GameObject obj in originalArray)
        {
            if(obj == addedObj)
            {
                return originalArray;
            }
        }

        GameObject[] newArray = new GameObject[originalArray.Length + 1];
        for (int i = 0; i < originalArray.Length; i++)
        {
            newArray[i] = originalArray[i];
        }



        newArray[originalArray.Length] = addedObj;

        //Debug.Log("Object is added to the array");

        return newArray;
    }
    private GameObject[] RemoveFromCustomArray(GameObject[] originalArray, GameObject removedObj)
    {
        if (removedObj == null)
        {
            //Debug.Log("Null obj is tried to remove from the array");
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
            //Debug.Log("Object not found in the array");
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

        //Debug.Log("Object is removed from the array");

        return newArray;
    }
    
    private void FillMachinesArray()
    {
        machines = new GameObject[machinesParent.transform.childCount];

        for(int i = 0; i< machines.Length; i++)
        {
            machines[i] = machinesParent.transform.GetChild(i).gameObject;

            switch (machines[i].tag)
            {
                case "appleMachine":
                    appleMachines = AddToCustomArray(appleMachines, machines[i]);
                    break;
                case "orangeMachine":
                    orangeMachines = AddToCustomArray(orangeMachines, machines[i]);
                    break;
            }
        }
    }

    public int GetMachineLevel(GameObject machine)
    {
        int count = 0;
        int level = -1;
        foreach (GameObject machineObj in machines)
        {
            if(machine.tag == machineObj.tag)
            {
                count++;
                if(machine == machineObj)
                {
                    level = PlayerPrefs.GetInt((machine.tag + count + "Level"), 1);
                }
            }
        }
        return level;
    }

    public void IncreaseMachineLevel(GameObject machine)
    {
        int count = 0;
        int level = -1;
        foreach (GameObject machineObj in machines)
        {
            if (machine.tag == machineObj.tag)
            {
                count++;
                if (machine == machineObj)
                {
                    level = PlayerPrefs.GetInt((machine.tag + count + "Level"), 1) + 1;
                    PlayerPrefs.SetInt((machine.tag + count + "Level"), level);
                }
            }
        }
    }

    public void ResetMachineLevels(string machineTag)
    {
        for (int i = 1; i < 4; i++)
        {
            if (machineTag == "appleMachine" && i == 1)
            {
                PlayerPrefs.SetInt((machineTag + i + "Level"), 1);
                i++;
            }
            PlayerPrefs.SetInt((machineTag + i + "Level"), 0);
        }
        foreach (GameObject machineObj in machines)
        {
            machineObj.GetComponent<MachineSc>().InitMachine();
        }
    }

    private void FillWorkersArray()
    {
        workers = new GameObject[workersParent.transform.childCount];

        for (int i = 0; i < workers.Length; i++)
        {
            //Debug.Log(workersParent.transform.GetChild(i).gameObject + " is added.");
            workers[i] = workersParent.transform.GetChild(i).gameObject;
        }
    }

    public Texture SetTexture(string product)
    {
        switch (product)
        {
            case "apple":
                return appleIcon;
            case "orange":
                return orangeIcon;

            case "happy":
                return happyIcon;
            case "unHappy":
                return warningIcon;
        }
        return warningIcon;
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
