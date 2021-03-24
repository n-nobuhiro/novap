using System.Collections;
using System.Collections.Generic;

using System.Collections.Specialized;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Sage/ItemList")]
public class ItemList : ScriptableObject
{
    public GameObject[] item_list;

    public Material[] skybox_list;
}
