using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCupSc : MonoBehaviour
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
            Vector3 cupScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            Instantiate(gameManager.cupIcon, cupScreenPos, Quaternion.identity, gameManager.cupCountTx.transform.parent);

            gameManager.FillACup();
            Destroy(gameObject);
        }
    }
}
