using UnityEngine;
using System.Collections;

public class Shotgun : Gun {

	public int bullets = 20;

		protected override void Fire () {
				this.ReduceAmmo ();
				for (int i = 0; i < bullets; i++) {
					GameObject proj = (GameObject) Instantiate(projectile, transform.position, transform.rotation);
					proj.GetComponent<ForwardProjectile> ().Gun = this;
					proj.transform.parent = this.mainPanel.transform;	
					
					Vector3 touchPos3D = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
					float deltaX = transform.position.x - touchPos3D.x;
					float deltaY = transform.position.y - touchPos3D.y;
					float angle = Mathf.Atan2 (deltaY, deltaX) * 180 / Mathf.PI;
					angle += Random.Range (-60f, -120f);
					proj.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle + 180));
				}

	}
}
