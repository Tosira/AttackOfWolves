using System.Collections;
using UnityEngine;
using TMPro;

public class Piggy
{
    private string piggy;
    // En que nivel esta el cerdito
    private int level;
    // En que ola esta el cerdito
    private int wave;
    private TextMeshProUGUI dialogBox;
    
    public Piggy(string name, TextMeshProUGUI dialogBox)
    {
        piggy = name;
        this.dialogBox = dialogBox;
    }

    public void ShowDialogueBox(char s)
    {
        dialogBox.text += s;
    }

    public string GetName()
    {
        return piggy;
    }
};
