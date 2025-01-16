using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Items : ScriptableObject
{
    public string name;
    public Sprite icon;
    public GameObject prefab;
    [TextArea(3,5)]
    public string description;
}
