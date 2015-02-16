using UnityEngine;
using System.Collections;
 
public class MovementC : MonoBehaviour 
{
    public float runSpeed = 5; // regular speed
    public float crchSpeed = 2; // crouching speed
    public float walkSpeed = 2; // slow speed
	public float velocity;
	public float colHeight;
	
	private Collider col;
    private CharacterMotor chMotor;
	private CharacterController ch;
    private Transform tr;
    private float dist; // distance to ground
 	private float height; // initial height
	
    // Use this for initialization
    void Start () 
    {
        chMotor =  GetComponent<CharacterMotor>();
        tr = transform;
        ch = GetComponent<CharacterController>();
        dist = ch.height/2; // calculate distance to ground
		height = ch.height;
		Vector3 s = new Vector3(0f, 0.5f, 0f);
		colHeight = GameObject.Find("Graphics").GetComponent<CapsuleCollider>().height;
		
    }
 
    // Update is called once per frame
    void Update ()
    {
		
		velocity = ch.velocity.magnitude;
		
        float h = height;
		float hc = colHeight;
		//float vScale = 1.0f;
        float currentSpeed = runSpeed;
 
        if ((Input.GetKey("left shift") || Input.GetKey("right shift")) && chMotor.grounded)
        {
            currentSpeed = walkSpeed;
        }
 
        if (Input.GetKey("c")) //GetKeyDown
        { // press C to crouch
			h = 0.37f * height;
			hc = 0.37f * colHeight;
            //vScale = 0.5f;
            currentSpeed = crchSpeed; // slow down when crouching
        }
		
 
		
        chMotor.movement.maxForwardSpeed = currentSpeed; // set max speed
		chMotor.movement.maxSidewaysSpeed = currentSpeed; // set max speed
		chMotor.movement.maxBackwardsSpeed = currentSpeed; // set max speed
		float lastHeight = ch.height; // crouch/stand up smoothly
		float lastColHeight = GameObject.Find("Graphics").GetComponent<CapsuleCollider>().height;
		ch.height = Mathf.Lerp(ch.height, h, 5*Time.deltaTime);
		GameObject.Find("Graphics").GetComponent<CapsuleCollider>().height = Mathf.Lerp(GameObject.Find("Graphics").GetComponent<CapsuleCollider>().height, hc, 5*Time.deltaTime);
        Vector3 tmpPosition = tr.position;
		tmpPosition.y += (ch.height-lastHeight)/2; // fix vertical position
		tr.position = tmpPosition;
    }
}