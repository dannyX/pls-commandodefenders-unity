using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Gun : MonoBehaviour {

	public GameObject projectile;

	public bool followClosestTarget = false;
	public bool followMouse = false;
	
	public List<string> attributes;

	protected float elapsedTime = 0.0f;
	protected float reloadTimer = 0.0f;
	protected float initialFiringSpeed;
	protected bool hasTargetInRange = false;
	protected bool isReloading = false;

	protected GameObject mainPanel;
	protected Survivor owner;
	protected GunModel model;

	public Survivor Owner {
		get {
			return owner;
		}
	}

	public GunModel Model {
		get {
			return model;
		}
		set {
			model = value;
		}
	}

	// Use this for initialization
	void Start () {
		this.initialFiringSpeed = this.model.FireRate;
		this.owner = GameObject.Find ("Survivor").GetComponent<Survivor> ();
		this.mainPanel = GameObject.Find ("MainPanel");
	}

	// Update is called once per frame
	void Update () {
		if (this.followMouse) {
				Vector3 touchPos3D = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
				float deltaX = transform.position.x - touchPos3D.x;
				float deltaY = transform.position.y - touchPos3D.y;
				float angle = Mathf.Atan2 (deltaY, deltaX) * 180 / Mathf.PI;
				transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle + 90));
		} else if (this.followClosestTarget) {
			Vector3 closestVector = Vector3.zero;
			Vector3 closestDelta = Vector3.zero;
			Collider[] colliders = Physics.OverlapSphere (transform.position, this.Model.Range);
			this.hasTargetInRange = false;
			foreach (Collider col in colliders ) {
				if (col.gameObject.tag == "Zombie") {
					if (!this.attributes.Contains ("freeze") || (this.attributes.Contains ("freeze") && !col.gameObject.GetComponent<Zombie> ().IsFrozen)) {
							Vector3 deltaVector = (transform.position - col.transform.position);
							if (closestVector == Vector3.zero || (closestVector != Vector3.zero && deltaVector.sqrMagnitude < closestDelta.sqrMagnitude)) {
									closestVector = col.transform.position;
									closestDelta = deltaVector;
							}
							this.hasTargetInRange = true;
					}
				}
			}

			float deltaX = transform.position.x - closestVector.x;
			float deltaY = transform.position.y - closestVector.y;
			float angle = Mathf.Atan2 (deltaY, deltaX) * 180 / Mathf.PI;
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle + 90));
		}

				if (!isReloading) {
						this.elapsedTime += Time.deltaTime;
				} else {
						this.reloadTimer += Time.deltaTime;
				}

				if (reloadTimer >= this.Model.ReloadSpeed) {
						isReloading = false;
						this.Model.Ammo = this.Model.MagazineSize;
						reloadTimer = 0.0f;
				}

				if (elapsedTime >= this.model.FireRate && (!this.followClosestTarget || (this.followClosestTarget && this.hasTargetInRange))) {
						this.Fire ();
						this.elapsedTime = 0.0f;		
				}
	}

		protected void ReduceAmmo () {
				this.Model.Ammo--;

				if (!this.HasAmmo () && !this.isReloading) {
						this.isReloading = true;
						this.elapsedTime = 0.0f;
				}
		}

		protected bool HasAmmo () {
				return this.Model.Ammo > 0;
		}

		protected abstract void Fire ();
}
