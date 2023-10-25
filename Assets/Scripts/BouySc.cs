using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BouySc : MonoBehaviour
{
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinalCanvasObj"))
        {
<<<<<<< HEAD
            other.GetComponent<BoxCollider>().enabled = false;
            //Vector3 cupScreenPos = Camera.main.WorldToScreenPoint(other.transform.position);
            //Instantiate(gameManager.cupIcon, cupScreenPos, Quaternion.identity, gameManager.cupCountTx.transform.parent);
            gameManager.IncreaseCupCount();
            other.transform.GetChild(0).GetComponent<Image>().color = Color.yellow; 

            //Take effect
            //other.transform.GetChild(3).gameObject.SetActive(true);
            //Destroy(other.transform.GetChild(3).gameObject, 1f);

            //gameManager.audioManager.Play("FillCup");
=======
            Vector3 cupScreenPos = Camera.main.WorldToScreenPoint(other.transform.position);
            Instantiate(gameManager.cupIcon, cupScreenPos, Quaternion.identity, gameManager.cupCountTx.transform.parent);

            other.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;

            other.transform.GetChild(3).gameObject.SetActive(true);
            Destroy(other.transform.GetChild(3).gameObject, 1f);

            gameManager.audioManager.Play("FillCup");
>>>>>>> e135bd62164667161091742e0478e6084b9b368d


            //GameObject getEffect = Instantiate(gameManager.getJuiceParticle, transform.position + Vector3.up, Quaternion.identity);
            //Destroy(getEffect, 1f);
<<<<<<< HEAD
        } 
=======
        }
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    }
}
