using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverMovement : MonoBehaviour
{
    [SerializeField]
    BoatMovement boatMovement;

    //Diver variables
    bool diving = false;
    [SerializeField]
    GameObject diver;
    Transform origTransform;
    Rigidbody diverRb;
    MeshRenderer diverMesh;

    [SerializeField]
    Transform ocean;
    float oceanHeight;

    // Start is called before the first frame update
    void Start()
    {
        diverRb = diver.GetComponent<Rigidbody>();
        diverMesh = diver.GetComponent<MeshRenderer>();
        diverMesh.enabled = false;
        origTransform = diver.transform;

        if (ocean == null)
        {
            Debug.LogError("AGGGH");
            Debug.Break();
        }
        else
        {
            oceanHeight = ocean.transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Dive, lock ship controls
        if (Input.GetKeyDown(KeyCode.Space))
        {
            diving = true;
            boatMovement.AllowMoving = false;
            diverRb.AddForce(Vector3.down * 2, ForceMode.Impulse);
            diverMesh.enabled = true;
        }
        if (diving)
        {
            float yPos = Input.GetAxisRaw("Vertical");
            Vector3 forceDir = new Vector3(0, yPos, 0);
            diverRb.AddForce(forceDir, ForceMode.Force);
        }
    }

    //Check if enter collider of the ship
    private void OnCollisionEnter(Collision collision)
    {
        //if tag is ship, set diving to false
        if(collision.gameObject.CompareTag("Ship"))
        {
            diving = false;
            boatMovement.AllowMoving = true;
            diverMesh.enabled = false;
            diver.transform.position = origTransform.position;
            diver.transform.rotation = origTransform.rotation;
            diver.transform.localScale = origTransform.localScale;
        }
    }
}
