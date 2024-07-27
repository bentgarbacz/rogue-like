using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    private Vector3 target;
    private bool shooting = false;
    private AudioSource projectileAudioSource;
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip hit;

    void Awake()
    {

        projectileAudioSource = GetComponent<AudioSource>();
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
                
                shooting = false;
                StartCoroutine(HitTarget());
            }
        }
    }

    private IEnumerator HitTarget()
    {

        //Remove all visual components of projectile on hit
        Destroy(gameObject.GetComponent<MeshFilter>());
        Destroy(gameObject.GetComponent<MeshRenderer>());

        foreach (Transform child in transform)
        {

            Destroy(child.gameObject);
        }

        //Play hit sound clip then wait for sound clip to finish
        projectileAudioSource.PlayOneShot(hit);
        yield return new WaitForSeconds(hit.length);   
        
        Destroy(gameObject);
    }

    public void Shoot(Vector3 target, AudioSource originAudioSource, float speed = 15f)
    {

        this.speed = speed;
        this.target = target;
        originAudioSource.PlayOneShot(shot);
        this.shooting = true;
    }
}
