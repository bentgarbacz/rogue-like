using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveToTarget))]
public class Levitating : MonoBehaviour
{
    private float speed = 4.5f; // Controls oscillation speed
    private bool isLevitating = false;
    private float lowerY;
    private float upperY;
    private float initialY;
    private MoveToTarget entityMovement;
    private float phaseOffset;

    void Awake()
    {

        phaseOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {

        if (!isLevitating)
        {

            return;
        }

        float t = (Mathf.Sin(Time.time * speed + phaseOffset) + 1f) / 2f; // t in [0,1]
        float newY = Mathf.Lerp(lowerY, upperY, t);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void StartLevitating()
    {

        initialY = transform.position.y;
        lowerY = initialY + 0.125f;
        upperY = initialY + 0.375f;

        entityMovement = GetComponent<MoveToTarget>();
        entityMovement.jumpsWhileMoving = false;

        isLevitating = true;
    }

    public void EndLevitating()
    {

        entityMovement.jumpsWhileMoving = true;
        isLevitating = false;
        transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
    }
}