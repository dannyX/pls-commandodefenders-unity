using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public GameObject goItem;
	public GameObject[] goObjectsToDeactivate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick () {
		goItem.SetActive (true);
		foreach (GameObject go in goObjectsToDeactivate) {
				go.SetActive (false);
		}
	}
}
