using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSc : MonoBehaviour
{
    public float fillAmount = 0.2f;
    private GameObject player;
    private GameManager gameManager;

    private void Awake()
    {
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("VacuumArea"))
        {
            transform.parent = player.transform.Find("Collecteds");
            InvokeRepeating("MoveToPlayer", 0, Time.fixedDeltaTime);
        }
    }

    private void MoveToPlayer()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, gameManager.collectSens * Time.fixedDeltaTime);
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, (gameManager.collectSens * 2 / 3) * Time.fixedDeltaTime);

        if (transform.localPosition == Vector3.zero) {
            Debug.Log("Object collected!");
            gameManager.FillTank(fillAmount);
            CancelInvoke("MoveToPlayer"); 
            Destroy(gameObject);
        }
    }
}
