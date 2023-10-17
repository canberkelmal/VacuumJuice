using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFruit : MonoBehaviour
{
    private GameManager gameManager;
    public Vector3 rotateDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void FixedUpdate()
    {
        transform.Rotate(rotateDirection * gameManager.rotateObjectsSens * Time.deltaTime);
        if(transform.childCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
