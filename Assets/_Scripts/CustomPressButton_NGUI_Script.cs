using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomPressButton_NGUI_Script : MonoBehaviour {

	public List<EventDelegate> onPress = new List<EventDelegate>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnPress (bool isPressed) {
	
		if(isPressed){
			//Debug.Log("PRESSED");
			EventDelegate.Execute(onPress);
		}
	
	}
}
