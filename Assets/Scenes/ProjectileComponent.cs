using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

[RequireComponent(typeof(Rigidbody))]   //this class requires a rigidbody in order to work
public class ProjectileComponent : MonoBehaviour
{

    //private GameObject target;

    [SerializeField]//set the initial velocity to 0, 0, 0 make it editable in the inspector
    private Vector3 initialVelocity = Vector3.zero;

    //create ref for the projectile's rigidbody
    private Rigidbody rb = null;
    //Make ref to victoryText
    private GameObject victoryTextRef = null;
    //create ref for the landingDisplay
    private GameObject landingDisplay = null;
    //Is the projectile on the ground? AKA has the projectile been shot yet?
    private bool isGrounded = true;

    private void Start()
    {


        //find the VictoryText gameObject
        victoryTextRef = GameObject.Find("VictoryText");
        //make sure it's not Null
        Assert.IsNotNull(victoryTextRef, "Someone call the NULL Police! victoryTextRef in ProjectileComponent is Null!");

        //assign the ref rigidbody the projectile's rigidbody
        rb = GetComponent<Rigidbody>();
        //assert for if the ref regidbody is null
        Assert.IsNotNull(rb, "something went very wrong. rb's null");

        //Create the landingDisplay object
        CreateLandingDisplay();
        //assert that the ref landingDisplay gameobject is not null
        Assert.IsNotNull(landingDisplay, "something went very wrong. landing display's null");
    }

    private void Update()
    {
        //recalculate the landing position of the projectile and move landingDisplay to that position
        UpdateLandingPosition();
    }

    private void CreateLandingDisplay()
    {
        //Add a primative mesh to the landingDisplay gameObject
        landingDisplay = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //Set the initial position of the landingDisplay to be 0, 0, 0
        landingDisplay.transform.position = Vector3.zero;
        //Set the scale of the landingDisplay to be 1, 0.1, 1 (Cylinder to flat disc)
        landingDisplay.transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
        //Set the colour of the landingDisplay to be red
        landingDisplay.GetComponent<Renderer>().material.color = Color.red;
        //disable the collision as to not interfere with the projectile
        landingDisplay.GetComponent<Collider>().enabled = false;
    }

    public bool OnLaunchProjectile()
    {
        //if projectile already has been launched, return
        if (!isGrounded)
        {
            ResetAll();
            return true;
        }
        //enable use of gravity
        rb.useGravity = true;
        //set landingDisplay position to be the position returned by the GetLandingPosition function one last time
        landingDisplay.transform.position = GetLandingPosition();
        //set grounded flag to false to prevent further editing
        isGrounded = false;
        //set the velocity of the projectile's rigidbody(and thus the velocity of the whole projectile) to be == to the initialvelocity
        rb.velocity = initialVelocity;
        return false;
    }

    private void UpdateLandingPosition()
    {
        //if the landingDisplay isn't null and the projectile is yet to be launched
        if (landingDisplay != null && isGrounded)
        {
            //set the position of the landingDisplay to be the position returned by the GetLandingPosition function
            landingDisplay.transform.position = GetLandingPosition();
        }
    }

    private Vector3 GetLandingPosition()
    {
        //calculate how much time it will take for the projectile to complete its parabolic trajectory
        float fTime = (2f * (0.0f - initialVelocity.y) / Physics.gravity.y);
        //create vector flatVelocity and set it to be initialvelocity
        Vector3 flatVelocity = initialVelocity;
        //remove flatvelocity's y (vertical) component
        flatVelocity.y = 0.0f;
        //find displacement by multiplying flatvelocity(how fast the projectile travels in a straight line parallel to the ground) by the amount of time it take the projectile to complete its unconstrained parabolic trajectory
        flatVelocity *= fTime;
        //return the original position of the unlaunched projectile + the calculated displacement
        return transform.position + flatVelocity;
    }

    #region INPUT_FUNCTIONS
    public void OnMoveForward(float val)
    {
        if (initialVelocity.z < 19)
        {
            initialVelocity.z += val;

        }
    }
    public void OnMoveBackward(float val)
    {
        if (initialVelocity.z > 1)
        {
            initialVelocity.z -= val;

        }
    }
    //public void OnMoveLeft(float val)
    //{
    //    initialVelocity.x += val;
    //}
    //public void OnMoveRight(float val)
    //{
    //    initialVelocity.x -= val;
    //}

    public void ResetAll()
    {
        //move projectile back to original position
        transform.position = new Vector3(0.0f, 0.7f, -18.7f);
        //set velocity back to zero
        rb.velocity = Vector3.zero;
        //Set the rotation to be default (no rotation offset)
        rb.rotation = Quaternion.identity;
        //Set angular velocity (speed of current rotation) to be 0 in all axis
        rb.angularVelocity = Vector3.zero;
        //make the victoryText invisible
        victoryTextRef.GetComponent<TextMeshProUGUI>().enabled = false;
        //disable use of gravity
        rb.useGravity = false;
        //allow for launch
        isGrounded = true;
    }
    #endregion


}
