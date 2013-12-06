using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineGun : Gun {

		protected override void Fire () {
				this.ReduceAmmo ();
				GameObject proj = (GameObject) Instantiate(projectile, transform.position, transform.rotation);
				proj.GetComponent<ForwardProjectile> ().Gun = this;
				proj.transform.parent = this.mainPanel.transform;
		
				Vector3 touchPos3D = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
				float deltaX = transform.position.x - touchPos3D.x;
				float deltaY = transform.position.y - touchPos3D.y;
				float angle = Mathf.Atan2 (deltaY, deltaX) * 180 / Mathf.PI;
				angle += Random.Range (-(this.model.Accuracy / 2), this.model.Accuracy / 2);
				proj.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle + 93));

				if (this.Model.ProjectileType == "Homing") {
						Vector3 closestVector = Vector3.zero;
						Vector3 closestDelta = Vector3.zero;
						Collider[] colliders = Physics.OverlapSphere (transform.position, this.Model.Range);
						this.hasTargetInRange = false;
						foreach (Collider col in colliders) {
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

						deltaX = transform.position.x - closestVector.x;
						deltaY = transform.position.y - closestVector.y;
						angle = Mathf.Atan2 (deltaY, deltaX) * 180 / Mathf.PI;
						proj.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle + 93));
				}


				if (this.Model.Attribute != null) {
						if (this.Model.Attribute.Id == "Spread") {
								for (int i = 0; i < 10; i++) {
										proj = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
										proj.GetComponent<ForwardProjectile> ().Gun = this;
										proj.transform.parent = this.mainPanel.transform;	

										touchPos3D = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
										deltaX = transform.position.x - touchPos3D.x;
										deltaY = transform.position.y - touchPos3D.y;
										angle = Mathf.Atan2 (deltaY, deltaX) * 180 / Mathf.PI;
										angle += Random.Range (-(this.model.Attribute.EffectRadius / 2), this.model.Attribute.EffectRadius / 2);
										proj.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle + 93));
								}
						} else if (this.Model.Attribute.Id == "Freeze") {
								proj.GetComponent<SpriteRenderer> ().sprite = proj.GetComponent <ForwardProjectile> ().sprites [1];
						}

				}
	}
}
