#pragma strict
 
var walk : AudioClip;
var slow : AudioClip;
 
var isWalking : boolean = false;
var isSlow : boolean = false;
 
 
function Update() 
{
    GetState();
    PlayAudio();
}
 
 
function GetState() 
{
    if ( Input.GetAxis( "Horizontal" ) || Input.GetAxis( "Vertical" ) )
    {
       if ( Input.GetKey( "left shift" ) || Input.GetKey( "right shift" ) || Input.GetKey( "c" ) )
       {
         // Slow
         isWalking = false;
         isSlow = true;
       }
       else
       {
         // Walking
         isWalking = true;
         isSlow = false;
       }
    }
    else
    {
       // Stopped
       isWalking = false;
       isSlow = false;
    }
}
 
 
function PlayAudio() 
{
    if ( isWalking )
    {
       if ( audio.clip != walk )
       {
         audio.Stop();
         audio.clip = walk;
       }
 
       if ( !audio.isPlaying )
       {
         audio.Play();
       }
    }
    else if ( isSlow )
    {
       if ( audio.clip != slow )
       {
         audio.Stop();
         audio.clip = slow;
       }
 
       if ( !audio.isPlaying )
       {
         audio.Play();
       }
    }
    else
    {
       audio.Stop();
    }
}