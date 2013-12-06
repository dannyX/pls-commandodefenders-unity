using UnityEngine;
using System.Collections;

public class GunAttribute {

		private string id;
		private int effectChance;
		private float effectDuration;
		private float effectValue;
		private float effectRadius;
	
	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	public int EffectChance {
		get {
			return effectChance;
		}
		set {
			effectChance = value;
		}
	}

	public float EffectDuration {
		get {
			return effectDuration;
		}
		set {
			effectDuration = value;
		}
	}

	public float EffectValue {
		get {
			return effectValue;
		}
		set {
			effectValue = value;
		}
	}

	public float EffectRadius {
		get {
			return effectRadius;
		}
		set {
			effectRadius = value;
		}
	}

		public GunAttribute () {

		}
}
