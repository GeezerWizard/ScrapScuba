using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
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

    [SerializeField]
    float turnRate;

    [SerializeField]
    AnimationCurve turnRateCurve;
    float turnRateModifier;
    float turnMagnitude;

    bool moving = false;

    [SerializeField]
    float speed;

    [SerializeField]
    AnimationCurve speedRateCurve;
    float speedModifier;
    float speedModRate;

    void Start()
    {
        layerMask = 1 << 4;
        currentDirection = initialDirection;
        speedModRate = 1;
        turnMagnitude = 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, clickDistanceToStartMoving);
        Gizmos.DrawLine(transform.position, targetPosition);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
            {
                targetPosition = hit.point;
            }

            if (GetDistance() > clickDistanceToStartMoving)
            {
                moving = true;
                print("moving = " + moving);
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
            print("moving = " + moving);
        }
    }

    float GetDistance()
    {
        return Vector3.Distance(targetPosition, transform.position);
    }
}
