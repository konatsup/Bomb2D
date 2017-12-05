using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState{
	Start,
	Play,
	Loading,
	Pause,
	Emergency,
	Result
}

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public int maxPlayerNum;
	public int maxFallenBombNum;
	private GameState currentGameState;
	public BombImage bombImageCircle;
	public BombImage bombImageTriangle;
	public BombImage bombImageSquare;
	public Canvas canvas;
	public Text playerText;
	public Text centerText;
	public float waitTime;
	private BombImage tmpPrefab;
	private int fallenBombCount; //台の下に落ちているボムの数
	private int nowBombNum;
	private bool isEmergency;
	[SerializeField]
	private int nowPlayerNum;
	public int NowPlayerNum { 
		get { return nowPlayerNum;}
	}
	public AudioSource sound1;
	public AudioClip buzzer;
	public AudioClip bgm;


	// Use this for initialization
	void Start ()
	{
		instance = this;
		sound1.PlayOneShot(bgm);
		fallenBombCount = 0;
		if (maxPlayerNum > 8) {
			maxPlayerNum = 8; //とりあえず8人を上限とする
		}
		SetCurrentState (GameState.Start);

	}
	public GameState GetCurrentState(){
		return currentGameState;
	}

	public void SetCurrentState(GameState state){
		currentGameState = state;
		OnGameStateChanged (currentGameState);
	}

	void OnGameStateChanged(GameState state){
		switch (state) {
		case GameState.Start:
			Debug.Log ("GameState:Start");
			StartAction ();
			break;
		case GameState.Play:
			Debug.Log ("GameState:Play");
			PlayAction ();
			break;
		case GameState.Loading:
			Debug.Log ("GameState:Loading");
			LoadingAction ();
			break;
		case GameState.Pause:
			Debug.Log ("GameState:Pause");
			PauseAction ();
			break;
		case GameState.Emergency:
			Debug.Log ("GameState:Emergency");
			EmergencyAction ();
			break;
		
		case GameState.Result:
			Debug.Log ("GameState:Result");
			ResultAction ();
			break;
		default:
			break;
		}
	}


	private void StartAction(){
		nowPlayerNum = 0;
		SetCurrentState (GameState.Pause);
	}

	private void PlayAction(){
		Time.timeScale = 1;
		Timer.instance.TimeCountFlg = true;
		if (isEmergency == true) {
			Timer.instance.LastTimeCountFlg = true;
		}
		tmpPrefab.bombDrag.enabled = true;
		centerText.enabled = false;
	}

	private void LoadingAction(){
		StartCoroutine(LoadingCoroutineMethod());
	}

	private void PauseAction(){
		Time.timeScale = 0;
		ChangeNowPlayerNumber ();
		if (isEmergency == false) {
			fallenBombCount = 0;
			foreach (Transform child in transform) {
				fallenBombCount += child.GetComponent<Bomb> ().CheckFallenBomb ();
			}
			Debug.Log ("FallenBombCount："+ fallenBombCount);
			if (fallenBombCount > maxFallenBombNum) {
				SetCurrentState (GameState.Emergency);
			} else {
				StartCoroutine (PauseCoroutineMethod ());
			}
		} else {
			StartCoroutine (PauseCoroutineMethod ());
		}

	}


	private void EmergencyAction(){
		centerText.enabled = true;
		isEmergency = true;
		centerText.color = new Color( 1.0f, 0f, 0f, 1.0f);
		StartCoroutine (EmergencyCoroutineMethod ());

	}

	private void ResultAction(){
		Time.timeScale = 0;
		centerText.enabled = true;
		centerText.text = "Player "+ nowPlayerNum + "の負け";
		centerText.color = GetMyColor();

	}



	private IEnumerator LoadingCoroutineMethod()
	{
		yield return new WaitForSeconds(2.0f);

		SetCurrentState (GameState.Pause);
	}
		
	private IEnumerator PauseCoroutineMethod()
	{
		centerText.enabled = true;
		GenerateBombImage ();
		waitTime = 3.0f;
		while (waitTime != 0) {
			centerText.text = (waitTime--).ToString ("N0");
			yield return new WaitForSecondsRealtime (1.0f);
		}
		SetCurrentState (GameState.Play);
	}

	private IEnumerator EmergencyCoroutineMethod()
	{
		waitTime = 2.0f;
		while (waitTime != 0) {
			centerText.text = "WARNING!!";
			sound1.PlayOneShot (buzzer);
			yield return new WaitForSecondsRealtime (0.5f);
			waitTime -= 0.5f;
			centerText.text = "";
			yield return new WaitForSecondsRealtime (0.5f);
			waitTime -= 0.5f;
		}
		centerText.color = new Color( 1.0f, 1.0f, 1.0f, 1.0f);
		StartCoroutine (PauseCoroutineMethod ());
	}


	private void ChangeNowPlayerNumber(){

		if (nowPlayerNum == maxPlayerNum) {
			nowPlayerNum = 1; //プレイヤー1にリセット
		} else {
			nowPlayerNum++;
		}
		playerText.text = "Player " + nowPlayerNum.ToString();
		playerText.color = GetMyColor();
	}

	private Color GetMyColor(){
		Color myColor;
		switch (nowPlayerNum) {
		case 1:
			myColor = new Color(255.0f/255.0f, 0f/255.0f, 0f/255.0f, 1.0f); //赤
			break;
		case 2:
			myColor = new Color(0f/255.0f, 0f/255.0f, 255.0f/255.0f, 1.0f); //青
			break;
		case 3:
			myColor = new Color(0f/255.0f, 255.0f/255.0f, 0f/255.0f, 1.0f); //緑
			break;
		case 4:
			myColor = new Color(255.0f/255.0f, 255.0f/255.0f, 0f/255.0f, 1.0f); //黄
			break;
		case 5:
			myColor = new Color(255.0f/255.0f, 0f/255.0f, 255.0f/255.0f, 1.0f); //紫
			break;
		case 6:
			myColor = new Color(0f/255.0f, 255.0f/255.0f, 255.0f/255.0f, 1.0f); //水色
			break;
		case 7:
			myColor = new Color(255.0f/255.0f, 102.0f/255.0f, 0f/255.0f, 1.0f); //オレンジ色
			break;
		case 8:
			myColor = new Color(255.0f/255.0f, 153.0f/255.0f, 204.0f/255.0f, 1.0f); //ピンク
			break;
		case 9:
			myColor = new Color(0f/255.0f, 204.0f/255.0f, 128.0f/255.0f, 1.0f); //ライム
			break;
		default:
			myColor = new Color(0f/255.0f, 0f/255.0f, 0f/255.0f, 1.0f); //例外は白色
			break;
		}
		return myColor;
	}
		
	public void GenerateBombImage(){
		
		nowBombNum = Random.Range (0, 3);
		switch (nowBombNum) {
		case 0:
			tmpPrefab = (BombImage)Instantiate (bombImageCircle);
			break;
		case 1:
			tmpPrefab = (BombImage)Instantiate (bombImageTriangle);
			break;
		case 2:
			tmpPrefab = (BombImage)Instantiate (bombImageSquare);
			break;
		default:
			break;
		}
		tmpPrefab.Initialize (GetMyColor());
		tmpPrefab.transform.SetParent(canvas.transform, false);
	}

	//デバッグ用
	public void GeneratorBombImage(int ID){
		
		switch (ID) {
		case 0:
			tmpPrefab = (BombImage)Instantiate (bombImageCircle);
			break;
		case 1:
			tmpPrefab = (BombImage)Instantiate (bombImageTriangle);
			break;
		case 2:
			tmpPrefab = (BombImage)Instantiate (bombImageSquare);
			break;
		default:
			break;
		}
		tmpPrefab.Initialize (GetMyColor());
		tmpPrefab.transform.SetParent(canvas.transform, false);
	}

}
