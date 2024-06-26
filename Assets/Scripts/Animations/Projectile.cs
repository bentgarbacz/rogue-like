using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 8f;
    private Vector3 target;
    private bool shooting = false;
    public AudioSource audioSource;
    public AudioClip shot;
    public AudioClip hit;

    void Awake()
    {

        shot = Resources.Load<AudioClip>("Sounds/OpenBag");
        hit = Resources.Load<AudioClip>("Sounds/Arrow");
    }

    void Update()
    {
        
        if(shooting == true)
        {

            //move to target from start position
            transform.position = Vector3.MoveTowards(
                                                        transform.position, 
                                                        target, 
                                                        speed * Time.deltaTime
                                                    );

            //Destroy self after reaching target
            if(transform.position == target)
            {

                audioSource.PlayOneShot(hit);
                Destroy(gameObject);
            }
        }
    }

    public void Shoot(Vector3 target, AudioSource audioSource, float speed = 15f)
    {

        this.speed = speed;
        this.target = target;
        this.audioSource = audioSource;
        this.audioSource.PlayOneShot(shot);
        this.shooting = true;
    }
}
