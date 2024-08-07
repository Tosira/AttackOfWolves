using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
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

    private static DialogsManager dm;

    private string dialog;
    private const int sizeBox = 160;
    private int factor;
    private int endIndicesForSubString;
    private int previousEndIndicesForSubString;
    private List<string> noDialogues;
    private int indexDialog;
    private float timeLetter = 0.05f;
    private float copyTimeLetter;
    private bool dialogueInProgress;
    private List<short> seenDialogues;
    private List<GameObject> characters;
    private GameObject currentPiggyInstance;
    private Piggy currentPiggy;

    public static DialogsManager Instance
    {
        get
        {
            if (dm == null)
            {
                GameObject gmDialogsManager = new GameObject("DialogsManager");
                dm = gmDialogsManager.AddComponent<DialogsManager>();
            }
            return dm;
        }
    }

    private void Awake()
    {
        Debug.Log("DM AWAKE");

        if (dm != null) { Destroy(gameObject); return; }
        dm = this;  // En caso de no ser creado con Instance
        DontDestroyOnLoad(gameObject);

        copyTimeLetter = timeLetter;
        indexDialog = 0;
        factor = 1;
        dialog = "";
        endIndicesForSubString = 0;
        previousEndIndicesForSubString = 0;
        noDialogues = new List<string>();

        dialogueInProgress = false;
        seenDialogues = new List<short>();
        characters = new List<GameObject>();
        currentPiggyInstance = null;
    }

    public void Initialze(List<GameObject> _characters)
    {
        if (_characters.Count == 0) return;
        characters.Clear();
        characters = _characters.ToList();
        currentPiggy = new Piggy();
    }

    public void ShowDialog()
    {
        if (!dialogueInProgress) return;
        // Inidice esta al inicio de nueva parte del dialogo. Indice ha llegado al final del dialogo.
        if (indexDialog>sizeBox*factor-1+endIndicesForSubString || indexDialog>=dialog.Length-1) return;

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

    private void SearchDialogue(string dialogOf)
    {
        if (characters.Count == 0
            || dialogueInProgress
            || noDialogues.Contains(dialogOf)
            || InputHandler.Instance.mainCanvas==null)
            return;

        /*
            seenDialogues
            Almacena el numero de la linea donde se encuentre el dialogOf(#) y personaje(*).
            No se usa para evitar procesar un dialogOf porque no contiene directamente a este.

            noDialogues
            Almacena los dialogOf que no han podido ser procesados o que no han encontrado un dialogo.
            Un dialogOf que si tiene un dialogo no sera procesado si se encuentra en seenDialogues.
        */

        TextAsset dialogsFile = Resources.Load<TextAsset>("Texto/Dialogs");
        if (dialogsFile == null)
        {
            Debug.LogError("No se pudo cargar el archivo de dialogos");
            return;
        }

        string dl = "";
        Stream StreamDialogsFile = ConvertTextAssetToStream(dialogsFile);
        try
        {
            using (StreamReader sr = new StreamReader(StreamDialogsFile))
            {
                string line;
                short lineNumber = 0, lineOfDialog = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    if (line.Contains("#")
                        && seenDialogues.Contains(lineOfDialog))
                        break;

                    if (line.Contains(dialogOf))
                    {
                        if (seenDialogues.Contains(lineNumber)) continue;
                        lineOfDialog = lineNumber;
                        seenDialogues.Add(lineNumber);
                        dl += line.Split('\n')[0] + '\n';
                        continue;
                    }

                    if (dl.Length != 0)
                    {
                        if (line.Contains("*"))
                        {
                            if (seenDialogues.Contains(lineNumber)) continue;

                            if (!SearchPiggy(line.Replace("* ", "")))
                            {
                                Debug.Log("Personaje " + line + " no encontrado");
                                dl = "";
                                break;
                            }
                            seenDialogues.Add(lineNumber);
                            currentPiggy.ResetDialogueBox();
                        }
                        dl += line.Split('\n')[0] + '\n';
                    }
                }
            }//using
        }
        catch (Exception e)
        {
            Debug.LogError("Error lectura de dialogos " + e.Message);
            dl = "";
        }
        StreamDialogsFile.Close();
        if (dl.Length != 0)
        {
            ForceClose();
            dialog=dl; dialogueInProgress=true;
            Debug.Log("Tamanno dialogo: " + dialog.Length);
            endIndicesForSubString=GetIndicesToEndSubString();
            InputHandler.Instance.txtDetails.text = "Presione ESPACIO para cerrar el dialogo o saltar el tiempo.\n" +
                                                          "Presione D para continuar con el dialogo.";
        }
        else
        {
            noDialogues.Add(dialogOf);
        }
    }

    private bool SearchPiggy(string namePiggy)
    {
        if (characters.Count <= 0) { Debug.Log("Sin personajes en los que buscar"); return false; }

        foreach (GameObject pg in characters)
        {
            if (pg.name == namePiggy)
            {
                currentPiggyInstance = Instantiate(pg, InputHandler.Instance.mainCanvas.transform);
                currentPiggyInstance.GetComponent<RectTransform>().anchoredPosition = new Vector3(-400, -200, 0);
                TextMeshProUGUI t = null;
                Debug.Log("Nombre: " + currentPiggyInstance.transform.Find("Dialogo").name);
                if ((t = currentPiggyInstance.transform.Find("Dialogo").GetComponent<TextMeshProUGUI>()) != null)
                    currentPiggy.Initialize(t);
                return true;
            }
        }
        return false;
    }

    private int GetIndicesToEndSubString()
    {
        int initialIndex = sizeBox*factor-1;
        if (initialIndex>dialog.Length-1) return 0;
        if (dialog[initialIndex] == ' '
            || dialog.Length-1 == indexDialog)
            return 0;  // No debe agregar valor para finalizar la sub cadena
        int i;
        // Debug.Log("initialIndex: " + initialIndex);
        for (i = initialIndex; dialog[i] != ' ' && dialog[i] != '\n'; ++i) {}
        // Debug.Log("endIndicesForSubString: " + (i - initialIndex) + " previousEndIndicesForSubString: " + previousEndIndicesForSubString);
        return i - initialIndex;
    }

    public void ShowRestDialog()
    {
        if (!dialogueInProgress) return;
        // true: Se ha mostrado una parte de tamanno sizeBox. Aun no se muestra todo el dialogo.
        if ((indexDialog-endIndicesForSubString)%sizeBox != 0 || indexDialog >= dialog.Length-1 && indexDialog != 0) return;
        factor = (indexDialog+1)/sizeBox + 1;
        previousEndIndicesForSubString = endIndicesForSubString;
        endIndicesForSubString = GetIndicesToEndSubString();
        currentPiggy.ResetDialogueBox();
        Debug.Log("Nuevo Factor: " + factor + " con indice: " + indexDialog);
    }

    public void Close()
    {
        // Cerrar solo cuando se haya mostrado el dialogo
        if (!dialogueInProgress || indexDialog<dialog.Length-1) return;

        indexDialog = 0;
        factor = 1;
        previousEndIndicesForSubString = 0;
        currentPiggy.ResetDialogueBox();
        if (currentPiggyInstance!=null) { Destroy(currentPiggyInstance); currentPiggyInstance=null; }
        dialogueInProgress = false;
        InputHandler.Instance.txtDetails.text = "";
        Debug.Log("Dialogo Cerrado");
    }

    public void SkipTime()
    {
        if (!dialogueInProgress) return;
        currentPiggy.ResetDialogueBox();
        int sizeSubString = sizeBox+endIndicesForSubString;
        int finalIndex = sizeBox*factor;
        int initialIndex = finalIndex-sizeBox+previousEndIndicesForSubString;
        if (finalIndex > dialog.Length-1)
        {
            finalIndex = dialog.Length-1;
            sizeSubString = finalIndex-initialIndex;
        }
        Debug.Log("Indice: " + indexDialog + " finalIndex: " + finalIndex + " initialIndex: " + initialIndex + " sizeSubString: " + sizeSubString);
        string partOfDialog = dialog.Substring(initialIndex, sizeSubString);
        currentPiggy.ShowDialogueBox(partOfDialog);
        indexDialog = finalIndex+endIndicesForSubString;
        Debug.Log("Tiempo de Dialogo Saltado");
    }

    private void ForceClose()
    {
        dialogueInProgress = false;
        currentPiggy.ResetDialogueBox();
        indexDialog = 0;
        // if (currentPiggyInstance!=null) { Destroy(currentPiggyInstance); currentPiggyInstance=null; }
        Debug.Log("Dialogo Forzado a Cerrar");
    }

    private bool TimeLetterFinished()
    {
        return (timeLetter-=Time.deltaTime) <= 0;
    }

    public void RestartDialogs()
    {
        seenDialogues.Clear();
        noDialogues.Clear();
    }

    private Stream ConvertTextAssetToStream(TextAsset textAsset)
    {
        // Convertir el contenido del TextAsset a un byte array
        // byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(textAsset.text);

        // Crear un MemoryStream a partir del byte array
        return new MemoryStream(textAsset.bytes);
    }

    public bool DialogueInProgress { get { return dialogueInProgress; } }
}// DialogManager