using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class Meta : MonoBehaviour
{    
    static public Meta insMeta;
    [SerializeField] private TextMeshProUGUI txtMesh;     
    string resTxt = "Vida Meta: ";     
    int vida = 100; 

    // Start is called before the first frame update
    void Start()
    {
        insMeta = this;
        txtMesh.text = resTxt + vida.ToString();                         
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         *  Physics colliders no son necesarios entre NavMesh Agents.
         *  Para que un NevMesh empuje objetos fisicos o use triggers fisicos, debe
         *  agregarle un collider, un rigidbody que use Kinematic(importante) y que todos
         *  sus contactos sean kinematic, amabas opciones habilitadas. 
         *  https://docs.unity3d.com/es/2018.4/Manual/nav-MixingComponents.html
         *  
         *  Necesario por colision entre NavMesh Agent Enemigo y Meta. 
         */

        Destroy(collision.gameObject);

        vida -= 1; 
        txtMesh.text = resTxt + vida.ToString();
        if (vida <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }        
    }    
}
