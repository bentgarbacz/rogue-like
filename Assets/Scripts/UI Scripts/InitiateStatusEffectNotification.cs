using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class InitiateStatusEffectNotification : MonoBehaviour
{

    [SerializeField] private Tooltip tooltip;
    public Button statusEffectIcon;
    public TextMeshProUGUI effectDuration;

    public void SetInfo(StatusEffect statusEffect)
    {

        tooltip.SetTooltip(statusEffect.GetDescription());
        this.statusEffectIcon.image.sprite = statusEffect.sprite;
        this.effectDuration.text = statusEffect.duration.ToString();
    }
}
