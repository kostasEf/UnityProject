#pragma strict

//weapon general vars
var reserveAmmo : int = 60;			//how many bullets does this weapon have, not including those in the clip?
var clipMax : int = 18;					//how many bullets is this weapon's clip cabable of holding, max?
var clipAmmo : int = 18;				//how many bullest are actually in this weapon's clip?
var damageValue : float = 10;		//how much damage does this weapon do?

//weapon sights and animations		
var weaponMesh : GameObject;					//the mesh object for this weapon
var sightTime : float = .15;						//how long it takes this weapon to swap between sight modes

var weaponCam_ScopedFOV : float = 20;	//the weapon cam FOV required when this weapon is scoped in
var weaponCam_DefaultFOV : float = 60;	//the player cam FOV required when this weapon is scoped in

//effects
var sfx_WeaponFire : AudioClip;			//the audio clip to play when this weapon fires
var sfx_WeaponReload : AudioClip;		//the audio clip to play when this weapon reloads
var sfx_WeaponClick : AudioClip;		//the audio clip to play when this weapon mis-fires
var enemyHitFX : GameObject;			//the hit effect to make when this weapon hits an enemy
var ricochetFX : GameObject;			//the hit effect to make when this weapon hits a non-enemy