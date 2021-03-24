using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using LuxWater;

public class SpawnManager : SingletonMonoBehaviour<SpawnManager>
{

	[SerializeField]
	public Camera _main_camera;

	[SerializeField]
	GameObject _player_prefab = null;


	[SerializeField]
	GameObject _partner_prefab = null;

	[SerializeField]
	GameObject _enemy_prefab = null;

	[SerializeField]
	ItemList stage_list = null;


	[SerializeField]
	GameObject _rock_prefab = null;



	[SerializeField]
	RuntimeAnimatorController _player_animator_prefab = null;


	[SerializeField]
	RuntimeAnimatorController _enemy_animator_prefab = null;



	[SerializeField]
	public GameObject _bullet_prefab = null;

	[SerializeField]
	public GameObject _missile_prefab = null;

	[SerializeField]
	public AudioClip _missile_sound;

	[SerializeField]
	GameObject _explosion_effect = null;

	[SerializeField]
	GameObject _explosion_sound = null;


	[SerializeField]
	public GameObject _forcus_effect;


	[SerializeField]
	public GameObject _dummy_target = null;



	[SerializeField]
	public ItemList _item_list = null;




	[SerializeField]
	public ItemList _skymap_list = null;

	//----------------------------------------------------------------------
	[HideInInspector]
	public GameObject stage_object = null;

	[HideInInspector]
	public Animator _player_animator = null;


	[HideInInspector]
	public Animator _enemy_animator = null;

	[HideInInspector]
	public GameObject _player_object = null;

	[HideInInspector]
	public GameObject _player_forward = null;

	[HideInInspector]
	public GameObject _partner_object = null;

	[HideInInspector]
	public List<GameObject> _enemy_object_list = null;

	[HideInInspector]
	public int _enemy_dead_num = 0;


	[HideInInspector]
	public List<GameObject> _item_object_list = null;

	float enemy_generate_interval = 0;
	float item_generate_interval = 0;

	public bool is_enemy_spawn = true;


	public bool is_enemy_attack = true;

	// Start is called before the first frame update
	void Start()
    {
		//CreatePrayer();
		//CreatePartner();


		for (int i = 0; i < 3; i++)
		{
			CreateEnemy(i);
		}

		CreateStage();

		
	}

    // Update is called once per frame
    void Update()
    {

		enemy_generate_interval += Time.deltaTime;
		item_generate_interval += Time.deltaTime;


		bool is_stage_end = false;
		if(UIManager.Instance._is_gameclear || UIManager.Instance._is_gameclear)
        {
			is_stage_end = true;
		}


		if (is_stage_end == false 
			&& is_enemy_spawn == true 
			&& enemy_generate_interval >= 10)
		{
			for (int i = 0; i < 3; i++)
			{
				CreateEnemy(i);
			}

			enemy_generate_interval = 0;
		}

		if (is_stage_end == false 
			&& is_enemy_spawn == true 
			&& item_generate_interval >= 5)
		{

			GameObject stage_item = Instantiate(_item_list.item_list[0]);
			stage_item.transform.position = _dummy_target.transform.position + new Vector3(8, -3, 0);

			_item_object_list.Add(stage_item);

			item_generate_interval = 0;
		}

	}


	public void CreatePrayer() {
	
		_player_object = Instantiate(_player_prefab);
		
		_player_forward = new GameObject();
		_player_forward.transform.position = new Vector3(0, 1.5f, -2);

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

		int random_value_y = UnityEngine.Random.Range(-10, 10);

		enemy_object.transform.position = _main_camera.gameObject.transform.position + new Vector3(2.17f + random_value + formation[i], random_value_y, 200f);

		//MeshCollider enemy_collider = enemy_object.AddComponent<MeshCollider>();
		BoxCollider enemy_collider = enemy_object.AddComponent<BoxCollider>();
		Rigidbody enemy_rigidbody = enemy_object.AddComponent<Rigidbody>();
		enemy_rigidbody.useGravity = false;
		//enemy_collider.convex = true;
		enemy_collider.isTrigger = true;
		enemy_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		enemy_collider.size = new Vector3(20, 20, 20);


		EnemyController enemy_controller = enemy_object.AddComponent<EnemyController>();

		enemy_controller._player_object = _player_object;

		enemy_controller._spawn_manager = this;


		enemy_controller._main_camera = _main_camera;

		//_enemy_object.GetComponent<Animator>().runtimeAnimatorController = _enemy_animator_prefab;


		//_enemy_animator = _enemy_object.GetComponent<Animator>();
	}



	public void ResetEnemy()
    {

		foreach (GameObject target_item in _item_object_list)
		{
			Destroy(target_item);
		}

		foreach (GameObject target_enemy in _enemy_object_list)
		{
			Destroy(target_enemy);
		}

		_item_object_list.Clear();

		_enemy_object_list.Clear();
		_enemy_dead_num = 0;

	}


	int[] sepalate = { 4, 12, 10, 10, 10, 10, 10 };


	public void CreateStage() {

		if (stage_object != null)
		{
			Destroy(stage_object);
		}

		int select_stage = UnityEngine.Random.Range(0, stage_list.item_list.Length);
		stage_object = Instantiate(stage_list.item_list[select_stage]);


		if (_main_camera.gameObject.GetComponent<LuxWater_UnderWaterRendering>() == null)
		{
			_main_camera.gameObject.AddComponent<LuxWater_UnderWaterRendering>();
		}


		if (_skymap_list && _skymap_list.skybox_list.Length > 0)
		{
			int select_skybox = UnityEngine.Random.Range(0, _skymap_list.skybox_list.Length);

			RenderSettings.skybox = _skymap_list.skybox_list[select_skybox];

		}

		//FormationCircleSpawn();
	}


	void FormationCircleSpawn()
    {


		float angle_split = 4;

		float PI = 3.14f;

		int syuukai = 0;
		int current_sepalate = 0;


		float not_keeep_num_sqrt = Mathf.Sqrt(30);
		float interval_size = 2;

		for (int i = 0; i < 30; i++)
		{
			GameObject _rock_object = Instantiate(_rock_prefab);

			float interval_x = (-(not_keeep_num_sqrt / 2) + (i % not_keeep_num_sqrt)) * interval_size * 1.15f;
			float interval_z = (-(not_keeep_num_sqrt / 2) + (i % not_keeep_num_sqrt)) * interval_size * 1.15f;

			//_rock_object.transform.position = _dummy_target.transform.position + new Vector3(-interval_x, 0, -interval_z);


			if (i == GetGoukei(syuukai))
			{
				syuukai++;
			}


			current_sepalate = sepalate[syuukai];


			int sepalate_no = (int)((i - angle_split * syuukai) % current_sepalate);

			float angle = 2 * PI * sepalate_no / current_sepalate;

			interval_x = Mathf.Cos(angle);
			interval_z = Mathf.Sin(angle);

			interval_x *= (1.5f + 2 * +syuukai) * interval_size / 1.5f;
			interval_z *= (1.5f + 2 * +syuukai) * interval_size / 1.5f;

			float interval_y = 2f * interval_size + (1f * syuukai);

			Vector3 instance_pos = new Vector3(interval_x, interval_y, interval_z);

			_rock_object.transform.position = instance_pos;
		}
	}


	int GetGoukei(int _syuukai)
	{
		int goukei = 0;

		for(int i = 0; i <= _syuukai; i++)
        {
			goukei += sepalate[i];
        }

		return goukei;
	}


	public float GetAngle(Vector2 p1, Vector2 p2)
	{
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		float rad = Mathf.Atan2(dx, dy);
		return rad * Mathf.Rad2Deg;
	}


	public GameObject SetExplosinEffect()
    {
		// Detonator コンポーネントのAuto Create Forceで自動でAddForceする機能を無効にする
		/*
		if (_explosion_effect && _explosion_effect.GetComponent<Detonator>() != null)
		{
			_explosion_effect.GetComponent<Detonator>().autoCreateForce = false;
		}
		*/

		GameObject explosion_effect_object = Instantiate(_explosion_effect);

		explosion_effect_object.GetComponent<Detonator>().size *= 1;

		GameObject explosion_effect_sound = Instantiate(_explosion_sound);

		return explosion_effect_object;
	}



	// 当たり判定のある銃弾オブジェクトを作成
	public GameObject SetAttackBulletSetting(GameObject bullet_object, GameObject luncher_pos_object, GameObject target_enemy_object, bool _is_player_bullet = true)
	{


		// タップして攻撃するときに自動でエネミーの向きにプレイヤーの方向を向ける
		//Quaternion euler_angle_player = SetPlayerAutoAimForEnemy(bullet_controller._target_object);


		//bullet_object.transform.position = _spawn_manager._player_object.transform.position + new Vector3(0, 1.3f, 0);
		bullet_object.transform.position = luncher_pos_object.transform.position;// + new Vector3(0, 1.3f, 0);

		BulletController bullet_controller = bullet_object.AddComponent<BulletController>();

		bullet_controller._start_pos_object = luncher_pos_object;

		if(_is_player_bullet && target_enemy_object == null)
        {
			target_enemy_object = _dummy_target;
        } 

		bullet_controller._target_object = target_enemy_object;
		bullet_controller._is_player_bullet = _is_player_bullet;

		bullet_controller._spawn_manager = this;

		//MeshCollider bullet_collider = bullet_object.AddComponent<MeshCollider>();
		BoxCollider bullet_collider = bullet_object.AddComponent<BoxCollider>();
		Rigidbody bullet_rigidbody = bullet_object.AddComponent<Rigidbody>();
		bullet_rigidbody.useGravity = false;
		//bullet_collider.convex = true;
		bullet_collider.isTrigger = true;
		bullet_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

		// 照準用の＋画像を表示する.
		//_reticle_cross.SetActive(true);

		return bullet_object;
	}


	public void SetGunBullet()
    {
		GameObject target_object = GetTargetEnemy();


		GameObject bullet_object = Instantiate(_bullet_prefab);
		// 当たり判定のある銃弾オブジェクトを作成
		SetAttackBulletSetting(bullet_object, _main_camera.gameObject, target_object);
	}



	public void SetMissile()
    {

		print("SetMissile");

		//GameObject missile_object = GameObject.CreatePrimitive(PrimitiveType.Cylinder);


		//GameObject missile_object = GameObject.CreatePrimitive(PrimitiveType.Sphere);

		GameObject missile_object = Instantiate(_missile_prefab);


		missile_object.name = "missile_bullet";

		GameObject target_object = GetTargetEnemy();

		if(target_object == null)
        {
			target_object = _dummy_target;
			
		}

		SetAttackBulletSetting(missile_object, _main_camera.gameObject, target_object);

		missile_object.GetComponent<BulletController>()._is_missile = true;


		target_object.GetComponent<EnemyController>()._is_lockon = true;

		AudioSource audioSource = missile_object.AddComponent<AudioSource>();

		// ミサイル発射音を再生
		audioSource.PlayOneShot(_missile_sound);

	}


	public GameObject GetTargetEnemy()
    {

		GameObject target_object = null;

		foreach (GameObject check_enemy in _enemy_object_list)
		{
			if (check_enemy != null 
				&& check_enemy.GetComponent<EnemyController>().is_dead == false
				&& check_enemy.GetComponent<EnemyController>()._is_lockon == false)
			{

				target_object = check_enemy;

				break;
			}
		}

		return target_object;

	}
}
