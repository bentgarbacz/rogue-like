using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class SpellAssignmentManager : MonoBehaviour
{

    private SpellSlot currentSpellSlot = null;
    private Tooltip currentTooltip = null;
    private RectTransform spellAssignmentContainerRect;
    [SerializeField] private PlayerCharacterSheet pc;
    [SerializeField] private GameObject spellIconPrefab;
    [SerializeField] private GameObject spellIconGrid;

    void Awake()
    {
        
        spellAssignmentContainerRect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {

        int iconCount = 1 + pc.knownSpells.Keys.Count; //one icon for clearing spell plus an icon for each known spell
        int rectDimensionX = 20 + 75 * Math.Min(iconCount, 3); //20 units for padding, 75 * number of columns, max 3 columns
        int rectDimensionY = 20 + 75 * ((iconCount + 2) / 3); //20 units for padding, 75 * number of rows

        GameObject clearSpellIcon = Instantiate(spellIconPrefab, spellIconGrid.transform);
        clearSpellIcon.GetComponent<SpellSelector>().SetSpell("", currentSpellSlot, currentTooltip);

        foreach(string spell in pc.knownSpells.Keys)
        {

            GameObject newSpellIcon = Instantiate(spellIconPrefab, spellIconGrid.transform);
            newSpellIcon.GetComponent<SpellSelector>().SetSpell(spell, currentSpellSlot, currentTooltip);
        }

        spellAssignmentContainerRect.sizeDelta = new Vector2(rectDimensionX, rectDimensionY);
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
