using UnityEngine;
using System.Collections;

public class Analytics_Script : MonoBehaviour {

	public GoogleAnalyticsV3 googleAnalytics;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetScreen(string screenName){
		googleAnalytics.LogScreen(screenName);
	}

	public void SetShapeStats(int shapeNum, bool correct, bool timeout){

		string tempShapeNumber = "Shape-" + shapeNum;

		//If the shape was correct.
		if(correct){
			googleAnalytics.LogEvent("Shape", tempShapeNumber, "CORRECT", 1);

		}

		//If the shape was incorrect
		else{
			//If guessed incorrect
			if(!timeout){
				googleAnalytics.LogEvent("Shape", tempShapeNumber, "INCORRECT", 1);

			}

			//If incorrect because of timeout
			else{
				googleAnalytics.LogEvent("Shape", tempShapeNumber, "TIMEOUT", 1);
			}
		}
	}

	public void SetScore(int score){
		googleAnalytics.LogEvent("Score", "Achievement", "" + score, 1);
	}

	public void SetShare(int score){
		googleAnalytics.LogSocial("twitter", "post", "score:" + score);

	}
}
