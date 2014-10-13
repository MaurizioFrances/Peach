using UnityEngine;
using System.Collections;

public class ShapeManager_Script : MonoBehaviour {

	private bool invertedColour = false;
	private Color currentColour;
	private Color clearColour;
	private Color overlayColour;

	public Texture2D shapeSpriteSheet;

	public Sprite[] shapesSprites;

	public int[] difficultyOrder;

	private GameObject leftShape;
	private GameObject rightShape;

	private UITweener leftShape_Tween;
	private UITweener rightShape_Tween;

	//private SpriteRenderer leftShapeRenderer;
	//private SpriteRenderer midShapeRenderer;
	//private SpriteRenderer rightShapeRenderer;

	private UI2DSprite leftShapeRenderer;
	private UI2DSprite midShapeRenderer;
	private UI2DSprite rightShapeRenderer;

	private UI2DSprite midShapeOverlayRenderer;

	//NEW
	private UI2DSprite midShapeBottomRenderer; 
	private UI2DSprite midShapeBottomOverlayRenderer;
	private UI2DSprite leftShapeBottomRenderer;
	private UI2DSprite rightShapeBottomRenderer;
	//NEW

	private int currentLeftSpriteNum;
	private int currentMidSpriteNum;
	private int currentRightSpriteNum;

	private UI2DSprite leftShapeRendererC1;
	private UI2DSprite leftShapeRendererC2;
	private UI2DSprite leftShapeRendererC3;
	private UI2DSprite leftShapeRendererC4;

	private UI2DSprite midShapeRendererC1;
	private UI2DSprite midShapeRendererC2;
	private UI2DSprite midShapeRendererC3;
	private UI2DSprite midShapeRendererC4;

	private UI2DSprite rightShapeRendererC1;
	private UI2DSprite rightShapeRendererC2;
	private UI2DSprite rightShapeRendererC3;
	private UI2DSprite rightShapeRendererC4;


	void Awake(){
		SetupDefaultColours(); //NEW

		shapesSprites = Resources.LoadAll<Sprite>(shapeSpriteSheet.name);	

		leftShape = GameObject.Find("Shape_Left");
		rightShape = GameObject.Find("Shape_Right");

		leftShapeRenderer = GameObject.Find("SL_Top").GetComponent<UI2DSprite>();
		midShapeRenderer = GameObject.Find("SM_Top").GetComponent<UI2DSprite>();
		rightShapeRenderer = GameObject.Find("SR_Top").GetComponent<UI2DSprite>();

		midShapeOverlayRenderer = GameObject.Find("SM_Overlay").GetComponent<UI2DSprite>();

		leftShapeRendererC1 = GameObject.Find("SL_Corner1").GetComponent<UI2DSprite>();
		leftShapeRendererC2 = GameObject.Find("SL_Corner2").GetComponent<UI2DSprite>();
		leftShapeRendererC3 = GameObject.Find("SL_Corner3").GetComponent<UI2DSprite>();
		leftShapeRendererC4 = GameObject.Find("SL_Corner4").GetComponent<UI2DSprite>();

		midShapeRendererC1 = GameObject.Find("SM_Corner1").GetComponent<UI2DSprite>();
		midShapeRendererC2 = GameObject.Find("SM_Corner2").GetComponent<UI2DSprite>();
		midShapeRendererC3 = GameObject.Find("SM_Corner3").GetComponent<UI2DSprite>();
		midShapeRendererC4 = GameObject.Find("SM_Corner4").GetComponent<UI2DSprite>();

		rightShapeRendererC1 = GameObject.Find("SR_Corner1").GetComponent<UI2DSprite>();
		rightShapeRendererC2 = GameObject.Find("SR_Corner2").GetComponent<UI2DSprite>();
		rightShapeRendererC3 = GameObject.Find("SR_Corner3").GetComponent<UI2DSprite>();
		rightShapeRendererC4 = GameObject.Find("SR_Corner4").GetComponent<UI2DSprite>();

		//NEW
		midShapeBottomRenderer = GameObject.Find("SM_Bottom").GetComponent<UI2DSprite>();
		midShapeBottomOverlayRenderer =  GameObject.Find("SM_BottomOverlay").GetComponent<UI2DSprite>();
		leftShapeBottomRenderer =  GameObject.Find("SL_Bottom").GetComponent<UI2DSprite>();
		rightShapeBottomRenderer =  GameObject.Find("SR_Bottom").GetComponent<UI2DSprite>();
		//NEW

		//MORE NEW
		leftShape_Tween = GameObject.Find("Shape_Left_StartTween").GetComponent<UITweener>();
		rightShape_Tween = GameObject.Find("Shape_Right_StartTween").GetComponent<UITweener>();
		//MORE NEW

		//ToggleAnswerShapes(false);
	}

	// Use this for initialization
	void Start () {
		ToggleAnswerShapes(false);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//NEW
	void SetupDefaultColours(){
		clearColour = Color.white;
		clearColour.a = 0;

		overlayColour = Color.black;
		overlayColour.a = 0.3921569f;
	}
	//NEW

	public void SetLeftShapeNum(int shapeNum){
		currentLeftSpriteNum = shapeNum;
		ChangeLeftShapeSprite();
	}

	public void SetMidShapeNum(int shapeNum){
		currentMidSpriteNum = shapeNum;
		ChangeMidShapeSprite();
	}

	public void SetRightShapeNum(int shapeNum){
		currentRightSpriteNum = shapeNum;
		ChangeRightShapeSprite();
	}

	void ChangeLeftShapeSprite(){
		leftShapeRenderer.sprite2D = shapesSprites[currentLeftSpriteNum];
	}

	void ChangeMidShapeSprite(){
		midShapeRenderer.sprite2D = shapesSprites[currentMidSpriteNum];
		midShapeOverlayRenderer.sprite2D = shapesSprites[currentMidSpriteNum];
	}

	void ChangeRightShapeSprite(){
		rightShapeRenderer.sprite2D = shapesSprites[currentRightSpriteNum];
	}



	public int GetAmountOfShapes(){
		return shapesSprites.Length;
	}

	//Converts a difficulty value (0-49) in a correct shape number (one of the matching pair)
	public int GetShapeOfDifficulty(int difficulty){
		int tempShape = difficultyOrder[difficulty];
		Debug.Log("diffOfder:" + tempShape);
		tempShape = (tempShape*2) - 1 - Random.Range(0, 2);

		Debug.Log("Tmep:" + tempShape);
		return tempShape;
	}

	public bool IsLeftCorrect(){
		if(currentLeftSpriteNum == currentMidSpriteNum){
			return true;
		}

		else{
			return false;
		}

	}

	public bool IsRightCorrect(){
		if(currentRightSpriteNum == currentMidSpriteNum){
			return true;
		}

		else{
			return false;
		}

	}

	public void SetCurrentColour(Color newColor){
		currentColour = newColor;
	}

	public void SetShapeColours(){
		//NEW
		if(invertedColour){

			midShapeBottomRenderer.color = clearColour;
			midShapeBottomOverlayRenderer.color = overlayColour;

			midShapeRenderer.color = Color.white;
			midShapeOverlayRenderer.color = clearColour;

			leftShapeRenderer.color = currentColour;
			rightShapeRenderer.color = currentColour;

			leftShapeBottomRenderer.color = Color.white;
			rightShapeBottomRenderer.color = Color.white;
		}

		else{
			midShapeBottomRenderer.color = Color.white;
			midShapeBottomOverlayRenderer.color = clearColour;

			midShapeRenderer.color = currentColour; //CURRENT

			midShapeOverlayRenderer.color = overlayColour;
			
			leftShapeRenderer.color = Color.white;
			rightShapeRenderer.color = Color.white;

			leftShapeBottomRenderer.color = clearColour;
			rightShapeBottomRenderer.color = clearColour;
		}
		//NEW
		
		Camera.main.backgroundColor = currentColour;
		SetCornerColours(currentColour);
		
	}

	//NEW
	public void SetColourInvert(bool invert){
		invertedColour = invert;
	}
	//NEW

	void SetCornerColours(Color newColor){

		leftShapeRendererC1.color = newColor;
		leftShapeRendererC2.color = newColor;
		leftShapeRendererC3.color = newColor;
		leftShapeRendererC4.color = newColor;

		midShapeRendererC1.color = newColor;
		midShapeRendererC2.color = newColor;
		midShapeRendererC3.color = newColor;
		midShapeRendererC4.color = newColor;

		rightShapeRendererC1.color = newColor;
		rightShapeRendererC2.color = newColor;
		rightShapeRendererC3.color = newColor;
		rightShapeRendererC4.color = newColor;
	}

	public void ShuffleMidShape(){
		SetMidShapeNum(Random.Range(0, shapesSprites.Length));

	}

	public void ToggleAnswerShapes(bool on){
		//leftShape.SetActive(on);
		//rightShape.SetActive(on);

		if(on){
			leftShape_Tween.PlayForward();
			rightShape_Tween.PlayForward();
		}

		else{
			leftShape_Tween.PlayReverse();
			rightShape_Tween.PlayReverse();
		}

	}
}
