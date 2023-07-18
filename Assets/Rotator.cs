using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(-Vector3.up * gameManager.rotateFanSens * Time.deltaTime);
    }
}
