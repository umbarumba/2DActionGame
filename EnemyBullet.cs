﻿using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

	private GameObject player;
	private GameObject BossEnemy;
	private int atackpoint = 4;
	private int speed = 6;
	private Life lifescript;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("UnityChan");
		BossEnemy = GameObject.FindWithTag ("Boss");
		Rigidbody2D RB2D = GetComponent<Rigidbody2D> ();
		lifescript = GameObject.FindGameObjectWithTag ("HP").GetComponent<Life> ();
		RB2D.velocity = new Vector2 (speed * BossEnemy.transform.localScale.x * -1, RB2D.velocity.y);
		Destroy (gameObject, 3);
	
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "UnityChan") {
			lifescript.LifeDown (atackpoint);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
