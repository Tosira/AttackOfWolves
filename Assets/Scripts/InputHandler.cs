using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{    
    private GameObject myTower;
    private Vector3 escalaOriginal;

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;   // Posible llamda sin un click
        if (DialogsManager.dm.isDialogueInProgress()) return;
        if (BtnBarricada.Instance.enPausa) return;

        //  'rayHit' guarda la interseccion del rayo(que va desde un punto de la pantalla; ScreenPointToRay) con los objetos 2D del juego. 
        var rayHit = Physics2D.GetRayIntersection(ParentInputHandler.Instance.mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit) return;

        if(rayHit.collider.gameObject.CompareTag("Bases") && !ParentInputHandler.Instance.activeInterface)
        {
            ParentInputHandler.Instance.btn = rayHit.collider.gameObject;   // objeto clicado
            ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.baseInterface);
            // ParentInputHandler.Instance._interface = Instantiate(ParentInputHandler.Instance.baseInterface, ParentInputHandler.Instance.mainCanvas.transform);
            // Vector3 worldPosition = rayHit.collider.gameObject.transform.position;
            // ParentInputHandler.Instance.btn = rayHit.collider.gameObject;
            // Vector3 screenPosition = ParentInputHandler.Instance.mainCamera.WorldToScreenPoint(worldPosition);
            // // RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentInputHandler.Instance.mainCanvas.GetComponent<RectTransform>(),
            // //                                                         screenPosition, ParentInputHandler.Instance.mainCamera, out Vector2 canvasPosition);
            // // _interface.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
            // ParentInputHandler.Instance._interface.transform.position = screenPosition;
            // ParentInputHandler.Instance._interface.GetComponent<RectTransform>().localScale = Vector3.one;

            // Debug.Log("World Position: " + worldPosition);
            // Debug.Log("Screen Position: " + screenPosition);
            // Debug.Log("Canvas Position: " + canvasPosition);


            // AdjustarEscala(btn1.GetComponent<SpriteRenderer>(), btn1);
            // AdjustarEscala(btn2.GetComponent<SpriteRenderer>(), btn2);
            // AdjustarEscala(btn3.GetComponent<SpriteRenderer>(), btn3);
            // AdjustarEscala(btn4.GetComponent<SpriteRenderer>(), btn4);

            // AdjustarCollider(btn1);
            // AdjustarCollider(btn2);
            // AdjustarCollider(btn3);
            // AdjustarCollider(btn4);

            // ParentInputHandler.Instance.activeInterface = true;
            // myTower = rayHit.collider.gameObject;
            return;
        }

        if (rayHit.collider.gameObject.CompareTag("Torre") && !ParentInputHandler.Instance.activeInterface)
        {
            ParentInputHandler.Instance.btn = rayHit.collider.gameObject;
            ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.towersInterface);
            // ParentInputHandler.Instance._interface = Instantiate(ParentInputHandler.Instance.towersInterface, ParentInputHandler.Instance.mainCanvas.transform);
            // Vector3 worldPosition = rayHit.collider.gameObject.transform.position;
            // ParentInputHandler.Instance.btn = rayHit.collider.gameObject;
            // Vector3 screenPosition = ParentInputHandler.Instance.mainCamera.WorldToScreenPoint(worldPosition);

            // ParentInputHandler.Instance._interface.transform.position = screenPosition;
            // ParentInputHandler.Instance._interface.GetComponent<RectTransform>().localScale = Vector3.one;

            // ParentInputHandler.Instance.activeInterface = true;
            return;
        }
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
