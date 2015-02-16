using UnityEngine;
using System.Collections;

public class ObjectInteraction : MonoBehaviour 
{
	
	public bool pickedItem = false;
	private GameObject item;
	private GameObject player;                      // Reference to the player.
    private PlayerInventory playerInventory;		// Reference to the player's inventory.
	private GameObject flashlight;
	private GUIText guiTextCenter, guiTextBot;
	private string itemName;	
	private float timer;
	public float timeToHide = 10f;
	
	public GameObject throwAble;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag(Tags.player);
        playerInventory = player.GetComponent<PlayerInventory>();
		flashlight = GameObject.Find("Flashlight");
		
		guiTextCenter = GameObject.Find("GUI_text_center").guiText;
		guiTextBot = GameObject.Find("GUI_text_bottom").guiText;
		
		
	}
	
	void Update() 
	{
		
        RaycastHit hit;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		
		
        if (Physics.Raycast(transform.position, fwd,out hit, 2.0f))
		{
			Debug.DrawLine (transform.position, hit.point, Color.red);
			
			if(hit.collider.gameObject.tag == "Pickable")
			{
				item = hit.collider.gameObject;
				itemName = item.name;
				
				guiTextCenter.text = "[E] Pick up " + itemName;
				
				if(Input.GetKey(KeyCode.E))
				{
					Destroy(item);			
					
					if(itemName == "Cup")
					{
						Vector3 position = GameObject.Find("Throw_Position").transform.position;
						Instantiate(throwAble, position, Quaternion.identity);
					}
					if(itemName == "prop_keycard_001")
					{
						playerInventory.prop_keycard_001 = true;
						pickedItem = true;
					}
					
					if(itemName == "prop_keycard_002")
					{
						playerInventory.prop_keycard_002 = true;
						pickedItem = true;
					}
					
					if(itemName == "flashlight")
					{
						flashlight.GetComponent<FlashLight>().hasFlashlight = true;
						pickedItem = true;
						GameObject.Find("torch").GetComponent<MeshRenderer>().enabled = true;
					}
					
					if(itemName == "gun")
					{
						pickedItem = true;
						GameObject.Find("torch").GetComponent<MeshRenderer>().enabled = false;
						GameObject.Find("lazergun").animation.CrossFade("itemUp");
						GameObject.Find("lazergun").GetComponent<MeshRenderer>().enabled = true;
						GameObject.Find ("Main Camera").GetComponent<AimAndShoot>().hasGun = true;
					}
					
				}
			}else if(hit.collider.gameObject.tag == "Throw")
			{
				item = hit.collider.gameObject;
				itemName = item.name;
				
				guiTextCenter.text = "[E] Pick up " + itemName;
				
				if(Input.GetKey(KeyCode.E))
				{
					
				}
			}
			
		}else{
			guiTextCenter.text = "";
		}
		
		if(pickedItem == true)
		{
			TextHide();
		}
        
    }
	
	
	
	void TextHide()
	{
		guiTextBot.text ="Picked up " + itemName;
		timer += Time.deltaTime;
		if(timer >= timeToHide)
		{
			guiTextBot.text = "";
			pickedItem = false;
			timer = 0;
		}
	}
	
}
