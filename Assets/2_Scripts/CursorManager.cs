using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Jam;
using UnityEngine;
using UnityEngine.UI; 

public class CursorManager : MonoBehaviour
{
    private static CursorManager _manager;
    public static CursorManager manager
    {
        get
        {
            if (_manager is null)
            {
                _manager = GameObject.FindObjectOfType<CursorManager>(); 
            }
            return _manager; 
        }
    }
    // Start is called before the first frame update
    public LayerMask raycastLayer;
    public bool isActive;

    public GameObject reticle;
    public bool debug;
    private Camera cam;

    [System.Serializable]
    public class PlayerView
    {
        public GameObject viewObj;
        public ItemBase viewItem;
        public ItemData viewItemData;
        public ItemBin viewBin;
        public Vector3 viewWorldPoint; 
    }

    
    public PlayerView playerView; 
    public GameObject ItemInView; 
    void Start()
    {
        cam = Camera.main;
        EnableReticle();
    }
    
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
            Ray ray = new Ray(cam.transform.position,cam.transform.forward); 
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
            {
                GameObject obj = hit.collider.gameObject;
                ItemBase item = obj.GetComponent<ItemBase>();
                ItemBin bin = obj.GetComponent<ItemBin>();

                playerView.viewWorldPoint = hit.point;
                //
                playerView.viewBin = bin; 
                playerView.viewObj = hit.collider.gameObject;
                playerView.viewItem = item;
            }
    }

    void SetView( GameObject obj)
    {
        
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
