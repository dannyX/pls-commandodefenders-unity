using UnityEngine;
using System.Collections;

public class DictionaryManager
{
	private JSONNode achievements;
	private JSONNode weaponTypes;

	public JSONNode Achievements {
		get {
			return achievements;
		}
	}

	public JSONNode WeaponTypes {
		get {
			return weaponTypes;
		}
	}

	private static DictionaryManager dictionaryManager = null;

	private DictionaryManager () {

	}

	public static DictionaryManager GetInstance () {
		if (dictionaryManager == null) {
			dictionaryManager = new DictionaryManager ();
		}

		return dictionaryManager;
	}

		public void LoadAchievementDictionary (string json) {
				this.achievements = JSON.Parse (json);
		}

		public void LoadWeaponTypeDictionary (string json) {
				this.weaponTypes = JSON.Parse (json);
		}
}