using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BombImage : MonoBehaviour{
	public Bomb bomb;
	public BombDrag bombDrag;
	private Color myColor;

	public void Initialize(Color myColor){
		this.GetComponent<Image> ().color = myColor;
		this.bomb.GetComponent<SpriteRenderer> ().color = myColor;
		bombDrag.enabled = false;
	}

}
