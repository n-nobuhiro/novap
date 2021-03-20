using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
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
        
       
    }


    public AudioSource GetBGM()
    {
        return GetComponent<AudioSource>();
    }

}
