﻿using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	private float T;
	public float Timer;

	private const string MAIN_CAMERA_TAG_NAME = "MainCamera";

	private Rigidbody2D RB2D;

	public GameObject BulletPrefab;
	public GameObject explosion;

	private bool _isRendered = false;
	private bool _canShot1 = true;
	private bool _canShot2 = true;

	private Life lifeScript;

	public int BossMaxHP = 20;
	public int BossHP;
	public int atackpoint;
	public int JumppowerY = 500;
	public int JumppowerX = 100;

	private Renderer Rend;

	// Use this for initialization
	private void Start () {
		Timer = 0f;
		lifeScript = GameObject.FindGameObjectWithTag ("HP").GetComponent<Life> ();
		Rend = GetComponent<Renderer> ();
		RB2D = GetComponent<Rigidbody2D> ();
		BossHP = BossMaxHP;
	}
	
	// Update is called once per frame
	private void Update () {
		if (_isRendered) {
			T = Time.deltaTime;
			Timer += T;

			if (Timer >= 0f && Timer <= 0.1f) {
				//Debug.Log (_canShot1);
			}

			if (Timer >= 2f && Timer <= 2.5f) {
				Shot1 ();
			}
			//...なんらかの行動パターン↓
			if (Timer >= 6f && Timer <= 6.5f) {
				Shot2 ();
			}
		
			if (Timer >= 8f) {
				_canShot1 = true;//初期化
				_canShot2 = true;
				Timer = 0f;//タイマー初期化
			}
		}
		if (BossHP <= 0) {
			Destroy (gameObject);
			Instantiate (explosion, transform.position, transform.rotation);
		}
	}

	private void Shot1() {
		if (_canShot1) {
			//Bossの座標にEnemyBulletを置く
			Instantiate (BulletPrefab, transform.position + new Vector3 (0f, 0f, 0f), transform.rotation);
			RB2D.AddForce (Vector2.up * JumppowerY);
			RB2D.AddForce (Vector2.right * JumppowerX * -1);
			Vector2 temp = transform.localScale;
			temp.x = 1;
			transform.localScale = temp;
		}
		_canShot1 = false;
	}

	private void Shot2() {
		if (_canShot2) {
			Instantiate (BulletPrefab, transform.position + new Vector3 (0f, 0f, 0f), transform.rotation);
			RB2D.AddForce (Vector2.up * JumppowerY);
			RB2D.AddForce (Vector2.right * JumppowerX);
			Vector2 temp = transform.localScale;
			temp.x = -1;
			transform.localScale = temp;
		}
		_canShot2 = false;
	}


	void OnCollisionEnter2D (Collision2D col) {
		//UnityChanとぶつかった時
		if (col.gameObject.tag == "UnityChan") {
			Debug.Log ("UnityChanとぶつかった！");
			//LifeScriptのLifeDownメソッドを実行
			lifeScript.LifeDown (atackpoint);
		}
		//自機の弾が当たった時
		//if (col.gameObject.tag == "Bullet") {
		//	StartCoroutine ("Damage");
		//}
	}

	void OnWillRenderObject() {
		//メインカメラに映った時だけ_isRenderedをtrue
		if (Camera.current.tag == MAIN_CAMERA_TAG_NAME) {
			_isRendered = true;
		}
	}

	public void LifeDown (int damage) {
		BossHP -= damage;
		StartCoroutine ("Damage");
	}

	private IEnumerator Damage () {
		gameObject.layer = LayerMask.NameToLayer ("EnemyDamage");
		int count = 10;
		while (count > 0) {
			//透明にする
			Rend.material.color = new Color (1, 1, 1, 0);
			//0.05秒待つ
			yield return new WaitForSeconds (0.05f);
			//元に戻す
			Rend.material.color = new Color (1, 1, 1, 1);
			//0.05秒待つ
			yield return new WaitForSeconds (0.05f);
			count--;
		}
		//レイヤーをEnemyに戻す
		gameObject.layer = LayerMask.NameToLayer ("Enemy");
	}
}
