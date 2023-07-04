using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject director;
    public float playerRotateSens = 1;
    public float playerMoveSens = 1f;
    public float playerMaxSpeed = 1f;
    public float playerXMin, playerXMax;
    public float collectSens = 1f;

    private GameObject mainCam;
    private GameObject tankShader;
    private Vector3 camOffset;
    private bool controller = true;
    private bool isFinished = false;
    private float playerCurrentSpeed = 0;
    private float directorOffsZ = 1.5f;
    private float directorOffsY = 1f;
    private float tankFillAmount = -0.8f;
    void Awake()
    {
        mainCam = GameObject.Find("Main Camera");
        tankShader = player.transform.Find("LiquidTank").Find("Shader").gameObject;
        camOffset = player.transform.position - mainCam.transform.position;
        directorOffsY = player.transform.position.y - director.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        InputController();
        CameraController();
    }

    public void FillTank(float fillAmount)
    {
        tankFillAmount += fillAmount;
        tankShader.GetComponent<Renderer>().material.SetFloat("_Fill", tankFillAmount);
    }

    void CameraController()
    {
        mainCam.transform.position = player.transform.position - camOffset;
    }

    void InputController()
    {
        if (Input.GetMouseButton(0) && controller)
        {
            UpdatePlayerRotationY();
            UpdateDirectorPositionX(true);
            MovePlayer(true);
        }
        else if ((director.transform.position.x != player.transform.position.x || playerCurrentSpeed > 0) && controller)
        {
            UpdateDirectorPositionX(false);
            UpdatePlayerRotationY();
            MovePlayer(false);
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
            if (Input.GetAxis("Mouse X") != 0)
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
            }
        }

        else
        {
            director.transform.position = Vector3.Lerp(director.transform.position, player.transform.position + Vector3.forward * directorOffsZ, playerRotateSens * 20 * Time.deltaTime);
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
