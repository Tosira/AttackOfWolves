using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private GameObject prefabButton;
    private int lim = 0;
    private GameObject btn1;
    private GameObject btn2;
    private GameObject btn3;
    private GameObject btn4;
    private GameObject Torre;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit)
        {
            Destroy(btn1); Destroy(btn2); Destroy(btn3); Destroy(btn4);
            lim = 0;
            return;
        }

        if(rayHit.collider.gameObject.CompareTag("Bases") && lim == 0)
        {
            lim = 1;
            btn1 = Instantiate(prefabButton, rayHit.collider.gameObject.transform.position + new Vector3(1.4f, 0, 0), Quaternion.identity);
            btn2 = Instantiate(prefabButton, rayHit.collider.gameObject.transform.position + new Vector3(-1.4f, 0, 0), Quaternion.identity);
            btn3 = Instantiate(prefabButton, rayHit.collider.gameObject.transform.position + new Vector3(0.6f, 1.3f, 0), Quaternion.identity);
            btn4 = Instantiate(prefabButton, rayHit.collider.gameObject.transform.position + new Vector3(-0.6f, 1.3f, 0), Quaternion.identity);
            btn1.GetComponent<SpriteRenderer>().color = Color.blue;
            btn2.GetComponent<SpriteRenderer>().color = Color.red;
            btn3.GetComponent<SpriteRenderer>().color = Color.magenta;
            btn4.GetComponent<SpriteRenderer>().color = Color.black;
            Torre = rayHit.collider.gameObject;
        }
        else
        {
            if (rayHit.collider.gameObject.CompareTag("Boton"))
            {
                if(rayHit.collider.gameObject == btn1)
                {
                    Torre.GetComponent<SpriteRenderer>().color = Color.blue;
                }
                if (rayHit.collider.gameObject == btn2)
                {
                    Torre.GetComponent<SpriteRenderer>().color = Color.red;
                }
                if (rayHit.collider.gameObject == btn3)
                {
                    Torre.GetComponent<SpriteRenderer>().color = Color.magenta;
                }
                if (rayHit.collider.gameObject == btn4)
                {
                    Torre.GetComponent<SpriteRenderer>().color = Color.black;
                }

            }
            else
            {
                lim = 0;
                Destroy(btn1); Destroy(btn2); Destroy(btn3); Destroy(btn4);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
