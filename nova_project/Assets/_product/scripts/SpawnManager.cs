using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
	[SerializeField]
	GameObject _player_prefab = null;


	[SerializeField]
	GameObject _partner_prefab = null;

	[SerializeField]
	GameObject _enemy_prefab = null;

	[SerializeField]
	GameObject stage_prefab = null;
	
	[SerializeField]
	RuntimeAnimatorController _player_animator_prefab = null;


	[SerializeField]
	RuntimeAnimatorController _enemy_animator_prefab = null;

	[HideInInspector]
	public Animator _player_animator = null;


	[HideInInspector]
	public Animator _enemy_animator = null;

	[HideInInspector]
	public GameObject _player_object = null;

	[HideInInspector]
	public GameObject _partner_object = null;

	[HideInInspector]
	public List<GameObject> _enemy_object_list = null;

	// Start is called before the first frame update
	void Start()
    {
        CreatePrayer();
		CreatePartner();

		for (int i = 0; i < 3; i++)
		{
			CreateEnemy(i);

		}

		CreateStage();
    }

    // Update is called once per frame
    void Update()
    {

	}

	public void CreatePrayer() {
	
		_player_object = Instantiate(_player_prefab);


		_player_object.transform.position = new Vector3(-1.61f, 0 , 0.4f);
		_player_object.transform.rotation = Quaternion.Euler(0, 90 , 0);

		_player_object.GetComponent<Animator>().runtimeAnimatorController = _player_animator_prefab;

		_player_animator = _player_object.GetComponent<Animator>();
	}


	public void CreatePartner() {
	
		_partner_object = Instantiate(_partner_prefab);
		_partner_object.transform.position = new Vector3(2.17f, 0 , 1.4f);
		_partner_object.transform.rotation = Quaternion.Euler(0, -90 , 0);

		_partner_object.GetComponent<Animator>().runtimeAnimatorController = _enemy_animator_prefab;


		_enemy_animator = _partner_object.GetComponent<Animator>();
	}



	float[] formation = {-50, 0 , 50 };

	public void CreateEnemy(int i)
	{

		GameObject enemy_object = Instantiate(_enemy_prefab);

		_enemy_object_list.Add(enemy_object);

		int random_value = UnityEngine.Random.Range(-10, 10);
		enemy_object.transform.position = new Vector3(2.17f + random_value + formation[i], 0, 70f);

		EnemyController enemy_controller = enemy_object.AddComponent<EnemyController>();

		enemy_controller._player_object = _player_object;

		//_enemy_object.GetComponent<Animator>().runtimeAnimatorController = _enemy_animator_prefab;


		//_enemy_animator = _enemy_object.GetComponent<Animator>();
	}




	public void CreateStage() {
	
		GameObject stage_object = Instantiate(stage_prefab);
		//enemy_object.transform.position = new Vector3(0, 0 , 7);
	}


	public float GetAngle(Vector2 p1, Vector2 p2)
	{
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		float rad = Mathf.Atan2(dx, dy);
		return rad * Mathf.Rad2Deg;
	}
}
