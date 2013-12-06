using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForwardProjectile : MonoBehaviour {

	public Sprite[] sprites;

	public float speed = 1.1f;
	public int damage = 1;

	public Gun Gun;

	private float distance = 0.0f;

	public void SetRotation(float rot) {
		transform.Rotate (new Vector3 (0f, 0f, rot));
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Vector3 previousPos = transform.position;
		transform.Translate(Vector3.up * Time.deltaTime * this.speed);
		this.distance += Vector3.Distance(previousPos, transform.position);//Mathf.Abs (previousPos.magnitude - transform.position.magnitude);

		if (this.distance > this.Gun.Model.Range) {
			Destroy (gameObject);
		}

		if (transform.position.y > 1.5f) {
			Destroy (gameObject);
		}
		else if (transform.position.y < -1.0f) {
			Destroy (gameObject);
		}

		if (transform.position.x > 0.8f) {
			Destroy (gameObject);
		}
		else if (transform.position.x < -0.8f) {
			Destroy (gameObject);
		}
	}
}
