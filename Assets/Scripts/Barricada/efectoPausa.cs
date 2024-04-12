using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class efectoPausa : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BtnBarricada.Instance.enPausa)
        {
            spriteRenderer.color = new Color(144, 255, 255, 0.1f);
        }
        else
        {
            spriteRenderer.color = new Color(0, 0, 0, 0);
        }
        
    }
}
