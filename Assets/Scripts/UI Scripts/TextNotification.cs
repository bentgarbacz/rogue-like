using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TextNotification : MonoBehaviour
{

    public Vector3 terminalTarget;
    public float speed;
    private bool initialized = false;
    [SerializeField] public Image background;
    [SerializeField] public TextMeshPro text;

    void Update()
    {
        
        if(initialized)
        {

            transform.position = Vector3.MoveTowards(transform.position, terminalTarget, speed * Time.deltaTime);

            if(transform.position == terminalTarget)
            {

                Destroy(gameObject);
            }
        }
    }

    public void InitText(Vector3 terminalTarget, float speed)
    {

        this.terminalTarget = terminalTarget;
        this.speed = speed;
        initialized = true;
    }
}
