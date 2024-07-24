using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Unity.VisualScripting;
using System.Linq;
using System;

public class DialogsManager : MonoBehaviour
{
    /*
    Los dialogos deben ser llamados cuando...
    * se presione un objeto o el jugador interactue con un objeto especifico
    * se actulice un nivel u ola, solo cuando se quiera
    * se desbloquee algun poder o funcion nueva
    Los dialogos brindan informacion relevante al jugador.
    */

    public static DialogsManager dm;

    private string dialog;
    private string previousDialog;
    private int indexDialog;
    private float timeLetter = 0.05f;
    private float copyTimeLetter;
    private bool dialogueInProgress;
    private List<short> seenDialogues;
    private List<GameObject> characters;
    private GameObject currentPiggyInstance;
    private Piggy currentPiggy;

    private void Awake()
    {
        dm = this;
        copyTimeLetter = timeLetter;
        indexDialog = 0;
        dialog = "";
        previousDialog = "";
        
        dialogueInProgress = false;
        seenDialogues = new List<short>();
        characters = new List<GameObject>();
        currentPiggyInstance = null;
    }

    public void Initialze(List<GameObject> _characters, TextMeshProUGUI _txtMeshDialog)
    {
        if (_characters.Count == 0) return;
        characters.Clear();
        characters = _characters.ToList();
        currentPiggy = new Piggy(_txtMeshDialog);
    }

    public void ShowDialog()
    {        
        if (dialog.Length-1==indexDialog || !dialogueInProgress) return;

        if (TimeLetterFinished())
        {
            string s = dialog[indexDialog].ToString();
            currentPiggy.ShowDialogueBox(s);
            timeLetter = copyTimeLetter;
            ++indexDialog;
        }
    }

    public void SetCharacterDialogue(string dialogOf)
    {
        SearchDialogue(dialogOf);
    }

    public void SetCharacterDialogue(int levelIndex, int waveIndex)
    {
        string dialogOf = "Level " + levelIndex.ToString() + " Wave " + waveIndex.ToString();
        SearchDialogue(dialogOf);
    }

    // Obtener dialogo para Piggy segun su estado
    private void SearchDialogue(string dialogOf)
    {
        if (characters.Count == 0
            || dialogueInProgress
            || (!dialogueInProgress && previousDialog == dialogOf))
            return;
        
        TextAsset dialogsFile = Resources.Load<TextAsset>("Dialogs");
        if (dialogsFile == null)
        {
            Debug.LogError("No se pudo cargar el archivo de dialogos");
            return;
        }

        ForceClose();   // No es necesaria luego de haber llamaddo a Close()
        string dl = "";
        previousDialog = dialogOf;
        Stream StreamDialogsFile = ConvertTextAssetToStream(dialogsFile);
        try
        {
            using (StreamReader sr = new StreamReader(StreamDialogsFile))
            {
                string line;
                short lineNumber = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("#")
                        && !line.Contains(dialogOf)
                        && dl.Length != 0)
                        break;
                    if (line.Contains(dialogOf))
                    {
                        if (seenDialogues.Contains(lineNumber))
                        {
                            break;
                        }
                        seenDialogues.Add(lineNumber);
                        dl += line.Split('\n')[0] + '\n';
                        continue;
                    }
                    if (dl.Length != 0)
                    {
                        if (line.Contains("*"))
                        {
                            if (!SearchPiggy(line.Replace("* ", "")))
                            {
                                Debug.Log("Personaje " + line + " no encontrado");
                                break;
                            }
                            currentPiggy.ResetDialogueBox();
                        }
                        dl += line.Split('\n')[0] + '\n';
                    }
                    lineNumber++;
                }
            }//using
        }
        catch (Exception e)
        {
            Debug.LogError("Error lectura de dialogos " + e.Message);
            dl = "";
        }
        StreamDialogsFile.Close();
        if (dl.Length != 0) { dialog=dl; dialogueInProgress=true; }
    }

    private bool SearchPiggy(string namePiggy)
    {
        if (characters.Count <= 0) { Debug.Log("Sin personajes en los que buscar"); return false; }
        
        foreach (GameObject pg in characters)
        {
            if (pg.name == namePiggy)
            {
                currentPiggyInstance = Instantiate(pg, new Vector3(-9.8f,-4f,0), Quaternion.identity);
                return true;
            }
        }
        return false;
    } 

    public void DetectedDialogueInInteraction(GameObject gm)
    {
        string gmName = gm.GetComponent<Torreta>().GetType().Name;
        Debug.Log("Nombre del objeto accionador de dialogo: " + gmName);
        // Peticion forzada porque es el jugador quien activa otro dialogo y cancela el que esta en progreso.
        SetCharacterDialogue(gmName);
    }

    public void Close()
    {
        // Cerrar solo cuando se haya mostrado el dialogo
        if (!dialogueInProgress || !currentPiggy.Compare(dialog)) return;
        indexDialog = 0;
        currentPiggy.ResetDialogueBox();
        if (currentPiggyInstance!=null) { Destroy(currentPiggyInstance); currentPiggyInstance=null; }
        dialogueInProgress = false;
        Debug.Log("Dialogo Cerrado");
    }

    public void SkipTime()
    {
        if (!dialogueInProgress) return;
        currentPiggy.ResetDialogueBox();
        currentPiggy.ShowDialogueBox(dialog);
        indexDialog = dialog.Length-1;
        Debug.Log("Tiempo de Dialogo Saltado");
    }

    private void ForceClose()
    {
        dialogueInProgress = false;
        currentPiggy.ResetDialogueBox();
        indexDialog = 0;
        if (currentPiggyInstance!=null) { Destroy(currentPiggyInstance); currentPiggyInstance=null; }
        Debug.Log("Dialogo Forzado a Cerrar");
    }

    public bool isDialogueInProgress()
    {
        return dialogueInProgress;
    } 

    private bool TimeLetterFinished()
    {
        return (timeLetter-=Time.deltaTime) <= 0;
    }

    private Stream ConvertTextAssetToStream(TextAsset textAsset)
    {
        // Convertir el contenido del TextAsset a un byte array
        // byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(textAsset.text);

        // Crear un MemoryStream a partir del byte array
        return new MemoryStream(textAsset.bytes);
    }
}// DialogManager