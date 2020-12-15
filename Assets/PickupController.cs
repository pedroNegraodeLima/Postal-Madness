using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    //public ProjectileGun gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;
    public MeshRenderer fakeNews;
    public MeshRenderer trueNews;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Start()
    {
        //Setup
        if (!equipped)
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
            fakeNews.enabled = false;
            trueNews.enabled = true;
        }

        if (equipped)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
            fakeNews.enabled = true;
            trueNews.enabled = false;
        }
    }

    private void Update()
    {
        //Check if player is in range and 'E' is pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) PickUp();

        //Drop if equipped and 'E' is pressed
        if (equipped && Input.GetKeyDown(KeyCode.Q)) Drop();
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //Make weapon a child of the camera and move it to default position when picked up
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        
        


        //Make rigidbody kinematic and BoxCollider normal
        rb.isKinematic = true;
        coll.isTrigger = true;

        fakeNews.enabled = true;
        trueNews.enabled = false;

    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;
        fakeNews.enabled = false;
        trueNews.enabled = true;

        //Set parent to null
        transform.SetParent(null);


        //Make rigidbody kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Gun carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //Add force when throwing weapon
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        //Add random torque rotation when throwing 
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 30);


    }
}
