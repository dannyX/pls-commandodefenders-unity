using UnityEngine;
using System.Collections;

public class GunModel {

		private float fireRate;
		private float accuracy;
		private float reloadSpeed;
		private int magazineSize;
		private float range;
		private int damage;

		private int ammo;

		private string projectileType;
		private GunAttribute attribute;

		public float FireRate {
			get {
				return fireRate;
			}
			set {
				fireRate = value;
			}
		}

		public float Accuracy {
			get {
				return accuracy;
			}
			set {
				accuracy = value;
			}
		}

		public float ReloadSpeed {
			get {
				return reloadSpeed;
			}
			set {
				reloadSpeed = value;
			}
		}

		public int MagazineSize {
			get {
				return magazineSize;
			}
			set {
				magazineSize = value;
				this.ammo = magazineSize;
			}
		}

		public float Range {
			get {
				return range;
			}
			set {
				range = value;
			}
		}

		public int Damage {
			get {
				return damage;
			}
			set {
				damage = value;
			}
		}

	public string ProjectileType {
		get {
			return projectileType;
		}
		set {
			projectileType = value;
		}
	}

	public GunAttribute Attribute {
		get {
			return attribute;
		}
		set {
			attribute = value;
		}
	}

	public int Ammo {
		get {
			return ammo;
		}
		set {
			ammo = value;
		}
	}

		public GunModel () {

		}
}
