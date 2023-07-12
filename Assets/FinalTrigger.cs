using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VacuumArea"))
        {
            gameManager.SetController(false);
            Destroy(gameObject);
        }
    }
}
