using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNotification : MonoBehaviour
{

    public Vector3 terminalTarget;
    public float speed;
    private bool initialized = false;

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
