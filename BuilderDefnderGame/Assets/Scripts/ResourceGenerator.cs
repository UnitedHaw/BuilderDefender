using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public static int GetNearbuyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        Collider2D[] Collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

        int nearBuyResourceAmount = 0;
        foreach (Collider2D collider2D in Collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                {
                    nearBuyResourceAmount++;
                }
            }
        }
        nearBuyResourceAmount = Mathf.Clamp(nearBuyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        return nearBuyResourceAmount;
    }

    private ResourceGeneratorData resourceGeneratorData;
    private float timer;
    private float timerMax;

    private void Awake()
    {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    private void Start()
    {
        int nearBuyResourceAmount = GetNearbuyResourceAmount(resourceGeneratorData, transform.position);

        if(nearBuyResourceAmount == 0)
        {
            enabled = false;
        }
        else
        {
            timerMax = (resourceGeneratorData.timerMax / 2f) + resourceGeneratorData.timerMax * (1 - (float)nearBuyResourceAmount / resourceGeneratorData.maxResourceAmount);
        }
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            timer += timerMax;
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }      
    }

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return resourceGeneratorData;
    }

    public float GetTimerNormalized()
    {
        return timer / timerMax;
    }
    public float GetAmountGeneratedPerSecond()
    {
        return 1 / timerMax;
    }
}
