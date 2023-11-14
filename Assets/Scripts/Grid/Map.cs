using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Map",menuName = "Game/Map",order =0)]
public class Map : ScriptableObject
{

    [SerializeField]
    public Grid grid = new Grid();

}