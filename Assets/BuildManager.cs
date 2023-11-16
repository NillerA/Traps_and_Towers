using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //public static BuildManager instance;

    //void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Debug.LogError("More than one BuildManager instance!");
    //    }
    //    instance = this; 
    //}

    public GameObject rangedTurretPrefab;

    private void Start()
    {
        turretToBuild = rangedTurretPrefab;
    }

    private GameObject turretToBuild;

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }


}
