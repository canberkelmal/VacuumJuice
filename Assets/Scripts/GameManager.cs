using System;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
    public GameObject buffTankParticle;
    public GameObject debuffTankParticle;
    public GameObject finalTankBouy;
    public Slider rotateSensSlider;
    public Animator playerAnimator;
    public int cupPerFruid = 4;
    public float finalTankLiquidMultiplier = 24;
    public float getCupSens = 1;
    public float camSensivity = 1f;
    public float playerRotateSens = 1;
    public float playerRotateLimit = 75;
    public float playerMoveSens = 1f;
    public float playerMaxSpeed = 1f;
    public float playerXMin, playerXMax;
    public float collectSens = 1f;
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

    //UI Elements
    public Text cupCountTx;
    public Text moneyCountTx;
    public Image soundBut, vibrationBut;

    [NonSerialized]
    public Color liquidColor, tempLiquidColor;
    [NonSerialized]
    public AudioManager audioManager;

    private GameObject mainCam;
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
        camOffset = player.transform.position - mainCam.transform.position;
        directorOffsY = player.transform.position.y - director.transform.position.y;
        controller = true;
        playerTempSpeed = playerMaxSpeed;
        playerMaxSpeed = 0;
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        InitializeScene();
    }
    // Update is called once per frame
    void Update()
    {
        InputController();
        CameraController();
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
        audioManager.Play("AddJuice");
        if (tankShader.GetComponent<Renderer>().material.GetFloat("_Fill") < tankFillAmount)
        {
            tankShader.GetComponent<Renderer>().material.SetFloat("_Fill", tankFillAmount);
        }

        if (tankFillAmount >= (0.75f + ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)))
        {
            Debug.Log("Tank is full!");
        }
        else
        {
            juiceAmount += (fillFactor * fillMultiplier);
            RefillTank();
        }
    }
    public void FillACup()
    {
        audioManager.Play("FillCup");
        juiceAmount -= (fillMultiplier / cupPerFruid);
        RefillTank();
    }

    public void RefillTank()
    {
        tankFillAmount = (0.25f - ((tankLevel-1) * tankVolumeMultiplier * 0.2f)) + (juiceAmount / (Mathf.Pow((1 + ((tankLevel - 1) * tankVolumeMultiplier)), 3)));
        if(tankFillAmount <= (0.25f - ((tankLevel - 1) * tankVolumeMultiplier * 0.2f)))
        {
            tankFillAmount = 0;
            isTankEmpty = true;
            if (isFinished)
            {
                EmptyTankOnFinish();
            }
        }
        else
        {
            isTankEmpty = false;
        }
        Debug.Log("Juice: " + juiceAmount + " || TankFill: " + tankFillAmount);
        InvokeRepeating("FillTankAnim", 0, Time.deltaTime);
    }
    
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


    private void FillTankAnim()
    {
        tempTankFill = Mathf.MoveTowards(tankShader.GetComponent<Renderer>().material.GetFloat("_Fill"), tankFillAmount, fillTankSens * Time.deltaTime);
        tankShader.GetComponent<Renderer>().material.SetFloat("_Fill", tempTankFill);

        if(tempTankFill == tankFillAmount)
        {
            CancelInvoke("FillTankAnim");
        }
    }
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

            Destroy(Instantiate(buffTankParticle, tankShader.transform.position, Quaternion.Euler(-90, 0, 0), tankShader.transform.parent), 1f);
            tankLevel += factor;
        }

        else
        {
            audioManager.Play("Debuff");

            Destroy(Instantiate(debuffTankParticle, tankShader.transform.position, Quaternion.Euler(90, 0, 0), tankShader.transform.parent), 1f);
            tankLevel -= factor;
        }

        tankObjs[0].transform.parent.localScale += Vector3.one * tankVolumeMultiplier * factor;
        //tankObjs[0].transform.parent.localPosition += Vector3.up * tankVolumeMultiplier * factor - Vector3.forward * tankVolumeMultiplier * factor * 2;

        /*foreach (GameObject obj in tankObjs)
        {
            obj.transform.localScale += Vector3.one * tankVolumeMultiplier * factor;
            obj.transform.localPosition -= Vector3.up * tankVolumeMultiplier * factor;
        }*/
        RefillTank();
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
        cupInsideShader.color = liquidColor;
        //tankShader.GetComponent<Renderer>().material.SetColor("_TopColor", liquidColor + Color.white / 5);
    }

    public void SetVacuum(int scaleFactor)
    {
        //Vector3 radiusScale = (Vector3.right + Vector3.forward) * (scaleFactor * vacuumRadiusMultiplier);
        if(scaleFactor > 0)
        {
            audioManager.Play("Buff");

            Destroy(Instantiate(buffParticle, vacuumParticle.transform.position + Vector3.up * 2, Quaternion.Euler(-90, 0, 0), vacuumCollider.transform), 1f);
            vacuumParticle.GetComponent<ParticleSystem>().emissionRate += 2;
        }
        else
        {
            audioManager.Play("Debuff");

            Destroy(Instantiate(debuffParticle, vacuumParticle.transform.position + Vector3.up * 3.5f, Quaternion.Euler(90,0,0), vacuumCollider.transform), 1f);
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
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, finalTankBouy.transform.position - camTankOffset, camSensivity / 2 * Time.deltaTime);
            //mainCam.transform.LookAt(finalTankBouy.transform.position);
        }
    }

    void InputController()
    {
        // During game
        if (controller)
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

    public void EmptyTankOnFinish()
    {
        finishPanel.transform.Find("Count").GetComponent<Text>().text = tempCupCount.ToString(); 
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
        PlayerPrefs.SetInt("soundState", a);
        SetVolumes();
    }
    public void SetVolumes()
    {
        soundBut.color = soundState ? Color.white : Color.red;
        soundBut.transform.parent.Find("Text").GetComponent<Text>().text = soundState ? "On" : "Off";

        float soundLevel = soundState ? 0.5f : 0f;
        audioManager.SetVolume(soundLevel);
    }
    public void SetVibration()
    {
        vibrationState = !vibrationState;
        Vibrate();
        int a = vibrationState ? 1 : 0;
        PlayerPrefs.SetInt("vibrationState", a);
        SetVibrations();
    }

    public void SetVibrations()
    {
        vibrationBut.color = vibrationState ? Color.white : Color.red;
        vibrationBut.transform.parent.Find("Text").GetComponent<Text>().text = vibrationState ? "On" : "Off";
    }

    public void SetRotateSens()
    {
        playerRotateSens = rotateSensSlider.value;
        PlayerPrefs.SetFloat("RotateSens", playerRotateSens);
        rotateSensSlider.transform.parent.Find("Amount").GetComponent<Text>().text = playerRotateSens.ToString();
    }

    public void ResetCounts()
    {
        cupCount = 0;
        PlayerPrefs.SetInt("cupCount", cupCount);
        cupCountTx.text = cupCount.ToString();
    }

    public void SettingsPanel(bool v)
    {
        if(isStarted)
        {
            ChangePlayerSpeed(!v);
        }
        settingsPanel.SetActive(v);
    }

    private void InitializeScene()
    {
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

    public void StartGame()
    {
        isStarted = true;
        ChangePlayerSpeed(true);
    }

    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}