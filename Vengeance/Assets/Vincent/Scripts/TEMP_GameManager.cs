using UnityEngine;
using System.Collections;

public class TEMP_GameManager_TimeBased : MonoBehaviour {

	public BGMPlayer_TimeBased bgmPlayer;

	// Use this for initialization
	void Start () {
		bgmPlayer = GetComponentInChildren<BGMPlayer_TimeBased>();
	}

	// Update is called once per frame
	void Update () {

	}

}