using UnityEngine;
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

	private Life lifeScript;

	public int BossMaxHP = 20;
	public int BossHP;
	public int atackpoint;

	private Renderer Rend;

	// Use this for initialization
	private void Start () {
		Timer = 0f;
		lifeScript = GameObject.FindGameObjectWithTag ("HP").GetComponent<Life> ();
		BossHP = BossMaxHP;
	}
	
	// Update is called once per frame
	private void Update () {
		if (_isRendered) {
			T = Time.deltaTime;
			Timer += T;
			if (Timer >= 0f && Timer <= 0.1f) {
				Debug.Log (_canShot1);
			}

			if (Timer >= 2f && Timer <= 2.5f) {
				Shot1 ();
				Debug.Log (_canShot1);
			}
			//...なんらかの行動パターン↓
		
			if (Timer >= 4f) {
				//パターンX
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
			Instantiate (BulletPrefab, transform.position + new Vector3 (0f, 0f, 0f), transform.rotation);
		}
		_canShot1 = false;
	}

	void OnTriggerEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Bullet") {
			StartCoroutine("Damage");
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		//UnityChanとぶつかった時
		if (col.gameObject.tag == "UnityChan") {
			Debug.Log ("UnityChanとぶつかった！");
			//LifeScriptのLifeDownメソッドを実行
			lifeScript.LifeDown (atackpoint);
		}
	}

	void OnWillRenderObject() {
		//メインカメラに映った時だけ_isRenderedをtrue
		if (Camera.current.tag == MAIN_CAMERA_TAG_NAME) {
			_isRendered = true;
		}
	}

	public void LifeDown (int damage) {
		BossHP -= damage;
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
