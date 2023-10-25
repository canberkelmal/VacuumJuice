using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    private GameManager gameManager;

    private void Start() 
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VacuumArea"))
        {
            GetComponent<BoxCollider>().enabled = false;
            transform.parent.Find("Confetties").gameObject.SetActive(true);
            gameManager.EnterToFinish();
            Destroy(transform.parent.gameObject, 2);
        }
    } 
} 
