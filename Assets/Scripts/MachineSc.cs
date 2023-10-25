using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        //PrepareTest();
        IfUpgradable();

        //transform.Find("PanelParticle").gameObject.SetActive(false);
        //transform.Find("MachineObj").GetComponent<Outline>().enabled = false;
    } 

    public void InitMachine(bool exist)
    {
        machineLevel = idleManager.GetMachineLevel(gameObject);
        CheckMaxLevel();
        SetProductPrice();
        SetMachineObject();
        if (!exist || machineLevel >= 1)
        {
            PrepareTest();
        }

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
        /*
        transform.Find("MachineObj").GetComponent<Outline>().enabled = false;
        transform.Find("MachineObj").GetComponent<Outline>().enabled = true;*/
    }

    public void SetMachineObject()
    {
        if(machineLevel == 0)
        {
            foreach (Transform mc in transform.Find("MachineObj"))
            {
                mc.gameObject.SetActive(false);
            }
            transform.Find("MachineObj").Find("Lv0").gameObject.SetActive(true);
        }
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
        //transform.Find("PanelParticle").gameObject.SetActive(false);
        //transform.Find("MachineObj").GetComponent<Outline>().LoadStroke();
    }

    public void SetMachineObjAtStart()
    {
        foreach (Transform mc in transform.Find("MachineObj"))
        {
            mc.gameObject.SetActive(false);
        }

        if(machineLevel == 0)
        {
            transform.Find("MachineObj").Find("Lv0").gameObject.SetActive(true);
        }
        else if(machineLevel < idleManager.firstMachineUpgradeLevel)
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

    private void OnMouseDown()
    {
        Debug.Log("Clicked machine");
        idleManager.CloseMachinePanel(gameObject);
        //Invoke("OpenMachinePanel", 0.1f);
        //OpenMachinePanel();
    }

    public void OpenMachinePanel()
    {
        Debug.Log("openmachinepanel started");
        Transform panel = idleManager.machinePanel.transform;
        panel.gameObject.SetActive(true);


        panel.Find("NameTX").GetComponent<Text>().text = char.ToUpper(product[0]) + product.Substring(1) + " Machine";
        machineLevel = idleManager.GetMachineLevel(gameObject);
        panel.Find("LevelTX").GetComponent<Text>().text = "Level " + machineLevel.ToString();

        switch (gameObject.tag)
        {
            case "appleMachine":
                //icon = idleManager.appleIcon;
                icon = idleManager.GetIcon("apple", machineLevel);
                break;
            case "orangeMachine":
                //icon = idleManager.orangeIcon;
                icon = idleManager.GetIcon("orange", machineLevel);
                break;
            case "frozenMachine":
                //icon = idleManager.frozenIcon;
                icon = idleManager.GetIcon("frozen", machineLevel);
                break;

        }
        panel.Find("ProductIcon").GetComponent<RawImage>().texture = icon;
        panel.Find("Duration").Find("Text").GetComponent<Text>().text = prepareDuration.ToString();
        Button lvButton = panel.Find("LevelButton").GetComponent<Button>();
        lvButton.onClick.AddListener(() => IncreaseMachineLevel(1));
        //panel.Find("LevelButton").Find("UpgradeCostTx").GetComponent<Text>().text = "Cost: " + idleManager.ConvertNumberToUIText(UpgradeCost()) + "$";

        //idleManager.machinePanel.SetActive(true);
        //lvButton.interactable = idleManager.CheckForMoneyCount(UpgradeCost());

        Debug.Log("Machine can be upgraded " + idleManager.CheckForMoneyCount(UpgradeCost()) + " UpgradeCost: " + UpgradeCost() + " moneyCount:" + idleManager.moneyCount);



        //transform.Find("PanelParticle").gameObject.SetActive(true); 

        //transform.Find("MachineObj").GetComponent<Outline>().enabled = true;
        //GetComponent<Outline>().enabled = true;
        CheckMaxLevel();
        Debug.Log("openmachinepanel end");
    }

    public void IncreaseMachineLevel(int addLevel)
    {
        Debug.Log("increaselevel start");
        GameObject upgEffect = Instantiate(idleManager.upgradeMachineParticle, transform.position + Vector3.up - Vector3.forward, Quaternion.Euler(Vector3.right * -90), idleManager.transform.parent);
        Destroy(upgEffect, 1); 
        idleManager.IncreaseMachineLevel(gameObject);
        idleManager.SetMoneyCount(-UpgradeCost());
        SetProductPrice();
        machineLevel = idleManager.GetMachineLevel(gameObject);

        idleManager.machinePanel.transform.Find("LevelTX").GetComponent<Text>().text = "Level " + machineLevel.ToString();

        idleManager.machinePanel.transform.Find("LevelButton").Find("UpgradeCostTx").GetComponent<TextMeshProUGUI>().text = "<sprite=12>" + idleManager.ConvertNumberToUIText(UpgradeCost());

        //idleManager.machinePanel.transform.Find("LevelButton").Find("UpgradeCostTx").GetComponent<Text>().text = idleManager.ConvertNumberToUIText(UpgradeCost());
        //idleManager.machinePanel.transform.Find("LevelButton").Find("UpgradeCostTxShadow").GetComponent<Text>().text = idleManager.ConvertNumberToUIText(UpgradeCost());
        InitMachine(true);
        CheckMaxLevel();
         
        switch (gameObject.tag)
        {
            case "appleMachine":
                //icon = idleManager.appleIcon;
                icon = idleManager.GetIcon("apple", machineLevel);
                break;
            case "orangeMachine":
                //icon = idleManager.orangeIcon;
                icon = idleManager.GetIcon("orange", machineLevel);
                break;
            case "frozenMachine":
                //icon = idleManager.frozenIcon;
                icon = idleManager.GetIcon("frozen", machineLevel);
                break;
        }

        idleManager.machinePanel.transform.Find("ProductIcon").GetComponent<RawImage>().texture = icon;
        Debug.Log("increaselevel end");
    }

    public void CheckMaxLevel() 
    {
        Debug.Log("Check max lv startted");
        Transform panel = idleManager.machinePanel.transform;
        isMaxLevel = machineLevel >= idleManager.currentMaxMachineLevel;
        panel.Find("Revenue").Find("Text").GetComponent<TextMeshProUGUI>().text = idleManager.ConvertNumberToUIText(productPrice) + "<sprite=13>" + idleManager.ConvertNumberToUIText(NextLevelProductPrice()); 

        if (isMaxLevel)
        {
            idleManager.machinePanel.transform.Find("LevelButton").Find("UpgradeCostTx").GetComponent<TextMeshProUGUI>().text = "Max Lv.";
            panel.Find("LevelButton").GetComponent<Button>().interactable = false;
            idleManager.CheckForNextLevel();
        }
        else if (idleManager.CheckForMoneyCount(UpgradeCost()))
        {
            idleManager.machinePanel.transform.Find("LevelButton").Find("UpgradeCostTx").GetComponent<TextMeshProUGUI>().text = "<sprite=12>" + idleManager.ConvertNumberToUIText(UpgradeCost());
            panel.Find("LevelButton").GetComponent<Button>().interactable = true;
        }
        else
        {
            idleManager.machinePanel.transform.Find("LevelButton").Find("UpgradeCostTx").GetComponent<TextMeshProUGUI>().text = "<sprite=12>" + idleManager.ConvertNumberToUIText(UpgradeCost());
            panel.Find("LevelButton").GetComponent<Button>().interactable = false;
        }

        Debug.Log("Check max lv end");
    }

    public void IfUpgradable()
    {
        isMaxLevel = machineLevel >= idleManager.currentMaxMachineLevel;
        if (!isMaxLevel && idleManager.CheckForMoneyCount(UpgradeCost()))
        {
            transform.Find("MachineCanvas").Find("UpgIcon").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("MachineCanvas").Find("UpgIcon").gameObject.SetActive(false);
        }
    }

    public float UpgradeCost()
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

    public float NextLevelProductPrice()
    {
        float nextPrice = firstLevelPrice * (MathF.Pow(levelPriceConstan, machineLevel+1));
        if (machineLevel+1 >= 10 && machineLevel+1 < 25)
        {
            nextPrice *= 2;
        }
        else if (machineLevel+1 >= 25)
        {
            nextPrice *= 4;
        }
        return (int)nextPrice;
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
        bool isReady = stat == 2 ? true : false;
        transform.Find("ReadyParticle").gameObject.SetActive(isReady);
        //Debug.Log("before SetReadyMachines");
        idleManager.SetReadyMachines();
    }

    private void PrepareProductLoop()
    {
        timer += Time.fixedDeltaTime;
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
                idleManager.ResetCupCount();
            }
            ProductPrepared();
            CancelInvoke("PrepareProductLoop");
        }
    }

    private void ProductPrepared()
    {
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
