using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }

    [SerializeField] private Building hqBuilding;

    private BuildingTypeSO activeBuildingType;
    private BuildingTypeListSO buildingTypeList;
    

    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

    }
    void Start()
    {
        mainCamera = Camera.main;

        hqBuilding.GetComponent<HealthSystem>().OnDied += HQ_OnDied;
    }

    private void HQ_OnDied(object sender, EventArgs e)
    {
        GameOverUI.Instance.Show();
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null)
            {
                if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMassage))
                {
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                    {
                        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        //Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    }
                    else
                    {
                        TooltipUI.Instance.Show("Cannot afford " + activeBuildingType.GetConstructionResourceCoastString(), 
                            new TooltipUI.TooltipTimer { timer = 2f });
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(errorMassage, new TooltipUI.TooltipTimer { timer = 2f });
                }
            }
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this,
            new OnActiveBuildingTypeChangedEventArgs{ activeBuildingType = activeBuildingType}
            );
    }
    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        var boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear) 
        {
            errorMessage = "Area is not clear!";
            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        foreach(Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if(buildingTypeHolder != null)
            {
                //Has BuildingTypeHolder
                if(buildingTypeHolder.buildingType == buildingType)
                {
                    //Theres already a buildings of this type within constructions radius
                    errorMessage = "To close to another building of the same type!";
                    return false;
                }
            }
        }

        if(buildingType.hasResourceGenerationData)
        {
            ResourceGeneratorData resourceGeneratorData = buildingType.resourceGeneratorData;
            int nearbuyResourceAmount = ResourceGenerator.GetNearbuyResourceAmount(resourceGeneratorData, position);

            if(nearbuyResourceAmount == 0)
            {
                errorMessage = "There are no nearby Resource Nodes!";
                return false;
            }
        }
        

        float maxConstructionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (var collider2D in collider2DArray)
        {
            var buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                errorMessage = "";
                return true; 
            }
        }
        errorMessage = "To far from any other building!";
        return false;
    }

    public Building GetHQBuilding()
    {
        return hqBuilding;
    }

}
