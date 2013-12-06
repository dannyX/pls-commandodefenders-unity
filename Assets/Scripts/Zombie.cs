using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zombie : MonoBehaviour {

	Vector3 survivorPos;

	Survivor attackedSurvivor;
	Shield attackedShield;

	public ParticleSystem BloodSpill;
	public ParticleSystem DeathSpill;
	public LevelGenerator levelManager;

	public int Health;
	public int MaxHealth;

	private float speed = 0.2f;
	private float initialSpeed = 0.2f;
	private float stepSpeed = 0.001f;
	private Vector3 direction;

	private int attackPower = 1;

	private bool isAttacking = false;
	private bool isFrozen = false;
	private bool isBurning = false;
	private float elapsedAttackTime = 0.0f;
	private float attackSpeed = 1.0f;

	// TODO: Build real attribute system
	private int burningDamage = 4;
	private float burningRate = 1f;
	private float elapsedBurningTime = 0.0f;

	float elapsedTime = 0.0f;

	public bool IsAttacking {
		set {
			isAttacking = value;
		}
	}

	public bool IsFrozen {
		get {
			return isFrozen;
		}
	}

	public bool IsBurning {
		get {
			return isBurning;
		}
	}

	// Use this for initialization
	void Start () {
		this.initialSpeed = this.speed;
		this.survivorPos = GameObject.Find ("Survivor").transform.position;
		this.direction = (this.survivorPos - transform.position).normalized;

		if (this.Health == 0) {
			this.Health = 1;
		}

		this.MaxHealth = this.Health;
	}
	
	// Update is called once per frame
	void Update () {
		this.elapsedTime += Time.deltaTime;
		this.elapsedAttackTime += Time.deltaTime;
		this.elapsedBurningTime += Time.deltaTime;

		if (this.elapsedTime >= stepSpeed && !this.isAttacking) {
			transform.Translate(this.direction * Time.deltaTime * this.speed);
			this.elapsedTime = 0.0f;
		}

		if (this.elapsedAttackTime >= this.attackSpeed && this.isAttacking) {
			if (attackedSurvivor != null) {
				attackedSurvivor.ReceiveDamage (this.attackPower);
			} else if (attackedShield != null) {
				attackedShield.ReceiveDamage (this.attackPower);
			}
			this.elapsedAttackTime = 0.0f;
		}

		if (this.isBurning && this.elapsedBurningTime >= this.burningRate) {
			this.ReceiveDamage (this.burningDamage);
			this.elapsedBurningTime = 0.0f;
		}

	}

	public void ReceiveDamage (int damage) {
		this.Health -= damage;
		this.BloodSpill.transform.position = transform.position;
		this.BloodSpill.Play ();
		Color color = gameObject.GetComponent<SpriteRenderer> ().color;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (color.r, (((float)this.Health / (float)this.MaxHealth))*2, (((float)this.Health / (float)this.MaxHealth))*2, color.a);

		if (this.Health <= 0) {
			this.levelManager.IncrementScore ();
			Destroy (gameObject);
			this.DeathSpill.transform.position = transform.position;
			this.DeathSpill.Play ();
		}
	}

	public void ReceiveDamage (int damage, Survivor survivor) {
		this.Health -= damage;
		this.BloodSpill.transform.position = transform.position;
		this.BloodSpill.Play ();
		Color color = gameObject.GetComponent<SpriteRenderer> ().color;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (color.r, (((float)this.Health / (float)this.MaxHealth)), (((float)this.Health / (float)this.MaxHealth)), color.a);

		if (this.Health <= 0) {
			this.levelManager.IncrementScore ();
			Destroy (gameObject);
			this.DeathSpill.transform.position = transform.position;
			this.DeathSpill.Play ();
			survivor.IncrementStat ("kill_num_zombies");
			PlayerPrefs.SetInt ("kill_num_zombies", (int)survivor.Stats ["kill_num_zombies"]);
		}
	}


	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Projectile") {
				gameObject.audio.Play ();
				transform.Translate (-this.direction * Time.deltaTime * (this.speed * 2));
				ForwardProjectile proj = col.gameObject.GetComponent<ForwardProjectile> ();
				transform.Translate (-this.direction * Time.deltaTime * (this.speed * 2 * proj.Gun.Model.Damage));
				ApplyAttribute (proj.Gun.Model.Attribute);
				
				if (proj.Gun.Model.Damage > 0) {
						if (this.isFrozen) {
							this.isFrozen = false;
							gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
						}
						this.ReceiveDamage (proj.Gun.Model.Damage, proj.Gun.Owner);
								GunAttribute attribute = proj.Gun.Model.Attribute;
								if (attribute != null && attribute.Id == "Pierce" && Random.Range (0, 100) < attribute.EffectChance) { // TODO: Clean this up
										Debug.Log ("don't");
										// Don't destroy
								} else {
										Debug.Log ("Do");
										Destroy (col.gameObject);
								}
				}
		} else if (col.gameObject.tag == "Player") {
			this.isAttacking = true;
			this.attackedSurvivor = col.gameObject.GetComponent<Survivor> ();
		} else if (col.gameObject.tag == "ZombieWall") {
			this.isAttacking = true;
			this.attackedShield = col.gameObject.GetComponent<Shield> ();
			ApplyAttributes (this.attackedShield.Attributes);
		}
	}

		void ApplyAttributes (List<string> attributes)
		{
				foreach (string attribute in attributes) {
						if (attribute == "flame") {
								this.isBurning = true;
								if (this.isFrozen) {
										this.isFrozen = false;
								}
								gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 0.5f, 0f, 1f);
						} else if (attribute == "Freeze") {
								this.speed = this.initialSpeed / 4;
								this.isFrozen = true;
								gameObject.GetComponent<SpriteRenderer> ().color = new Color (0f, 0.5f, 1f, 1f);
						}
				}
		}

		void ApplyAttribute (GunAttribute attribute) {
				if (attribute != null && attribute.Id == "Freeze" && Random.Range (0, 100) < attribute.EffectChance) {
						this.speed = attribute.EffectValue * this.initialSpeed;
						this.isFrozen = true;
						gameObject.GetComponent<SpriteRenderer> ().color = new Color (0f, 0.5f, 1f, 1f);
				}
		}
}
