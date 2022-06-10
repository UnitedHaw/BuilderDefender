using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;


    [SerializeField] private List<ResourceAmount> startingResourceAmountList;
    private Dictionary<ResourceTypeSO, int> resorceAmountDictionary;
    private void Awake()
    {
        Instance = this;

        resorceAmountDictionary = new Dictionary<ResourceTypeSO, int>();
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resorceAmountDictionary[resourceType] = 0;
        }
        foreach(ResourceAmount resourceAmount in startingResourceAmountList)
        {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }
    }
    private void TestLogForDictinary()
    {
        foreach(ResourceTypeSO resourceType in resorceAmountDictionary.Keys)
        {
            Debug.Log(resourceType.nameString +": " + resorceAmountDictionary[resourceType]);
        }
    }
    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resorceAmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

    }

    public int GetResourceAmount(ResourceTypeSO resourseType)
    {
        return resorceAmountDictionary[resourseType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        foreach(ResourceAmount resourceAmount in resourceAmountArray)
        {
            if(GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {
    
            }
            else
            {
                return false;
            }
        }return true;
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            resorceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }
}
