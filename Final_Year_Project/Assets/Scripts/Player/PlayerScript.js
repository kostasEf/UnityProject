#pragma strict

// ---
//misc
var rayCastLayers : LayerMask; //we use this to make sure the player cannot "shoot themselves"

//player states
var player_AllowFire : boolean = true;					//is the player allowed to fire at this time?
var player_AllowReload : boolean = true;				//is the player allowed to reload at this time?
var player_isReloading : boolean = false;				//is the player currently reloading?
var player_AllowSights : boolean = true;					//is the player allowed to scope in at this time?
var player_isSightModeSwapping : boolean = false;	//is the player currently scoping in/out?
var player_isScopedIn : boolean = false;				//is the player currently scoped in?
var player_reloadAfter : boolean = false;				//should the player reload after the current action?
var player_AllowButtonPress : boolean = true;			//is the player allowed to press buttons?

//weapon
var availableWeapons : GameObject[];			//an array of all available weapons
var currentWeapon : Weapon_Base;				//the weapon currently being used
var hipSocket : Transform;							//the position of the weapon in hip-firing mode
var sightsSocket : Transform;						//the position of hte weapon when scoped in or iron sights
var weaponCam : Camera;								//the camera for the weapon
var playerCam_ScopedFOV : float = 30;			//the FOV at which the player camera should be when scoped in
var playerCam_DefaultFOV : float = 60;			//the FOV at which the weapon camera should be when scoped in
private var sightingTimePassed : float = 0.0;	//internal var for counting passage of time while scoping in/out
private var lerpAmount : float = 0.0;				//internal var for smoothly transitioning between scoped in/out

//GUI
var gui_ClipCount : UILabel;			//the gui item which displays how many bullets are in the current clip
var gui_ReserveCount : UILabel;		//gui item which displays how many reserve bullets we have (not including current clip)
var gui_Crosshairs : UISprite;			//gui item which displays the crosshairs
// ---

// ---
function Start() //this happens right away when the game starts
{
	var currentWeaponObject = Instantiate(availableWeapons[0], hipSocket.position, hipSocket.rotation); //spawn in the starting weapon, at the hipSocket location and rotation
	currentWeaponObject.transform.parent = weaponCam.transform; //set the weapon to be a child of the weapon camera
	currentWeapon = currentWeaponObject.GetComponent(Weapon_Base); //set the "currentWeapon" to this spawned weapon
	
	//set the gui to match the current weapon data
	if(gui_ClipCount)
		gui_ClipCount.text = ""+currentWeapon.clipAmmo;
	if(gui_ReserveCount)
		gui_ReserveCount.text = ""+currentWeapon.reserveAmmo;
}
// ---

// ---
function Update () //This automatically loops through each and every frame
{
	if(player_AllowFire) //player is allowed to fire?
	{
		if(Input.GetButtonDown("Fire")) //did the player just hit the "fire" button?
		{
			PullTrigger(); //"pull the trigger"- we don't fire just yet!
		}
	}
	
	//if the player is allowed to reload, and they are not in the middle of scoping in/out, and their clip is not currently full, AND they have reserve ammo...then, only then, do we allow a relaod to happen or be queued
	if(player_AllowReload && currentWeapon.clipAmmo != currentWeapon.clipMax && currentWeapon.reserveAmmo != 0)
	{
		if(Input.GetButtonDown("Reload")) //did the player just hit the "reload" button?
		{
			if(!player_isScopedIn) //if the player is not scoped, in, we can move forward toward reloading normally
			{
				Reload(); //reload that weapon, pronto!
			}
			else //...however, if the player is currently scoped in, we need to first scope-out, then reload
			{
				SwapSightMode(); //swap the sight mode- in this case, that will mean scoping out (back to hip-shooting)
				player_reloadAfter = true; //this way the script knows it should reload right after scoping out
			}
		}
	}

	if(player_AllowSights) //player is allowed to scope in or out?
	{
		if(Input.GetButtonDown("Sights")) //did the player just hit the "sights" button?
		{
			SwapSightMode(); //swap to the opposition sight mode (hip vs iron sights/scope)
		}
	}
	
	//we use this loop for smoothly moving between hip and scoped/iron sights modes
	if(player_isSightModeSwapping) //should we be swapping the mode right now?
	{
		if(!player_isScopedIn) //is the player NOT scoped in? Then we will be scoping in! (remember, we are swapping to whatever the opposite is!)
		{
			sightingTimePassed += Time.deltaTime; //keep track of how much time has passed since we began swapping
			lerpAmount = sightingTimePassed/currentWeapon.sightTime; //get the percentage to lerp with
			currentWeapon.transform.localPosition = Vector3.Lerp(hipSocket.localPosition, sightsSocket.localPosition, sightingTimePassed/currentWeapon.sightTime); //lerp the weapon position
			camera.main.fieldOfView = Mathf.Lerp(playerCam_DefaultFOV, playerCam_ScopedFOV, lerpAmount); //lerp the player camera's FOV
			weaponCam.fieldOfView = Mathf.Lerp(currentWeapon.weaponCam_DefaultFOV, currentWeapon.weaponCam_ScopedFOV, lerpAmount); //lerp the player camera's FOV
			if(sightingTimePassed >= currentWeapon.sightTime) //if the swapping action has gone on for the length specified, it must be finished, so...
			{
				currentWeapon.transform.localPosition = sightsSocket.localPosition; //just to be sure, force the weapon to the exact position
				camera.main.fieldOfView = playerCam_ScopedFOV; //force the player cam FOV to the exact value
				weaponCam.fieldOfView = currentWeapon.weaponCam_ScopedFOV; //force the weapon cam FOV to the exact value
				player_isSightModeSwapping = false; //now that the swapping is done, we'll set this to false so it doesn't continue on the next loop
				player_AllowSights = true; //now that we are done swapping, we can re-allow a new swap...
				player_AllowReload = true; //...we can also allow reloading
				player_isScopedIn = true;  // we are now fully scoped in
				gui_Crosshairs.enabled = false; //disable the gui crosshairs, since we are scoped in
			}
		}
		else //ah, so the player IS already scoped in? Then, we shall be scoping out here!
		{
			sightingTimePassed += Time.deltaTime; //keep track of how much time has passed since we began swapping
			lerpAmount = sightingTimePassed/currentWeapon.sightTime; //get the percentage to lerp with
			currentWeapon.transform.localPosition = Vector3.Lerp(sightsSocket.localPosition, hipSocket.localPosition, sightingTimePassed/currentWeapon.sightTime); //lerp the weapon position
			camera.main.fieldOfView = Mathf.Lerp(playerCam_ScopedFOV, playerCam_DefaultFOV, lerpAmount); //lerp the player camera's FOV
			weaponCam.fieldOfView = Mathf.Lerp(currentWeapon.weaponCam_ScopedFOV, currentWeapon.weaponCam_DefaultFOV, lerpAmount); //lerp the player camera's FOV
			if(sightingTimePassed >= currentWeapon.sightTime) //if the swapping action has gone on for the length specified, it must be finished, so...
			{
				currentWeapon.transform.localPosition = hipSocket.localPosition;//just to be sure, force the weapon to the exact position
				camera.main.fieldOfView = playerCam_DefaultFOV; //force the player cam FOV to the exact value
				weaponCam.fieldOfView = currentWeapon.weaponCam_DefaultFOV; //force the weapon cam FOV to the exact value
				player_isSightModeSwapping = false; //now that the swapping is done, we'll set this to false so it doesn't continue on the next loop
				player_AllowSights = true; //now that we are done swapping, we can re-allow a new swap...
				player_AllowReload = true; //...we can also allow reloading
				player_isScopedIn = false;  // we are now fully scoped out
				gui_Crosshairs.enabled = true; //enable the gui crosshairs, since we are scoped out (hip-shooting)
				if(player_reloadAfter) //waitup- were we supposed to do a reload after scoping out? 
					Reload(); //...if so, then lets do that reload now!
			}
		}
	}
	
	//Checking for the player attempting to press a world-button (not GUI)
	if(player_AllowButtonPress)
	{
		if(Input.GetButtonDown("Use"))
		{
			AttemptUse(); //attempt to use that button
		}
	}
}
// ---

// ---
function SwapSightMode() //function to swap between "hip" and "iron sights/scope" modes
{
	player_AllowSights = false; //not allowed to re-sight in the middle of a swap!
	player_AllowReload = false; //not allowed to reload in the middle of a swap!
	sightingTimePassed = 0.0; //reset the swapping timer
	player_isSightModeSwapping = true; //tell the script it should be running the swap loop
}
// ---

// --
function PullTrigger() //trigger pulled- now it's either "click!" or "boom!"
{
	if(currentWeapon.clipAmmo != 0 && !currentWeapon.weaponMesh.animation.isPlaying) //are there still bullets in the clip? 
	{
		currentWeapon.gameObject.GetComponent(AudioSource).PlayOneShot(currentWeapon.sfx_WeaponFire); //play weapon fire audio, just once
		
		//play firing animation
		currentWeapon.weaponMesh.animation.Play("Fire", PlayMode.StopAll);
		
		currentWeapon.clipAmmo--; //remove a bullet from the clip
		gui_ClipCount.text = ""+currentWeapon.clipAmmo; //update the HUD info
		
		//To test where the player hit, we fire a test-ray directly from the center of the camera, through the crosshairs
		//once the ray hits something, it sends back data that we can use to determine many things- what it hit, how far, the angle, etc
		
		//First, we get the crosshair position, in Screen Space (pixels)
		var crossHairPos : Vector2 = Vector2(Screen.width*.5, Screen.height*.5); 
		
		//Next, we take the Screen Position, convert it to a position in "real" space
		//from this new position, we define a ray traveling directly out from the camera, in the direction it is facing
		var ray = Camera.main.ScreenPointToRay(crossHairPos); 
		
		//If the ray hits something, all the data about that hit will be held in this variable
		var rayHit : RaycastHit;
		
		//Now, lets actually fire that ray!
		//		The "300" means it will only fire 300 meters
		//		The rayCastLayers makes sure we can't hit the player's own collision
		if(Physics.Raycast(ray, rayHit, 300, rayCastLayers)) 
		{
			var hitObject : GameObject = rayHit.collider.gameObject; //We hit something? Hurray! Now, lets save the hit object into a new variable
			
			if(hitObject.tag == "Enemy") //did we hit an Enemy?
			{
				hitObject.SendMessage("TakeHit", currentWeapon.damageValue); //tell the enemy to take damage
				var splat = Instantiate(currentWeapon.enemyHitFX, rayHit.point, Quaternion.identity); //spawn a special hit effect where the enemy was hit
			}
			else //we hit something, but it isn't an Enemy?
			{
				var ricochet = Instantiate(currentWeapon.ricochetFX, rayHit.point, Quaternion.identity); //spawn a richochet effect where the bullet hit
				ricochet.transform.eulerAngles = rayHit.normal; //align the hit effect to look like it is bouncing correctly off the hit surface
			}
		}
	}
	else //uho, no bullets in the clip- click click PANIC!
	{
		currentWeapon.gameObject.GetComponent(AudioSource).PlayOneShot(currentWeapon.sfx_WeaponClick); //play the dreaded "click!" noise
	}
}
// --

// --
function Reload() //reloading the weapon
{
	currentWeapon.gameObject.GetComponent(AudioSource).PlayOneShot(currentWeapon.sfx_WeaponReload); //play the reload audio
	currentWeapon.weaponMesh.animation.Play("Reload"); //play the reload animation
	player_AllowFire = false; //don't allow the player to fire while reloading
	player_AllowReload = false; //don't allow the player to start another reload, while already reloading
	player_AllowSights = false; //don't allow the player to swap the sight mode while reloading
	
	//Okay, time to reload! First, some math though- weee!
	var bulletsNeeded : int = currentWeapon.clipMax-currentWeapon.clipAmmo; //what is the maximum more bullets can this clip take?
	var bulletsToLoad : int = 0; //initializing a variable to hold the final amount of bullets to load
	
	if(bulletsNeeded <= currentWeapon.reserveAmmo) //do we have enough spare bullets to fully load the clip?
	{
		bulletsToLoad = bulletsNeeded; //that was simple- load that clip completely!
	}
	else //uho, we can't reload the full clip!
	{
		bulletsToLoad = currentWeapon.reserveAmmo; //load in all the bullets we have left!
	}
	
	//now that we know how many bullets are being loaded, subtract that number from the reserve bullets
	currentWeapon.reserveAmmo-=bulletsToLoad;
	//Update the HUD info to match		
	gui_ReserveCount.text = ""+currentWeapon.reserveAmmo;
	
	//wait for reload audio to finish playing- this is a good way to wait exactly the right amount of time!
	yield WaitForSeconds(currentWeapon.sfx_WeaponReload.length);
	
	//Add the new bullets into the "clip"
	currentWeapon.clipAmmo+=bulletsToLoad;
	//Update the HUD info to match
	gui_ClipCount.text = ""+currentWeapon.clipAmmo;
	
	player_AllowReload = true; //now that we are done reloading, we can allow the player to reload again (though, it won't work if the clip is still at max, of course)
	player_AllowFire = true; //allow the player to fire
	player_AllowSights = true; //allow the player to swap the sight mode
	player_reloadAfter = false; //if we were supposed to do this reload as an "after something else thing", it's now been done, so cross it off the list!
}
// --

function AttemptUse()
{
	//Debug.Log("Attempting Use!");
	//To test where the player hit, we fire a test-ray directly from the center of the camera, through the crosshairs
	//once the ray hits something, it sends back data that we can use to determine many things- what it hit, how far, the angle, etc
	
	//First, we get the crosshair position, in Screen Space (pixels)
	var crossHairPos : Vector2 = Vector2(Screen.width*.5, Screen.height*.5); 
	
	//Next, we take the Screen Position, convert it to a position in "real" space
	//from this new position, we define a ray traveling directly out from the camera, in the direction it is facing
	var ray = Camera.main.ScreenPointToRay(crossHairPos); 
	
	//If the ray hits something, all the data about that hit will be held in this variable
	var rayHit : RaycastHit;
	
	//Now, lets actually fire that ray!
	//		The "300" means it will only fire 300 meters
	//		The rayCastLayers makes sure we can't hit the player's own collision
	if(Physics.Raycast(ray, rayHit, 300, rayCastLayers)) 
	{
		var hitObject : GameObject = rayHit.collider.gameObject; //We hit something? Hurray! Now, lets save the hit object into a new variable
		//Debug.Log("Something hit with Use!");
		if(hitObject.tag == "Button") //did we hit a button?
		{
			//Debug.Log("Button Pressed!");
			hitObject.SendMessage("ButtonPressed"); //tell the button it has been pressed
		}
	}
}