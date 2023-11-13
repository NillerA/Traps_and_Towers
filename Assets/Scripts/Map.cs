using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenuAttribute(fileName = "Map",menuName = "Game/Map",order =0)]
public class Map : ScriptableObject
{

    public Grid grid = new Grid();

}