using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BombDrag : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler {

	public Bomb bomb;
	[SerializeField]
	private GameObject parent;
	private float nokoriTime;
	private int i;
	private float y;
	private float _displayWidth = 1080;
	private float _displayHeight = 1920;
	private float minRangeY;
	private float maxRangeY;
	private float minRangeX;
	private float maxRangeX;
	private float tmpX;
	private float tmpY;

	void Start(){
		minRangeX = 0.078f * _displayWidth;
		maxRangeX = _displayWidth - 0.078f * _displayWidth;
		minRangeY = _displayHeight - 0.55f * _displayHeight;
		maxRangeY = _displayHeight - 0.09f * _displayHeight;
		parent = GameObject.FindWithTag("GameManager");
	}

	void FixedUpdate(){
		nokoriTime = Timer.instance.NokoriTime;
		if (nokoriTime <= 0) {
			InstantiateBomb (this.transform.position.x, this.transform.position.y);
		}
	}

	public void OnBeginDrag(PointerEventData eventData){
		
	}

	public void OnDrag(PointerEventData eventData){
		if ((eventData.position.x > minRangeX && eventData.position.x < maxRangeX)
		    && (eventData.position.y > minRangeY && eventData.position.y < maxRangeY)) {
			this.transform.position = new Vector2 (eventData.position.x, eventData.position.y);
		}
	}

	public void OnEndDrag(PointerEventData eventData){
		InstantiateBomb (eventData.position.x,eventData.position.y);
	}

	void InstantiateBomb(float x, float y){
		if (x < minRangeX) {
			tmpX = ((minRangeX - _displayWidth / 2) * 6) / _displayWidth;
		} else if (x > maxRangeX) {
			tmpX = ((maxRangeX - _displayWidth / 2) * 6) / _displayWidth;
		} else {
			tmpX = ((x - _displayWidth / 2) * 6) / _displayWidth;
		}

		if (y < minRangeY) {
			tmpY = ((minRangeY - _displayHeight / 2) * 10) / _displayHeight;
		} else if (y > maxRangeY) {
			tmpY = ((maxRangeY - _displayHeight / 2) * 10) / _displayHeight;
		} else {
			tmpY = ((y - _displayHeight / 2) * 10) / _displayHeight;
		}

		Instantiate (bomb, new Vector3 (tmpX, tmpY, 0), Quaternion.Euler (0, 0, 0), parent.transform);
		Timer.instance.ChangeTurn ();
		Destroy (this.gameObject);

	}

}
