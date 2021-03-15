using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public GameObject _player_object = null;


    public SpawnManager _spawn_manager = null;

    public bool is_dead = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        SetRotationEnemy();
        SetPositionEnemy();
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

        if(is_dead)
        {
            enemy_pitch = 45;
        }

        gameObject.transform.rotation = Quaternion.Euler(enemy_pitch, enemy_angle, 0);

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
            //return;
        }

        is_dead = true;
        print("Enemy OnTriggerEnter");

        //GameObject effect_object = _spawn_manager.SetExplosinEffect();
        //effect_object.transform.position = gameObject.transform.position;

        StartCoroutine(delayDestroy(2f));
    }


    IEnumerator delayDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        print("delayDestroy");

        Destroy(gameObject);

    }


}
