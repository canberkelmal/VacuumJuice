using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    private GameManager gameManager;

<<<<<<< HEAD
    private void Start() 
=======
    private void Awake()
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VacuumArea"))
        {
<<<<<<< HEAD
            GetComponent<BoxCollider>().enabled = false;
            transform.parent.Find("Confetties").gameObject.SetActive(true);
            gameManager.EnterToFinish();
            Destroy(transform.parent.gameObject, 2);
        }
    } 
} 
=======
            gameManager.EnterToFinish();
            Destroy(gameObject);
        }
    }
}
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
