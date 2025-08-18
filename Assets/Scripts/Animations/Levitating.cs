using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitating : MonoBehaviour
{

    public float speed = 5f;
    protected float distance;
    private float arcPos;
    protected bool isLevitating = false;

    void Start()
    {

        distance = 0;
    }

    void Update()
    {


        if (!isLevitating)
        {

            return;
        }

        if (distance > 0)
        {

            //distance = Vector3.Distance(new Vector3(target.x, 0, target.z), new Vector3(transform.position.x, 0, transform.position.z));

            //arcPos = Mathf.Clamp(CalcArc(distance), 0f, 1f);

            //transform.position = Vector3.MoveTowards(
                                                        //transform.position,
                                                        //target + new Vector3(0, arcPos, 0),
                                                        //speed * Time.deltaTime
                                                    //);

        }
    }
}

