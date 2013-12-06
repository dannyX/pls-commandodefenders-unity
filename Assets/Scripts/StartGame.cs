using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick () {
		GameObject.Find ("LevelGenerator").GetComponent<LevelGenerator> ().IsStarted = true;
		GameObject.Find ("LevelGenerator").GetComponent<LevelGenerator> ().MusicSource.audio.Play ();
		GameObject.Find ("LevelGenerator").GetComponent<LevelGenerator> ().survivor.gameObject.SetActive (true);
		GameObject.Find ("ItemSelection").SetActive (false);
		Destroy (gameObject);
	}
}
