using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricada : MonoBehaviour
{
    private bool colocar = true;
    private float rotacion = 100f;
    void Update()
    {
        if( colocar)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                transform.Rotate(Vector3.forward, scroll * rotacion);
            }
        }
        
    }

    private void OnMouseDown()
    {
        colocar = false;
        BtnBarricada.Instance.cambiarPausa();
    }
}
