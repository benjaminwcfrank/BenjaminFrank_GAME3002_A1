using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;



[RequireComponent(typeof(ProjectileComponent))] //Require a projectileComponent for this script to run
public class ProjectileControlling : MonoBehaviour
{

    private GameObject target;

    //create ref for projectileComponent
    private ProjectileComponent projectileControl = null;



    // Start is called before the first frame update
    void Start()
    {
        //find target 
        target = GameObject.Find("Target");
        target.GetComponent<Renderer>().material.color = Color.yellow;

        //Assign projectileControl to be the projectileComponent of this script's parent
        projectileControl = GetComponent<ProjectileComponent>();

        //Check if projectileControl is null.
        Assert.IsNotNull(projectileControl, "something went very wrong. ProjectileComponent's null");
    }

    // Update is called once per frame
    void Update()
    {
        //check once per frame for what keys are being pressed
        HandleUserInput();
    }

    private void HandleUserInput()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {   //is space is pressed, launch projectile
            if (projectileControl.OnLaunchProjectile())
            {
                target.GetComponent<TargettRandomPos>().NewRandomOffset();
            }

        }

        //if(Input.GetKey(KeyCode.UpArrow))
        //{   //is up arrow is pressed, increase initialVelocity on the z axis
        //    projectileControl.OnMoveForward(0.01f);
        //}

        //if (Input.GetKey(KeyCode.DownArrow))
        //{   //is down arrow is pressed, decrease initialVelocity on the z axis
        //    projectileControl.OnMoveBackward(0.01f);
        //}

        if (Input.GetKey(KeyCode.LeftArrow))
        {   //is right arrow is pressed, increase initialVelocity on the x axis
            projectileControl.OnMoveBackward(0.01f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {   //is left arrow is pressed, decrease initialVelocity on the x axis
            projectileControl.OnMoveForward(0.01f);
        }
    }





}
