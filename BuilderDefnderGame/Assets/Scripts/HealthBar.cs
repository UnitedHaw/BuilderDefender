using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    private Transform barTransform;
    private Transform separatorContainer;
    private Transform separatorTemplate;

    private void Awake()
    {
        barTransform = transform.Find("bar");
    }

    private void Start()
    {
        separatorContainer = transform.Find("separatorContainer");
        ConstructHealthBarSeparators();

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnHealthAmountMaxChanged += HealthSystem_OnHealthAmountMaxChanged;

        UpdateBar();
        UpdateHelthBarVisible();
    }

    private void ConstructHealthBarSeparators()
    {
        
        separatorTemplate = separatorContainer.Find("separatorTemplate");
        separatorTemplate.gameObject.SetActive(false);

        foreach(Transform separatorTransform in separatorContainer)
        {
            if (separatorTransform == separatorTemplate) continue;
            Destroy(separatorTransform.gameObject);
        }

        int healthAmountPerSeparator = 10;
        float barSize = 4f;
        float barOneHealthAmountSize = barSize / healthSystem.GetHealthAmountMax();
        int healthSeparatorCount = Mathf.FloorToInt(healthSystem.GetHealthAmountMax() / healthAmountPerSeparator);

        for (int i = 1; i < healthSeparatorCount; i++)
        {
            Transform separatorTransform = Instantiate(separatorTemplate, separatorContainer);
            separatorTransform.gameObject.SetActive(true);
            separatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i * healthAmountPerSeparator, 0, 0);
        }
    }

    private void HealthSystem_OnHealthAmountMaxChanged(object sender, System.EventArgs e)
    {
        ConstructHealthBarSeparators();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHelthBarVisible();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHelthBarVisible();
    }

    private void UpdateBar()
    {
        barTransform.localScale = new Vector3(healthSystem.GetHealthNormalized(), 1, 1);
    }

    private void UpdateHelthBarVisible()
    {
        if (healthSystem.IsFullHealth())
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }
}
