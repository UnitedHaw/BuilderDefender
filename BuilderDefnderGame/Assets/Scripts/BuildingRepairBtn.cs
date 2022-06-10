using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairBtn : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSO goldResourceType;
    private void Awake()
    {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => 
        {
            int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
            int repairCoast = missingHealth / 2;

            ResourceAmount[] resourceAmountCoast = new ResourceAmount[] {
                new ResourceAmount { resourceType = goldResourceType, amount = repairCoast} };

            if (ResourceManager.Instance.CanAfford(resourceAmountCoast))
            {
                ResourceManager.Instance.SpendResources(resourceAmountCoast);
                healthSystem.HealFull();
            }
            else
            {
                TooltipUI.Instance.Show("Cannot afford repair coast!", new TooltipUI.TooltipTimer { timer = 2f });
            }

            healthSystem.HealFull();
        });
    }
}
