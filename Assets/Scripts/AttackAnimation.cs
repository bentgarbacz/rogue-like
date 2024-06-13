using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{

    public float attackDistance = 1;
    public float speed = 5f;
    private float startPosition;
    private float currentPosition;
    private float endPosition;
    private Vector3 target;
    private bool returning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       /*  if(target != null && returning == false)
        {

            if()
            {
                transform.position = Vector3.MoveTowards(
                                                        transform.position, 
                                                        target.transform.positon, 
                                                        speed * Time.deltaTime
                                                    );

            }else
            {

                returning = true;
            }

        }else if(target != null && returning == true)
        {

            if()
            {
                transform.position = Vector3.MoveTowards(
                                                        transform.position, 
                                                        target.transform.positon, 
                                                        speed * Time.deltaTime
                                                    );

            }else
            {

                returning = false;
                target = null;
            } */
        //}
    }
}
