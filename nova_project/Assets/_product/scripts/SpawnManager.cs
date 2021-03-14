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
	
	[SerializeField]
	RuntimeAnimatorController animator_prefab = null;

	[SerializeField]
	public Animator _player_animator = null;


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

		
		player_object.transform.position = new Vector3(-1.61f, 0 , 0.4f);
		player_object.transform.rotation = Quaternion.Euler(0, 90 , 0);

		player_object.GetComponent<Animator>().runtimeAnimatorController = animator_prefab;

		_player_animator = player_object.GetComponent<Animator>();
	}


	public void CreateEnemy() {
	
		GameObject enemy_object = Instantiate(enemy_prefab);
		enemy_object.transform.position = new Vector3(2.17f, 0 , 1.4f);
		enemy_object.transform.rotation = Quaternion.Euler(0, -90 , 0);
		//enemy_object.GetComponent<Animator>().runtimeAnimatorController = animator_prefab;
	}


	public void CreateStage() {
	
		GameObject stage_object = Instantiate(stage_prefab);
		//enemy_object.transform.position = new Vector3(0, 0 , 7);
	}
}
