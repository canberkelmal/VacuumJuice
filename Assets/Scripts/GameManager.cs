using System;
<<<<<<< HEAD
/*using System.Collections;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using Unity.VisualScripting;*/
=======
using System.Collections;
using System.Runtime.ConstrainedExecution;
using System.Threading;
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
<<<<<<< HEAD
    public GameObject finalTankShader; 
=======
    public GameObject finalTankShader;
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    public Material cupInsideShader;
    public GameObject[] tankObjs;
    public GameObject cupIcon;
    public GameObject finishPanel;
    public GameObject settingsPanel;
    public GameObject buffParticle;
    public GameObject debuffParticle;
    public GameObject getJuiceParticle;
<<<<<<< HEAD
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
=======
    public GameObject buffTankParticle;
    public GameObject debuffTankParticle;
    public GameObject finalTankBouy;
    public Slider rotateSensSlider;
    public Animator playerAnimator;
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    public int cupPerFruid = 4;
    public float finalTankLiquidMultiplier = 24;
    public float getCupSens = 1;
    public float camSensivity = 1f;
<<<<<<< HEAD
    public float finalTankCamYOffs = 1f;
=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
<<<<<<< HEAD
    public LayerMask gatesLayerMask;

    //UI Elements
    public Text cupCountTx;
    public Text moneyCountTx;
    public Text levelTx;
    public Image soundBut, vibrationBut;

    [NonSerialized]
    public Color liquidColor, tempLiquidColor;
    [NonSerialized]
    public AudioManager audioManager;

    public GameObject mainCam;
=======

    //UI Elements
    public Text cupCountTx;
    public Text moneyCountTx;
    public Image soundBut, vibrationBut;

    [NonSerialized]
    public Color liquidColor, tempLiquidColor;
    [NonSerialized]
    public AudioManager audioManager;

    private GameObject mainCam;
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
<<<<<<< HEAD
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
=======
    private int moneyCount = 0;
    private float juiceAmount = 0;
    private int tankLevel = 1;
    private bool soundState = true;
    private bool vibrationState = true;
    //private bool isFirstFruit
    void Awake()
    {
        mainCam = GameObject.Find("Main Camera");
        //tankShader = player.transform.Find("LiquidTank").Find("Shader").gameObject;
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        camOffset = player.transform.position - mainCam.transform.position;
        directorOffsY = player.transform.position.y - director.transform.position.y;
        controller = true;
        playerTempSpeed = playerMaxSpeed;
        playerMaxSpeed = 0;
        cupCount = PlayerPrefs.GetInt("cupCount", 0);
        cupCountTx.text = cupCount.ToString();
        moneyCount = PlayerPrefs.GetInt("moneyCount", 0);
        moneyCountTx.text = moneyCount.ToString();
    }
<<<<<<< HEAD
=======

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        InitializeScene();
    }
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
        SetController(false);
        vacuumParticle.transform.parent.Find("MagicAuraBlue").GetComponent<ParticleSystem>().startSpeed *= -1;
        vacuumParticle.transform.parent.Find("MagicAuraBlue").transform.localPosition += Vector3.forward * 0.5f;
    }

    public void SetController(bool value)
    {
        controller = value;
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
        SetController(false);
        vacuumParticle.transform.parent.Find("MagicAuraBlue").GetComponent<ParticleSystem>().startSpeed *= -1;
        vacuumParticle.transform.parent.Find("MagicAuraBlue").transform.localPosition += Vector3.forward * 0.5f;
    }

    public void SetController(bool value)
    {
        controller = value;
    }

    public void FillTank(int fillFactor)
    {
<<<<<<< HEAD
        if(fillFactor > 0)
        {
            audioManager.Play("AddJuice");
        }
        else if(fillFactor < 0)
        {
            audioManager.Play("Debuff");
        }
=======
        audioManager.Play("AddJuice");
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        if (tankShader.GetComponent<Renderer>().material.GetFloat("_Fill") < tankFillAmount)
        {
            tankShader.GetComponent<Renderer>().material.SetFloat("_Fill", tankFillAmount);
        }

<<<<<<< HEAD
        if (tankFillAmount >= (0.75f + ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)) && fillFactor>0)
        {
            Debug.Log("Tank is full!");
            healthBar.SetFillAmount(1, true);
        }
        else if (tankFillAmount <= (0.25f - ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)) && fillFactor < 0)
        {
            Debug.Log("Tank is empty!");
            healthBar.SetFillAmount(1, true);
=======
        if (tankFillAmount >= (0.75f + ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)))
        {
            Debug.Log("Tank is full!");
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        }
        else
        {
            juiceAmount += (fillFactor * fillMultiplier);
<<<<<<< HEAD
            RefillTank(true);
=======
            RefillTank();
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        }
    }
    public void FillACup()
    {
        audioManager.Play("FillCup");
        juiceAmount -= (fillMultiplier / cupPerFruid);
<<<<<<< HEAD
        RefillTank(true);
    }

    public void RefillTank(bool glow)
=======
        RefillTank();
    }

    public void RefillTank()
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    {
        tankFillAmount = (0.25f - ((tankLevel-1) * tankVolumeMultiplier * 0.2f)) + (juiceAmount / (Mathf.Pow((1 + ((tankLevel - 1) * tankVolumeMultiplier)), 3)));
        if(tankFillAmount <= (0.25f - ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)))
        {
<<<<<<< HEAD
            healthBar.SetFillAmount(0, glow);
=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
            tankFillAmount = 0;
            isTankEmpty = true;
            if (isFinished)
            {
                EmptyTankOnFinish();
            }
        }
<<<<<<< HEAD
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
=======
        else
        {
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
            isTankEmpty = false;
        }
        Debug.Log("Juice: " + juiceAmount + " || TankFill: " + tankFillAmount);
        InvokeRepeating("FillTankAnim", 0, Time.deltaTime);
    }
    
<<<<<<< HEAD
=======
    public void ReachToFinalTank()
    {
        finalTankBouy.transform.localPosition = -Vector3.up;

        camTankOffset = finalTankBouy.transform.position - mainCam.transform.position - Vector3.forward*4;
        camTankOffset.y = -5;
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
        InvokeRepeating("FillFinalTankAnim", fillFinalTankDelay, Time.deltaTime);
        Invoke("WobbleFinalTank", fillFinalTankDelay);

        isTankEmpty = true;
        tankFillAmount = 0;
        Debug.Log("Juice: " + juiceAmount + " || TankFill: " + tankFillAmount);
        InvokeRepeating("FillTankAnim", fillFinalTankDelay, Time.deltaTime);
    }
>>>>>>> e135bd62164667161091742e0478e6084b9b368d


    private void FillTankAnim()
    {
        tempTankFill = Mathf.MoveTowards(tankShader.GetComponent<Renderer>().material.GetFloat("_Fill"), tankFillAmount, fillTankSens * Time.fixedDeltaTime);
        tankShader.GetComponent<Renderer>().material.SetFloat("_Fill", tempTankFill);

        if(tempTankFill == tankFillAmount)
        {
            CancelInvoke("FillTankAnim");
        }
    }
<<<<<<< HEAD
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
=======
    private void FillFinalTankAnim()
    {
        tempFinalTankFill = Mathf.MoveTowards(finalTankShader.GetComponent<Renderer>().material.GetFloat("_Fill"), finalTankFillAmount, fillFinalTankSens * Time.deltaTime);
        finalTankShader.GetComponent<Renderer>().material.SetFloat("_Fill", tempFinalTankFill);

        //finalTankBouy.transform.localPosition = Vector3.MoveTowards(finalTankBouy.transform.localPosition, Vector3.up * Remap(tempFinalTankFill, -50, 50, -1, 1), bouySens * Time.deltaTime) ;
        finalTankBouy.transform.localPosition = Vector3.up * Remap(tempFinalTankFill, -50, 50, -1, 1);

        if (tempFinalTankFill == finalTankFillAmount)
        {
            //finalTankBouy.transform.localPosition = Vector3.up * Remap(tempFinalTankFill, -50, 50, -1, 1);
            Invoke("EmptyTankOnFinish", 1f);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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

<<<<<<< HEAD
            Destroy(Instantiate(buffParticle, tankShader.transform.position + Vector3.up * 2, Quaternion.Euler(-90, 0, 0), tankShader.transform.parent), 1f);
            //Destroy(Instantiate(buffTankParticle, tankShader.transform.position, Quaternion.Euler(-90, 0, 0), tankShader.transform.parent), 1f);
=======
            Destroy(Instantiate(buffTankParticle, tankShader.transform.position, Quaternion.Euler(-90, 0, 0), tankShader.transform.parent), 1f);
            tankLevel += factor;
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        }

        else
        {
            audioManager.Play("Debuff");
<<<<<<< HEAD
            
            //Destroy(Instantiate(debuffTankParticle, tankShader.transform.position + Vector3.up * 4, Quaternion.Euler(90, 0, 0), tankShader.transform.parent), 1f);
            Destroy(Instantiate(debuffParticle, tankShader.transform.position + Vector3.up * 7, Quaternion.Euler(90, 0, 0), tankShader.transform.parent), 1f);
            //tankLevel -= factor;
        }
        tankLevel += factor;
=======

            Destroy(Instantiate(debuffTankParticle, tankShader.transform.position, Quaternion.Euler(90, 0, 0), tankShader.transform.parent), 1f);
            tankLevel -= factor;
        }
>>>>>>> e135bd62164667161091742e0478e6084b9b368d

        tankObjs[0].transform.parent.localScale += Vector3.one * tankVolumeMultiplier * factor;
        //tankObjs[0].transform.parent.localPosition += Vector3.up * tankVolumeMultiplier * factor - Vector3.forward * tankVolumeMultiplier * factor * 2;

        /*foreach (GameObject obj in tankObjs)
        {
            obj.transform.localScale += Vector3.one * tankVolumeMultiplier * factor;
            obj.transform.localPosition -= Vector3.up * tankVolumeMultiplier * factor;
        }*/
<<<<<<< HEAD
        RefillTank(false);
=======
        RefillTank();
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
<<<<<<< HEAD
        healthBar.SetFillColor(liquidColor);
=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        cupInsideShader.color = liquidColor;
        //tankShader.GetComponent<Renderer>().material.SetColor("_TopColor", liquidColor + Color.white / 5);
    }

    public void SetVacuum(int scaleFactor)
    {
<<<<<<< HEAD
=======
        //Vector3 radiusScale = (Vector3.right + Vector3.forward) * (scaleFactor * vacuumRadiusMultiplier);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        if(scaleFactor > 0)
        {
            audioManager.Play("Buff");

            Destroy(Instantiate(buffParticle, vacuumParticle.transform.position + Vector3.up * 2, Quaternion.Euler(-90, 0, 0), vacuumCollider.transform), 1f);
            vacuumParticle.GetComponent<ParticleSystem>().emissionRate += 2;
        }
        else
        {
            audioManager.Play("Debuff");

<<<<<<< HEAD
            Destroy(Instantiate(debuffParticle, vacuumCollider.transform.position + Vector3.up * 7f, Quaternion.Euler(90,0,0), vacuumCollider.transform), 1f);
=======
            Destroy(Instantiate(debuffParticle, vacuumParticle.transform.position + Vector3.up * 3.5f, Quaternion.Euler(90,0,0), vacuumCollider.transform), 1f);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
<<<<<<< HEAD
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, finalTankBouy.transform.position - camTankOffset, camSensivity * Time.deltaTime);
=======
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, finalTankBouy.transform.position - camTankOffset, camSensivity / 2 * Time.deltaTime);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
            //mainCam.transform.LookAt(finalTankBouy.transform.position);
        }
    }

    void InputController()
    {
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
        }

        // Restart the game when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }
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
    }

    void ChangePlayerSpeed(bool fc)
    {
        playerAnimator.SetBool("IsRunning", fc);
        playerMaxSpeed = fc ? playerTempSpeed : 0;
    }

<<<<<<< HEAD
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

=======
    public void EmptyTankOnFinish()
    {
        finishPanel.transform.Find("Count").GetComponent<Text>().text = tempCupCount.ToString(); 
        finishPanel.SetActive(true);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
<<<<<<< HEAD
        settingsPanel.transform.Find("SoundUI").Find("Stroke").gameObject.SetActive(!soundState);
=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        PlayerPrefs.SetInt("soundState", a);
        SetVolumes();
    }
    public void SetVolumes()
    {
<<<<<<< HEAD
        //soundBut.color = soundState ? Color.white : Color.red;
        //soundBut.transform.parent.Find("Text").GetComponent<Text>().text = soundState ? "On" : "Off"; 

        Debug.Log("Sound button is pressed.");
=======
        soundBut.color = soundState ? Color.white : Color.red;
        soundBut.transform.parent.Find("Text").GetComponent<Text>().text = soundState ? "On" : "Off";

>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        float soundLevel = soundState ? 0.5f : 0f;
        audioManager.SetVolume(soundLevel);
    }
    public void SetVibration()
    {
        vibrationState = !vibrationState;
        Vibrate();
        int a = vibrationState ? 1 : 0;
<<<<<<< HEAD
        Debug.Log("Vibration button is pressed.");
        settingsPanel.transform.Find("VibrationUI").Find("Stroke").gameObject.SetActive(!vibrationState);
=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        PlayerPrefs.SetInt("vibrationState", a);
        SetVibrations();
    }

    public void SetVibrations()
    {
<<<<<<< HEAD
        //vibrationBut.color = vibrationState ? Color.white : Color.red;
        //vibrationBut.transform.parent.Find("Text").GetComponent<Text>().text = vibrationState ? "On" : "Off";
=======
        vibrationBut.color = vibrationState ? Color.white : Color.red;
        vibrationBut.transform.parent.Find("Text").GetComponent<Text>().text = vibrationState ? "On" : "Off";
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    }

    public void SetRotateSens()
    {
        playerRotateSens = rotateSensSlider.value;
        PlayerPrefs.SetFloat("RotateSens", playerRotateSens);
        rotateSensSlider.transform.parent.Find("Amount").GetComponent<Text>().text = playerRotateSens.ToString();
    }

    public void ResetCounts()
    {
<<<<<<< HEAD
        ResetCupCount();
        moneyCount = 0;
        PlayerPrefs.SetFloat("moneyCount", moneyCount);
        moneyCountTx.text = ConvertNumberToUIText(moneyCount);
    }

    public void ResetCupCount()
    {
=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        cupCount = 0;
        PlayerPrefs.SetInt("cupCount", cupCount);
        cupCountTx.text = cupCount.ToString();
    }

<<<<<<< HEAD
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
=======
    public void SettingsPanel(bool v)
    {
        if(isStarted)
        {
            ChangePlayerSpeed(!v);
        }
        settingsPanel.SetActive(v);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    }

    private void InitializeScene()
    {
<<<<<<< HEAD
        GameObject level = Instantiate(runnerLevels[currentRunnerLevel], levelParent.transform);
        level.transform.localPosition = Vector3.zero;
        levelTx.text = "Lv. " + runnerUILevel.ToString();

=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        soundState = PlayerPrefs.GetInt("soundState", 1) == 1 ? true : false;
        SetVolumes();

        vibrationState = PlayerPrefs.GetInt("vibrationState", 1) == 1 ? true : false;
        SetVibrations();

        cupCount = PlayerPrefs.GetInt("cupCount", 0);
        cupCountTx.text = cupCount.ToString();

        playerRotateSens = PlayerPrefs.GetFloat("RotateSens", playerRotateSens);
        rotateSensSlider.value = PlayerPrefs.GetFloat("RotateSens", playerRotateSens);
        rotateSensSlider.transform.parent.Find("Amount").GetComponent<Text>().text = playerRotateSens.ToString();
    }
<<<<<<< HEAD
     
    public void StartGame()
    {
        if(mainCam == null)
        {
            mainCam = GameObject.Find("Main Camera");
            camOffset = player.transform.position - mainCam.transform.position;
            directorOffsY = player.transform.position.y - director.transform.position.y;
        }
=======

    public void StartGame()
    {
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        isStarted = true;
        ChangePlayerSpeed(true);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

<<<<<<< HEAD
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

=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}