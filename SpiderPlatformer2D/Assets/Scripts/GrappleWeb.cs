using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleWeb : MonoBehaviour
{

    private void Awake()
    {
        PlayerController.DestroyWebs += DestroyMe;
        Grapple.DestroyWebsInGrapple += DestroyMe;
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        PlayerController.DestroyWebs -= DestroyMe;
        Grapple.DestroyWebsInGrapple -= DestroyMe;
    }



}
