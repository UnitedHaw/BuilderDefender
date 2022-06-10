using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private ResourceNearbuyOverlay resourceNearbuyOverlay;
    private GameObject spriteGameObject;
    private void Awake()
    {
        spriteGameObject = transform.Find("sprite").gameObject;
        resourceNearbuyOverlay = transform.Find("pfResourceNearbuyOverlay").GetComponent<ResourceNearbuyOverlay>();
        Hide();
    }
    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if(e.activeBuildingType ==null)
        {
            Hide();
            resourceNearbuyOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);
            if (e.activeBuildingType.hasResourceGenerationData)
            {
                resourceNearbuyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            }
            else
            {
                resourceNearbuyOverlay.Hide();
            }
            
        }
    }

    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite)
    {
        spriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void Hide()
    {
        spriteGameObject.SetActive(false);
    }
}
