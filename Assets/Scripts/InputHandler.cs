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
    private GameObject btn;
    private GameObject btn1;
    private GameObject btn2;
    private GameObject btn3;
    private GameObject btn4;
    private GameObject Torre;
    public Sprite spriteAgua;
    public Sprite spritePiedra;
    public Sprite spriteLodo;
    private Vector3 escalaOriginal;

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
            btn1.GetComponent<SpriteRenderer>().sprite = spritePiedra;
            btn2.GetComponent<SpriteRenderer>().sprite = spriteLodo;
            btn3.GetComponent<SpriteRenderer>().sprite = spriteAgua;
            btn4.GetComponent<SpriteRenderer>().color = Color.black;
            escalaOriginal = btn1.transform.localScale;


            AdjustarEscala(btn1.GetComponent<SpriteRenderer>(), btn1);
            AdjustarEscala(btn2.GetComponent<SpriteRenderer>(), btn2);
            AdjustarEscala(btn3.GetComponent<SpriteRenderer>(), btn3);
            AdjustarEscala(btn4.GetComponent<SpriteRenderer>(), btn4);

            AdjustarCollider(btn1);
            AdjustarCollider(btn2);
            AdjustarCollider(btn3);
            AdjustarCollider(btn4);

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

    void AdjustarEscala(SpriteRenderer spriteRend, GameObject btn)
    {
        if (spriteRend.sprite != null)
        {
            Vector2 spriteSize = spriteRend.sprite.bounds.size;
            float widthScale = escalaOriginal.x / spriteSize.x;
            float heightScale = escalaOriginal.y / spriteSize.y;

            float targetScale = Mathf.Min(widthScale, heightScale);

            btn.transform.localScale = new Vector3(targetScale, targetScale, btn.transform.localScale.z);
        }
    }

    void AdjustarCollider(GameObject btn)
    {
        SpriteRenderer spriteRenderer = btn.GetComponent<SpriteRenderer>();
        CircleCollider2D circleCollider = btn.GetComponent<CircleCollider2D>();
        if (circleCollider != null && spriteRenderer.sprite != null)
        {
            circleCollider.radius = spriteRenderer.sprite.bounds.size.x/2;
        }
    }
}
