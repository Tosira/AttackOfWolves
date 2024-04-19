using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private GameObject prefabButton;
    [SerializeField] private GameObject prefabTorrePiedra;
    [SerializeField] private GameObject prefabTorreBarro;
    [SerializeField] private GameObject prefabTorreAgua;
    private bool interfazTorretaActiva = false;
    private GameObject btn1;
    private GameObject btn2;
    private GameObject btn3;
    private GameObject btn4;
    private GameObject Torre;    

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;   // Posible llamda sin un click

        //  'rayHit' guarda la interseccion del rayo(que va desde un punto de la pantalla; ScreenPointToRay) con los objetos 2D del juego. 
        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit)    // Interseccion con objeto collider
        {
            destruirBotones();
            interfazTorretaActiva = false; 
            return;
        }

        if(rayHit.collider.gameObject.CompareTag("Bases") && !interfazTorretaActiva)
        {
            btn1 = Instantiate(prefabButton, rayHit.collider.gameObject.transform.position + new Vector3(1.4f, 0, 0), Quaternion.identity);
            btn2 = Instantiate(prefabButton, rayHit.collider.gameObject.transform.position + new Vector3(-1.4f, 0, 0), Quaternion.identity);
            btn3 = Instantiate(prefabButton, rayHit.collider.gameObject.transform.position + new Vector3(0.6f, 1.3f, 0), Quaternion.identity);
            btn4 = Instantiate(prefabButton, rayHit.collider.gameObject.transform.position + new Vector3(-0.6f, 1.3f, 0), Quaternion.identity);
            btn1.GetComponent<SpriteRenderer>().color = Color.blue;
            btn2.GetComponent<SpriteRenderer>().color = Color.red;
            btn3.GetComponent<SpriteRenderer>().color = Color.magenta;
            btn4.GetComponent<SpriteRenderer>().color = Color.black;
            interfazTorretaActiva = true;
            Torre = rayHit.collider.gameObject;
        }
        else
        {
            if (rayHit.collider.gameObject.CompareTag("Boton"))
            {
                //  Refactorizar lineas de esta seccion en una funcion
                generarTorreta(rayHit.collider.gameObject, btn1, prefabTorrePiedra);
                generarTorreta(rayHit.collider.gameObject, btn2, prefabTorreBarro);
                //  Cambiar en un futuro
                generarTorreta(rayHit.collider.gameObject, btn3, prefabTorreAgua);
                generarTorreta(rayHit.collider.gameObject, btn4, prefabTorreBarro);

            }
            else
            {
                destruirBotones(); 
                interfazTorretaActiva = false;
            }
        }
    }

    private void generarTorreta(GameObject rayHit, GameObject boton, GameObject prefabTorre)
    {
        if(rayHit == boton)
        {
            destruirBotones();
            Instantiate(prefabTorre, Torre.transform.position, Quaternion.identity);
            Destroy(Torre);
            Torre = null;
        }
    }

    private void destruirBotones()
    {
        Destroy(btn1); Destroy(btn2); Destroy(btn3); Destroy(btn4);
    }
}
