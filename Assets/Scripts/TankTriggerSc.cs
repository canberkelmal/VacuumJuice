using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTriggerSc : MonoBehaviour
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
<<<<<<< HEAD
            gameManager.fireworks.SetActive(true);

=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
            gameManager.ReachToFinalTank();
            Destroy(gameObject);
        }
    }
}
