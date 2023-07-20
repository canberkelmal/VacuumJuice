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
        if (other.CompareTag("VacuumArea") && !gameManager.isTankEmpty)
        {
            Vector3 cupScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            Instantiate(gameManager.cupIcon, cupScreenPos, Quaternion.identity, gameManager.cupCountTx.transform.parent);
            //Destroy(Instantiate(gameManager.getCupParticle, transform.position, Quaternion.identity),1f);

            GameObject getEffect = Instantiate(gameManager.getJuiceParticle, transform.position + Vector3.up, Quaternion.identity);
            getEffect.GetComponent<ParticleSystem>().startColor = gameManager.liquidColor;
            getEffect.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = gameManager.liquidColor;
            getEffect.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = gameManager.liquidColor;
            Destroy(getEffect, 1f);

            gameManager.FillACup();
            transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject, gameManager.getCupDelay);
        }
    }
}
