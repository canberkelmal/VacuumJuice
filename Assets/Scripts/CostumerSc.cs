using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CostumerSc : MonoBehaviour
{
    public string askFor = "apple";
    public float movementSpeed = 1f;
    public bool isWaiting = false;
    public bool isHandled = false;
    public bool onQueue = false;
    public bool noResource = false;
    public Transform productPlace;
    public SkinnedMeshRenderer shirtRenderer, pantRenderer, hairRenderer;

    private IdleManager idleManager;
    private Vector3 destination = Vector3.zero;
    private RawImage statuUI;
    private Material shirtMat, pantMat, hairMat; 

    void Start()
    {
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
        switch (idleManager.currentLevel)
        {
            case 1:
                askFor = Random.Range(1, 100) < 50 ? "apple" : "orange";
                break;
            case 2: 
                int a = Random.Range(1, 150); 
                if (a <= 50)
                    askFor = "apple"; 
                else if (a > 50 && a <= 100)
                    askFor = "orange";
                else
                    askFor = "frozen";

                break;
        }
        statuUI = transform.Find("Canvas").Find("Statu").GetComponent<RawImage>();
        statuUI.texture = idleManager.SetTexture(askFor); 
        productPlace = transform.Find("ProductPlace");
        SetCostume();
    } 

    public void SetCostume()
    {
        shirtMat = new Material(shirtRenderer.material);
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        shirtMat.color = randomColor;
        shirtRenderer.material = shirtMat;

        pantMat = new Material(pantRenderer.material);
        randomColor = new Color(Random.value, Random.value, Random.value);
        pantMat.color = randomColor;
        pantRenderer.material = pantMat;

        hairMat = new Material(hairRenderer.material);
        int randomInt = Mathf.FloorToInt(Random.Range(0, idleManager.hairColors.Length));
        randomColor = idleManager.hairColors[randomInt];
        hairMat.color = randomColor;
        hairRenderer.material = hairMat;
    }

    public void SendTo(Vector3 finalPoint)
    {
        destination = finalPoint;
        transform.LookAt(destination);
        transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
        //transform.Find("CostumerObj").GetComponent<Animator>().SetBool("Walk", true);
        transform.Find("Obj").GetComponent<Animator>().SetTrigger("Walk");
        InvokeRepeating("GoToDestination", 0, Time.fixedDeltaTime);
    }

    private void GoToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
        if (transform.position == destination)
        {
            onQueue = true;
            transform.Find("Obj").GetComponent<Animator>().SetTrigger("Idle");
            transform.rotation= Quaternion.Euler(0f, 180f, 0f);
            transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
            idleManager.CostumerAsksFor(askFor, gameObject);
            CancelInvoke("GoToDestination");
        }
    }


    public void TakeAndGo(bool taken, GameObject handledProduct)
    {
        transform.parent = idleManager.transform.parent; 
        transform.LookAt(idleManager.costumerExitPoint.position);
        transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
        if (taken) 
        {
            statuUI.texture = idleManager.SetTexture("happy"); 
            transform.Find("Obj").GetComponent<Animator>().SetTrigger("Handle");
            GameObject particle = Instantiate(idleManager.takeProductParticle, transform.position + Vector3.up, Quaternion.Euler(Vector3.right * -90), idleManager.transform.parent);
            Destroy(particle, 1);
        }
        else
        { 
            statuUI.texture = idleManager.SetTexture("unHappy");
            transform.Find("Obj").GetComponent<Animator>().SetTrigger("Walk");
            noResource = true;
        }

        if(handledProduct != null)
        {
            handledProduct.transform.parent = productPlace; 
            handledProduct.transform.localPosition = Vector3.zero;
        } 
        idleManager.SetCostumerPlaceAvailable(transform.position); 
        idleManager.SentCostumer(gameObject, taken);
        //transform.Find("CostumerObj").GetComponent<Animator>().SetBool("Walk", true);
        InvokeRepeating("GoToExit", 0, Time.fixedDeltaTime);
    }
     
    private void GoToExit()
    {
        transform.position = Vector3.MoveTowards(transform.position, idleManager.costumerExitPoint.position, movementSpeed * Time.deltaTime);
        if (transform.position == idleManager.costumerExitPoint.position)
        {
            //Debug.Log("Costumer went."); 
            Destroy(gameObject, 1);
            CancelInvoke("GoToExit");
        }
    }
}
