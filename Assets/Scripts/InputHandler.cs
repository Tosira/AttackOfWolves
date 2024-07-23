using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject TowersInterface;
    [SerializeField] private Canvas mainCanvas; 

    private Camera mainCamera;
    [SerializeField] private GameObject prefabButton;
    [SerializeField] private GameObject prefabTorrePiedra;
    [SerializeField] private GameObject prefabTorreBarro;
    [SerializeField] private GameObject prefabTorreAgua;
    [SerializeField] private GameObject prefabTorreRepeticionMultiple;
    private bool activeTowerInterface = false;
    private GameObject btn;
    private GameObject btn1;
    private GameObject btn2;
    private GameObject btn3;
    private GameObject btn4;
    private GameObject myTower;
    public Sprite spriteAgua;
    public Sprite spritePiedra;
    public Sprite spriteLodo;
    private Vector3 escalaOriginal;

    private void Awake()
    {
        mainCamera = Camera.main;
        btn = null;
    }

    public void  OnPointerClick(PointerEventData eventData)
    {
        if (DialogsManager.dm.isDialogueInProgress()
            || btn == null)
            return;
                
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;   // Posible llamda sin un click
        if (DialogsManager.dm.isDialogueInProgress()) return;
        if (BtnBarricada.Instance.enPausa) return;

        //  'rayHit' guarda la interseccion del rayo(que va desde un punto de la pantalla; ScreenPointToRay) con los objetos 2D del juego. 
        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit)    // Interseccion con objeto collider
        {
            DestroyButtons();
            activeTowerInterface = false; 
            return;
        }

        if(rayHit.collider.gameObject.CompareTag("Bases") && !activeTowerInterface)
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

            activeTowerInterface = true;
            myTower = rayHit.collider.gameObject;
        }
        else
        {
            if (rayHit.collider.gameObject.CompareTag("Boton"))
            {
                InstantiateTower(rayHit.collider.gameObject, btn1, prefabTorrePiedra);
                InstantiateTower(rayHit.collider.gameObject, btn2, prefabTorreBarro);                
                InstantiateTower(rayHit.collider.gameObject, btn3, prefabTorreAgua);
                InstantiateTower(rayHit.collider.gameObject, btn4, prefabTorreRepeticionMultiple);

            }
            else
            {
                DestroyButtons();
                activeTowerInterface = false;
            }
        }
    }

    private void InstantiateTower(GameObject rayHit, GameObject boton, GameObject prefabTower)
    {
        if (rayHit != boton) return;
        DialogsManager.dm.DetectedDialogueInInteraction(prefabTower);

        Torreta tower = prefabTower.GetComponent<Torreta>();
        bool gs_buy = GameState.gs.Buy(tower.GetPrecio());
                
        if (gs_buy)
        {
            DestroyButtons();
            Instantiate(prefabTower, new Vector3(myTower.transform.position.x, myTower.transform.position.y + 0.7f, myTower.transform.position.z), Quaternion.identity);
            Destroy(myTower);
            myTower = null;
        }
    }

    private void DestroyButtons()
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
