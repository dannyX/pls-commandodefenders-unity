using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shield : MonoBehaviour {

	public ParticleSystem shieldBreakEffect;

	public int Health = 3;
	public List<string> Attributes;

	private int MaxHealth;

	// Use this for initialization
	void Start () {
		this.MaxHealth = this.Health;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReceiveDamage (int damage) {
		this.Health -= damage;
		Color color = gameObject.GetComponent<SpriteRenderer> ().color;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (color.r, (((float)this.Health / (float)this.MaxHealth)), (((float)this.Health / (float)this.MaxHealth)), color.a);

		if (this.Health <= 0) {
			shieldBreakEffect.startColor = gameObject.GetComponent<SpriteRenderer> ().color;
			shieldBreakEffect.Play ();
			Destroy (gameObject);
		}
	}
}
