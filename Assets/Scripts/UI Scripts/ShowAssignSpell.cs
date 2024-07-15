using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAssignSpell : MonoBehaviour
{

    private Tooltip tooltip;
    private SpellSlot spellSlot;
    private IsMouseOverUI mouseOver;
    private UIActiveManager uiam;
    [SerializeField] private SpellAssignmentManager sam;

    void Start()
    {

        tooltip = gameObject.GetComponent<Tooltip>();
        spellSlot = gameObject.GetComponent<SpellSlot>();
        mouseOver = gameObject.GetComponent<IsMouseOverUI>();

        GameObject managers = GameObject.Find("System Managers");
        uiam = managers.GetComponent<UIActiveManager>();
    }

    void Update()
    {

        if(mouseOver.IsMouseOverSelf() && Input.GetMouseButtonUp(1))
        {
            if(!uiam.assignSpellContainerIsOpen)
            {

                sam.EnableSpellAssignment(spellSlot, tooltip);
                uiam.ShowAssignSpell();   

            }else
            {

                sam.DisableSpellAssignment();
                uiam.HideAssignSpell(); 
            }
                       
        }
    }
}
