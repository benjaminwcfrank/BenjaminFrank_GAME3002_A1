using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;


public class TargettRandomPos : MonoBehaviour
{
    //int that determines what level the player is on and thus how the target should act.
    private int level = 0;
    //bool so that level count will increment only the first time the target is hit by projectile (until reset)
    private bool allowLevelIncrement = true;
    [SerializeField]//set the offset position to 0, 0, 0 and make it editable in the inspector
    private Vector3 offsetPosition = Vector3.zero;
    
    #region Define Refs
    //a ref to the victoryText UI
    private GameObject victoryTextRef = null;
    //a ref to the levelText UI
    private GameObject levelTextRef = null;
    //a ref to the first obsticle
    private GameObject obsticle0 = null;
    //a ref to the second obsticle
    private GameObject obsticle1 = null;
    //a ref to the tutorialText UI
    private GameObject tutorialTextRef = null;
    //create ref for the projectile's rigidbody
    private Rigidbody rbRef = null;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        #region Get Refs
        //assign the ref rigidbody the target's rigidbody
        rbRef = GetComponent<Rigidbody>();
        //assert for if the ref regidbody is null
        Assert.IsNotNull(rbRef, "something went very wrong. rbRef's null");
        //find the VictoryText gameObject
        victoryTextRef = GameObject.Find("VictoryText");
        //make sure it's not Null
        Assert.IsNotNull(victoryTextRef, "Someone call the NULL Police! victoryTextRef in TargetRandomPos is Null!");
        //find the VictoryText gameObject
        levelTextRef = GameObject.Find("LevelText");
        //make sure it's not Null
        Assert.IsNotNull(levelTextRef, "Someone call the NULL Police! levelTextRef in TargetRandomPos is Null!");
        //find the tutorialText gameObject
        tutorialTextRef = GameObject.Find("TutorialText");
        //make sure it's not Null
        Assert.IsNotNull(tutorialTextRef, "Someone call the NULL Police! victoryTextRef in TargetRandomPos is Null!");
        //Find the 1st obsticle gameobject
        obsticle0 = GameObject.Find("Obsticle0");
        //ensure it isn't null
        Assert.IsNotNull(obsticle0, "Someone call the NULL Police! obsticle0 in TargetRandomPos is Null!");
        //deactivate obsticle0 for the time being
        obsticle0.SetActive(false);
        //Find the 2nd obsticle gameobject
        obsticle1 = GameObject.Find("Obsticle1");
        //ensure it isn't null
        Assert.IsNotNull(obsticle1, "Someone call the NULL Police! obsticle0 in TargetRandomPos is Null!");
        //deactivate obsticle1 for the time being
        obsticle1.SetActive(false);
        #endregion
        //generate and apply random offset
        NewRandomOffset();
    }

    void IncrementLevel()
    {
        //if allowed to increment the level and level is < 4
        if (allowLevelIncrement && level < 4)
        {
            //increment level count
            level++;
            //and disable further level increments until next level starts
            allowLevelIncrement = false;
            //deactivate the tutorial text (done here every level change rather than a custom else if for first level increment. Not perfect but ehhhhhhh)
            tutorialTextRef.SetActive(false);
        }//if level 5 is beaten...
        else if(allowLevelIncrement && level == 4)
        {
            //change the victory text to something congratulatory yet dismissive
            victoryTextRef.GetComponent<TextMeshProUGUI>().SetText("You have won this \"game\".\nYou should feel very proud of yourself or something.");
            //change victory text colour to red
            victoryTextRef.GetComponent<TextMeshProUGUI>().faceColor = new Color32(255, 0, 0, 255);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //tell the cube to allow gravity
        rbRef.useGravity = true;
        //make the victoryText visible
        victoryTextRef.GetComponent<TextMeshProUGUI>().enabled = true;
        //increment the level of the game.
        IncrementLevel();
    }

        public void NewRandomOffset()
    {

        #region LevelDetails
        switch (level)
        {
            case 0://for level 1, pos.z will be between -11 and 11 inclusive. Easy to hit.
                offsetPosition.Set(0.0f, 12.0f, Random.Range(-11.0f, 11.0f));
                break;
            case 1://for level 2 set the target a little lower so you can't just place the aimdisc at double the length from the projectile to the target
                offsetPosition.Set(0.0f, 9.0f, Random.Range(-3.0f, 22.0f));
                break;
            case 2://for level 3 set the target even lower.
                offsetPosition.Set(0.0f, 6.0f, Random.Range(6.0f, 26.0f));
                break;
            case 3://for level 4 add an obsticle you have to bounce off of
                offsetPosition.Set(0.0f, Random.Range(2.0f, 6.5f), Random.Range(4.0f, 20.0f));
                //enable obsticle
                obsticle0.SetActive(true);
                //enable second  obsticle
                obsticle1.SetActive(true);
                //set obsticle0 position based on offset position
                obsticle0.transform.position = new Vector3(0.0f, offsetPosition.y + 1, offsetPosition.z - 2f);
                //set obsticle1 position based on offset position
                obsticle1.transform.position = new Vector3(0.0f, offsetPosition.y + 1, offsetPosition.z + 5f);
                break;
            case 4://for level 5 add only a single obsticle you have to bounce off the top of
                offsetPosition.Set(0.0f, Random.Range(4.0f, 6.0f), Random.Range(9.0f, 15.0f));
                //disable second obsticle
                obsticle1.SetActive(false);
                //set obsticle0 position based on offset position
                obsticle0.transform.position = new Vector3(0.0f, offsetPosition.y + 1, offsetPosition.z - 4f);
                //adjust obsticle 0 to make it a little easier to hit the top
                obsticle0.transform.localScale = new Vector3(1.0f, 6.6f, 2.0f);
                break;
        }
        #endregion

        #region Only applicable to level changes
        //set the position to be the generated offset position
        transform.position = offsetPosition;
        //Set the rotation to be default (no rotation offset)
        transform.rotation = Quaternion.identity;
        //Set angular velocity (speed of current rotation) to be 0 in all axis
        rbRef.angularVelocity = Vector3.zero;
        //Set translational velocity to be 0 in all axis
        rbRef.velocity = Vector3.zero;
        //disable use of gravity
        rbRef.useGravity = false;
        //change the level text
        levelTextRef.GetComponent<TextMeshProUGUI>().SetText("LEVEL: " + (level + 1) + "/5");
        //allow target to increment level again
        allowLevelIncrement = true;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
