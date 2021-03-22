using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.ComponentModel;

public partial class SROptions
{
    [Category("Player Setting"), DisplayName("Forcus effect")]
    public bool SetForcusEffect
    {
        get { return SpawnManager.Instance._forcus_effect.activeSelf; }
        set
        {
            bool is_active = !SpawnManager.Instance._forcus_effect.activeSelf;
            SpawnManager.Instance._forcus_effect.SetActive(is_active);
          
        }
    }



    [Category("Enemy Setting"), DisplayName("Enemy Spawn")]
    public bool SetEnemySpawn
    {
        get { return SpawnManager.Instance.is_enemy_spawn; }
        set
        {
            SpawnManager.Instance.is_enemy_spawn = !SpawnManager.Instance.is_enemy_spawn;
            OnPropertyChanged(nameof(SetEnemySpawn));
        }
    }



    [Category("Enemy Setting"), DisplayName("Enemy Attack")]
    public bool SetEnemyAttack
    {
        get { return SpawnManager.Instance.is_enemy_attack; }
        set
        {
            SpawnManager.Instance.is_enemy_attack = !SpawnManager.Instance.is_enemy_attack;

        }
    }


    [Category("Stage"), DisplayName("Visible map")]
    public bool VisibleStageMap
    {
        get { return SpawnManager.Instance.stage_object.activeSelf; }
        set
        {
            bool is_active = !SpawnManager.Instance.stage_object.activeSelf;
            SpawnManager.Instance.stage_object.SetActive(is_active);

        }
    }


    [Category("Sound"), DisplayName("BGM")]
    public bool SetBGM
    {
        get { return GameManager.Instance.GetBGM().enabled; }
        set
        {
            bool is_active = !GameManager.Instance.GetBGM().enabled;
            GameManager.Instance.GetBGM().enabled = (is_active);

        }
    }


    [Category("Camera"), DisplayName("Camera near")]
    public float SetCameraNear
    {
        get { return SpawnManager.Instance._main_camera.nearClipPlane; }
        set
        {
            OnValueChanged("SetCameraNear", value);
            SpawnManager.Instance._main_camera.nearClipPlane = value;
        }
    }



    [Category("Camera"), DisplayName("Camera far")]
    public float SetCameraFar
    {
        get { return SpawnManager.Instance._main_camera.farClipPlane; }
        set
        {

            OnValueChanged("SetCameraFar", value);
            SpawnManager.Instance._main_camera.farClipPlane = value;

        }
    }



    private void OnValueChanged(string n, object newValue)
    {
        //Debug.Log("[SRDebug] {0} value changed to {1}".Fmt(n, newValue));
        OnPropertyChanged(n);
    }
}
