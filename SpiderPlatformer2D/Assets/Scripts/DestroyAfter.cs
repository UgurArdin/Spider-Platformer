using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float after;
    void Start()
    {
        Destroy(gameObject, after);
    }

}
