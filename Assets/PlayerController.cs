using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public bool controller = true;
    public float playerMoveSens = 1f;
    public float playerMaxSpeed = 1f;
    public bool isEnded = false;
    public bool isFinished = false;
    public GameObject director;
    public float directorOffsZ = 2.5f;
    public float playerRotateSens = 1;
    public float playerXMin, playerXMax;
    public float playerRotateLimit = 75;
    public float playerCurrentSpeed = 0;
    public float directorOffsY = 1f;
    public float playerTempSpeed;

    void Start()
    {
        controller = true;
        playerTempSpeed = playerMaxSpeed;
        playerMaxSpeed = 0;
        directorOffsY = transform.position.y - director.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // During game(if UI elements are not touched.)
        if (controller && !EventSystem.current.IsPointerOverGameObject())
        {
            if (playerMaxSpeed > 0)
            {
                bool r = Input.GetMouseButton(0) ? true : false;
                UpdateDirectorPositionX(true);
                UpdatePlayerRotationY(r);
                //MovePlayer(true);
            }
        }

        // Final Run
        else if (!isEnded)
        {
            Vector3 directorTarget = new Vector3(0, director.transform.position.y, transform.position.z + directorOffsZ);
            director.transform.position = Vector3.MoveTowards(director.transform.position, directorTarget, playerRotateSens * 20 * Time.deltaTime);
            director.transform.position = new Vector3(director.transform.position.x, director.transform.position.y, transform.position.z + directorOffsZ);
            UpdatePlayerRotationY(false);
            //MovePlayer(true);
        }
    }
    void UpdateDirectorPositionX(bool factor)
    {
        if (factor)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float moveAmount = mouseX * playerRotateSens;
            Vector3 newPosX = director.transform.position + director.transform.right * moveAmount;
            newPosX.x = Mathf.Clamp(newPosX.x, playerXMin, playerXMax);
            newPosX.z = transform.position.z + directorOffsZ;
            newPosX.y = transform.position.y;
            director.transform.position = newPosX;
        }

        else
        {
            Vector3 newPosX = Vector3.Lerp(director.transform.position, transform.position + Vector3.forward * directorOffsZ, playerRotateSens * 50 * Time.deltaTime);
            newPosX.z = transform.position.z + directorOffsZ;
            director.transform.position = newPosX;
            //director.transform.position = Vector3.Lerp(director.transform.position, transform.position + Vector3.forward * directorOffsZ, playerRotateSens * 50 * Time.deltaTime);
            //director.transform.position = new Vector3(director.transform.position.x, director.transform.position.y, transform.position.z + directorOffsZ);
        }
    }

    void UpdatePlayerRotationY(bool a)
    {
        if (a)
        {
            float mouseX = Input.GetAxis("Mouse X");

            float rotationAmount = mouseX * playerRotateSens;

            float currentRotation = transform.rotation.eulerAngles.y + rotationAmount;

            if ((currentRotation < playerRotateLimit && currentRotation >= 0) || (currentRotation > -playerRotateLimit && currentRotation <= 0) || (currentRotation > 360 - playerRotateLimit && currentRotation <= 360 + playerRotateLimit))
            {
                transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), playerRotateSens * Time.deltaTime);
            }
        }
        else
        {
            if (isFinished)
            {
                Vector3 target = transform.position;
                target.x = 0f;
                target.z += 1;
                //player.transform.LookAt(target);
                Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

                // Yavaþça dönüþü uygula
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, playerRotateSens * 7 * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), playerRotateSens * Time.deltaTime * 1.5f);
            }
        }
        MovePlayer(true);
    } 
    void MovePlayer(bool direction)
    {
        if (direction)
        {
            playerCurrentSpeed += playerMoveSens;
            //playerCurrentSpeed += playerMoveSens * Time.deltaTime;
        }
        else
        {
            playerCurrentSpeed -= playerMoveSens * 5;
            //playerCurrentSpeed -= playerMoveSens * 5 * Time.deltaTime;
        }
        playerCurrentSpeed = Mathf.Clamp(playerCurrentSpeed, 0f, playerMaxSpeed);
        Vector3 playerTargetPos = transform.position + transform.forward * playerCurrentSpeed * Time.deltaTime;
        playerTargetPos.x = Mathf.Clamp(playerTargetPos.x, playerXMin, playerXMax);
        //transform.position += transform.forward * playerCurrentSpeed * Time.deltaTime;
        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, playerXMin, playerXMax), transform.position.y, transform.position.z);
        transform.position = playerTargetPos;
        //transform.position = Vector3.Lerp(transform.position, playerTargetPos, playerMoveSens * Time.deltaTime);

        //pipeLine.SetLinePositions();
    }
}
