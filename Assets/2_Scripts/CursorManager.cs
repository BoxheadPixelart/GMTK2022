using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CursorManager : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask raycastLayer;
    public bool isActive;

    public GameObject reticle;
    public bool debug;
    private Camera cam; 
    void Start()
    {
        cam = Camera.main;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!debug)
        {
            UpdateCursorPos();
            if (Input.GetMouseButton(1))
            {
                if (!isActive)
                {
                    EnableReticle();
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                DisableReticle();
            }
        } else
        {
            EnableReticle();
        }
    }
    void FixedUpdate()
    {
        if (!debug)
        {
            RaycastHit hit;
            Ray ray = new Ray(cam.transform.position,cam.transform.forward); 
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
            {
                reticle.transform.position = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
            }
            else
            {
                
            }
        }
    }
    void UpdateCursorPos()
    {
    }
    void EnableReticle()
    {
        isActive = true;
        reticle.SetActive(isActive); 
    }
    void DisableReticle()
    {
        isActive = false;
        reticle.SetActive(isActive);
    }
    void ToggleReticle()
    {
        isActive = !isActive;
        reticle.SetActive(isActive);
    }
}
