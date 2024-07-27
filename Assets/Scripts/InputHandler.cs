using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{    
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;   // Posible llamda sin un click
        if (DialogsManager.dm.isDialogueInProgress()) return;
        if (BtnBarricada.Instance.enPausa) return;

        //  'rayHit' guarda la interseccion del rayo(que va desde un punto de la pantalla; ScreenPointToRay) con los objetos 2D del juego.
        // if (ParentInputHandler.Instance.mainCamera == null)
        // {
        //     ParentInputHandler.Instance.mainCamera = Camera.main;
        //     ParentInputHandler.Instance.mainCanvas = FindObjectOfType<Canvas>();
        //     Debug.Log("Nueva configuracion camara y main");
        // }
        // if (ParentInputHandler.Instance.detailsInterface == null && ParentInputHandler.Instance.mainCanvas != null)
        // {
        //     ParentInputHandler.Instance.detailsInterface = ParentInputHandler.Instance.mainCanvas.transform.Find("InterfazDetalles").gameObject;
        //     ParentInputHandler.Instance.txtDetails = ParentInputHandler.Instance.detailsInterface.GetComponent<TextMeshProUGUI>();
        //     Debug.Log("Nueva configuracion detailsInterface");
        // }
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
            Torreta tower = rayHit.collider.gameObject.GetComponent<Torreta>();
            if (tower == null) { Debug.Log("Componente Torreta no encontrado"); return; }
            ParentInputHandler.Instance.txtDetails.text = tower.GetDetailsTower();
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
}
