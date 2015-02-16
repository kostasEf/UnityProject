#pragma strict

var myClips : AudioClip[]; 					//Audio to randomly choose from
var myParticleFX : ParticleSystem[]; 	//Particle effects to randomly choose from

var fxDuration : float = 1.0;				//the length of time (seconds) this fx will last for
private var selfTimer : float = 0;		//a timer for self-destruction

//this happens right away, when the effect is spawned in
function Start () 
{
	//first, get a random integeger to choose the audio and particle fx
	var clipChoice : int = Random.Range(0,myClips.length); 
	var particleChoice : int = Random.Range(0,myParticleFX.length);
	
	audio.PlayOneShot(myClips[clipChoice]); //play the chosen audio
	var particleFX : ParticleSystem = Instantiate(myParticleFX[particleChoice], transform.position, transform.rotation); //spawn in the particle fx
	particleFX.transform.parent = transform; //make th particle fx a child of the main FX object- that way it will be destroyed with it
}

//this automatically loops through every frame
function Update()
{
	selfTimer+=Time.deltaTime; //update the timer, keeping it synced with how long this effect has been "alive" for
	if(selfTimer >= fxDuration) //if the FX object has been "alive" past it's set duration...
		Destroy(gameObject); //destroy the FX object
}
