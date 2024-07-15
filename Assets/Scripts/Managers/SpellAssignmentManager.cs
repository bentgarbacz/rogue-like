using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpellAssignmentManager : MonoBehaviour
{

    private SpellSlot currentSpellSlot = null;
    private Tooltip currentTooltip = null;
    private RectTransform spellAssignmentContainerRect;
    [SerializeField] private PlayerCharacter pc;
    [SerializeField] private GameObject spellIconPrefab;
    [SerializeField] private GameObject spellIconGrid;

    void Start()
    {
        
        spellAssignmentContainerRect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {

        GameObject clearSpellIcon = Instantiate(spellIconPrefab, spellIconGrid.transform);
        clearSpellIcon.GetComponent<SpellSelector>().SetSpell("", currentSpellSlot, currentTooltip);

        foreach(string spell in pc.knownSpells.Keys)
        {

            GameObject newSpellIcon = Instantiate(spellIconPrefab, spellIconGrid.transform);
            newSpellIcon.GetComponent<SpellSelector>().SetSpell(spell, currentSpellSlot, currentTooltip);
        }

        //spellAssignmentContainerRect.sizeDelta = spellIconGrid.GetComponent<RectTransform>().sizeDelta + new Vector2(10, 10);
    }

    void OnDisable()
    {

        currentSpellSlot = null;
        currentTooltip = null;

        foreach (Transform child in spellIconGrid.transform)
        {

            Destroy(child.gameObject);
        }
    }

    public void EnableSpellAssignment(SpellSlot spellSlot, Tooltip tooltip)
    {

        currentSpellSlot = spellSlot;
        currentTooltip = tooltip;
    }

    public void DisableSpellAssignment()
    {

        currentSpellSlot = null;
        currentTooltip = null;
    }
}
