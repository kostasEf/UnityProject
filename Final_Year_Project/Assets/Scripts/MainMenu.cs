using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public GUISkin MySkin;
	private bool mainMenu = true;
	private bool optionsMenu = false;
	private bool controlsMenu = false;
	private bool qualityMenu = false;
	private bool difficultyMenu = false;
	private string quality = "";
	private string difficulty = "";
	
	void Awake()
	{
		int qualityLevel = QualitySettings.GetQualityLevel();
		if(qualityLevel == 0)
		{
			quality = "High";
		}else if(qualityLevel == 1)
		{
			quality = "Medium";
		}else if(qualityLevel == 2)
		{
			quality = "Low";
		}
		
		if(PlayerPrefs.GetString("difficulty") == "")
		{
			PlayerPrefs.SetString("difficulty", "Medium");
		}
		
		
	}
	
	void OnGUI()
	{
		GUI.skin = MySkin;
		//Screen.showCursor = false;
		
		if(mainMenu)
		{
			GUI.Box(new Rect(Screen.width*0.5f+50, Screen.height*0.1f, 350, 50), "Main Menu");
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, Screen.height*0.3f, 350, 80), "Start"))
			{
				Application.LoadLevel(2);
			}
		
			if(GUI.Button(new Rect(Screen.width*0.5f+50, Screen.height*0.5f, 350, 80), "Options"))
			{
				optionsMenu = true;
				mainMenu = false;
			}
		
			if(GUI.Button(new Rect(Screen.width*0.5f+50, Screen.height*0.8f, 350, 80), "Exit"))
			{
				Application.Quit();
			}
		}
		
		if(optionsMenu)
		{
			GUI.Box(new Rect(Screen.width*0.5f+50, Screen.height*0.1f, 350, 50), "Options");
		
			if(GUI.Button(new Rect(Screen.width*0.5f+50, Screen.height*0.23f, 350, 80), "Controls"))
			{
				controlsMenu = true;
				optionsMenu = false;
			}
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, Screen.height*0.4f, 350, 80), "Quality"))
			{
				optionsMenu = false;
				qualityMenu = true;
			}
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, Screen.height*0.57f, 350, 80), "Difficulty"))
			{
				optionsMenu = false;
				difficultyMenu = true;
			}
		
			if(GUI.Button(new Rect(Screen.width*0.5f+50, Screen.height*0.8f, 350, 80), "Back"))
			{
				optionsMenu = false;
				mainMenu = true;
			}
		}
		
		if(controlsMenu)
		{
			GUI.Box(new Rect(Screen.width*0.5f+50, Screen.height*0.1f, 350, 50), "Controls");
			GUI.Box(new Rect(Screen.width*0.5f+50, 180, 350, 200), "");
			GUI.Label(new Rect(Screen.width*0.5f+55, 180, 350, 20), "Move Forward \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t W");
			GUI.Label(new Rect(Screen.width*0.5f+55, 200, 350, 20), "Move Backwards \t\t\t\t\t\t\t\t\t\t\t\t\t\t S");
			GUI.Label(new Rect(Screen.width*0.5f+55, 220, 350, 20), "Move Left \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t A");
			GUI.Label(new Rect(Screen.width*0.5f+55, 240, 350, 20), "Move Right \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t D");
			GUI.Label(new Rect(Screen.width*0.5f+55, 260, 350, 20), "Move Slowly \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t Shift");
			GUI.Label(new Rect(Screen.width*0.5f+55, 280, 350, 20), "Crouch \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t C");
			GUI.Label(new Rect(Screen.width*0.5f+55, 300, 350, 20), "Use/Pick Up \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t E");
			GUI.Label(new Rect(Screen.width*0.5f+55, 320, 350, 20), "Use Flashlight \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t F");
			GUI.Label(new Rect(Screen.width*0.5f+55, 340, 350, 20), "Fire/Throw \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t LMB");
			GUI.Label(new Rect(Screen.width*0.5f+55, 360, 350, 20), "Reload \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t R");
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, Screen.height*0.8f, 350, 80), "Back"))
			{
				controlsMenu = false;
				optionsMenu = true;
			}
		}
		
		if(qualityMenu)
		{			
			GUI.Box(new Rect(Screen.width*0.5f+50, Screen.height*0.1f, 350, 100), "Quality"  + "\r\n" + quality);
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, 220, 350, 60), "High"))
			{
				QualitySettings.SetQualityLevel(0);
				quality = "High";
			}
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, 300, 350, 60), "Medium"))
			{
				QualitySettings.SetQualityLevel(1);
				quality = "Medium";
			}
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, 380, 350, 60), "Low"))
			{
				QualitySettings.SetQualityLevel(2);
				quality = "Low";
			}
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, Screen.height*0.8f, 350, 80), "Back"))
			{
				qualityMenu = false;
				optionsMenu = true;
			}
		}
		
		if(difficultyMenu)
		{			
			GUI.Box(new Rect(Screen.width*0.5f+50, Screen.height*0.1f, 350, 100), "Difficulty"  + "\r\n" + PlayerPrefs.GetString("difficulty"));
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, 220, 350, 60), "Hard"))
			{
				PlayerPrefs.SetString("difficulty", "Hard");
				Debug.Log("Hard");
			}
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, 300, 350, 60), "Medium"))
			{
				PlayerPrefs.SetString("difficulty", "Medium");
				Debug.Log("Medium");
			}
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, 380, 350, 60), "Easy"))
			{
				PlayerPrefs.SetString("difficulty", "Easy");
				Debug.Log("Easy");
			}
			
			if(GUI.Button(new Rect(Screen.width*0.5f+50, Screen.height*0.8f, 350, 80), "Back"))
			{
				difficultyMenu = false;
				optionsMenu = true;
			}
		}
		
		
	}	
}
