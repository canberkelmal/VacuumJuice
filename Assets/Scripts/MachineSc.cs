using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineSc : MonoBehaviour
{
    // 0 No resource, 1 Preparing, 2 Ready
    public int status = 0;
    public string product = "apple";
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

    private Texture icon;
    private float timer = 0;
    private Image fillImage;
    private GameObject statuIcon;
    private IdleManager idleManager;

    int HesaplaKazanc(int level)
    {
        if (level >= 1 && level <= 9)
        {
            return level + 5;
        }
        else if (level >= 10 && level <= 25)
        {
            return 21 + (level - 10) * 3;
        }
        else if (level >= 26)
        {
            return 25 + (level - 25) * 7;
        }

        return 0; // Geçersiz level
    }


    int GetExpectedGain(int level)
    {
        int[] expectedGains = new int[]
        {
            6, 7, 8, 9, 10, 11, 12, 13, 14,
            31, 34, 37, 40, 43, 46, 50, 54, 59, 63, 69,
            74, 80, 86, 93, 202
        };

        if (level >= 1 && level <= 25)
        {
            return expectedGains[level - 1];
        }
        return 0; // Geçersiz level
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 25; i++)
        {
            int hesaplananKazanc = HesaplaKazanc(i);
            int beklenenKazanc = GetExpectedGain(i);
            Debug.Log($"Level {i} için hesaplanan kazanç: {hesaplananKazanc} | Beklenen: {beklenenKazanc}");
        }

        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
        fillImage = transform.Find("MachineCanvas").Find("BG").GetComponent<Image>();
        statuIcon = transform.Find("MachineCanvas").Find("Statu").gameObject;
        InitMachine();
        PrepareTest();
    } 

    public void InitMachine()
    {
        machineLevel = idleManager.GetMachineLevel(gameObject);
        SetProductPrice();
        Transform obj = transform.Find("Obj");
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
        }
        PrepareTest();
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

            }
            panel.Find("ProductIcon").GetComponent<RawImage>().texture = icon;

            Button lvButton = panel.Find("LevelButton").GetComponent<Button>();
            lvButton.onClick.AddListener(() => IncreaseMachineLevel(1));
            panel.Find("UpgradeCostTx").GetComponent<Text>().text = "Cost: " + ((int)UpgradeCost()).ToString() + "$";

            idleManager.machinePanel.SetActive(true);
        }
    }

    public void IncreaseMachineLevel(int addLevel)
    {
        idleManager.IncreaseMachineLevel(gameObject);
        idleManager.SetMoneyCount(-UpgradeCost());
        SetProductPrice();
        machineLevel = idleManager.GetMachineLevel(gameObject);

        idleManager.machinePanel.transform.Find("LevelTX").GetComponent<Text>().text = "Level " + machineLevel.ToString();

        idleManager.machinePanel.transform.Find("UpgradeCostTx").GetComponent<Text>().text = "Cost: " + ((int)UpgradeCost()).ToString() + "$";
        InitMachine();
    }

    float UpgradeCost()
    {
        return firstLevelCost * MathF.Pow(levelCostConstan, (machineLevel + 1));
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
            idleManager.SetCupCount(+1);
            //resourceCount++;
            SetStatus(1);
        }
    }

    public void SetStatus(int stat)
    {
        if(machineLevel <= 0)
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

        fillImage.fillAmount = x;
        fillImage.color = Color.Lerp(unreadyColor, readyColor, x);

        if(x >= 1 && status == 1)
        {
            if(idleManager.resourceCount <= 0)
            {
                //idleManager.resourceCount = 0;
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


}
