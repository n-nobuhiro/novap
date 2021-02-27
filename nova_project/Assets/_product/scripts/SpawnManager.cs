using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
	[SerializeField]
	GameObject player_prefab = null;

	[SerializeField]
	GameObject enemy_prefab = null;

	[SerializeField]
	GameObject stage_prefab = null;


	public GameObject player_object = null;

    // Start is called before the first frame update
    void Start()
    {
        CreatePrayer();
		CreateEnemy();
		CreateStage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void CreatePrayer() {
	
		player_object = Instantiate(player_prefab);
	}


	public void CreateEnemy() {
	
		GameObject enemy_object = Instantiate(enemy_prefab);
		enemy_object.transform.position = new Vector3(0, 0 , 7);
	}


	public void CreateStage() {
	
		GameObject stage_object = Instantiate(stage_prefab);
		//enemy_object.transform.position = new Vector3(0, 0 , 7);
	}
}
