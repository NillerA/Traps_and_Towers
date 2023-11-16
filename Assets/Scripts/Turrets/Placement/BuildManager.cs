using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    

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
