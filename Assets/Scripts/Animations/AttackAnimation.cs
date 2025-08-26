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

        if (attacking == false && returning == false)
        {

            return;
        }

        Vector3 currentOffestY = new(0f, transform.position.y, 0f);

        if (attacking == true && returning == false)
        {

            //move to target from start position
            transform.position = Vector3.MoveTowards(
                                                        transform.position,
                                                        target + currentOffestY,
                                                        speed * Time.deltaTime
                                                    );

            if (transform.position == target + currentOffestY)
            {

                //transition from attacking to returning
                returning = true;
            }

        }
        else if (attacking == true && returning == true)
        {

            //return to start position from target
            transform.position = Vector3.MoveTowards(
                                                        transform.position,
                                                        start + currentOffestY,
                                                        speed * Time.deltaTime
                                                    );

            if (transform.position == start + currentOffestY)
            {

                //finish animation and return to neutral state
                attacking = false;
                returning = false;
            }
        }
    }

    public void MeleeAttack(float speed = 8f, float attackDistance = 0.5f)
    {

        Vector3 startVector2D = new(transform.position.x, 0f, transform.position.z);

        this.speed = speed;
        this.start = startVector2D;
        this.target = startVector2D + transform.forward * attackDistance;
        attacking = true;
    }

    public bool IsAttacking()
    {

        return attacking;
    }
}
