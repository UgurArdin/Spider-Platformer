using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BeeEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public AIPath aiPath;
    [SerializeField] private SpriteRenderer sr;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            sr.flipX = true;
        }
        else if(aiPath.desiredVelocity.x <=-0.01f)
        {
            sr.flipX = false;
        }
    }
}
