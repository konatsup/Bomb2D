using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
	private int ID;
	private float minRangeY;
	private float maxRangeY;
	private float minRangeX;
	private float maxRangeX;

	// Use this for initialization
	void Start () {
//		this.ID = GameManager.instance.NowPlayerNum;
		minRangeX = -1.8f;
		maxRangeX = 1.8f;
		minRangeY = -2.7f;
		maxRangeY = 4.35f;
	}

	public int CheckFallenBomb(){
		if ((this.transform.position.x > minRangeX && this.transform.position.x < maxRangeX)
			&& (this.transform.position.y > minRangeY && this.transform.position.y < maxRangeY)) {
			return 0; //台の上にある
		}

		return 1; //台から落ちている
	}

}
