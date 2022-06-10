using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private float constructionTimer;
    private float constructionTimerMax;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private Material constructionMaterial;

    private void Awake()
    {
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        constructionMaterial = spriteRenderer.material;

        Instantiate(GameAssets.Instance.pfBuildingPlacedParticles, transform.position, Quaternion.identity);
    }
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        Transform buildingConstructionTransform = Instantiate(GameAssets.Instance.pfBuildingConstruction, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingType);

        return buildingConstruction;
    }

    private void Update()
    {
        constructionTimer -= Time.deltaTime;

        constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalized());
        if(constructionTimer <= 0f)
        {
            Debug.Log("Ding!");
            Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
            Instantiate(GameAssets.Instance.pfBuildingPlacedParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void SetBuildingType(BuildingTypeSO buildingType)
    {
        this.buildingType = buildingType;

        constructionTimerMax = buildingType.constructionTimerMax;
        constructionTimer = constructionTimerMax;

        spriteRenderer.sprite = buildingType.sprite;

        boxCollider2D.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider2D.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;
    }
    public float GetConstructionTimerNormalized()
    {
        return 1 - constructionTimer / constructionTimerMax;
    }
}
