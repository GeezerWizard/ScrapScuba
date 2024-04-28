using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    MouseInputMovement mouseInputMovement;

    Vector3 targetPosition;
    int layerMask;

    Vector3 initialDirection = Vector3.forward;
    Vector3 currentDirection;
    Vector3 targetDirection;
    Quaternion targetRotation;

    [SerializeField]
    float clickDistanceToStartMoving;

    [SerializeField]
    float slowDownDistance, stopDistance;
    float distance;

    //Turning variables
    [SerializeField]
    float turnRate;
    [SerializeField]
    AnimationCurve turnRateCurve;
    float turnRateModifier;
    float turnMagnitude;

    bool moving = false;
    bool allowMoving = true;
    public bool AllowMoving 
    { 
        get 
        { return allowMoving; } 
        set 
        { allowMoving = value; } 
    }

    //Speed variables
    [SerializeField]
    float speed;
    [SerializeField]
    AnimationCurve speedRateCurve;
    float speedModifier;
    float speedModRate;

    void Start()
    {
        mouseInputMovement = new MouseInputMovement();
        layerMask = 1 << 4;
        currentDirection = initialDirection;
        speedModRate = 1;
        turnMagnitude = 1;
    }

/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, clickDistanceToStartMoving);
        Gizmos.DrawLine(transform.position, targetPosition);
    }*/

    void Update()
    {
        if (Input.GetMouseButton(0) && allowMoving == true)
        {
            targetPosition = mouseInputMovement.GetTargetPosition(layerMask);

            if (GetDistance() > clickDistanceToStartMoving)
            {
                moving = true;
            }
        }

        if (moving)
        {
            distance = GetDistance();
        }

        if (distance < slowDownDistance)
        {
            speedModifier = speedRateCurve.Evaluate(distance / slowDownDistance);
            speedModRate = distance / slowDownDistance * speedModifier;
            turnRateModifier = turnRateCurve.Evaluate(distance / slowDownDistance);
            turnMagnitude = distance / slowDownDistance * turnRateModifier;
        }
        else
        {
            speedModRate = 1;
            turnMagnitude = 1;
        }
        
        if (distance > stopDistance)
        {
            targetDirection = (targetPosition - transform.position).normalized;
            targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnRate * turnMagnitude * Time.deltaTime); ;
            transform.position = transform.position + transform.forward * speed * speedModRate * Time.deltaTime;
        }
        else
        {
            moving = false;
        }
    }

    float GetDistance()
    {
        return Vector3.Distance(targetPosition, transform.position);
    }
}
