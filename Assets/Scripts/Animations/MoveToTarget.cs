using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class MoveToTarget : MonoBehaviour
{

    public Vector3 target;
    public float speed = 5f;
    protected float distance;
    private float arcPos;
    protected bool moving = false;
    protected bool makesNoise = false;
    public AudioSource audioSource;
    public AudioClip stepAudioClip;
    public float waitTime = 0f;

    void Start()
    {

        distance = 0;
    }

    void Update()
    {

        if (waitTime > 0)
        {

            waitTime -= Time.deltaTime;

        }
        else
        {

            if (distance > 0)
            {

                distance = Vector3.Distance(new Vector3(target.x, 0, target.z), new Vector3(transform.position.x, 0, transform.position.z));

                arcPos = Mathf.Clamp(CalcArc(distance), 0f, 1f);

                transform.position = Vector3.MoveTowards(
                                                            transform.position,
                                                            target + new Vector3(0, arcPos, 0),
                                                            speed * Time.deltaTime
                                                        );

            }else if (moving && distance == 0)
            {

                OnArrive();
            }
        }
    }

    public void SetTarget(Vector3 target, float waitTime = 0f)
    {

        this.waitTime = waitTime;

        moving = true;
        this.target = target;
        distance = Vector3.Distance(new Vector3(target.x, 0, target.z), new Vector3(transform.position.x, 0, transform.position.z));
    }

    private float CalcArc(float xVal)
    {

        return -Mathf.Pow(xVal, 2) + Mathf.Sqrt(2) * xVal;
    }

    public float GetRemainingDistance()
    {

        return distance;
    }

    public void SetNoise(AudioSource audioSource, AudioClip audioClip)
    {

        makesNoise = true;
        this.audioSource = audioSource;
        this.stepAudioClip = audioClip;
    }

    public bool IsMoving()
    {

        return moving;
    }

    protected virtual void OnArrive()
    {

        if (makesNoise)
        {

            audioSource.PlayOneShot(stepAudioClip);
        }

        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        moving = false;
    }
}
