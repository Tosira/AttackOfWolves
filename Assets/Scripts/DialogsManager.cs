using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Unity.VisualScripting;

public class DialogsManager : MonoBehaviour
{
    /*
    Los dialogos deben ser llamados cuando...
    * se presione un objeto o el jugador interactue con un objetoi especifico
    * se actulice un nivel u ola
    * se desbloquee algun poder o funcion nueva
    Los dialogos brindan informacion relevante al jugador.
    */

    private string dialog;
    private int indexDialog;
    private float timeLetter = 0.05f;
    private float copyTimeLetter;
    private bool dialogReady;

    private void Start()
    {
        copyTimeLetter = timeLetter;
        indexDialog = 0;
        dialog = "";
        dialogReady = false;
    }

    public void ShowDialog(Piggy piggy)
    {
        // foreach (char s in dialog){
        //     boxOfDialog.text += s.ToString();
        // }
        if (indexDialog==dialog.Length-1 || !dialogReady) return;
        // Debug.Log("Valor indice: " + indexDialog);
        if (TimeLetterFinished()){
            char s = dialog[indexDialog];
            // boxOfDialog.text += s.ToString();
            piggy.ShowDialogueBox(s);
            timeLetter = copyTimeLetter;
            ++indexDialog;
        }
    }

    // Obtener dialogo para Piggy segun su estado
    public void SetCharacterDialogue(Piggy piggy)
    {
        if (dialogReady) return;

        TextAsset dialogsFile = Resources.Load<TextAsset>("Dialogs");
        if (dialogsFile == null)
        {
            Debug.LogError("No se pudo cargar el archivo de dialogos");
            return;
        }
        Stream StreamDialogsFile = ConvertTextAssetToStream(dialogsFile);
        using (StreamReader sr = new StreamReader(StreamDialogsFile))
        {
            string line;
            string dl = "";
            while ((line = sr.ReadLine()) != null)
            {
                if ((line.StartsWith("Cerdito Paja")
                    || line.StartsWith("Cerdito Madera")
                    || line.StartsWith("Cerdito Ladrillo"))
                    && !line.StartsWith(piggy.GetName())
                    && dl.Length != 0)
                    break;
                if (line.StartsWith(piggy.GetName()) || dl.Length != 0) dl += line.Split('\n')[0] + '\n';
            }
            sr.Close();
            dialog = dl;
        }
        dialogReady = true;
        Debug.Log("Dialogo: " + dialog);
    }

    public bool DialogState()
    {
        return dialogReady;
    } 

    private bool TimeLetterFinished()
    {
        return (timeLetter-=Time.deltaTime) <= 0;
    }

    private Stream ConvertTextAssetToStream(TextAsset textAsset)
    {
        // Convertir el contenido del TextAsset a un byte array
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(textAsset.text);

        // Crear un MemoryStream a partir del byte array
        return new MemoryStream(byteArray);
    }
}// DialogManager