using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web_Shooter : MonoBehaviour
{
    public  int rotationOffset =90;

    
    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rot2 = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot2 + rotationOffset);
    }
}
