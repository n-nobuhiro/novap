using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

// TapGestureRecognizer用の宣言
using DigitalRubyShared;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;


    [SerializeField]
    SpawnManager _spawn_manager = null;

    [SerializeField]
    GameObject[] _ui_canvas_list;


    [SerializeField]
    Camera _main_camera;


    [SerializeField]
    GameObject _ui_black_fade = null;


    [SerializeField]
    GameObject _ui_black_fadein = null;


    [SerializeField]
    GameObject _reticle_cross = null;

    [SerializeField]
    TextMeshProUGUI _score_text = null;

    public bool _is_boost = false;


    [SerializeField]
    Image _player_hp_ui;

    [SerializeField]
    public TextMeshProUGUI _missile_num_ui = null;

    public float _player_hp_max = 300;
    public float _current_player_hp = 300;

    bool is_move = true;

    private TapGestureRecognizer tapGesture;
    private TapGestureRecognizer doubleTapGesture;
    private TapGestureRecognizer tripleTapGesture;
    private SwipeGestureRecognizer swipeGesture;
    private PanGestureRecognizer panGesture;
    private ScaleGestureRecognizer scaleGesture;
    private RotateGestureRecognizer rotateGesture;
    private LongPressGestureRecognizer longPressGesture;

    // Start is called before the first frame update
    void Start()
    {
        _spawn_manager._player_object = _main_camera.gameObject;

        panGesture = new PanGestureRecognizer();
        panGesture.StateUpdated += PanGestureCallback;

        panGesture.MinimumNumberOfTouchesToTrack = 1;
        FingersScript.Instance.AddGesture(panGesture);

        CreateTapGesture();
        // pan, scale and rotate can all happen simultaneously
        //panGesture.AllowSimultaneousExecution(rotateGesture);


        // pan, scale and rotate can all happen simultaneously
        // panGesture.AllowSimultaneousExecution(scaleGesture);
        //panGesture.AllowSimultaneousExecution(rotateGesture);


        PinchInit();

        ResetGame();
    }



    // Update is called once per frame
    void Update()
    {


        if (is_move == true)
        {
            //SetMainCameraRotationByMouse();

            float move_add = 0.05f;

            if(_is_boost)
            {
                move_add = 0.5f;
            }

            //float current_pos = _main_camera.transform.position;
            _main_camera.transform.position += new Vector3(0, 0, move_add) ;

            SetLookAtEnemy();

            _score_text.text = "Score " + _spawn_manager._enemy_dead_num;
        }


        //PinchZoom();
    }


    void ResetGame()
    {

        _is_gameclear = false;
        _is_gameclear = false;

        _spawn_manager.ResetEnemy();

        _spawn_manager.CreateStage();

        _score_text.text = "Score " + _spawn_manager._enemy_dead_num;

        _current_player_hp = _player_hp_max;

        _player_hp_ui.fillAmount = _current_player_hp / _player_hp_max;

        _missile_num_ui.text = "6";
        _ui_canvas_list[(int)GAME_UI.CLEAR].SetActive(false);
        _ui_canvas_list[(int)GAME_UI.GAMEOVER].SetActive(false);

    }


    IEnumerator delayBlackFadeOutResetGame(float delay)
    {

        _ui_canvas_list[(int)GAME_UI.CLEAR].SetActive(false);
        _ui_canvas_list[(int)GAME_UI.GAMEOVER].SetActive(false);

        _ui_black_fade.SetActive(true);

        yield return new WaitForSeconds(delay);

        ResetGame();


        _ui_black_fadein.SetActive(true);
        _ui_black_fade.SetActive(false);



        yield return new WaitForSeconds(delay);
        _ui_black_fadein.SetActive(false);

        //_spawn_manager._player_animator.SetBool("stand_fire", false);
        //_spawn_manager._enemy_animator.SetBool("is_damage", false);
    }




    bool CheckGameEnd()
    {
        if(_ui_canvas_list[(int)GAME_UI.CLEAR].activeSelf == true
           || _ui_canvas_list[(int)GAME_UI.GAMEOVER].activeSelf == true)
        {

            return true;
        }

        return false;

    }


    void SetMainCameraRotationByMouse()
    {
        float rotationX = _main_camera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        _main_camera.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }

    private void CreateTapGesture()
    {
        tapGesture = new TapGestureRecognizer();
        tapGesture.StateUpdated += TapGestureCallback;
        tapGesture.RequireGestureRecognizerToFail = doubleTapGesture;


        tapGesture.AllowSimultaneousExecution(panGesture);

        FingersScript.Instance.AddGesture(tapGesture);
    }


    Vector2 begin_pos;

    Vector2 current_pos;

    //スワイプ操作
    void PanGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {

        if (gesture.State == GestureRecognizerState.Began)
        {
            is_move = true;
            begin_pos = new Vector2(gesture.FocusX, gesture.FocusY);
        }
        else if (gesture.State == GestureRecognizerState.Executing)
        {
            //DebugText("Panned, Location: {0}, {1}, Delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY);

            float deltaX = panGesture.DeltaX / 25.0f;
            float deltaY = panGesture.DeltaY / 25.0f;
            //Vector3 pos = Earth.transform.position;
            //pos.x += deltaX;
            //pos.y += deltaY;
            // Earth.transform.position = pos;

            current_pos = new Vector2(gesture.FocusX, gesture.FocusY);

            Vector2 pos_delta = current_pos - begin_pos;

            GameObject player_object = _spawn_manager._player_object;



            // 照準用の＋画像を非表示する.
            //_reticle_cross.SetActive(false);

            // プレイヤーを移動させる
            //player_object.transform.position += new Vector3(pos_delta.x, 0, pos_delta.y) /3000;
            //_main_camera.transform.position = _spawn_manager._player_object.transform.position + new Vector3(0, 1.5f, -3);

            _main_camera.transform.position += new Vector3(pos_delta.x, pos_delta.y, 0) / 200;
            //_main_camera.transform.rotation = Quaternion.Euler(0, 0, 0);

            float swipe_angle = _spawn_manager.GetAngle(begin_pos, current_pos);

            // player_object.transform.rotation = Quaternion.Euler(0, swipe_angle, 0);

            // 歩きモーション
            //_spawn_manager._player_animator.SetBool("is_walk", true);
        }
        else if (gesture.State == GestureRecognizerState.Ended)
        {
            //_spawn_manager._player_animator.SetBool("is_walk", false);
        }
    }



    int anim_index = 0;

    private void TapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {

            if (is_move == true)
            {
                //_main_camera.transform.rotation = Quaternion.Euler(0, 0, 0);

            }

            _spawn_manager.SetGunBullet();


            if(CheckGameEnd() == true)
            {

                StartCoroutine(delayBlackFadeOutResetGame(1.5f)); 
            }

            //_main_camera.transform.position = _spawn_manager._player_forward.transform.position;
            //_main_camera.transform.rotation = euler_angle_player;


            //StartCoroutine(delayAnimation(0.5f));

            //DebugText("Tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
            // CreateAsteroid(gesture.FocusX, gesture.FocusY);
        }
    }


    enum GAME_UI  {
            TITLE =0,
            HUD,
            CLEAR,
            GAMEOVER
        };


    public bool _is_gameclear = false;

    public bool _is_gameover = false;


    public void GameClear()
    {

        if (_ui_canvas_list[(int)GAME_UI.CLEAR].activeSelf == false)
        {
            _spawn_manager.ResetEnemy();

            _is_gameclear = true;
            _ui_canvas_list[(int)GAME_UI.CLEAR].SetActive(true);

            return;
        }
    }


    void GameOver()
    {
        
        if (_ui_canvas_list[(int)GAME_UI.GAMEOVER].activeSelf == false)
        {

            _spawn_manager.ResetEnemy();

            _is_gameover = true;
            _ui_canvas_list[(int)GAME_UI.GAMEOVER].SetActive(true);

            return;
        }
    }




    IEnumerator delayAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        //_spawn_manager._player_animator.SetBool("stand_fire", false);
        //_spawn_manager._enemy_animator.SetBool("is_damage", false);
    }


    Quaternion SetPlayerAutoAimForEnemy(GameObject target_object)
    {
        if (target_object == null)
        {
            return _spawn_manager._player_object.transform.rotation;
        }

        Vector3 temp_pos = target_object.transform.position;
        Vector2 player_pos = new Vector2(temp_pos.x, temp_pos.z);


        temp_pos = _spawn_manager._partner_object.transform.position;
        Vector2 enemy_pos = new Vector2(temp_pos.x, temp_pos.z);

        float target_angle = _spawn_manager.GetAngle(enemy_pos, player_pos);

        _spawn_manager._player_object.transform.rotation = Quaternion.Euler(0, target_angle, 0);

        return _spawn_manager._player_object.transform.rotation;
    }


    private void DebugText(string text, params object[] format)
    {
        //bottomLabel.text = string.Format(text, format);
        Debug.Log(string.Format(text, format));
    }


    void SetAttackMotion()
    {
        if (_spawn_manager._player_animator == null)
        {
            return;
        }
        _spawn_manager._player_animator.SetInteger("anim_index", anim_index);

        bool stand_fire = _spawn_manager._player_animator.GetBool("stand_fire");
        _spawn_manager._player_animator.SetBool("stand_fire", true);

        anim_index++;

        if (anim_index >= 3)
        {
            anim_index = 0;
        }


        _spawn_manager._enemy_animator.SetBool("is_damage", true);

    }


    float wid, hei, diag;//スクリーンサイズ
    float tx, ty; //変数

    float vMin = 0.01f, vMax = 4.0f; // 倍率制限
    float sDist = 0.01f, nDist = 0.0f;// 距離変数
    Vector3 initScale;//最初の大きさ
    float v = 1.0f;//現在の倍率


    Vector3 _initialize_camera_position;

    void PinchInit()
    {
        wid = Screen.width;
        hei = Screen.height;
        diag = Mathf.Sqrt(Mathf.Pow(wid, 2) + Mathf.Pow(hei, 2));
        _initialize_camera_position = _main_camera.transform.position;
    }


    void PinchZoom()
    {
        if(Input.touchCount >=2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if(t2.phase == UnityEngine.TouchPhase.Began)
            {
                sDist = Vector2.Distance(t1.position, t2.position);
            }
            else if ((t1.phase == UnityEngine.TouchPhase.Moved || t1.phase == UnityEngine.TouchPhase.Stationary) &&
                     (t2.phase == UnityEngine.TouchPhase.Moved || t2.phase == UnityEngine.TouchPhase.Stationary)
                )
            {
                nDist = Vector2.Distance(t1.position, t2.position);
                v = v + (nDist - sDist) / diag;
                sDist = nDist;
                if (v > vMax) v = vMax;
                if (v < vMin) v = vMin;

                _main_camera.transform.position = _initialize_camera_position / v;
                _main_camera.fieldOfView = v;
            }
        }
    }


    public void SetHPbarValue(float player_damage)
    {

        _current_player_hp +=  player_damage;

        _player_hp_ui.fillAmount = _current_player_hp / _player_hp_max; 
        //RectTransform rt = _player_hp_ui.gameObject.GetComponent(typeof(RectTransform)) as RectTransform;
        //rt.sizeDelta = new Vector2(rt.sizeDelta.x - player_damage, rt.sizeDelta.y);

        if(_current_player_hp <= 0) {
            GameOver();
        }

    }


    bool is_lockon = false;

    public void SetLockOn()
    {

        is_lockon = !is_lockon;

        if (is_lockon == false)
        {
            _main_camera.transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }


        if(_spawn_manager.GetTargetEnemy() == null)
        {
            return;
        }


        float lockon_x = _spawn_manager.GetTargetEnemy().transform.position.x;

        float lockon_y = _spawn_manager.GetTargetEnemy().transform.position.y;

        //_main_camera.transform.position = new Vector3(lockon_x, lockon_y, _main_camera.transform.position.z);

    }



    Quaternion SetLookAtEnemy()
    {
        if (_spawn_manager.GetTargetEnemy() == null || is_lockon == false)
        {

            _main_camera.transform.rotation = Quaternion.Euler(0, 0, 0);
            return Quaternion.Euler(0, 0, 0);
        }


        GameObject _player_object = _spawn_manager._player_object;
        GameObject enemy_object = _spawn_manager.GetTargetEnemy();


        Vector2 enemy_pos_horizon = new Vector2(enemy_object.transform.position.x, enemy_object.transform.position.z);
        Vector2 player_pos_horizon = new Vector2(_player_object.transform.position.x, _player_object.transform.position.z);

        Vector2 enemy_pos_vertical = new Vector2(enemy_object.transform.position.y, enemy_object.transform.position.x);
        Vector2 player_pos_vertical = new Vector2(_player_object.transform.position.y, _player_object.transform.position.x);



        //print("SetRotaionEnemy = " + GetAngle(enemy_pos_vertical, player_pos_vertical));
        float target_picth = GetAngle(enemy_pos_vertical, player_pos_vertical);


        float pitch_limit = 90;

        if (target_picth >= pitch_limit)
        {
           // target_picth = pitch_limit;
        }


        if (target_picth <= -pitch_limit)
        {
            //target_picth = -pitch_limit;
        }

        float enemy_angle = GetAngle(player_pos_horizon, enemy_pos_horizon);
        float enemy_pitch = target_picth;// Mathf.LerpAngle(_player_object.transform.localEulerAngles.x, target_picth, 0.01f);
        float enemy_roll = 0;


        Quaternion target_euler = Quaternion.Euler(enemy_pitch, enemy_angle, enemy_roll);

        _main_camera.transform.rotation = target_euler;

        return target_euler;

    }


    public float GetAngle(Vector2 p1, Vector2 p2)
    {
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float rad = Mathf.Atan2(dx, dy);
        return rad * Mathf.Rad2Deg;
    }
}

