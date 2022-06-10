using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string stringName;
    public Transform prefab;
    public bool hasResourceGenerationData;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstructionRadius;
    public ResourceAmount[] constructionResourceCostArray; 
    public int healthAmountMax;
    public float constructionTimerMax;

    public string GetConstructionResourceCoastString()
    {
        string str = "";
        foreach(ResourceAmount resourceAmount in constructionResourceCostArray)
        {
            str += "<color=#" + resourceAmount.resourceType.colorHex + ">" + 
                resourceAmount.resourceType.nameShort + resourceAmount.amount + 
                "</color> ";

        }
        return str;
    }
}
