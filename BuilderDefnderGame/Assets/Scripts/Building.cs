using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;
    private Transform buildingDemolishBtn;
    private Transform buildingRepairBtn;

    private void Awake()
    {
        buildingDemolishBtn = transform.GetChild(3);
        buildingRepairBtn = transform.GetChild(4);

        HideBuildingDemolishBtn();
        HideBuildingReapairBtn();
       
    }
    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        healthSystem = GetComponent<HealthSystem>();

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        if(healthSystem.IsFullHealth())
        {
            HideBuildingReapairBtn();
        }
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        ShowBuildingRepairBtn();
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        CinemachineShake.Instance.ShakeCamera(5f, .15f);
        ChromaticAberration.Instance.SetWeight(1f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        Instantiate(GameAssets.Instance.pfBuildingDestroyedParticles, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(7f, .2f);
        ChromaticAberration.Instance.SetWeight(1.5f);
    }

    private void OnMouseEnter()
    {
        ShowBuildingDemolishBtn();
    }

    private void OnMouseExit()
    {
        HideBuildingDemolishBtn();
    }

    private void ShowBuildingDemolishBtn()
    {
        if (buildingDemolishBtn != null)
        {
            buildingDemolishBtn.gameObject.SetActive(true);
        }
    }

    private void HideBuildingDemolishBtn()
    {
        if (buildingDemolishBtn != null)
        {
            buildingDemolishBtn.gameObject.SetActive(false);
        }
    }

    private void ShowBuildingRepairBtn()
    {
        if (buildingRepairBtn != null)
        {
            buildingRepairBtn.gameObject.SetActive(true);
        }
    }

    private void HideBuildingReapairBtn()
    {
        if (buildingRepairBtn != null)
        {
            buildingRepairBtn.gameObject.SetActive(false);
        }
    }
}
