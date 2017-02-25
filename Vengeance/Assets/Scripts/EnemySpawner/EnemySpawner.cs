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
public class EnemySpawner : MonoBehaviour {

	[System.Serializable]
	public class EnemySpawnInfo {
		public string enemyType;
		public Vector3 position;
		public float delayInSecs;

	}
	public bool startSpawningOnCreation;
	public float startDelay;
	public string spawnPlanXmlFilename = "test.xml";
	public bool saveSpawnPlanOnExit = true;
	public EnemySpawnInfo[] spawnPlan;

	private Dictionary<string, GameObject> enemyDict;
	private XmlSerializer spawnPlanSerializer;
	
	// Use this for initialization
	void Start () {
		enemyDict = new Dictionary<string, GameObject>();

		// enemyDict only adds children GameObjects if they have the "Enemy" tag
		foreach (Transform child in transform) {
			if (child.tag == "Enemy") {
				Debug.Log("Added " + child.gameObject.name);
				enemyDict.Add(child.gameObject.name, child.gameObject);
			}
		}

		spawnPlanSerializer = new XmlSerializer(typeof(EnemySpawnInfo[]));

        if (spawnPlanXmlFilename.Length > 0) {
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
			Debug.Log("Spawning " + spawn.enemyType + " with delay " + spawn.delayInSecs);
			if (spawn.delayInSecs > 0.0f) {
				yield return new WaitForSeconds(spawn.delayInSecs);
			}
			Instantiate(enemyDict[spawn.enemyType], spawn.position, Quaternion.identity);
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
