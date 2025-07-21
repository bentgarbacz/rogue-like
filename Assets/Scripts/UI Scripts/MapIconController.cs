using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIconController : MonoBehaviour
{

    [SerializeField] private Image icon;
    private GameObject parentObject = null;
    public RectTransform iconRectTransform;

    public void InitializeController(GameObject parentObject, Sprite sprite, Vector2Int coord, int sortingOrder, float scaleFactor = 1.0f)
    {

        iconRectTransform = GetComponent<RectTransform>();

        //transform.SetSiblingIndex(sortingOrder);

        SetParentObject(parentObject);
        SetIcon(sprite);
        SetCoord(coord);
        ScaleIcon(scaleFactor);
    }

    public void SetParentObject(GameObject parentObject)
    {

        this.parentObject = parentObject;
    }

    public void SetIcon(Sprite sprite)
    {

        icon.sprite = sprite;
    }

    public Image Icon()
    {

        return icon;
    }

    public void ScaleIcon(float scaleFactor)
    {

        iconRectTransform.localScale *= scaleFactor;
    }

    public bool UpdateIcon()
    {

        if (parentObject == null)
        {

            Destroy(gameObject);
            return false;
        }

        if (parentObject.GetComponent<Loot>() is Loot loot)
        {

            SetCoord(loot.loc.coord);

        } else if (parentObject.GetComponent<CharacterSheet>() is CharacterSheet character)
        {

            SetCoord(character.loc.coord);

        }

        return true;
    }

    public void SetCoord(Vector2Int coord)
    {

        iconRectTransform.anchoredPosition = coord;
        iconRectTransform.anchoredPosition *= 11;
    }

    public void SetVisibility(bool state)
    {
        
        gameObject.SetActive(state);
    }
}
