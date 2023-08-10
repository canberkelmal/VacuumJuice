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
        InitMachine();
        PrepareTest();
    }

    public void InitMachine()
    {
        machineLevel = idleManager.GetMachineLevel(gameObject);
        if(machineLevel == 0 )
        {
            transform.Find("Obj").localPosition = Vector3.up * (-0.25f);
            transform.Find("Obj").localScale = Vector3.one - Vector3.up * (0.5f);
        }
        else
        {
            transform.Find("Obj").localPosition = Vector3.zero;
            transform.Find("Obj").localScale = Vector3.one;
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

            }
            panel.Find("ProductIcon").GetComponent<RawImage>().texture = icon;

            Button lvButton = panel.Find("LevelButton").GetComponent<Button>();
            lvButton.onClick.AddListener(() => IncreaseMachineLevel(1));

            idleManager.machinePanel.SetActive(true);
        }
    }

    public void IncreaseMachineLevel(int addLevel)
    {
        idleManager.IncreaseMachineLevel(gameObject);
        machineLevel = idleManager.GetMachineLevel(gameObject);

        idleManager.machinePanel.transform.Find("LevelTX").GetComponent<Text>().text = "Level " + machineLevel.ToString();
        InitMachine();
    }

    public void PrepareTest()
    {
        if(status != 1)
        {
            resourceCount++;
            SetStatus(1);
        }
    }

    public void SetStatus(int stat)
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
                resourceCount--;
                InvokeRepeating("PrepareProductLoop", 0, Time.fixedDeltaTime);
                
                break;

            case 2: // Ready
                statuIcon.SetActive(true);
                fillImage.color = readyColor;
                break;
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
            if(resourceCount <= 0)
            {
                resourceCount = 0;
            }
            ProductPrepared();
            CancelInvoke("PrepareProductLoop");
        }
    }

    private void ProductPrepared()
    {
        SetStatus(2);
    }
    public void TakeProduct()
    {
        // Take product
        if (status == 2)
        {
            // Prepare new one
            if (resourceCount > 0)
            {
                SetStatus(1);
            }
            // No more resources
            else if (resourceCount == 0)
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
    }


}
