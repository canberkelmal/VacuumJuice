using SupersonicWisdomSDK;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IdleManager : MonoBehaviour 
{
    public int startLevel = 0;
    public int currentLevel = 0;
    public int maxCostumerCount = 4;
    public float workerSpeed = 3f;
    public float workerDefSpeed = 3f;
    public int workerCount = 1;
    public float appleSpeedDevider = 1f;
    public float orangeSpeedDevider = 1f;
    public float frozenSpeedDevider = 1f;
    public float appleDefDuration = 1f;
    public float orangeDefDuration = 1f;
    public float frozenDefDuration = 1f;
    public int defMaxCostumerCount = 3;
    public Color[] hairColors = new Color[6];
    public GameObject runnerPrefab;
    public GameObject idlePrefab;
    public GameObject outOfResourcePanel;
    public GameObject machinesParent;
    public GameObject workersParent;
    public GameObject costumersParent;
    public GameObject costumer1, costumer2, testCostumer;
    public GameObject sideWorker;
    public GameObject machinePanel;
    public GameObject settingsPanel;
    public GameObject upgradesPanel;
    public GameObject tutorialPanel; 
    public GameObject cashAnimUI;
    public GameObject upgradeMachineParticle, takeProductParticle;
    public Transform costumerSpawnPoint;
    public Transform costumerLastPoint;
    public bool tutorial = false;
    public HealthBarSc levelBar;
    public Color levelBarDefColor, levelBarMaxColor;

    public Transform costumerExitPoint; 
    public Transform workerSpawnPoint;
    //public LayerMask machinesLayerMask;
    public Texture appleIcon, orangeIcon, frozenIcon, warningIcon, happyIcon;
    public Texture apple1, apple2, apple3, orange1, orange2, orange3, frozen1, frozen2, frozen3;
    public GameObject[] levels;
    public GameObject[] upgradesForLevels;
    public int resourceCount = 0;
    public float moneyCount = 0;
    public int[] maxMachineLevels = new int[0];
    public int currentMaxMachineLevel = -1;
    public Material appleMachineMat, orangeMachineMat, defMachineMat;
    public Dictionary<Vector3, bool> costumerPlaces = new Dictionary<Vector3, bool>();
    //UI Elements
    public Text cupCountTx;
    public Text moneyCountTx;
    public Transform nextLevelButton;
    public int firstMachineUpgradeLevel, secondMachineUpgradeLevel, thirdMachineUpgradeLevel;


    private bool anyAvailableWorker = true;
    private int costumerCount = 0;
    public GameObject[] machines = new GameObject[0];
    public GameObject[] readyMachines = new GameObject[0]; 
    public GameObject[] availableWorkers = new GameObject[0]; 
    //public GameObject[] onQueueCostumers = new GameObject[0]; 
    private GameObject[] appleMachines = new GameObject[0];
    private GameObject[] orangeMachines = new GameObject[0];
    private GameObject[] frozenMachines = new GameObject[0];
    private GameObject[] workers = new GameObject[0]; 
    private GameObject[] costumers = new GameObject[0];
    private GameObject[] handledCostumers = new GameObject[0];
    public GameObject[] waitingCostumers = new GameObject[0];
    private bool soundState = true;
    private bool vibrationState = true;
    private bool noResource = false;
    public GameObject upgradingMachine;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("IdleLevel", startLevel);
        resourceCount = PlayerPrefs.GetInt("cupCount", 0);
        moneyCount = PlayerPrefs.GetFloat("moneyCount", 0);
        InitIdleScene();
        FillMachinesArray();
        FillWorkersArray();
        SetCostumerPlaces(false);
        costumerCount = costumersParent.transform.childCount;
        SetAvailableWorkers();

        //postProcessVolume.profile.components.ForEach(c => Debug.Log(c.GetType().Name));

        tutorial = PlayerPrefs.GetInt("tutorial", 0) == 0 ? true : false;

        if (tutorial)
        {
            StartTutorial();
        }
        else
        {
            SupersonicWisdom.Api.NotifyLevelStarted(currentLevel, null, "idle");
            InvokeRepeating("SpawnCostumer", 1, 1);
            Time.timeScale = 1;
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("runnerRestarted", 0);
    }

    void StartTutorial()
    {
        //ResetMachineLevels("all");
        Time.timeScale = 0;

        tutorialPanel.transform.GetChild(0).gameObject.SetActive(true);
        tutorialPanel.transform.GetChild(1).gameObject.SetActive(false);
        tutorialPanel.transform.GetChild(2).gameObject.SetActive(false);

        tutorialPanel.SetActive(true);
    }

    public void SecondTutorial()
    {
        tutorialPanel.transform.GetChild(0).gameObject.SetActive(false);
        tutorialPanel.transform.GetChild(1).gameObject.SetActive(true);
        tutorialPanel.transform.GetChild(2).gameObject.SetActive(false);
    }
    public void ThirdTutorial()
    {
        tutorialPanel.transform.GetChild(0).gameObject.SetActive(false);
        tutorialPanel.transform.GetChild(1).gameObject.SetActive(false);
        tutorialPanel.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void FinishTutorial()
    {
        SupersonicWisdom.Api.NotifyLevelStarted(currentLevel, null, "idle");
        tutorialPanel.SetActive(false);
        tutorialPanel.transform.GetChild(0).gameObject.SetActive(false);
        tutorialPanel.transform.GetChild(1).gameObject.SetActive(false);
        tutorialPanel.transform.GetChild(2).gameObject.SetActive(false);

        tutorial = false;
        PlayerPrefs.SetInt("tutorial", 1);
        Time.timeScale = 1;
        InvokeRepeating("SpawnCostumer", 1, 1);
    }
     
    void InitIdleScene()
    {
        currentMaxMachineLevel = maxMachineLevels[currentLevel];

        soundState = PlayerPrefs.GetInt("soundState", 1) == 1 ? true : false; 
        SetVolumes();

        vibrationState = PlayerPrefs.GetInt("vibrationState", 1) == 1 ? true : false;
        SetVibrations();

        maxCostumerCount = PlayerPrefs.GetInt("maxCostumerCount", defMaxCostumerCount); 

        GameObject scene = Instantiate(levels[currentLevel], GameObject.Find("Levels").transform);
        machinesParent = scene.transform.GetChild(0).gameObject;

        for(int i = 0; i < upgradesForLevels.Length; i++)
        {
            //Debug.Log(currentLevel + " - " + i);
            if(i == currentLevel)
            {
                upgradesForLevels[i].SetActive(true);
                foreach(Transform upg in upgradesForLevels[i].transform)
                {
                    bool act = PlayerPrefs.GetInt(upg.name, 1) == 1 ? true : false;
                    upg.gameObject.SetActive(act);
                }
                upgradesForLevels[i].GetComponent<UpgradeSc>().OrderUpgrades(); 
            }
            else
            {
                upgradesForLevels[i].SetActive(false);
            }
        }

        //SetPrepareDurations();
        SetWorkers();
        SetCupCount(0);
        SetMoneyCount(0);
    }

    private void Update()
    {
        InputController();
    } 

    void InputController()
    { 

        /*if(Input.GetMouseButtonDown(0)*//* && !EventSystem.current.IsPointerOverGameObject()*//*)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Open machine panel
            if (Physics.Raycast(ray, out hit, 100, machinesLayerMask))
            {
                if(machinePanel.activeSelf)
                {
                    CloseMachinePanel();
                }

                hit.collider.gameObject.GetComponent<MachineSc>().OpenMachinePanel(true);
                upgradingMachine = hit.collider.gameObject;
            }
        } */

        // Restart the game when the "R" key is pressed 
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public Texture GetIcon(String product, int lv)
    {
        Texture icon = null;

        if(product == "apple")
        {
            if (lv < firstMachineUpgradeLevel)
            {
                icon = apple1;
            }
            else if (lv >= firstMachineUpgradeLevel && lv < secondMachineUpgradeLevel)
            {
                icon = apple2;
            }
            else if (lv >= secondMachineUpgradeLevel)
            {
                icon = apple3;
            }
        }
        else if (product == "orange")
        {
            if (lv < firstMachineUpgradeLevel)
            {
                icon = orange1;
            }
            else if (lv >= firstMachineUpgradeLevel && lv < secondMachineUpgradeLevel)
            {
                icon = orange2;
            }
            else if (lv >= secondMachineUpgradeLevel)
            {
                icon = orange3;
            }
        }
        else if (product == "frozen")
        {
            if (lv < firstMachineUpgradeLevel)
            {
                icon = frozen1;
            }
            else if (lv >= firstMachineUpgradeLevel && lv < secondMachineUpgradeLevel)
            {
                icon = frozen2;
            }
            else if (lv >= secondMachineUpgradeLevel)
            {
                icon = frozen3;
            }
        }

        return icon;
    }

    public void CheckForNextLevel()
    {
        bool available1 = true;
        foreach (GameObject machineObj in machines)
        {
            available1 = machineObj.GetComponent<MachineSc>().isMaxLevel ? true : false; 
            if (!available1)
            {
                break; 
            }
        }


        bool available2 = true;
        for (int i = 0; i < upgradesForLevels.Length; i++)
        {
            if (i == currentLevel)
            {
                foreach (Transform upg in upgradesForLevels[i].transform)
                {
                    upg.transform.Find("Button").GetComponent<Button>().interactable = CheckForMoneyCount(upg.gameObject.GetComponent<IdleUpgradesSc>().upgradeCost);
                    available2 = !upg.gameObject.activeSelf;
                    if (!available2)
                    {
                        break;
                    }
                }
            }
        }

        if (available1 && available2)
        { 
            if (currentLevel == 1)
            {
                nextLevelButton.GetComponent<Button>().interactable = false;
                nextLevelButton.Find("Icon").gameObject.SetActive(false);
                nextLevelButton.GetComponent<Image>().color = Color.green;
                nextLevelButton.Find("Text").GetComponent<Text>().text = "Next Level SOON!";


            }
            else
            {
                nextLevelButton.GetComponent<Button>().interactable = true;
                nextLevelButton.Find("Icon").gameObject.SetActive(false);
                nextLevelButton.GetComponent<Image>().color = Color.green;
                nextLevelButton.Find("Text").GetComponent<Text>().text = "Next Level";
            }
        }
        else
        {
            nextLevelButton.GetComponent<Button>().interactable = false;
            nextLevelButton.Find("Icon").gameObject.SetActive(true);
            nextLevelButton.GetComponent<Image>().color = Color.red;
            nextLevelButton.Find("Text").GetComponent<Text>().text = "Next Level";
        }


    }

    //For prefab passing
    /*public void GoNextLevel()
    {
        currentLevel = PlayerPrefs.GetInt("IdleLevel", startLevel) + 1;
        PlayerPrefs.SetInt("IdleLevel", currentLevel);
        ResetUpgrades();
        ResetMachineLevels("all");

        moneyCount = 0;
        PlayerPrefs.SetFloat("moneyCount", moneyCount);
        moneyCountTx.text = ConvertNumberToUIText(moneyCount);

        Instantiate(idlePrefab);
        Destroy(transform.parent.gameObject);
    }*/

    //For scene passing
    public void GoNextLevel()
    {
        SupersonicWisdom.Api.NotifyLevelCompleted(currentLevel, null, "idle");
        currentLevel = PlayerPrefs.GetInt("IdleLevel", startLevel) + 1;
        PlayerPrefs.SetInt("IdleLevel", currentLevel);
        ResetUpgrades();
        ResetMachineLevels("all");

        moneyCount = 0;
        PlayerPrefs.SetFloat("moneyCount", moneyCount);
        moneyCountTx.text = ConvertNumberToUIText(moneyCount);

        Restart();
    }

    public void ResetUpgrades()
    {
        for (int i = 0; i < upgradesForLevels.Length; i++)
        {
            foreach (Transform upg in upgradesForLevels[i].transform)
            {
                PlayerPrefs.SetInt(upg.name, 1);
                upg.gameObject.SetActive(true);
            }
            if (i == currentLevel)
            {
                upgradesForLevels[i].SetActive(true);
                upgradesForLevels[i].GetComponent<UpgradeSc>().OrderUpgrades();
            }
            else
            {
                upgradesForLevels[i].SetActive(false);
            }
        }

        SetWorkerSpeed(true, 1);
        SetMachineSpeed(true, "a", 1);
        SetMaxCostumerCount(true, 0);
        SetWorkerCount(true);
    }

    public void StartFromFirstLevel()
    {
        PlayerPrefs.SetInt("IdleLevel", startLevel);
        PlayerPrefs.SetInt("tutorial", 0);
        ResetUpgrades();
        ResetMachineLevels("all");
        Restart();
    }

    public void SetCostumerPlaces(bool isNew)
    {
        if (isNew)
        {
            costumerPlaces.Add(costumerLastPoint.position - (Vector3.right * 1.5f * (maxCostumerCount)), false);
        }
        else
        {
            for (int i = 0; i < maxCostumerCount; i++)
            {
                costumerPlaces.Add(costumerLastPoint.position - (Vector3.right * 1.5f * i), false);
            }
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
        cupCountTx.text = resourceCount.ToString();
    }
    public void ResetCupCount()
    {
        resourceCount = 0;
        PlayerPrefs.SetInt("cupCount", resourceCount);
        cupCountTx.text = resourceCount.ToString();
    }

    public void SetMoneyCount(float count) 
    {
        moneyCount += count;
        PlayerPrefs.SetFloat("moneyCount", moneyCount);
        moneyCountTx.text = ConvertNumberToUIText(moneyCount);
        //Debug.Log(count + "harcandý.");
        if(machinePanel.activeSelf )
        {
            bool act = !upgradingMachine.GetComponent<MachineSc>().isMaxLevel && CheckForMoneyCount(upgradingMachine.gameObject.GetComponent<MachineSc>().UpgradeCost(false));
            machinePanel.transform.Find("LevelButton").GetComponent<Button>().interactable = act;
            machinePanel.transform.Find("LevelButton").Find("Icon").gameObject.SetActive(act);
        }

        if(upgradesPanel.activeSelf)
        {
            for (int i = 0; i < upgradesForLevels.Length; i++)
            {
                if (i == currentLevel)
                {
                    foreach (Transform upg in upgradesForLevels[i].transform)
                    {
                        //bool act = PlayerPrefs.GetInt(upg.name, 1) == 1 ? true : false;
                        upg.transform.Find("Button").GetComponent<Button>().interactable = CheckForMoneyCount(upg.gameObject.GetComponent<IdleUpgradesSc>().upgradeCost);
                    }
                }
            }
        }

        foreach(GameObject mac in machines)
        {
            mac.GetComponent<MachineSc>().IfUpgradable();
        }

        for (int i = 0; i < upgradesForLevels.Length; i++)
        {
            if (i == currentLevel)
            {
                upgradesPanel.transform.parent.Find("UpgradesButton").Find("UpgIcon").gameObject.SetActive(false);
                foreach (Transform upg in upgradesForLevels[i].transform)
                {
                    if (CheckForMoneyCount(upg.GetComponent<IdleUpgradesSc>().upgradeCost) && upg.gameObject.activeSelf)
                    {
                        upgradesPanel.transform.parent.Find("UpgradesButton").Find("UpgIcon").gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }


    }

    public bool CheckForMoneyCount(float amount)
    {
        return amount <= moneyCount;
    }

    public String ConvertNumberToUIText(float number)
    {
        String UITx = ">B";
        float operatedNumber;
        if (number > 1000000000000) // Trillion
        {
            operatedNumber = (int)(number / 100000000000);
            UITx = (operatedNumber / 10).ToString() + "B";
        }
        else if (number > 1000000000) // Billion
        {
            operatedNumber = (int)(number / 100000000);
            UITx = (operatedNumber / 10).ToString() + "B";
        }
        else if (number > 1000000) // Million
        {
            operatedNumber = (int)(number / 100000);
            UITx = (operatedNumber / 10).ToString() + "M";
        }
        else if (number > 1000) // Thousand
        {
            operatedNumber = (int)(number / 100);
            UITx = (operatedNumber / 10).ToString() + "K";
        }
        else
        {
            operatedNumber = (int)(number * 10);
            UITx = (operatedNumber / 10).ToString();
        }
        return UITx;
    }

    public void AddCredit(float a) 
    {
        SetMoneyCount(a);
    }
    public void AddCup(int a)
    {
        SetCupCount(a);
    }

    public void CloseMachinePanel(GameObject upgMac) 
    {
        upgradingMachine = upgMac;
        if (machinePanel.activeSelf) 
        {
            Button lvlButton = machinePanel.transform.Find("LevelButton").GetComponent<Button>();
            lvlButton.onClick.RemoveAllListeners();
            machinePanel.SetActive(false);

        }
        if (upgMac != null && !noResource && !tutorial)
        {
            upgMac.GetComponent<MachineSc>().OpenMachinePanel();
        }
    }
    public void CloseMachinePanelUI()
    {
        upgradingMachine = null;
        if (machinePanel.activeSelf)
        {
            Button lvlButton = machinePanel.transform.Find("LevelButton").GetComponent<Button>();
            lvlButton.onClick.RemoveAllListeners();
            machinePanel.SetActive(false);

        }
    }

    public void SpawnCostumer()
    {
        if(costumerCount < maxCostumerCount && resourceCount > 0)
        {
            costumerCount++;
            GameObject spawned = UnityEngine.Random.Range(1, 100) < 50 ? costumer1 : costumer2;
            GameObject newCostumer = Instantiate(spawned, costumerSpawnPoint.position, Quaternion.identity, costumersParent.transform);
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
        costumers = RemoveFromCustomArray(costumers, sentCostumer);
        //RemoveFromQueue(sentCostumer);
        if (costumerCount <= 0 && resourceCount <= 0)
        {
            //SupersonicWisdom.Api.NotifyLevelFailed(currentLevel, null);
            noResource = true;
            CloseMachinePanel(null);
            upgradesPanel.SetActive(false);
            outOfResourcePanel.SetActive(true);
        }
    }

    public void CashAnimation(GameObject costumer, float inc)
    {
        Vector3 costumerScreenPosition = Camera.main.WorldToScreenPoint(costumer.transform.position);
        GameObject animCash = Instantiate(cashAnimUI, costumerScreenPosition, Quaternion.identity, moneyCountTx.transform.parent);
        animCash.GetComponent<CashAnimUI>().SpawnCashAnim(ConvertNumberToUIText(inc));
    }

    /*public void EditOrder()
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
    }*/

    /*public void AddToQueue(GameObject addedCostumer)
    {
        onQueueCostumers = AddToCustomArray(onQueueCostumers, addedCostumer);
    }
    public void RemoveFromQueue(GameObject addedCostumer)
    {
        onQueueCostumers = RemoveFromCustomArray(onQueueCostumers, addedCostumer);
    }*/

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
                //Debug.Log(costumer + " is waiting for worker.");

                if (costumer.GetComponent<CostumerSc>().isWaiting == false)
                {
                    costumer.GetComponent<CostumerSc>().isWaiting = true;
                    waitingCostumers = AddToCustomArray(waitingCostumers, costumer);
                }
            }
            // NO ready machine && available worker
            else if (!CheckForReadyMachine(productName) && (availableWorkers.Length > 0))
            {
                //Debug.Log(costumer + " is waiting for machine.");

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
                waiting.GetComponent<CostumerSc>().TakeAndGo(false, null);
                waitingCostumers = RemoveFromCustomArray(waitingCostumers, waiting);
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
        //EditOrder();
    }

    private bool CheckForReadyMachine(string obj)
    {
        foreach (GameObject machine in readyMachines)
        {
            if (machine.CompareTag(obj + "Machine") && !machine.GetComponent<MachineSc>().hasOwner)
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
                ////Debug.Log("____No available worker.___");
            }
        }

        return null;
    }
    private GameObject AvailableMachine(string machineProduct)
    {
        foreach (GameObject machine in readyMachines)
        {
            if (machine.CompareTag(machineProduct + "Machine") && !machine.GetComponent<MachineSc>().hasOwner)
            {
                //availableMachine = machine;
                machine.GetComponent<MachineSc>().hasOwner = true;
                return machine;
            }
            else
            {
                ////Debug.Log("Next machine.");
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
            ////Debug.Log("Null obj is tried to add to the array");
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

        ////Debug.Log("Object is added to the array");

        return newArray;
    }
    private GameObject[] RemoveFromCustomArray(GameObject[] originalArray, GameObject removedObj)
    {
        if (removedObj == null)
        {
            ////Debug.Log("Null obj is tried to remove from the array");
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
            ////Debug.Log("Object not found in the array");
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

        ////Debug.Log("Object is removed from the array");

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
                case "frozenMachine":
                    frozenMachines = AddToCustomArray(frozenMachines, machines[i]);
                    break;
            }
        }

        //Debug.Log("Machines array filled");
    }

    public int GetMachineCount(string tag)
    {
        int count = 0;
        foreach (GameObject machineObj in machines)
        {
            count += machineObj.CompareTag(tag) ? 1 : 0;
        }
        return count;
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
                    if(count == 1 && machine.tag == "appleMachine")
                    {
                        level = PlayerPrefs.GetInt((machine.tag + count + "Level"), 1);
                    }
                    else
                    {
                        level = PlayerPrefs.GetInt((machine.tag + count + "Level"), 0);
                    }
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
                    if (count == 1 && machine.tag == "appleMachine")
                    {
                        level = PlayerPrefs.GetInt((machine.tag + count + "Level"), 1) + 1;
                        PlayerPrefs.SetInt((machine.tag + count + "Level"), level);
                    }
                    else
                    {
                        level = PlayerPrefs.GetInt((machine.tag + count + "Level"), 0) + 1;
                        PlayerPrefs.SetInt((machine.tag + count + "Level"), level);
                    }
                }
            }
        }
    }

    public void ResetMachineLevels(string machineTag)
    {
        if(machineTag != "all")
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
        }
        else
        {
            machineTag = "appleMachine";
            for (int i = 1; i < 4; i++)
            {
                if (machineTag == "appleMachine" && i == 1)
                {
                    PlayerPrefs.SetInt((machineTag + i + "Level"), 1);
                    i++;
                }
                PlayerPrefs.SetInt((machineTag + i + "Level"), 0);
            }
            machineTag = "orangeMachine";
            for (int i = 1; i < 4; i++)
            {
                PlayerPrefs.SetInt((machineTag + i + "Level"), 0);
            }
            machineTag = "frozenMachine";
            for (int i = 1; i < 4; i++)
            {
                PlayerPrefs.SetInt((machineTag + i + "Level"), 0);
            }
        }
        foreach (GameObject machineObj in machines)
        {
            machineObj.GetComponent<MachineSc>().InitMachine(false);
        }
    }

    private void FillWorkersArray()
    {
        workers = new GameObject[workersParent.transform.childCount];

        for (int i = 0; i < workers.Length; i++)
        {
            ////Debug.Log(workersParent.transform.GetChild(i).gameObject + " is added.");
            workers[i] = workersParent.transform.GetChild(i).gameObject;
        }

        workerSpeed = PlayerPrefs.GetFloat("workerSpeed", workerDefSpeed);
        SetWorkerSpeed(false, 1);
    }
    public void SetSound()
    {
        soundState = !soundState;
        int a = soundState ? 1 : 0;
        settingsPanel.transform.Find("SoundUI").Find("Stroke").gameObject.SetActive(!soundState);
        PlayerPrefs.SetInt("soundState", a);
        SetVolumes();
    }
    public void SetVolumes()
    {
        //Debug.Log("Sound button is pressed.");
        float soundLevel = soundState ? 0.5f : 0f;
        //audioManager.SetVolume(soundLevel);
    }
    public void SetVibration()
    {
        vibrationState = !vibrationState;
        //Vibrate();
        int a = vibrationState ? 1 : 0;
        //Debug.Log("Vibration button is pressed.");
        settingsPanel.transform.Find("VibrationUI").Find("Stroke").gameObject.SetActive(!vibrationState);
        PlayerPrefs.SetInt("vibrationState", a);
        SetVibrations();
    }
    public void SettingsPanel()
    {
        bool v = settingsPanel.activeSelf;
        
        Time.timeScale = v ? 1f : 0f;

        settingsPanel.SetActive(!v);

        soundState = PlayerPrefs.GetInt("soundState") == 1 ? true : false;
        vibrationState = PlayerPrefs.GetInt("vibrationState") == 1 ? true : false;

        settingsPanel.transform.Find("VibrationUI").Find("Stroke").gameObject.SetActive(!vibrationState);
        settingsPanel.transform.Find("SoundUI").Find("Stroke").gameObject.SetActive(!soundState);
    }
     
    public void SetVibrations()
    {
        //vibrationBut.color = vibrationState ? Color.white : Color.red;
        //vibrationBut.transform.parent.Find("Text").GetComponent<Text>().text = vibrationState ? "On" : "Off";
    }

    public Texture SetTexture(string product)
    {
        switch (product)
        {
            case "apple":
                return appleIcon;
            case "orange":
                return orangeIcon;
            case "frozen":
                return frozenIcon;

            case "happy":
                return happyIcon;
            case "unHappy":
                return warningIcon;
        }
        return warningIcon;
    }

    public void UpgradesPanel(bool openPanel) 
    {
        if (machinePanel.activeSelf && openPanel)
        {
            CloseMachinePanel(null);
        }

        if (!noResource && !tutorial)
        {
            upgradesPanel.SetActive(openPanel);
            if (openPanel)
            {
                for (int i = 0; i < upgradesForLevels.Length; i++)
                {
                    if (i == currentLevel)
                    {
                        foreach (Transform upg in upgradesForLevels[i].transform)
                        {
                            //bool act = PlayerPrefs.GetInt(upg.name, 1) == 1 ? true : false;
                            upg.transform.Find("Button").GetComponent<Button>().interactable = CheckForMoneyCount(upg.gameObject.GetComponent<IdleUpgradesSc>().upgradeCost);
                        }
                    }
                }
            }
        }
    }

    public void IncreaseWorkerSpeed(float mult)
    {
        SetWorkerSpeed(false, mult);
    }

    public void IncreaseAppleSpeed(float devider)
    {
        SetMachineSpeed(false, "appleMachine", devider);
    }
    public void IncreaseOrangeSpeed(float devider)
    {
        SetMachineSpeed(false, "orangeMachine", devider);
    }
    public void IncreaseFrozenSpeed(float devider)
    {
        SetMachineSpeed(false, "frozenMachine", devider);
    }

    public void SetWorkerSpeed(bool reset, float multiplier)
    {
        workerSpeed = reset ? workerDefSpeed : workerSpeed * multiplier;
        PlayerPrefs.SetFloat("workerSpeed", workerSpeed);
        foreach (GameObject worker in workers)
        {
            worker.GetComponent<WorkerSc>().speed = workerSpeed;
        }
    }

    public void SetMachineSpeed(bool reset, String tag, float devider)
    {
        if(!reset)
        {
            switch (tag)
            {
                case "appleMachine":
                    appleSpeedDevider /= devider;
                    PlayerPrefs.SetFloat("appleSpeedDevider", appleSpeedDevider);
                    break;
                case "orangeMachine":
                    orangeSpeedDevider /= devider;
                    PlayerPrefs.SetFloat("orangeSpeedDevider", orangeSpeedDevider);
                    break;
                case "frozenMachine":
                    frozenSpeedDevider /= devider;
                    PlayerPrefs.SetFloat("frozenSpeedDevider", frozenSpeedDevider);
                    break;
            }
        }
        else
        {
            PlayerPrefs.SetFloat("appleSpeedDevider", 1);
            PlayerPrefs.SetFloat("orangeSpeedDevider", 1);
            PlayerPrefs.SetFloat("frozenSpeedDevider", 1); 
        }
        SetPrepareDurations();
    }

    public void SetPrepareDurations()
    {
        appleSpeedDevider = PlayerPrefs.GetFloat("appleSpeedDevider", 1);
        orangeSpeedDevider = PlayerPrefs.GetFloat("orangeSpeedDevider", 1);
        frozenSpeedDevider = PlayerPrefs.GetFloat("frozenSpeedDevider", 1);
        foreach (GameObject machineObj in machines)
        {
            switch (machineObj.tag)
            {
                case "appleMachine":
                    machineObj.GetComponent<MachineSc>().prepareDuration = appleDefDuration * appleSpeedDevider;
                    break;
                case "orangeMachine":
                    machineObj.GetComponent<MachineSc>().prepareDuration = orangeDefDuration * orangeSpeedDevider;
                    break;
                case "frozenMachine":
                    machineObj.GetComponent<MachineSc>().prepareDuration = frozenDefDuration * frozenSpeedDevider;
                    break;
            }
        }
        if(machinePanel.activeSelf)
        {
            machinePanel.transform.Find("Duration").Find("Text").GetComponent<Text>().text = upgradingMachine.GetComponent<MachineSc>().prepareDuration.ToString();
        }
    }

    public void AddCostumer(int add)
    {
        SetMaxCostumerCount(false, add);
    }

    public void SetMaxCostumerCount(bool reset, int add)
    {
        maxCostumerCount = reset ? defMaxCostumerCount : maxCostumerCount + add;
        PlayerPrefs.SetInt("maxCostumerCount", maxCostumerCount);
        if(!reset)
        {
            SetCostumerPlaces(true);
        }
    }

    public void IncreaseWorkerCount()
    {
        int wrk = PlayerPrefs.GetInt("workerCount", 1);
        PlayerPrefs.SetInt("workerCount", wrk + 1);
        SetWorkers();
    }

    public void SetWorkers()
    {
        workerCount = workersParent.transform.childCount;
        if(workerCount < PlayerPrefs.GetInt("workerCount", 1))
        {
            SetWorkerCount(false);
        }
    }

    public void SetWorkerCount(bool reset)
    {
        if (!reset)
        {
            Instantiate(sideWorker, workerSpawnPoint.position, Quaternion.identity, workersParent.transform);
        }
        else
        {
            GameObject[] deletedWorkers = new GameObject[workers.Length-1];
            for(int i = 0; i<workers.Length; i++)  
            {
                if (i > 0)
                {
                    AddToCustomArray(deletedWorkers, workers[i]);
                }
            }
            foreach(GameObject worker in deletedWorkers)
            {
                Destroy(worker);
            }
            PlayerPrefs.SetInt("workerCount", 1);
        }
        FillWorkersArray();
        SetAvailableWorkers(); 

        workerSpeed = PlayerPrefs.GetFloat("workerSpeed", workerDefSpeed);
        SetWorkerSpeed(false, 1);
        SetWorkers();
    }

    public void LoadScene(int sceneIndex)
    {
        PlayerPrefs.SetInt("runnerRestarted", 1);
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadRunner()
    {
        Instantiate(runnerPrefab);
        Destroy(transform.parent.gameObject);
    }

    // Reload the current scene to restart the game 
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
