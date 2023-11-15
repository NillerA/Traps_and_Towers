using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition3D : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    [SerializeField] private LayerMask layerMask;
    // Start is called before the first frame update
    private void Update()
    {
        
       Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {

            transform.position = raycastHit.point;
        }
    }
     
    

    // Update is called once per frame
    
}
