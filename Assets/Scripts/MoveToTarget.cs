using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveToTarget : MonoBehaviour
{
    
    public Vector3 target;
    private float distance;
    public float speed = 5f;

    void Start()
    {
        distance = 0;
    }

    void Update()
    {
        if(distance > 0)
        {

            distance = Vector3.Distance(new Vector3(target.x, 0 , target.z), new Vector3(transform.position.x, 0, transform.position.z));
            float arcPos = CalcArc(distance);
            arcPos = Mathf.Clamp(arcPos, 0f, 1f);

            transform.position = Vector3.MoveTowards(transform.position, 
                                target + new Vector3(0, arcPos, 0), 
                                speed * Time.deltaTime);
        } 
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
        distance = Vector3.Distance(new Vector3(target.x, 0 , target.z), new Vector3(transform.position.x, 0, transform.position.z));
    }

    private float CalcArc(float xVal)
    {

        return -Mathf.Pow(xVal,2) + Mathf.Sqrt(2) * xVal;
    }

    public float GetRemainingDistance()
    {

        return distance;
    }
}
