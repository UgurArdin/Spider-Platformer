 using UnityEngine;
 
 public class PlayerInteract : MonoBehaviour {
 
     public GameObject dialogue;
 
     void OnTriggerEnter2D(Collider2D collision)
     {
         if (collision.CompareTag("InterObject"))
         {
             dialogue.SetActive(true);
         }
     }
 
     // Update is called once per frame
     void Update () {
         
     }
 }