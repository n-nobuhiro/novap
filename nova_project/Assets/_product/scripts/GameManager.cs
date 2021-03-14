using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{



	[SerializeField]
	Camera _main_camera = null;
	


	[SerializeField]
	SpawnManager _spawn_manager = null;

	public GameObject player = null;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
       _main_camera.transform.position =  _spawn_manager.player_object.transform.position + new Vector3(0,1.5f, -3);
    }
}
