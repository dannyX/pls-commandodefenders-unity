using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

	public Survivor survivor;
	public GameObject zombie;
	public ParticleSystem BloodSpill;
	public ParticleSystem DeathSpill;

	public GameObject LabelScore;
	public GameObject BestScore;
	public GameObject LabelGameOver;
	public GameObject LabelRound;

	public GameObject AchievementPopup;

	public GameObject MusicSource;

	public TextAsset AchievementsAsset;
	public TextAsset WeaponTypesAsset;

	public bool IsStarted = false;

	private int score = 0;
	private int round = 1;
	private float spawnDelay = 5f;
	private float spawnQuantity = 2.0f;
	private float roundDuration = 15.0f;

	private float elapsedSpawnTime = 0.0f;
	private float elapsedRoundTime = 0.0f;

	private GameObject zombieObjects;

	// Use this for initialization
	void Start () {
		DictionaryManager dm = DictionaryManager.GetInstance ();
		dm.LoadAchievementDictionary (AchievementsAsset.text);
		dm.LoadWeaponTypeDictionary (WeaponTypesAsset.text);
		GunTypeManager gunTypeManager = ((GameObject)GameObject.Find ("GunTypeManager")).GetComponent<GunTypeManager> ();

		GameObject goSurvivor = GameObject.Find ("Survivor");
		GunModel model = GunGenerator.GetInstance ().GenerateGunForLevel (1);

		GameObject gun = (GameObject) Instantiate(gunTypeManager.machineGun, this.survivor.transform.position, this.survivor.transform.rotation);
		gun.transform.parent = goSurvivor.transform;
		gun.transform.localPosition = new Vector3 (this.survivor.transform.localPosition.x - 0.08f, this.transform.localPosition.y - 1f, this.transform.localPosition.z);

		this.survivor.Gun = gun.GetComponent<Gun> ();
		this.survivor.Gun.Model = model;
		this.zombieObjects = GameObject.Find ("Zombies");

		int bestScore = PlayerPrefs.GetInt ("BestScore");
		this.BestScore.GetComponent<UILabel> ().text = "Highest Score: " + bestScore;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.IsStarted) {
				UpdateTimers ();
				SpawnZombies ();
				UpdateRound ();

				if (survivor.IsDead) {
						int bestScore = PlayerPrefs.GetInt ("BestScore");
						if (this.score > bestScore) {
								PlayerPrefs.SetInt ("BestScore", this.score);
						}
						PlayerPrefs.SetInt ("Money", this.survivor.Money);
						PlayerPrefs.Save ();

						Destroy (this.zombieObjects);
						Destroy (GameObject.Find ("Survivor"));

						LabelGameOver.SetActive (true);
				}
		}
	}

	void UpdateTimers ()
	{
		this.elapsedSpawnTime += Time.deltaTime;
		this.elapsedRoundTime += Time.deltaTime;
	}

	void SpawnZombies () {
		if (!this.survivor.IsDead && this.elapsedSpawnTime >= this.spawnDelay) {
			int spawnQuantity = (int)Mathf.Floor (this.spawnQuantity);
			for (int i = 0; i < spawnQuantity; i++) {
					SpawnZombie ();
			}

			this.elapsedSpawnTime = 0.0f;
		}
	}

	void SpawnZombie ()
	{
		//
		// Instantiate a zombie
		float xPos = Random.Range (-1f, 1f);
		Vector3 zPos = new Vector3 (xPos, transform.position.y, transform.position.z);
		GameObject z = (GameObject)Instantiate (this.zombie, zPos, transform.rotation);
		z.GetComponent<Zombie> ().levelManager = this;
		//
		// Assign Particle Systems
		z.GetComponent<Zombie> ().BloodSpill = this.BloodSpill;
		z.GetComponent<Zombie> ().DeathSpill = this.DeathSpill;
		//
		// Flip image if zombie is on the right side of the player
		if (zPos.x > 0) {
			Vector3 ls = z.transform.localScale;
			z.transform.localScale = new Vector3 (ls.x * -1, ls.y, ls.z);
		}
		//
		// Put the transform on the zombies panel
		z.transform.parent = this.zombieObjects.transform;
	}

	void UpdateRound () {
		if (this.elapsedRoundTime >= this.roundDuration) {
			this.round++;
			this.LabelRound.GetComponent<UILabel> ().text = "Round: " + this.round;
			PlayerPrefs.Save ();

			Debug.Log (this.round);

			this.spawnQuantity += 0.35f;
			this.spawnDelay -= 0.35f;

			this.elapsedRoundTime = 0.0f;
		}
	}

	public void IncrementScore () {
		this.survivor.IncrementMoney ();
		this.score += 1;
		this.LabelScore.GetComponent<UILabel> ().text = "Score: " + this.score;
	}
}
