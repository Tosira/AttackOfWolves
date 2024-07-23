using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnBarricada : MonoBehaviour
{
    public static BtnBarricada Instance;
    public GameObject barricada;
    public bool enPausa = false;

    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Enfriamiento.instancia.circulo.fillAmount == 0)
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            if(hit && hit.collider != null && hit.collider.gameObject == gameObject)
            {
                cambiarPausa();
            }
        }
    }

    public void cambiarPausa()
    {
        enPausa = !enPausa;

        if(enPausa)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(barricada, mousePosition, Quaternion.identity);
            Time.timeScale = 0;
            Enfriamiento.instancia.circulo.fillAmount = 1;
            Enfriamiento.instancia.circuloTiempo = 0f;
        }
        else
        {
            
            Time.timeScale = 1;
        }
    }
}
