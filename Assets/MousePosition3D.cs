using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition3D : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    // Start is called before the first frame update
    private void Update()
    {
       Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    
}
