using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineSc : MonoBehaviour
{
    // 0 No resource, 1 Preparing, 2 Ready
    public int status = 0;
    public float shakePower = 8;
    public float shakeSpeed = 10;
    public string product = "apple";
    public GameObject productObject;
    public float prepareDuration = 1;
    public float resourceCount = 0;
    public Color readyColor, unreadyColor;
    public int machineLevel = 1;
    public float productPrice = 1;
    public float firstLevelCost = 2;
    public float levelCostConstan = 1.2571f;
    public float firstLevelPrice = 6;
    public float levelPriceConstan = 1.09f;
    public bool hasOwner = false;
    public bool isMaxLevel = false;

    private Texture icon;
    private float timer = 0;
    private Image fillImage;
    private GameObject statuIcon;
    private IdleManager idleManager;

    // Start is called before the first frame update
    void Start()
    {
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
        fillImage = transform.Find("MachineCanvas").Find("BG").GetComponent<Image>();
        statuIcon = transform.Find("MachineCanvas").Find("Statu").gameObject;
        InitMachine(false);
        SetMachineObjAtStart();
        PrepareTest();
    } 

    public void InitMachine(bool exist)
    {
        machineLevel = idleManager.GetMachineLevel(gameObject);
        CheckMaxLevel();
        SetProductPrice();
        SetMachineObject();
            /*Transform obj = transform.Find("Obj");
            if (machineLevel == 0 )
            {
                obj.localPosition = Vector3.up * (-0.25f);
                obj.localScale = Vector3.one - Vector3.up * (0.5f);
                obj.GetComponent<Renderer>().material = idleManager.defMachineMat;
            }
            else
            {
                obj.localPosition = Vector3.zero;
                obj.localScale = Vector3.one;
                if(gameObject.tag == "appleMachine")
                {
                    obj.GetComponent<Renderer>().material = idleManager.appleMachineMat;
                }
                else if (gameObject.tag == "orangeMachine")
                {
                    obj.GetComponent<Renderer>().material = idleManager.orangeMachineMat;
                }
            }*/
        if (!exist || machineLevel == 1)
        {
            PrepareTest();
        }
    }

    public void SetMachineObject()
    {
        if(machineLevel < idleManager.firstMachineUpgradeLevel && !transform.Find("MachineObj").Find("V0").gameObject.active)
        {
            foreach (Transform mc in transform.Find("MachineObj"))
            {
                mc.gameObject.SetActive(false);
            }
            transform.Find("MachineObj").Find("V0").gameObject.SetActive(true);
        }
        else if(machineLevel == idleManager.firstMachineUpgradeLevel)
        {
            foreach(Transform mc in transform.Find("MachineObj"))
            {
                mc.gameObject.SetActive(false);
            }
            transform.Find("MachineObj").Find("V1").gameObject.SetActive(true);
        }
        else if (machineLevel == idleManager.secondMachineUpgradeLevel)
        {
            foreach (Transform mc in transform.Find("MachineObj"))
            {
                mc.gameObject.SetActive(false);
            }
            transform.Find("MachineObj").Find("V2").gameObject.SetActive(true);
        }
    }

    public void SetMachineObjAtStart()
    {
        foreach (Transform mc in transform.Find("MachineObj"))
        {
            mc.gameObject.SetActive(false);
        }

        if(machineLevel < idleManager.firstMachineUpgradeLevel)
        {
            transform.Find("MachineObj").Find("V0").gameObject.SetActive(true);
        }
        else if (machineLevel >= idleManager.firstMachineUpgradeLevel && machineLevel<idleManager.secondMachineUpgradeLevel)
        {
            transform.Find("MachineObj").Find("V1").gameObject.SetActive(true);
        }
        else if (machineLevel >= idleManager.secondMachineUpgradeLevel && machineLevel < idleManager.thirdMachineUpgradeLevel)
        {
            transform.Find("MachineObj").Find("V2").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("MachineObj").Find("V2").gameObject.SetActive(true);
        }
    }

    public void OpenMachinePanel(bool open)
    {
        Transform panel = idleManager.machinePanel.transform;
        if (open)
        {
            panel.Find("NameTX").GetComponent<Text>().text = product + " Machine";
            machineLevel = idleManager.GetMachineLevel(gameObject);
            panel.Find("LevelTX").GetComponent<Text>().text = "Level " + machineLevel.ToString();

            switch (gameObject.tag)
            {
                case "appleMachine":
                    icon = idleManager.appleIcon;
                    break;
                case "orangeMachine":
                    icon = idleManager.orangeIcon;
                    break;
                case "frozenMachine":
                    icon = idleManager.frozenIcon;
                    break;

            }
            panel.Find("ProductIcon").GetComponent<RawImage>().texture = icon;

            Button lvButton = panel.Find("LevelButton").GetComponent<Button>();
            lvButton.onClick.AddListener(() => IncreaseMachineLevel(1));
            panel.Find("UpgradeCostTx").GetComponent<Text>().text = "Cost: " + ((int)UpgradeCost()).ToString() + "$";

            idleManager.machinePanel.SetActive(true);
        }
        CheckMaxLevel();
    }

    public void IncreaseMachineLevel(int addLevel)
    {
        Destroy(Instantiate(idleManager.upgradeMachineParticle, transform.position + Vector3.up, Quaternion.Euler(Vector3.right*-90)), 1);
        idleManager.IncreaseMachineLevel(gameObject);
        idleManager.SetMoneyCount(-UpgradeCost());
        SetProductPrice();
        machineLevel = idleManager.GetMachineLevel(gameObject);

        idleManager.machinePanel.transform.Find("LevelTX").GetComponent<Text>().text = "Level " + machineLevel.ToString();

        idleManager.machinePanel.transform.Find("UpgradeCostTx").GetComponent<Text>().text = "Cost: " + ((int)UpgradeCost()).ToString() + "$";
        InitMachine(true);
        CheckMaxLevel();
    }

    public void CheckMaxLevel()
    {
        Transform panel = idleManager.machinePanel.transform;
        isMaxLevel = machineLevel >= idleManager.currentMaxMachineLevel;
        if (isMaxLevel)
        {
            panel.Find("LevelButton").Find("Text").GetComponent<Text>().text = "Max Lv.";
            panel.Find("LevelButton").GetComponent<Button>().interactable = false;
            idleManager.CheckForNextLevel();
        }
        else
        { 
            panel.Find("LevelButton").Find("Text").GetComponent<Text>().text = "UPGRADE";
            panel.Find("LevelButton").GetComponent<Button>().interactable = true;
        }
    }

    float UpgradeCost()
    {
        //Debug.Log(idleManager.GetMachineCount(gameObject.tag));
        return firstLevelCost * MathF.Pow(levelCostConstan, (machineLevel + 1)) / idleManager.GetMachineCount(gameObject.tag);
    }

    void SetProductPrice()
    {
        productPrice = firstLevelPrice * (MathF.Pow(levelPriceConstan, machineLevel));
        if(machineLevel >= 10 && machineLevel<25)
        {
            productPrice *= 2;
        }
        else if (machineLevel >= 25)
        {
            productPrice *= 4;
        }
        productPrice = (int)productPrice;
    }

    public void PrepareTest()
    {
        if(status != 1)
        {
            //idleManager.SetCupCount(1);
            //resourceCount++;
            SetStatus(1);
        }
    }

    public void SetStatus(int stat)
    {
        if(product != "frozen")
        {
            Vector3 tempRot = transform.Find("MachineObj").eulerAngles;
            tempRot.x = 0;
            transform.Find("MachineObj").eulerAngles = tempRot;
        }

        if (machineLevel <= 0)
        {
            status = -1;
            
            statuIcon.SetActive(false);
            fillImage.color = Color.gray;
        }
        else
        {
            status = stat;
            switch (status)
            {
                case 0: // No resource
                    statuIcon.SetActive(false);

                    fillImage.color = unreadyColor;
                    break;

                case 1: // Preparing
                    statuIcon.SetActive(false);

                    timer = 0;
                    idleManager.SetCupCount(-1);
                    //idleManager.resourceCount--;
                    InvokeRepeating("PrepareProductLoop", 0, Time.fixedDeltaTime);

                    break;

                case 2: // Ready
                    statuIcon.SetActive(true);
                    fillImage.color = readyColor;
                    break;
            }
        }

        //Debug.Log("before SetReadyMachines");
        idleManager.SetReadyMachines();
    }

    private void PrepareProductLoop()
    {
        timer += Time.deltaTime;
        float x = timer / prepareDuration;

        if (product != "frozen")
        {
            Vector3 tempRot = transform.Find("MachineObj").eulerAngles;
            tempRot.x = Mathf.PingPong(timer * shakeSpeed, shakePower) - shakePower / 2;
            transform.Find("MachineObj").eulerAngles = tempRot;
        }

        fillImage.fillAmount = x;
        fillImage.color = Color.Lerp(unreadyColor, readyColor, x);

        if(x >= 1 && status == 1)
        {
            if(idleManager.resourceCount <= 0)
            {
                //idleManager.resourceCount = 0;
            }
            ProductPrepared();
        }
    }

    private void ProductPrepared()
    {
        CancelInvoke("PrepareProductLoop");
        SetStatus(2);
    }
    public float TakeProduct()
    {
        hasOwner = false;
        // Take product
        if (status == 2)
        {
            // Prepare new one
            if (idleManager.resourceCount > 0)
            {
                SetStatus(1);
            }
            // No more resources
            else if (idleManager.resourceCount == 0)
            {
                //Debug.Log("Taken and no more resources.");
                SetStatus(0);
            }
        }
        else if(status == 1)
        {
            //Debug.Log("Product is being prepared.");
        }
        else if(status == 0)
        {
            //Debug.Log("No resource.");
        }
        return productPrice;
    }
    public GameObject HandleProduct()
    {
        return productObject;
    }
}
