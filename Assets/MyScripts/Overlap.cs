using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Overlap : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(gameObject.transform.position, new Vector3(0, 0, 1)); //Z+•ûŒü‚ÌRay
        float maxDistance = 3f;
        hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance, layerMask);
        if (hit.collider)
        {
            Destroy(hit.collider.gameObject);
        }
    }
}
