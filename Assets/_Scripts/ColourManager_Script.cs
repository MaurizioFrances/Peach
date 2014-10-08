using UnityEngine;
using System.Collections;

public class ColourManager_Script : MonoBehaviour {

	public ShapeManager_Script SMS;

	public Color[] colours;

	private int currentColour = 0;
	private int lastColour = 0;

	void Awake (){
		//this.GetComponent<ShapeManager_Script>();
	}
	// Use this for initialization
	void Start () {
		//this.GetComponent<ShapeManager_Script>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NewColour(){
		lastColour = currentColour;
		currentColour = Random.Range(0, colours.Length); 

		if(currentColour == lastColour){
			NewColour();
		}

		else{
			SMS.SetCurrentColour(colours[currentColour]);

			//SMS.SetShapeColours();
		}

	}
}
