using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{

    private float speed = 8f;
    private Vector3 start;
    private Vector3 target;
    private bool attacking = false;
    private bool returning = false;

    void Update()
    {
        
         if(attacking == true && returning == false)
        {

            //move to target from start position
            transform.position = Vector3.MoveTowards(
                                                        transform.position, 
                                                        target, 
                                                        speed * Time.deltaTime
                                                    );

            if(transform.position == target)
            {

                //transition from attacking to returning
                returning = true;
            }

        }else if(attacking == true && returning == true)
        {
            
            //return to start position from target
            transform.position = Vector3.MoveTowards(
                                                        transform.position, 
                                                        start, 
                                                        speed * Time.deltaTime
                                                    );

            if(transform.position == start)
            {

                //finish animation and return to neutral state
                attacking = false;
                returning = false;
            }
        }
    }

    public void MeleeAttack(float speed = 8f, float attackDistance = 0.5f)
    {

        this.speed = speed;
        this.start = transform.position;
        this.target = transform.position + transform.forward * attackDistance;
        attacking = true;
    }

    public bool IsAttacking()
    {

        return attacking;
    }
}
