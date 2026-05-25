using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CharacterModificationNotification : MonoBehaviour
{

    [SerializeField] private Tooltip tooltip;
    public Button modifierIcon;
    public TextMeshProUGUI effectDuration;
    public GameObject effectDurationBox;

    public void SetInfo(CharacterModifier mod, bool removeDuration = false)
    {

        tooltip.SetTooltip(mod.GetDescription());
        this.modifierIcon.image.sprite = mod.sprite;
        this.effectDuration.text = mod.duration.ToString();

        if(removeDuration)
        {

            RemoveEffectDuration();
        }
    }

    public void SetDuration(string text)
    {
        
        this.effectDuration.text = text;
    }

    public void RemoveEffectDuration()
    {

        if (effectDuration != null)
        {

            Destroy(effectDurationBox);
            effectDuration = null;
        }
    }
}
