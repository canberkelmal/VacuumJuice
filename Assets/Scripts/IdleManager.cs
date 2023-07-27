using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleManager : MonoBehaviour
{
    public GameObject machinesParent;
    public GameObject costumersParent;
    public GameObject costumer;
    public Transform costumerSpawnPoint;
    public Transform costumerLastPoint;

    private int costumerCount = 0;
    private GameObject[] machines;
    private GameObject[] costumers = new GameObject[0];
    // Start is called before the first frame update
    void Start()
    {
        FillMachinesArray();
        costumerCount = costumersParent.transform.childCount;
    }

    private void FillMachinesArray()
    {
        machines = new GameObject[machinesParent.transform.childCount-1];

        for(int i = 0; i< machines.Length; i++)
        {
            machines[i] = machinesParent.transform.GetChild(i).gameObject;
        }
    }

    public void SpawnCostumer()
    {
        costumerCount++;
        GameObject newCostumer = Instantiate(costumer, costumerSpawnPoint.position, Quaternion.identity, costumersParent.transform);
        newCostumer.GetComponent<CostumerSc>().SendTo(costumerLastPoint.position - (Vector3.right * 1.5f * (costumerCount-1) ));
        AddToCostumersArray(newCostumer);
    }

    private void DeleteFromCostumersArray(GameObject deletedObj)
    {
        //X array inden deletedObj isimli objeyi bulup sil
    }
    private void AddToCostumersArray(GameObject addedObj)
    {
        if (addedObj == null)
        {
            Debug.Log("Null obj is tried to add costumers array");
            return;
        }

        GameObject[] newArray = new GameObject[costumers.Length + 1];
        for (int i = 0; i < costumers.Length; i++)
        {
            newArray[i] = costumers[i];
        }

        newArray[costumers.Length] = addedObj;
        costumers = newArray;

        Debug.Log("Costumers is added to costumers array");
    }

}
