using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour {

	//What the fade looks like
	private Image image;

	//These floats handle how, when, and how fast the fade occurs
	public float startingAlpha;
	public float currentAlpha;
	public float fadeSpeed = 0.8f;

	//Booleans track whether the fader is currently fading into
	//a scene, or out of one
	public bool fadingIn = false;
	private bool fadingOut = false;

	//If fading out of a scene, these takes the index number of
	//the scene the game is transitioning to
	private int sceneTransitionNumber;


	//Grabs the image
	void Awake(){
		image = this.GetComponent <Image> ();

	}

	//Sets the image's color
	void Start () {
		image.color = new Color (255, 255, 255, startingAlpha);
	}

	// Fade is handled here
	void Update () {
		image.color = new Color (255, 255, 255, currentAlpha);

		if (fadingOut && currentAlpha < 2) {
			currentAlpha += fadeSpeed * Time.deltaTime;
		}
		if (fadingIn && currentAlpha > 0) {
			currentAlpha -= fadeSpeed * Time.deltaTime;
		}

		//Loads level after fade out
		if (fadingOut && currentAlpha >= 2) {
			Application.LoadLevel (sceneTransitionNumber);
		}

		if (currentAlpha <= 0) {
			this.gameObject.SetActive (false);
		}

	}

	//These methods are called by GameObjects in the scene in
	//order to initiate a fade
	public void FadeIn(){
		fadingOut = false;
		fadingIn = true;
	}
	public void FadeOut(int x){
		fadingIn = false;
		fadingOut = true;
		sceneTransitionNumber = x;

		if(x == 0)
			Destroy(GameObject.Find ("Music (battle)"));

	}
}
