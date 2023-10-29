using System;
/*using System.Collections;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using Unity.VisualScripting;*/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    public GameObject idlePrefab;
    public GameObject player;
    public GameObject director;
    public GameObject vacuumCollider, vacuumParticle;
    public GameObject tankShader;
    public GameObject finalTankShader; 
    public Material cupInsideShader;
    public GameObject[] tankObjs;
    public GameObject cupIcon;
    public GameObject finishPanel;
    public GameObject settingsPanel;
    public GameObject buffParticle;
    public GameObject debuffParticle;
    public GameObject getJuiceParticle;
    public GameObject getPoisonParticle;
    public GameObject buffTankParticle;
    public GameObject debuffTankParticle;
    public GameObject finalTankBouy;
    public GameObject fireworks;
    public Slider rotateSensSlider;
    public Animator playerAnimator;
    public GameObject levelParent;
    public GameObject[] runnerLevels = new GameObject[1];
    public HealthBarSc healthBar;
    public int currentRunnerLevel = 1;
    public int runnerUILevel = 1;
    public int cupPerFruid = 4;
    public float finalTankLiquidMultiplier = 24;
    public float getCupSens = 1;
    public float camSensivity = 1f;
    public float finalTankCamSensivity = 1f;
    public float finalTankCamYOffs = 1f;
    public float playerRotateSens = 1;
    public float playerRotateLimit = 75;
    public float playerMoveSens = 1f;
    public float playerMaxSpeed = 1f;
    public float playerXMin, playerXMax;
    public float collectSens = 1f;
    public float shapeSens = 1f;
    public float fillTankSens = 1f;
    public float fillFinalTankSens = 1f;
    public float bouySens = 1;
    public float fillFinalTankDelay = 0.7f;
    public float rotateObjectsSens = 1f;
    public float rotateFanSens = 1f;
    public float swingObjectsSens = 1f;
    public float fillMultiplier = 0.1f;
    public float vacuumRadiusMultiplier = 0.1f;
    public float vacuumLengthMultiplier = 0.5f;
    public float tankVolumeMultiplier = 0.2f;
    public float getCupDelay = 0.25f;
    public bool isTankEmpty = true;
    public bool isTankFull = false;
    public LayerMask gatesLayerMask;
    public LineConnector pipeLine;

    //UI Elements
    public Text cupCountTx;
    public Text moneyCountTx;
    public Text levelTx;
    public Image soundBut, vibrationBut;

    [NonSerialized]
    public Color liquidColor, tempLiquidColor;
    [NonSerialized]
    public AudioManager audioManager;

    public PlayerController playerController;
    public GameObject mainCam;
    private Vector3 camOffset;
    private Vector3 camTankOffset;
    private bool controller = true;
    private bool isStarted = false;
    private bool isFinished = false;
    private bool isFinalTankFilling = false;
    private bool isEnded = false;
    private float playerTempSpeed;
    private float playerCurrentSpeed = 0;
    //private float currentRotation = 0f;
    private float directorOffsZ = 2.5f;
    private float directorOffsY = 1f;
    private float tankFillAmount = 0;
    private float finalTankFillAmount = -5;
    private float tempTankFill = 1;
    private float tempFinalTankFill = -5;
    private int cupCount = 0;
    private int tempCupCount = 0;
    private float moneyCount = 0;
    public float juiceAmount = 0;
    private int tankLevel = 1;
    private bool soundState = true;
    private bool vibrationState = true;

    /*void Awake()
    {
        mainCam = GameObject.Find("Main Camera");
        //tankShader = player.transform.Find("LiquidTank").Find("Shader").gameObject;
        camOffset = player.transform.position - mainCam.transform.position;
        directorOffsY = player.transform.position.y - director.transform.position.y;
        controller = true;
        playerTempSpeed = playerMaxSpeed;
        playerMaxSpeed = 0;

        SetCupCount(0);
        SetMoneyCount(0);
    }*/

    private void Start()
    {
        healthBar.SetFillAmount(0, false);
        controller = true;
        playerTempSpeed = playerMaxSpeed;
        playerMaxSpeed = 0;

        SetCupCount(0);
        SetMoneyCount(0);

        audioManager = FindObjectOfType<AudioManager>();
        currentRunnerLevel = PlayerPrefs.GetInt("runnerLevel", 1);
        runnerUILevel = PlayerPrefs.GetInt("runnerUILevel", 1);

        InitializeScene();

        mainCam = GameObject.Find("Main Camera");
        //tankShader = player.transform.Find("LiquidTank").Find("Shader").gameObject;
        camOffset = player.transform.position - mainCam.transform.position;
        directorOffsY = player.transform.position.y - director.transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        InputController(); 
        CameraController();
    }
    public void SetCupCount(int count)
    {
        cupCount = PlayerPrefs.GetInt("cupCount", 0);
        cupCount += count;
        cupCount = cupCount < 0 ? 0 : cupCount;
        PlayerPrefs.SetInt("cupCount", cupCount);
        cupCountTx.text = cupCount.ToString();
    }

    public void SetMoneyCount(float count)
    {
        moneyCount = PlayerPrefs.GetFloat("moneyCount", 0);
        moneyCount += count;
        PlayerPrefs.SetFloat("moneyCount", moneyCount);
        moneyCountTx.text = ConvertNumberToUIText(moneyCount);
    }

    public void IncreaseCupCount()
    {
        tempCupCount++;
        cupCount++;
        PlayerPrefs.SetInt("cupCount", cupCount);
        cupCountTx.text = cupCount.ToString();
    }

    public void EnterToFinish()
    {
        isFinished = true;
        playerController.isFinished = isFinished;
        SetController(false);
        vacuumParticle.transform.parent.Find("MagicAuraBlue").GetComponent<ParticleSystem>().startSpeed *= -1;
        vacuumParticle.transform.parent.Find("MagicAuraBlue").transform.localPosition += Vector3.forward * 0.5f;
    }

    public void SetController(bool value)
    {
        controller = value;
        playerController.controller = controller;
    }

    public void FillTank(int fillFactor)
    {
        if(fillFactor > 0)
        {
            audioManager.Play("AddJuice");
        }
        else if(fillFactor < 0)
        {
            audioManager.Play("Debuff");
        }
        if (tankShader.GetComponent<Renderer>().material.GetFloat("_Fill") < tankFillAmount)
        {
            tankShader.GetComponent<Renderer>().material.SetFloat("_Fill", tankFillAmount);
        }

        if (tankFillAmount >= (0.75f + ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)) && fillFactor>0)
        {
            Debug.Log("Tank is full!");
            healthBar.SetFillAmount(1, true);
        }
        else if (tankFillAmount <= (0.25f - ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)) && fillFactor < 0)
        {
            Debug.Log("Tank is empty!");
            healthBar.SetFillAmount(1, true);
        }
        else
        {
            juiceAmount += (fillFactor * fillMultiplier);
            RefillTank(true);
        }
    }
    public void FillACup()
    {
        audioManager.Play("FillCup");
        juiceAmount -= (fillMultiplier / cupPerFruid);
        RefillTank(true);
    }

    public void RefillTank(bool glow)
    {
        tankFillAmount = (0.25f - ((tankLevel-1) * tankVolumeMultiplier * 0.2f)) + (juiceAmount / (Mathf.Pow((1 + ((tankLevel - 1) * tankVolumeMultiplier)), 3)));
        if(tankFillAmount <= (0.25f - ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)))
        {
            healthBar.SetFillAmount(0, glow);
            tankFillAmount = 0;
            isTankEmpty = true;
            if (isFinished)
            {
                EmptyTankOnFinish();
            }
        }
        else if (tankFillAmount >= (0.75f + ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)))
        {
            healthBar.SetFillAmount(1, glow);
            tankFillAmount = (0.75f + ((tankLevel - 1) * tankVolumeMultiplier * 0.2f));
            isTankEmpty = false;
        }
        else
        {
            float borderAmount = Mathf.Lerp(0, 1, Mathf.InverseLerp((0.25f - ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)), (0.75f + ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)), tankFillAmount));
            healthBar.SetFillAmount(borderAmount, glow);
            isTankEmpty = false;
        }
        Debug.Log("Juice: " + juiceAmount + " || TankFill: " + tankFillAmount);
        InvokeRepeating("FillTankAnim", 0, Time.deltaTime);
    }
    


    private void FillTankAnim()
    {
        tempTankFill = Mathf.MoveTowards(tankShader.GetComponent<Renderer>().material.GetFloat("_Fill"), tankFillAmount, fillTankSens * Time.fixedDeltaTime);
        tankShader.GetComponent<Renderer>().material.SetFloat("_Fill", tempTankFill);

        if(tempTankFill == tankFillAmount)
        {
            CancelInvoke("FillTankAnim");
        }
    }
    public void ReachToFinalTank()
    {
        finalTankBouy.transform.localPosition = -Vector3.up;

        camTankOffset = finalTankBouy.transform.position - mainCam.transform.position - Vector3.forward*4;
        camTankOffset.y = finalTankCamYOffs;
        ChangePlayerSpeed(false);
        isFinalTankFilling = true;
        FillFinalTank();
    }

    public void FillFinalTank()
    {
        finalTankShader.GetComponent<Renderer>().material.color = liquidColor;
        finalTankShader.GetComponent<Renderer>().material.SetColor("_TopColor", liquidColor + Color.white / 5);

        isEnded = true;
        finalTankFillAmount = (-50) + (juiceAmount * finalTankLiquidMultiplier);
        InvokeRepeating("FillFinalTankAnim", fillFinalTankDelay, Time.fixedDeltaTime);
        Invoke("WobbleFinalTank", fillFinalTankDelay);

        isTankEmpty = true;
        tankFillAmount = 0;
        Debug.Log("Juice: " + juiceAmount + " || TankFill: " + tankFillAmount);
        healthBar.SetFillAmount(0, true);
        InvokeRepeating("FillTankAnim", fillFinalTankDelay, Time.fixedDeltaTime);
    }
    private void FillFinalTankAnim()
    {
        tempFinalTankFill = Mathf.MoveTowards(finalTankShader.GetComponent<Renderer>().material.GetFloat("_Fill"), finalTankFillAmount, fillFinalTankSens * Time.fixedDeltaTime);
        finalTankShader.GetComponent<Renderer>().material.SetFloat("_Fill", tempFinalTankFill);


        //finalTankBouy.transform.localPosition = Vector3.MoveTowards(finalTankBouy.transform.localPosition, Vector3.up * Remap(tempFinalTankFill, -50, 50, -1, 1), bouySens * Time.deltaTime) ;
        
        finalTankBouy.transform.localPosition = Vector3.up * Remap(tempFinalTankFill, -50, 50, -1, 1);
         
        if (tempFinalTankFill == finalTankFillAmount) 
        {
            //finalTankBouy.transform.localPosition = Vector3.up * Remap(tempFinalTankFill, -50, 50, -1, 1);
            Invoke("EmptyTankOnFinish", 1f);
            audioManager.Play("FillCup");
            CancelInvoke("FillFinalTankAnim");
        }

    }
    private void WobbleFinalTank()
    {
        finalTankShader.GetComponent<Wobble>().lastPos.x = 300;
    }
    public void SetTankCapacity(int factor)
    {
        if(factor > 0)
        {
            audioManager.Play("Buff");

            Destroy(Instantiate(buffParticle, tankShader.transform.position + Vector3.up * 2, Quaternion.Euler(-90, 0, 0), tankShader.transform.parent), 1f);
            //Destroy(Instantiate(buffTankParticle, tankShader.transform.position, Quaternion.Euler(-90, 0, 0), tankShader.transform.parent), 1f);
        }

        else
        {
            audioManager.Play("Debuff");
            
            //Destroy(Instantiate(debuffTankParticle, tankShader.transform.position + Vector3.up * 4, Quaternion.Euler(90, 0, 0), tankShader.transform.parent), 1f);
            Destroy(Instantiate(debuffParticle, tankShader.transform.position + Vector3.up * 7, Quaternion.Euler(90, 0, 0), tankShader.transform.parent), 1f);
            //tankLevel -= factor;
        }
        tankLevel += factor;

        tankObjs[0].transform.parent.localScale += Vector3.one * tankVolumeMultiplier * factor;
        //tankObjs[0].transform.parent.localPosition += Vector3.up * tankVolumeMultiplier * factor - Vector3.forward * tankVolumeMultiplier * factor * 2;

        /*foreach (GameObject obj in tankObjs)
        {
            obj.transform.localScale += Vector3.one * tankVolumeMultiplier * factor;
            obj.transform.localPosition -= Vector3.up * tankVolumeMultiplier * factor;
        }*/
        RefillTank(false);
    }

    public void ChangeLiquidColor(Color clr)
    {
        if(isTankEmpty)
        {
            // tankShader.GetComponent<Renderer>().material.color = clr;
            // tankShader.GetComponent<Renderer>().material.SetColor("_TopColor", clr + Color.white/5);

            tankShader.GetComponent<Renderer>().material.SetColor("_BaseColor", clr);
        }
        else
        {
            tempLiquidColor = tankShader.GetComponent<Renderer>().material.GetColor("_BaseColor");
            tempLiquidColor.a = 1;
            Color setColor = Color.Lerp(tempLiquidColor, clr, 0.5f);
            setColor.a = 1;
            tankShader.GetComponent<Renderer>().material.SetColor("_BaseColor", setColor);
        }
        liquidColor = tankShader.GetComponent<Renderer>().material.GetColor("_BaseColor");
        healthBar.SetFillColor(liquidColor);
        cupInsideShader.color = liquidColor;
        //tankShader.GetComponent<Renderer>().material.SetColor("_TopColor", liquidColor + Color.white / 5);
    }

    public void SetVacuum(int scaleFactor)
    {
        if(scaleFactor > 0)
        {
            audioManager.Play("Buff");

            Destroy(Instantiate(buffParticle, vacuumParticle.transform.position + Vector3.up * 2, Quaternion.Euler(-90, 0, 0), vacuumCollider.transform), 1f);
            vacuumParticle.GetComponent<ParticleSystem>().emissionRate += 2;
        }
        else
        {
            audioManager.Play("Debuff");

            Destroy(Instantiate(debuffParticle, vacuumCollider.transform.position + Vector3.up * 7f, Quaternion.Euler(90,0,0), vacuumCollider.transform), 1f);
            vacuumParticle.GetComponent<ParticleSystem>().emissionRate -= 2;
        }
        Vector3 radiusScale = Vector3.one * (scaleFactor * vacuumRadiusMultiplier);
        vacuumCollider.transform.localScale += radiusScale;
        Vector3 position = Vector3.forward * (scaleFactor * vacuumRadiusMultiplier * 0.6f);
        vacuumCollider.transform.localPosition += position;
    }

    void CameraController()
    {
        if(!isFinalTankFilling)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, player.transform.position - camOffset, camSensivity * Time.deltaTime);
        }
        else
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, finalTankBouy.transform.position - camTankOffset, finalTankCamSensivity * Time.deltaTime);
            //mainCam.transform.LookAt(finalTankBouy.transform.position);
        }
    }

    void InputController()
    {
        /* ***************************
        // During game(if UI elements are not touched.)
        if (controller && !EventSystem.current.IsPointerOverGameObject()) 
        {
            if(playerMaxSpeed > 0)
            {
                bool r = Input.GetMouseButton(0) ? true : false;
                UpdatePlayerRotationY(r);
                UpdateDirectorPositionX(true);
                MovePlayer(true);
            }
        }

        // Final Run
        else if(!isEnded)
        {
            Vector3 directorTarget = new Vector3(0, director.transform.position.y, player.transform.position.z + directorOffsZ);
            director.transform.position = Vector3.MoveTowards(director.transform.position, directorTarget, playerRotateSens * 20 * Time.deltaTime);
            director.transform.position = new Vector3(director.transform.position.x, director.transform.position.y, player.transform.position.z + directorOffsZ);
            UpdatePlayerRotationY(false);
            MovePlayer(true);
        }*/

        pipeLine.SetLinePositions();

        // Restart the game when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }
    /* *********************
    void UpdateDirectorPositionX(bool factor)
    {
        if(factor)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float moveAmount = mouseX * playerRotateSens;
            Vector3 newPosX = director.transform.position + director.transform.right * moveAmount;
            newPosX.x = Mathf.Clamp(newPosX.x, playerXMin, playerXMax);
            newPosX.z = player.transform.position.z + directorOffsZ;
            newPosX.y = player.transform.position.y;
            director.transform.position = newPosX;
        }

        else
        {
            director.transform.position = Vector3.Lerp(director.transform.position, player.transform.position + Vector3.forward * directorOffsZ, playerRotateSens * 50 * Time.deltaTime);
            director.transform.position = new Vector3(director.transform.position.x, director.transform.position.y, player.transform.position.z + directorOffsZ);
        }
    }

    void UpdatePlayerRotationY(bool a)
    {
        if (a)
        {
            float mouseX = Input.GetAxis("Mouse X");

            float rotationAmount = mouseX * playerRotateSens;

            float currentRotation = player.transform.rotation.eulerAngles.y + rotationAmount;

            if((currentRotation < playerRotateLimit && currentRotation >= 0) || (currentRotation > -playerRotateLimit && currentRotation <= 0) || (currentRotation > 360 - playerRotateLimit && currentRotation <= 360 + playerRotateLimit))
            {
                player.transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
            }
            else
            {
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.Euler(0, 0, 0), playerRotateSens * Time.deltaTime);
            }
        }
        else 
        {
            if(isFinished)
            {
                Vector3 target = player.transform.position;
                target.x = 0f;
                target.z += 1;
                //player.transform.LookAt(target);
                Quaternion targetRotation = Quaternion.LookRotation(target - player.transform.position);

                // Yavaþça dönüþü uygula
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetRotation, playerRotateSens * 7 * Time.deltaTime);
            }
            else
            {
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.Euler(0, 0, 0), playerRotateSens * Time.deltaTime * 1.5f);
            }
        }
    }

    void MovePlayer(bool direction)
    {
        if (direction)
        {
            playerCurrentSpeed += playerMoveSens * Time.deltaTime;
        }
        else
        {
            playerCurrentSpeed -= playerMoveSens * 5 * Time.deltaTime;
        }
        playerCurrentSpeed = Mathf.Clamp(playerCurrentSpeed, 0f, playerMaxSpeed);
        player.transform.position += player.transform.forward * playerCurrentSpeed * Time.deltaTime;
        player.transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, playerXMin, playerXMax), player.transform.position.y, player.transform.position.z);

        //pipeLine.SetLinePositions();
    }*/

    void ChangePlayerSpeed(bool fc)
    {
        playerAnimator.SetBool("IsRunning", fc);
        playerMaxSpeed = fc ? playerTempSpeed : 0;
        playerController.playerMaxSpeed = playerMaxSpeed;
    }

    public void StartFrom(int lv)
    {
        PlayerPrefs.SetInt("runnerLevel", lv);
        PlayerPrefs.SetInt("runnerUILevel", lv);
        Restart();
    }

    public void EmptyTankOnFinish() 
    {
        if(currentRunnerLevel < runnerLevels.Length-1)
        {
            PlayerPrefs.SetInt("runnerLevel", currentRunnerLevel + 1);
        } 
        else
        {
            int randomInt = Mathf.FloorToInt(UnityEngine.Random.Range(5, 11));
            PlayerPrefs.SetInt("runnerLevel", randomInt);
        }
        runnerUILevel++;
        PlayerPrefs.SetInt("runnerUILevel", runnerUILevel);

        finishPanel.transform.Find("Count").GetComponent<Text>().text = tempCupCount.ToString();

        if (PlayerPrefs.GetInt("cupCount", 0)>=4)
        {
            finishPanel.transform.Find("ShopButton").gameObject.SetActive(true);
        }
        else
        {
            finishPanel.transform.Find("NextButton").gameObject.SetActive(true);
        }

        //finishPanel.transform.Find("NextButton").gameObject.SetActive(true);

        finishPanel.SetActive(true);

        ChangePlayerSpeed(false);
        isEnded = true;
    }

    public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float normalizedValue = Mathf.InverseLerp(fromMin, fromMax, value);

        return Mathf.Lerp(toMin, toMax, normalizedValue);
    }

    public void Vibrate()
    {
        if (vibrationState)
        {
            //Handheld.Vibrate();
        }
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
        //soundBut.color = soundState ? Color.white : Color.red;
        //soundBut.transform.parent.Find("Text").GetComponent<Text>().text = soundState ? "On" : "Off"; 

        Debug.Log("Sound button is pressed.");
        float soundLevel = soundState ? 0.5f : 0f;
        audioManager.SetVolume(soundLevel);
    }
    public void SetVibration()
    {
        vibrationState = !vibrationState;
        Vibrate();
        int a = vibrationState ? 1 : 0;
        Debug.Log("Vibration button is pressed.");
        settingsPanel.transform.Find("VibrationUI").Find("Stroke").gameObject.SetActive(!vibrationState);
        PlayerPrefs.SetInt("vibrationState", a);
        SetVibrations();
    }

    public void SetVibrations()
    {
        //vibrationBut.color = vibrationState ? Color.white : Color.red;
        //vibrationBut.transform.parent.Find("Text").GetComponent<Text>().text = vibrationState ? "On" : "Off";
    }

    public void SetRotateSens()
    {
        playerRotateSens = rotateSensSlider.value;
        playerController.playerRotateSens = playerRotateSens;
        PlayerPrefs.SetFloat("RotateSens", playerRotateSens);
        rotateSensSlider.transform.parent.Find("Amount").GetComponent<Text>().text = playerRotateSens.ToString();
    }

    public void ResetCounts()
    {
        ResetCupCount();
        moneyCount = 0;
        PlayerPrefs.SetFloat("moneyCount", moneyCount);
        moneyCountTx.text = ConvertNumberToUIText(moneyCount);
    }

    public void ResetCupCount()
    {
        cupCount = 0;
        PlayerPrefs.SetInt("cupCount", cupCount);
        cupCountTx.text = cupCount.ToString();
    }

    public void SettingsPanel()
    {
        bool v = settingsPanel.activeSelf;
        if (isStarted)
        {
            ChangePlayerSpeed(v);
        }
        settingsPanel.SetActive(!v);

        soundState = PlayerPrefs.GetInt("soundState") == 1 ? true : false;
        vibrationState = PlayerPrefs.GetInt("vibrationState") == 1 ? true : false;

        settingsPanel.transform.Find("VibrationUI").Find("Stroke").gameObject.SetActive(!vibrationState);
        settingsPanel.transform.Find("SoundUI").Find("Stroke").gameObject.SetActive(!soundState);
    }
    private String ConvertNumberToUIText(float number)
    {
        String UITx = ">B";
        float operatedNumber;
        if (number > 1000000000) // Billion
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

    private void InitializeScene()
    {
        GameObject level = Instantiate(runnerLevels[currentRunnerLevel], levelParent.transform);
        level.transform.localPosition = Vector3.zero;
        levelTx.text = "Lv. " + runnerUILevel.ToString();

        soundState = PlayerPrefs.GetInt("soundState", 1) == 1 ? true : false;
        SetVolumes();

        vibrationState = PlayerPrefs.GetInt("vibrationState", 1) == 1 ? true : false;
        SetVibrations();

        cupCount = PlayerPrefs.GetInt("cupCount", 0);
        cupCountTx.text = cupCount.ToString();

        playerRotateSens = PlayerPrefs.GetFloat("RotateSens", playerRotateSens);
        playerController.playerRotateSens = playerRotateSens;
        rotateSensSlider.value = PlayerPrefs.GetFloat("RotateSens", playerRotateSens);
        rotateSensSlider.transform.parent.Find("Amount").GetComponent<Text>().text = playerRotateSens.ToString();
    }
     
    public void StartGame()
    {
        if(mainCam == null)
        {
            mainCam = GameObject.Find("Main Camera");
            camOffset = player.transform.position - mainCam.transform.position;
            directorOffsY = player.transform.position.y - director.transform.position.y;
            playerController.directorOffsY = directorOffsY;
        }
        isStarted = true;
        ChangePlayerSpeed(true);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadIdle()
    {
        Instantiate(idlePrefab);
        Destroy(transform.parent.gameObject);
    }


    /*
        //int a = 0;
        public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        *//*a = sceneIndex;
        Invoke("LoadSceneInv", 0.5f);*//* 
        //StartCoroutine(LoadSceneCor(sceneIndex));

    }
    *//*void LoadSceneInv()
    {
        SceneManager.LoadScene(a);  
    }*/
    /*void LoadSceneInv()
    {
        //Start loading the Scene asynchronously and output the progress bar
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadSceneCor(int a)
    {
        yield return null; 

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(a);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false; 
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        /*while (!asyncOperation.isDone)
        {

            // Check if the load has finished
            if (asyncOperation.progress >= 0.90f)
            {
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }*/

    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}