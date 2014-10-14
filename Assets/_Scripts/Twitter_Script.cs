using UnityEngine;
using System.Collections;

public class Twitter_Script : MonoBehaviour {

	private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
	private const string TWEET_LANGUAGE = "en";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShareScore(int score){
		ShareToTwitter("I just got " + score + "! @ThisGameIPlayed #ThisGameIPlayed");
	}

	void ShareToTwitter (string textToDisplay){

	Application.OpenURL(TWITTER_ADDRESS +
	            "?text=" + WWW.EscapeURL(textToDisplay) +
	            "&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
	}
}
