using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParentInputHandler : MonoBehaviour
{
    private static ParentInputHandler instance = null;
    
    [SerializeField] public Canvas mainCanvas;
    [HideInInspector] public Camera mainCamera;
    [SerializeField] public TextMeshProUGUI txtDetails;
    [SerializeField] public GameObject squareDetails;

    [HideInInspector] public GameObject btn;
    [HideInInspector] public GameObject _interface;
    [HideInInspector] public GameObject gm;
    [HideInInspector] public bool activeInterface;
    [SerializeField] public GameObject towersInterface;
    [SerializeField] public GameObject baseInterface;
    [SerializeField] public GameObject optionInterface;

    [SerializeField] public List<GameObject> allObjects;
    private List<GameObject> instantiatedObjcts;

    private ParentInputHandler() { }
    public void Start()
    {
        instance = this;
        mainCamera=Camera.main;
        instantiatedObjcts = new List<GameObject>();
    }

    public static ParentInputHandler Instance
    {
        get
        {
            if (instance == null) instance = new ParentInputHandler();
            return instance;
        }
    }

    public void AddInstance(GameObject gm)
    {
        instantiatedObjcts.Add(gm);
    }

    public void RemoveInstance(GameObject g)
    {
        if (instantiatedObjcts.Remove(g)) Debug.Log("Objeto hallado y removido");
    }

    public GameObject SearchObject(string nameTower)
    {
        foreach (GameObject tower in allObjects)
        {
            if (tower.name == nameTower) return tower;
        }
        return null;
    }

    public void InstantiateInterface(GameObject interface_)
    {
        _interface = Instantiate(interface_, mainCanvas.transform);
        Vector3 worldPosition = btn.transform.position;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        _interface.transform.position = screenPosition;
        _interface.GetComponent<RectTransform>().localScale = Vector3.one;

        activeInterface = true;
    }

    public void DeleteInterface()
    {
        if (!activeInterface) return;
        
        txtDetails.text = "";
        Destroy(_interface);
        activeInterface = false;
    }

    public void DeleteInterfaceAndButton()
    {
        if (!activeInterface) return;

        txtDetails.text = "";
        RemoveInstance(btn);
        Destroy(btn);
        Destroy(_interface);
        activeInterface = false;
    }
}