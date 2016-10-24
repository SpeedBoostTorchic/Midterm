﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour {

	//Options Menu prefab and children
	//public GameObject optionsMenu;
	public GameObject cancel;
	public GameObject accept;
	//public GameObject musicSlider;
	//public GameObject SFXSlider;

	public static float musicVolume;
	public static float sfxVolume;

	//Menu buttons stored here
	public GameObject start;
	public GameObject options;
	public GameObject exit;

	//Music and sound cues stored here
	public AudioSource music;
	public AudioSource sfx;
	public AudioSource sfx2;
	public AudioClip mouseOver;
	public AudioClip click;
	public AudioClip fadeOut;

	//Screen fader thing here
	public GameObject faderObject;
	private ScreenFader fade;

	//Sets menu defaults
	public void Start(){
		//optionsMenu.SetActive (false);
		//music.volume = musicSlider.GetComponent<Slider> ().value;
		//sfx.volume = SFXSlider.GetComponent<Slider> ().value;

		music.Play ();
		faderObject.SetActive (false);
		Cursor.visible = true;
	}

	//Starts the game when start button is clicked
	public void sceneTransition(int x){
		//Grabs screen fader object and component
		faderObject.SetActive (true);
		fade = faderObject.GetComponent<ScreenFader> ();

		/*//Music and SFX for the transition here
		sfx.Stop();
		sfx2.clip = fadeOut;
		sfx2.Play ();
		music.Stop ();*/

		//This causes the scene transition; see the
		//"ScreenFader" class for more info
		fade.FadeOut (x);
	}

	//Exits the applications when Quit button is clicked
	public void quitGame(){
		Application.Quit ();
	}

	//Opens options menu and diables buttons
	/*public void openOptions(){
		optionsMenu.SetActive (true);
	}

	//Applies the changes made in the options menu
	public void optionsAccept(){
		//Takes slider values
		musicVolume = musicSlider.GetComponent<Slider> ().value;
		sfxVolume = SFXSlider.GetComponent<Slider> ().value;

		//sets volume according to sliders
		music.volume = musicVolume;
		sfx.volume = sfxVolume;
		sfx2.volume = sfxVolume;
	}

	//Closes the options menu
	public void closeOptions(){
		optionsMenu.SetActive (false);
	}*/

	public void rollOver(){
		sfx.clip = mouseOver;

		if (!sfx.isPlaying)
			sfx.Play ();
	}

	public void clickButton(){
		sfx.clip = click;

		if (!sfx.isPlaying)
			sfx.Play ();

	}


}
