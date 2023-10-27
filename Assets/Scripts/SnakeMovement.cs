using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float speed = 1f;
    public float delay = 0.2f;
    public float movementX = 2f;

    private Vector3[] startPos = new Vector3[5];
    private bool[] isRight = {true, true, true, true, true};

    private void Start()
    {
        /* Old snake movement
        for(int i = 0; i < startPos.Length; i++)
        {
            startPos[i] = transform.GetChild(i).localPosition;
        }
        InvokeRepeating("Move0thChild", delay, Time.fixedDeltaTime);
        InvokeRepeating("Move1stChild", delay*2, Time.fixedDeltaTime);
        InvokeRepeating("Move2ndChild", delay*3, Time.fixedDeltaTime);
        InvokeRepeating("Move3thChild", delay*4, Time.fixedDeltaTime);
        InvokeRepeating("Move4thChild", delay*5, Time.fixedDeltaTime);*/

        for(int i=0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CollectableSc>().SnakeMovement(speed, movementX, delay * i);
        }
    }

    /* Old snake movement
    private void FixedUpdate()
    {
        if(transform.childCount < 5)
        {
            CancelInvoke("Move0thChild");
            CancelInvoke("Move1stChild");
            CancelInvoke("Move2ndChild");
            CancelInvoke("Move3thChild");
            CancelInvoke("Move4thChild");
        }
    } 
    void Move0thChild()
    {
        int childIndex = 0;
        if (isRight[childIndex])
        {
            Vector3 pos = transform.GetChild(childIndex).localPosition;
            transform.GetChild(childIndex).localPosition = Vector3.MoveTowards(pos, startPos[childIndex] + Vector3.right * movementX, speed * Time.fixedDeltaTime);
            if(transform.GetChild(childIndex).localPosition == startPos[childIndex] + Vector3.right * movementX)
            {
                isRight[childIndex] = false;
            }
        }
        else
        {
            Vector3 pos = transform.GetChild(childIndex).localPosition;
            transform.GetChild(childIndex).localPosition = Vector3.MoveTowards(pos, startPos[childIndex] - Vector3.right * movementX, speed * Time.fixedDeltaTime);
            if (transform.GetChild(childIndex).localPosition == startPos[childIndex] - Vector3.right * movementX)
            {
                isRight[childIndex] = true;
            }
        }
    }
    void Move1stChild()
    {
        int childIndex = 1;
        if (isRight[childIndex])
        {
            Vector3 pos = transform.GetChild(childIndex).localPosition;
            transform.GetChild(childIndex).localPosition = Vector3.MoveTowards(pos, startPos[childIndex] + Vector3.right * movementX, speed * Time.fixedDeltaTime);
            if (transform.GetChild(childIndex).localPosition == startPos[childIndex] + Vector3.right * movementX)
            {
                isRight[childIndex] = false;
            }
        }
        else
        {
            Vector3 pos = transform.GetChild(childIndex).localPosition;
            transform.GetChild(childIndex).localPosition = Vector3.MoveTowards(pos, startPos[childIndex] - Vector3.right * movementX, speed * Time.fixedDeltaTime);
            if (transform.GetChild(childIndex).localPosition == startPos[childIndex] - Vector3.right * movementX)
            {
                isRight[childIndex] = true;
            }
        }
    }
    void Move2ndChild()
    {
        int childIndex = 2;
        if (isRight[childIndex])
        {
            Vector3 pos = transform.GetChild(childIndex).localPosition;
            transform.GetChild(childIndex).localPosition = Vector3.MoveTowards(pos, startPos[childIndex] + Vector3.right * movementX, speed * Time.fixedDeltaTime);
            if (transform.GetChild(childIndex).localPosition == startPos[childIndex] + Vector3.right * movementX)
            {
                isRight[childIndex] = false;
            }
        }
        else
        {
            Vector3 pos = transform.GetChild(childIndex).localPosition;
            transform.GetChild(childIndex).localPosition = Vector3.MoveTowards(pos, startPos[childIndex] - Vector3.right * movementX, speed * Time.fixedDeltaTime);
            if (transform.GetChild(childIndex).localPosition == startPos[childIndex] - Vector3.right * movementX)
            {
                isRight[childIndex] = true;
            }
        }
    }
    void Move3thChild()
    {
        int childIndex = 3;
        if (isRight[childIndex])
        {
            Vector3 pos = transform.GetChild(childIndex).localPosition;
            transform.GetChild(childIndex).localPosition = Vector3.MoveTowards(pos, startPos[childIndex] + Vector3.right * movementX, speed * Time.fixedDeltaTime);
            if (transform.GetChild(childIndex).localPosition == startPos[childIndex] + Vector3.right * movementX)
            {
                isRight[childIndex] = false;
            }
        }
        else
        {
            Vector3 pos = transform.GetChild(childIndex).localPosition;
            transform.GetChild(childIndex).localPosition = Vector3.MoveTowards(pos, startPos[childIndex] - Vector3.right * movementX, speed * Time.fixedDeltaTime);
            if (transform.GetChild(childIndex).localPosition == startPos[childIndex] - Vector3.right * movementX)
            {
                isRight[childIndex] = true;
            }
        }
    }
    void Move4thChild()
    {
        int childIndex = 4;
        if (isRight[childIndex])
        {
            Vector3 pos = transform.GetChild(childIndex).localPosition;
            transform.GetChild(childIndex).localPosition = Vector3.MoveTowards(pos, startPos[childIndex] + Vector3.right * movementX, speed * Time.fixedDeltaTime);
            if (transform.GetChild(childIndex).localPosition == startPos[childIndex] + Vector3.right * movementX)
            {
                isRight[childIndex] = false;
            }
        }
        else
        {
            Vector3 pos = transform.GetChild(childIndex).localPosition;
            transform.GetChild(childIndex).localPosition = Vector3.MoveTowards(pos, startPos[childIndex] - Vector3.right * movementX, speed * Time.fixedDeltaTime);
            if (transform.GetChild(childIndex).localPosition == startPos[childIndex] - Vector3.right * movementX)
            {
                isRight[childIndex] = true;
            }
        }
    }*/
}
