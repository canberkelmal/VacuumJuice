using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using UnityEditor;
=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
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
<<<<<<< HEAD
    public Transform productPlace;
    public SkinnedMeshRenderer shirtRenderer, pantRenderer, hairRenderer;

    private IdleManager idleManager;
    private Vector3 destination = Vector3.zero;
    private RawImage statuUI;
    private Material shirtMat, pantMat, hairMat; 

    void Start()
    {
        Debug.Log("Awake costumer");
=======

    private Transform productPlace;
    private IdleManager idleManager;
    private Vector3 destination = Vector3.zero;
    private RawImage statuUI;

    void Awake()
    {
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
        switch (idleManager.currentLevel)
        {
            case 1:
                askFor = Random.Range(1, 100) < 50 ? "apple" : "orange";
                break;
<<<<<<< HEAD
            case 2: 
                int a = Random.Range(1, 150); 
                if (a <= 50)
                    askFor = "apple"; 
=======
            case 2:
                int a = Random.Range(1, 150);
                if (a <= 50)
                    askFor = "apple";
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
                else if (a > 50 && a <= 100)
                    askFor = "orange";
                else
                    askFor = "frozen";

                break;
        }
        statuUI = transform.Find("Canvas").Find("Statu").GetComponent<RawImage>();
<<<<<<< HEAD
        statuUI.texture = idleManager.SetTexture(askFor); 
        productPlace = transform.Find("ProductPlace");
        SetCostume();
    } 

    public void SetCostume()
    {
        Debug.Log("Set costumer costume");
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
=======
        statuUI.texture = idleManager.SetTexture(askFor);
        productPlace = transform.Find("ProductPlace");
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    }

    public void SendTo(Vector3 finalPoint)
    {
<<<<<<< HEAD
        Debug.Log("Sendto costumer");
        destination = finalPoint;
        transform.LookAt(destination);
        transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
        //transform.Find("CostumerObj").GetComponent<Animator>().SetBool("Walk", true);
        transform.Find("Obj").GetComponent<Animator>().SetTrigger("Walk");
=======
        destination = finalPoint;
        transform.LookAt(destination);
        transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
        transform.Find("CostumerObj").GetComponent<Animator>().SetBool("Walk", true);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        InvokeRepeating("GoToDestination", 0, Time.fixedDeltaTime);
    }

    private void GoToDestination()
    {
<<<<<<< HEAD
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.fixedDeltaTime);
=======
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        if (transform.position == destination)
        {
            if(!onQueue)
            {
<<<<<<< HEAD
                //idleManager.AddToQueue(gameObject);
                //Debug.Log("Reached to destination.");
            }
            //transform.Find("CostumerObj").GetComponent<Animator>().SetBool("Walk", false);
            onQueue = true;
            transform.Find("Obj").GetComponent<Animator>().SetTrigger("Idle");
            transform.rotation= Quaternion.Euler(0f, 180f, 0f);
            transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
            idleManager.CostumerAsksFor(askFor, gameObject);
=======
                idleManager.AddToQueue(gameObject);
                onQueue = true;
                //Debug.Log("Reached to destination.");
                idleManager.CostumerAsksFor(askFor, gameObject);
            }
            transform.Find("CostumerObj").GetComponent<Animator>().SetBool("Walk", false);
            transform.rotation= Quaternion.Euler(0f, 180f, 0f);
            transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
            CancelInvoke("GoToDestination");
        }
    }


    public void TakeAndGo(bool taken, GameObject handledProduct)
    {
<<<<<<< HEAD
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
        transform.position = Vector3.MoveTowards(transform.position, idleManager.costumerExitPoint.position, movementSpeed * Time.fixedDeltaTime);
        if (transform.position == idleManager.costumerExitPoint.position)
        {
            //Debug.Log("Costumer went."); 
            Destroy(gameObject, 1);
            CancelInvoke("GoToExit");
=======
        Destroy(Instantiate(idleManager.takeProductParticle, transform.position + Vector3.up, Quaternion.Euler(Vector3.right * -90)), 1);
        if (taken)
        {
            statuUI.texture = idleManager.SetTexture("happy");
        }
        else
        { 
            noResource = true;
            statuUI.texture = idleManager.SetTexture("unHappy");
        }
        if(handledProduct != null)
        {
            handledProduct.transform.parent = productPlace;
            handledProduct.transform.localPosition = Vector3.zero;
        }
        idleManager.SetCostumerPlaceAvailable(transform.position);
        idleManager.SentCostumer(gameObject, taken);
        transform.LookAt(idleManager.costumerExitPoint.position);
        transform.Find("Canvas").rotation = Quaternion.Euler(-45f, 180f, 0f);
        transform.Find("CostumerObj").GetComponent<Animator>().SetBool("Walk", true);
        InvokeRepeating("GoToExit", 0, Time.fixedDeltaTime); 
    }

    private void GoToExit()
    {
        CancelInvoke("GoToDestination");
        transform.position = Vector3.MoveTowards(transform.position, idleManager.costumerExitPoint.position, movementSpeed * Time.deltaTime);
        if (transform.position == idleManager.costumerExitPoint.position)
        {
            //Debug.Log("Costumer went.");
            CancelInvoke("GoToExit");
            Destroy(gameObject);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        }
    }
}
