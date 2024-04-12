using System.Collections;
using UnityEngine;
using TMPro;

public class Piggy
{
    private TextMeshProUGUI dialogBox;

    public Piggy()
    {
        
    }

    public void Initialize(TextMeshProUGUI _dialogBox)
    {
        dialogBox = _dialogBox;
    }

    public void ShowDialogueBox(string s)
    {
        dialogBox.text += s;
    }

    public void ResetDialogueBox()
    {
        dialogBox.text = "";
    }

    public bool Compare(string s)
    {
        // Salto de linea no incluido en la integracion de caracter a caracter
        return dialogBox.text + "\n" == s || dialogBox.text == s;
    }
};
