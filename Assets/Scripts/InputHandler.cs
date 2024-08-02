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
        if (DialogsManager.Instance.DialogueInProgress) return;
        if (BtnBarricada.Instance.enPausa) return;

        var rayHit = Physics2D.GetRayIntersection(ParentInputHandler.Instance.mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit) return;

        if(rayHit.collider.gameObject.CompareTag("Bases") && !ParentInputHandler.Instance.activeInterface)
        {
            ParentInputHandler.Instance.clickedObject = rayHit.collider.gameObject;   // objeto clicado
            ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.baseInterface);
            return;
        }

        if (rayHit.collider.gameObject.CompareTag("Torre") && !ParentInputHandler.Instance.activeInterface)
        {
            Torreta tower = rayHit.collider.gameObject.GetComponent<Torreta>();
            if (tower == null) { Debug.Log("Componente Torreta no encontrado"); return; }
            ParentInputHandler.Instance.txtDetails.text = tower.GetDetailsTower();
            ParentInputHandler.Instance.clickedObject = rayHit.collider.gameObject;
            ParentInputHandler.Instance.InstantiateInterface(ParentInputHandler.Instance.towersInterface);
            return;
        }
    }
}
