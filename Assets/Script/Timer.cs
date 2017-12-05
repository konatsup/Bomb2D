using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	public static Timer instance;
	public float limitTime; //制限時間
	public float flashSpeed;
	public SpriteRenderer foreGround;
	public AudioSource sound1;
	public float maxLastTime; //最終カウントの時間
	[SerializeField]
	private float nokoriLastTime; //最終カウントの時間
	[SerializeField]
	private float passedTime; //経過時間
	[SerializeField]
	private float nokoriTime; //残り時間
	public float NokoriTime { 
		get { return nokoriTime;}
		set { nokoriTime = value;}
	}
	public Text timerText1;
	private bool timeCountFlg;
	public bool TimeCountFlg { 
		get { return timeCountFlg;}
		set { timeCountFlg = value;}
	}
	private bool lastTimeCountFlg;
	public bool LastTimeCountFlg { 
		get { return lastTimeCountFlg;}
		set { lastTimeCountFlg = value;}
	}

	private float sineWave;
	private float preSineWave;

	void Start(){
		instance = this;
		nokoriTime = limitTime;
		nokoriLastTime = 0.0f;
		timerText1.text = nokoriTime.ToString("N2");
		sineWave = 0f;
		foreGround.color = new Color(1.0f, 0f, 0f, 0.0f);

	}

	// Update is called once per frame
	void Update () {
		if (timeCountFlg == true) {
			passedTime += Time.deltaTime;
			nokoriTime = limitTime - passedTime;
			timerText1.text = nokoriTime.ToString ("N2");
		}
		if(lastTimeCountFlg == true){
			nokoriLastTime += Time.deltaTime;
			preSineWave = sineWave;
			sineWave = Mathf.Sin (flashSpeed * Time.time * (nokoriLastTime / maxLastTime));
			if(sineWave*preSineWave < 0 && preSineWave < 0) {
				sound1.PlayOneShot(sound1.clip);
			}
			foreGround.color = new Color(0.7f, 0f, 0f, 0.5f + 0.2f * sineWave); //赤
			if (nokoriLastTime >= maxLastTime) {
				GameManager.instance.SetCurrentState (GameState.Result);
			}
		}

	}

	public void ChangeTurn(){
		timeCountFlg = false;
		lastTimeCountFlg = false;
		passedTime = 0.0f;
		nokoriTime = limitTime;
		timerText1.text = nokoriTime.ToString("N2");
		GameManager.instance.SetCurrentState (GameState.Loading);
	}

	float GetLimitTime(){
		return limitTime;
	}

}
