using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

// ================
// || HOW TO USE ||
// ================
// 1. Add one instance of each enemy type to this spawner as a child object
// 2. Set enemy spawner settings
//		Start Spawning On Creation - execute spawn plan once this spawner is created
//			If this is set to false, enemies will not start spawning until startSpawning() is called by another script
//		Start Delay - how long to wait once this spawner is started before creating the first enemy
// 3. Set spawnPlan's length to the total number of enemes to spawn
// 4. Edit each entry in spawnPlan
// 		Enemy Type - the name of the child enemy game object to spawn
//		Position - coordinates to spawn it at
// 		Delay in Secs - how long to wait before spawning the enemy

public enum BGMFlag { NONE, START_MUSIC, STOP_MUSIC, CHANGE_MUSIC }

public class EnemySpawner : MonoBehaviour {
    [System.Serializable]
	public class EnemySpawnInfo {
        public GameObject enemy;
		public float delayInSecs;
		public BGMFlag bgmFlag = BGMFlag.NONE;
        // TODO: make the below field appear in the inspector ONLY if bgmFlag equals BGMFlag.CHANGE_TRACK
        // public int indexOfMusicToPlay = 0;
    }
	public bool startSpawningOnCreation;
	public float startDelay;
	public string spawnPlanXmlFilename = "test.xml";
    public bool loadSpawnPlanOnStartup = false;
	public bool saveSpawnPlanOnExit = true;
	public EnemySpawnInfo[] spawnPlan;

	private Dictionary<string, GameObject> enemyDict;
	private XmlSerializer spawnPlanSerializer;
	private BGMPlayer bgmPlayer;
	
	// Use this for initialization
	void Start () {
		enemyDict = new Dictionary<string, GameObject>();
		bgmPlayer = FindObjectOfType<BGMPlayer>();

		// enemyDict only adds children GameObjects if they have the "Enemy" tag
		foreach (Transform child in transform) {
			if (child.tag == "Enemy") {
				Debug.Log("Added " + child.gameObject.name);
				enemyDict.Add(child.gameObject.name, child.gameObject);
			}
		}

		spawnPlanSerializer = new XmlSerializer(typeof(EnemySpawnInfo[]));

        if (spawnPlanXmlFilename.Length > 0 && loadSpawnPlanOnStartup) {
			loadSpawnPlanFromXml(spawnPlanXmlFilename);
			Debug.Log("Successfully loaded spawnPlan from " + spawnPlanXmlFilename);
		}

		if (startSpawningOnCreation) {
			startSpawning();
		}
	}

	void OnApplicationQuit() {
        if (saveSpawnPlanOnExit) {
			spawnPlanToXml();
			Debug.Log("Successfully saved spawnPlan to " + spawnPlanXmlFilename);
		}
	}

	public void startSpawning() {
		StartCoroutine(startSpawnPlan());
	}

	// All enemies are spawned with rotation Quaternion.identity for now
	// spawn.position uses absolute coordinates for now
	private IEnumerator startSpawnPlan() {
		if (startDelay > 0.0f) {
			yield return new WaitForSeconds(startDelay);
		}

		foreach (EnemySpawnInfo spawn in spawnPlan) {
            if (spawn.enemy != null) {
				if (spawn.delayInSecs > 0.0f) {
					yield return new WaitForSeconds(spawn.delayInSecs);
				}
                spawn.enemy.SetActive(true);
            }
            if (spawn.bgmFlag != BGMFlag.NONE) {
				updateBGMPlayer(spawn.bgmFlag);
			}
		}
	}

	private void updateBGMPlayer(BGMFlag flag) {
		switch (flag) {
			case BGMFlag.START_MUSIC:
				bgmPlayer.start();
				break;
			case BGMFlag.STOP_MUSIC:
				bgmPlayer.stop();
				break;
			case BGMFlag.CHANGE_MUSIC:
				bgmPlayer.changeTrack(bgmPlayer.currentTrackIndex + 1);
				break;
			case BGMFlag.NONE:
			default:
				Debug.Log("Invalid flag passed to updateBGMPlayer(): " + flag.ToString());
				break;
		}
	}

	public void spawnPlanToXml() {
        try {
			using (var xmlFile = File.OpenWrite(spawnPlanXmlFilename)) {
				spawnPlanSerializer.Serialize(xmlFile, spawnPlan);
			}
		} catch (IOException e) {
			Debug.Log("Error saving spawnPlan: " + e.Message);
		} catch (XmlException e) {
			Debug.Log("Error savingSpawnPlan: " + e.Message);
		}
	}

	public void loadSpawnPlanFromXml(string filename) {
		try {
			using (var inFile = File.OpenRead(filename)) {
				spawnPlan = (EnemySpawnInfo[]) spawnPlanSerializer.Deserialize(inFile);
			}
		} catch (IOException e) {
			Debug.Log("Error loading spawnPlan: " + e.Message);
		} catch (XmlException e) {
			Debug.Log("Error loading spawnPlan: " + e.Message);
		}
	}
}
