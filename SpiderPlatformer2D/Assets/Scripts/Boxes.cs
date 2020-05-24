using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    private void Awake()
    {
        PlayerController.DestroyBoxesInPlayerController += DestroyMe;
        Grapple.DestroyBoxes += DestroyMe;
    }
    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        PlayerController.DestroyBoxesInPlayerController -= DestroyMe;
        Grapple.DestroyBoxes -= DestroyMe;
    }
}
