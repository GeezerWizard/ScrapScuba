using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputMovement
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public Vector3 GetTargetPosition(int layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
        {
            return hit.point;
        }
        else return Vector3.zero;
/*            if (GetDistance() > clickDistanceToStartMoving)
            {
                moving = true;
            }*/
    }
}