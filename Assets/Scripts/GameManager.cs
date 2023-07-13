using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject director;
    public GameObject vacuumCollider, vacuumParticle;
    public GameObject tankShader;
    public GameObject[] tankObjs;
    public GameObject cupIcon;
    public Text cupCountTx;
    public GameObject buffParticle;
    public GameObject debuffParticle;
    public GameObject getJuiceParticle;
    public float getCupSens = 1;
    public float camSensivity = 1f;
    public float playerRotateSens = 1;
    public float playerMoveSens = 1f;
    public float playerMaxSpeed = 1f;
    public float playerXMin, playerXMax;
    public float collectSens = 1f;
    public float fillTankSens = 1f;
    public float fillMultiplier = 0.1f;
    public float vacuumRadiusMultiplier = 0.1f;
    public float vacuumLengthMultiplier = 0.5f;
    public float tankVolumeMultiplier = 0.2f;

    private GameObject mainCam;
    private Vector3 camOffset;
    private bool controller = true;
    private bool isFinished = false;
    private float playerCurrentSpeed = 0;
    private float directorOffsZ = 1.5f;
    private float directorOffsY = 1f;
    private float tankFillAmount = 0;
    private float tempTankFill = 1;
    private int cupCount = 0;
    private float juiceAmount = 0;
    private int tankLevel = 1;
    void Awake()
    {
        mainCam = GameObject.Find("Main Camera");
        //tankShader = player.transform.Find("LiquidTank").Find("Shader").gameObject;
        camOffset = player.transform.position - mainCam.transform.position;
        directorOffsY = player.transform.position.y - director.transform.position.y;
        controller = true;
    }

    // Update is called once per frame
    void Update()
    {
        InputController();
        CameraController();
    }

    public void IncreaseCupCount()
    {
        cupCount++;
        cupCountTx.text = cupCount.ToString();
    }

    public void SetController(bool value)
    {
        controller = value;
    }

    public void FillTank(int fillFactor)
    {
        if(tankShader.GetComponent<Renderer>().material.GetFloat("_Fill") < tankFillAmount)
        {
            tankShader.GetComponent<Renderer>().material.SetFloat("_Fill", tankFillAmount);
        }
        juiceAmount += (fillFactor * fillMultiplier);
        RefillTank();
    }
    public void FillACup()
    {
        juiceAmount -= (fillMultiplier / 2);
        RefillTank();
    }

    public void RefillTank()
    {
        tankFillAmount = (0.25f - ((tankLevel-1) * tankVolumeMultiplier * 0.2f)) + (juiceAmount / (Mathf.Pow((1 + ((tankLevel - 1) * tankVolumeMultiplier)), 3)));
        if(tankFillAmount <= (0.25f - (tankLevel * tankVolumeMultiplier * 0.2f)))
        {
            tankFillAmount = 0;
        }
        Debug.Log("Juice: " + juiceAmount + " || TankFill: " + tankFillAmount);
        InvokeRepeating("FillTankAnim", 0, Time.fixedDeltaTime);
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
    public void SetTankCapacity(int factor)
    {
        tankLevel += factor;
        foreach(GameObject obj in tankObjs)
        {
            obj.transform.localScale += Vector3.one * tankVolumeMultiplier * factor;
            obj.transform.localPosition -= Vector3.up * tankVolumeMultiplier * factor;
        }
        RefillTank();
    }

    public void SetVacuum(int scaleFactor)
    {
        //Vector3 radiusScale = (Vector3.right + Vector3.forward) * (scaleFactor * vacuumRadiusMultiplier);
        if(scaleFactor > 0)
        {
            Destroy(Instantiate(buffParticle, vacuumParticle.transform.position + Vector3.up, Quaternion.identity, vacuumCollider.transform), 1f);
            vacuumParticle.GetComponent<ParticleSystem>().emissionRate += 2;
        }
        else
        {
            Destroy(Instantiate(debuffParticle, vacuumParticle.transform.position + Vector3.up * 2, Quaternion.Euler(180,0,0), vacuumCollider.transform), 1f);
            vacuumParticle.GetComponent<ParticleSystem>().emissionRate -= 2;
        }
        Vector3 radiusScale = Vector3.one * (scaleFactor * vacuumRadiusMultiplier);
        vacuumCollider.transform.localScale += radiusScale;
        Vector3 position = Vector3.forward * (scaleFactor * vacuumRadiusMultiplier * 0.6f);
        vacuumCollider.transform.localPosition += position;
    }

    void CameraController()
    {
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, player.transform.position - camOffset, camSensivity * Time.deltaTime);
    }

    void InputController()
    {
        if (controller)
        {
            if (Input.GetMouseButton(0))
            {
                UpdatePlayerRotationY();
                UpdateDirectorPositionX(true);
                MovePlayer(true);
            }
            else if (director.transform.position.x != player.transform.position.x || playerCurrentSpeed > 0)
            {
                UpdateDirectorPositionX(false);
                UpdatePlayerRotationY();
                MovePlayer(false);
            }
        }
        else
        {
            Vector3 directorTarget = new Vector3(0, director.transform.position.y, player.transform.position.z + directorOffsZ);
            director.transform.position = Vector3.MoveTowards(director.transform.position, directorTarget, playerRotateSens * 20 * Time.deltaTime);
            director.transform.position = new Vector3(director.transform.position.x, director.transform.position.y, player.transform.position.z + directorOffsZ);
            UpdatePlayerRotationY();
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
/*
            if (Input.GetAxis("Mouse X") < -0.05 || Input.GetAxis("Mouse X") > 0.05)
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
                director.transform.position = Vector3.Lerp(director.transform.position, player.transform.position + Vector3.forward * directorOffsZ, playerRotateSens * Time.deltaTime);
                director.transform.position = new Vector3(director.transform.position.x, director.transform.position.y, player.transform.position.z + directorOffsZ);
            }*/
        }

        else
        {
            director.transform.position = Vector3.Lerp(director.transform.position, player.transform.position + Vector3.forward * directorOffsZ, playerRotateSens * 50 * Time.deltaTime);
            director.transform.position = new Vector3(director.transform.position.x, director.transform.position.y, player.transform.position.z + directorOffsZ);
        }
    }

    void UpdatePlayerRotationY()
    {
        if (!isFinished)
        {
            player.transform.LookAt(director.transform.position);
        }
        else
        {
            player.transform.eulerAngles = Vector3.zero;
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
    }
    
    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
