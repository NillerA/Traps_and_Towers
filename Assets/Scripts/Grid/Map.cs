using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map",menuName = "Game/Map",order =0)]
public class Map : ScriptableObject
{

    [SerializeField]
    public Grid grid = new Grid();

}