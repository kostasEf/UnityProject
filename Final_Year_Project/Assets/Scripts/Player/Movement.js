﻿#pragma strict

var walkSpeed: float = 5; // regular speed
var crchSpeed: float = 2; // crouching speed
var slowSpeed: float = 2; // slow speed
 
private var chMotor: CharacterMotor;
private var ch: CharacterController;
private var tr: Transform;
private var height: float; // initial height
 
function Start(){
    chMotor = GetComponent(CharacterMotor);
    tr = transform;
    ch = GetComponent(CharacterController);
	height = ch.height;
}
 
function Update(){
 
    var h = height;
    var speed = walkSpeed;
    
    if (ch.isGrounded && Input.GetKey("left shift") || Input.GetKey("right shift")){
        speed = slowSpeed;
    }
    if (Input.GetKey("c")){ // press C to crouch
        h = 0.37 * height;
        speed = crchSpeed; // slow down when crouching
    }
    
    chMotor.movement.maxForwardSpeed = speed; // set max speed
    var lastHeight = ch.height; // crouch/stand up smoothly 
    ch.height = Mathf.Lerp(ch.height, h, 5*Time.deltaTime);
    tr.position.y += (ch.height-lastHeight)/2; // fix vertical position
}