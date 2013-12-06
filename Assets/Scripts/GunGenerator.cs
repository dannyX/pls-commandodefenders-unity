using UnityEngine;
using System.Collections;
using System.Reflection;

public class GunGenerator {

		public enum GunQuality
		{
				normal,
				average,
				worst,
				perfect
		};

		private static GunGenerator gunGenerator = null;

		private GunGenerator () {

		}

		public static GunGenerator GetInstance () {
				if (gunGenerator == null) {
						gunGenerator = new GunGenerator ();
				}

				return gunGenerator;
		}


		public GunModel GenerateGunForLevel (int level) {
				GunModel gun = new GunModel ();

				JSONNode weaponTypes = DictionaryManager.GetInstance ().WeaponTypes;
				JSONNode selectedWeaponType = SelectWeaponType (weaponTypes);
				JSONNode selectedClass = SelectClass (level, selectedWeaponType);

				RollGunProperties (gun, selectedWeaponType, selectedClass, GunQuality.normal);
				RollProjectileType (gun, selectedWeaponType, selectedClass);
				RollAttribute (gun, selectedWeaponType, selectedClass);

				Debug.Log ("Fire Rate: " + gun.FireRate);
				Debug.Log ("Accuracy: " + gun.Accuracy);
				Debug.Log ("Reload Speed: " + gun.ReloadSpeed);
				Debug.Log ("Magazine Size: " + gun.MagazineSize);
				Debug.Log ("Range: " + gun.Range);
				Debug.Log ("Damage: " + gun.Damage);
				Debug.Log ("Projectile Type: " + gun.ProjectileType);
				Debug.Log (selectedWeaponType);
				Debug.Log (selectedClass);

				return gun;
		}

		static JSONNode SelectWeaponType (JSONNode weaponTypes)
		{
			int numTypes = weaponTypes.Count;
			int selectedIndex = Random.Range (0, numTypes);
			JSONNode selectedWeaponType = null;
			int i = 0;
			foreach (JSONNode weaponType in weaponTypes.Childs) {
				if (i != selectedIndex) {
					i++;
					continue;
				}
				selectedWeaponType = weaponType;
				break;
			}
			return selectedWeaponType;
		}

		static string SelectClass (int level, JSONNode selectedWeaponType)
		{
			string selectedClass = "0";
			foreach (JSONNode c in selectedWeaponType ["Classes"].Childs) {
				if (level >= c ["Level"] ["min"].AsInt && level <= c ["Level"] ["max"].AsInt) {
					selectedClass = c ["ClassId"];
					break;
				}
			}
			return selectedClass;
		}

		static void RollGunProperties (GunModel gun, JSONNode selectedWeaponType, JSONNode selectedClass, GunQuality quality)
		{
				foreach (JSONNode properties in selectedWeaponType ["Classes"] [selectedClass] ["PropertyDistribution"].Childs) {
						PropertyInfo prop = gun.GetType ().GetProperty (properties ["id"]);
						if (properties ["min"].ToString ().Contains (".")) {
								switch (quality) {
								case GunQuality.normal: 
										prop.SetValue (gun, Random.Range (properties ["min"].AsFloat, properties ["max"].AsFloat), null);
										break;
								case GunQuality.perfect: 
										prop.SetValue (gun, properties ["max"].AsFloat, null);
										break;
								case GunQuality.worst: 
										prop.SetValue (gun, properties ["min"].AsFloat, null);
										break;
								case GunQuality.average:
										prop.SetValue (gun, (properties ["max"].AsFloat + properties ["min"].AsFloat) / 2, null);
										break;
								default:
										break;
								}
						} else {
								switch (quality) {
								case GunQuality.normal: 
										prop.SetValue (gun, Random.Range (properties ["min"].AsInt, properties ["max"].AsInt), null);
										break;
								case GunQuality.perfect: 
										prop.SetValue (gun, properties ["max"].AsInt, null);
										break;
								case GunQuality.worst: 
										prop.SetValue (gun, properties ["min"].AsInt, null);
										break;
								case GunQuality.average:
										prop.SetValue (gun, (properties ["max"].AsInt + properties ["min"].AsInt) / 2, null);
										break;
								default:
										break;
								}
						}
				}
		}

		static void RollProjectileType (GunModel gun, JSONNode selectedWeaponType, JSONNode selectedClass)
		{
			float rand = Random.Range (0.0f, 100.0f);
			float weight = 0.0f;
			foreach (JSONNode projectileType in selectedWeaponType ["Classes"] [selectedClass] ["ProjectileType"].Childs) {
				weight += projectileType ["chance"].AsFloat;
				if (weight >= rand) {
					gun.ProjectileType = projectileType ["id"];
					break;
				}
			}
		}

	static void RollAttribute (GunModel gun, JSONNode selectedWeaponType, JSONNode selectedClass)
	{
		if (selectedWeaponType ["Classes"] [selectedClass] ["AttributeChance"].AsFloat >= Random.Range (0.0f, 100.0f)) {
			float rand = Random.Range (0.0f, 100.0f);
			float weight = 0.0f;
			foreach (JSONNode attribute in selectedWeaponType ["Classes"] [selectedClass] ["Attributes"].Childs) {
				weight += attribute ["RollChance"].AsFloat;
				if (weight >= rand) {
					gun.Attribute = new GunAttribute ();
					gun.Attribute.Id = attribute ["id"];
					Debug.Log ("Attribute Id: " + gun.Attribute.Id);
					if (attribute ["Chance"] != null) {
						gun.Attribute.EffectChance = Random.Range (attribute ["Chance"] ["min"].AsInt, attribute ["Chance"] ["max"].AsInt);
						Debug.Log ("Effect Chance: " + gun.Attribute.EffectChance);
					}
					if (attribute ["Duration"] != null) {
						gun.Attribute.EffectDuration = Random.Range (attribute ["Duration"] ["min"].AsFloat, attribute ["Duration"] ["max"].AsFloat);
						Debug.Log ("Attribute Duration: " + gun.Attribute.EffectDuration);
					}
					if (attribute ["Effectiveness"] != null) {
						gun.Attribute.EffectValue = Random.Range (attribute ["Effectiveness"] ["min"].AsFloat, attribute ["Effectiveness"] ["max"].AsFloat);
						Debug.Log ("Attribute Value: " + gun.Attribute.EffectValue);
					}
					if (attribute ["Radius"] != null) {
						gun.Attribute.EffectRadius = Random.Range (attribute ["Radius"] ["min"].AsFloat, attribute ["Radius"] ["max"].AsFloat);
						Debug.Log ("Attribute Radius: " + gun.Attribute.EffectRadius);
					}
					break;
				}
			}
		}
	}
}
