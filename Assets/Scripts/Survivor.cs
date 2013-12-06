using UnityEngine;
using System.Collections;

public class Survivor : MonoBehaviour {

	public Gun Gun;

	public int health = 10;

	private int money;
	private int maxHealth;
	private bool isDead = false;

	private Hashtable stats;
	private Hashtable achievements;

	public Hashtable Stats {
		get {
			return stats;
		}
	}

	public bool IsDead {
		get {
			return isDead;
		}
	}

	public int Money {
		get {
			return money;
		}
	}

	// Use this for initialization
	void Start () {
				PlayerPrefs.DeleteKey ("kill_num_zombies");
				PlayerPrefs.DeleteKey ("kill_num_zombies_1");
				PlayerPrefs.DeleteKey ("kill_num_zombies_56");
		this.maxHealth = this.health;
		this.money = PlayerPrefs.GetInt ("Money");

		stats = new Hashtable ();
		achievements = new Hashtable ();

		LoadAchievements ();
		LoadStat ("kill_num_zombies");

//				animation.Play ("Reloading");

//		this.Gun = GameObject.Find ("Gun").GetComponent<Gun> ();
	}

	void LoadAchievements () {
				foreach (JSONNode node in DictionaryManager.GetInstance ().Achievements.Childs) {
						foreach (JSONNode goalsNode in node ["goals"].Childs) {
							if (PlayerPrefs.GetInt (goalsNode ["id"]) != 0) {
								this.achievements.Add (goalsNode ["id"], PlayerPrefs.GetInt (node ["id"]));
							}
						}
				}
	}

	void LoadStat (string statName)
	{
		if (PlayerPrefs.GetInt (statName) != 0) {
			this.stats.Add (statName, PlayerPrefs.GetInt (statName));
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 touchPos3D = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
		float deltaX = transform.position.x - touchPos3D.x;
		float deltaY = transform.position.y - touchPos3D.y;
		float angle = Mathf.Atan2 (deltaY, deltaX) * 180 / Mathf.PI;
		transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle + 90));

		if (Input.touches.Length > 0) {
			Touch touch = Input.touches[0];
			touchPos3D = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
			transform.position = new Vector3(touchPos3D.x, touchPos3D.y + 0.1f, 0f);
		}
		else if (Input.touches.Length == 0) {
			//			Time.timeScale = 0.0f;
		}
	}

	public void ReceiveDamage (int attackPower)
	{
		Debug.Log (string.Format("damaging player for {0} damage", attackPower));
		this.health -= attackPower;
		Color color = gameObject.GetComponent<SpriteRenderer> ().color;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (color.r, (((float)this.health / (float)this.maxHealth)), (((float)this.health / (float)this.maxHealth)), color.a);
		if (this.health <= 0) {
			this.isDead = true;
		}
	}

	public void IncrementStat (string statName)
	{
		IncrementStat (statName, 1);
	}

	public void IncrementStat (string statName, int amount) {
		if (this.stats [statName] != null) {
			this.stats [statName] = (int)this.stats [statName] + amount;
		} else {
			this.stats.Add(statName, amount);
		}

		DictionaryManager dm = DictionaryManager.GetInstance ();

		if (dm.Achievements [statName] != null) {
			foreach (JSONNode goalsNode in dm.Achievements [statName]["goals"].Childs) {
				if ((int)this.stats [statName] >= goalsNode ["amount"].AsInt && !this.achievements.Contains(goalsNode ["id"])) {
					this.achievements.Add (goalsNode ["id"], 1);
					PlayerPrefs.SetInt(goalsNode ["id"], 1);
					PlayerPrefs.Save ();
					GameObject popup = GameObject.Find ("LevelGenerator").GetComponent<LevelGenerator> ().AchievementPopup;
					foreach (Transform child in popup.transform) {
						if (child.name == "LabelAchievementName") {
							child.GetComponent<UILabel> ().text = "You've destroyed the population of: " + goalsNode ["name"];
						}
					}
					popup.SetActive (true);
					StartCoroutine ("HideAchievementPopup");
				}
			}
		}

	}

	IEnumerator HideAchievementPopup () {
		yield return new WaitForSeconds(3f);
		GameObject.Find ("LevelGenerator").GetComponent<LevelGenerator> ().AchievementPopup.SetActive (false);
	}

	public void IncrementMoney ()
	{
		this.money++;
	}
}
