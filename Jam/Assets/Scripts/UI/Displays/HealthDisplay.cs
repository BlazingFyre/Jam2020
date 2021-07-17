using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [Header("Data")]
    public EntityData EntityData;
    public ColorVariable HealthColor;
    public ColorVariable GraceColor;
    public ColorVariable GritColor;

    [Header("Images")]
    public Image HealthBar;
    public Image GraceBar;
    public Image GritBar;

    [Header("Text")]
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI GraceText;
    public TextMeshProUGUI GritText;

    public void Start()
    {
        HealthBar.color = HealthColor.Value;
        GraceBar.color = GraceColor.Value;
        GritBar.color = GritColor.Value;

        GraceText.outlineColor = GraceColor.Value;
        GritText.outlineColor = GritColor.Value;
    }

    public void FixedUpdate()
    {
        UpdateHealthBar();
        UpdateGraceAndGrit();
        UpdateText();
    }

    private void UpdateHealthBar()
    {
        if (EntityData.Health.Value > EntityData.Health.Max)
        {
            HealthBar.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            HealthBar.transform.localPosition = new Vector3(
            -1 * (1 - (EntityData.Health.Value / EntityData.Health.Max)) * HealthBar.rectTransform.rect.width,
            HealthBar.transform.localPosition.y,
            HealthBar.transform.localPosition.z);
        }
    }

    private void UpdateGraceAndGrit()
    {
        GraceBar.enabled = EntityData.Grace.Value > EntityData.Grace.Min;
        GritBar.enabled = EntityData.Grit.Value > EntityData.Grit.Min;

        GraceText.enabled = EntityData.Grace.Value > EntityData.Grace.Min;
        GritText.enabled = EntityData.Grit.Value > EntityData.Grit.Min;
    }

    private void UpdateText()
    {
        HealthText.text = EntityData.Health.Value + " / " + EntityData.Health.Max;
        GraceText.text = EntityData.Grace.Value.ToString();
        GritText.text = EntityData.Grit.Value.ToString();
    }
}
