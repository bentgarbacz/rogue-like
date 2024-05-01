using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveToTarget : MonoBehaviour
{
    
    public Vector3 target;
    public float speed = 5f;

    void Update()
    {
        
        //transform.position = Vector3.MoveTowards(transform.position, target + new Vector3(0,calcArc(Vector3.Distance(target, transform.position)),0), speed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        
        
    }

/*     public IEnumerator Move(Vector3 target)
    {
        while(transform.position.x != target.x && transform.position.z != target.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        yield return new WaitForSeconds(2);
    }

    public IEnumerator MoveRoutine(Vector3 target){

        while(transform.position.x != target.x && transform.position.z != target.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
        
    } */

    private float calcArc(float xVal)
    {
        return -(xVal*xVal) + xVal;
    }
}
