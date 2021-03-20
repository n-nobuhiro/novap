using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public GameObject _player_object = null;


    public SpawnManager _spawn_manager = null;

    public bool is_dead = false;

    public Camera _main_camera = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    float bullet_generate_interval = 0;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -100)
        {
            is_dead = true;
        }


        SetRotationEnemy();
        SetPositionEnemy();


        bullet_generate_interval += Time.deltaTime;

        if (SpawnManager.Instance.is_enemy_attack && bullet_generate_interval >= 1.5f)
        {

            GameObject bullet_object = Instantiate(SpawnManager.Instance._bullet_prefab);
            // 当たり判定のある銃弾オブジェクトを作成
            _spawn_manager.SetAttackBulletSetting(bullet_object, gameObject, _main_camera.gameObject, false);

            bullet_generate_interval = 0;
        }
       
    }


    public float GetAngle(Vector2 p1, Vector2 p2)
    {
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float rad = Mathf.Atan2(dx, dy);
        return rad * Mathf.Rad2Deg;
    }


    void SetRotationEnemy()
    {
        Vector2 enemy_pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        Vector2 player_pos = new Vector2(_player_object.transform.position.x, _player_object.transform.position.z);

        if(Vector3.Distance(player_pos, enemy_pos) <= 60 && is_dead == false)
        {
            return;
        }

        float enemy_angle = GetAngle(enemy_pos, player_pos);
        float enemy_pitch = 0;
        float enemy_roll = 0;

        if (is_dead)
        {
            enemy_pitch = 45;
            enemy_roll = 10;
        }

        gameObject.transform.rotation = Quaternion.Euler(enemy_pitch, enemy_angle, enemy_roll);

    }


    void SetPositionEnemy()
    {
        Vector2 enemy_pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        Vector2 player_pos = new Vector2(_player_object.transform.position.x, _player_object.transform.position.z);

        if (Vector3.Distance(player_pos, enemy_pos) <= 50 && is_dead == false)
        {
            return;
        }


        if (is_dead == false)
        {
            gameObject.transform.position += gameObject.transform.forward * 0.1f;
        } else
        {
            gameObject.transform.position += new Vector3(0 , -0.1f, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("当たった!");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<BulletController>() == null)
        {
            // 銃弾以外に接触しても以降の死亡処理は実行しない
            return;
        }

        if(other.gameObject.GetComponent<BulletController>()._is_player_bullet == false)
        {
            // 自分の玉では死亡しない
            return;
        }


        print("Enemy OnTriggerEnter");

        if (is_dead == true)
        {
            return;
        }

        //effect_object.transform.localScale *= 1.5f;
        StartCoroutine(delayDestroy(2f));
    }


    IEnumerator delayDestroy(float delay)
    {
        if(is_dead == true)
        {
            yield break;
        }

        is_dead = true;

        _spawn_manager._enemy_dead_num++;


        GameObject effect_object = _spawn_manager.SetExplosinEffect();


        effect_object.transform.position = gameObject.transform.position;

        yield return new WaitForSeconds(delay);

        print("delayDestroy");


        Destroy(gameObject);

    }


}
