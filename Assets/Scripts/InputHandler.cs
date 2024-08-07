using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class InputHandler : MonoBehaviour
{
    private static InputHandler instance = null;

    private PlayerInput playerInput;
    
    [SerializeField] public Canvas mainCanvas;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public TextMeshProUGUI txtDetails;

    [HideInInspector] public GameObject clickedObject;
    [HideInInspector] public GameObject _interface;
    [HideInInspector] public GameObject objectToInstantiate;
    [HideInInspector] public bool activeInterface;
    [SerializeField] public GameObject detailsInterface;
    [SerializeField] public GameObject towersInterface;
    [SerializeField] public GameObject baseInterface;
    [SerializeField] public GameObject optionInterface;

    [SerializeField] public List<GameObject> allObjects;
    private List<GameObject> instantiatedObjcts;

    private InputHandler() { }
    
    private void Awake()
    {
        Debug.Log("IH AWAKE");

        if (instance != null) {Destroy(gameObject); return;}

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Processor
        Process process = new Process(ReConfigure);
        Processor.Instance.AddSceneUpdateProcess(process);

        mainCamera=Camera.main;
        instantiatedObjcts = new List<GameObject>();
        TextMeshProUGUI txtMeshDetails;
        if ((txtMeshDetails = detailsInterface.transform.Find("Detalles").GetComponent<TextMeshProUGUI>()) != null)
        {
            txtDetails = txtMeshDetails;
            Debug.Log("IH Configurado Interfaz Detalles");
        }
        detailsInterface.SetActive(false);

        playerInput = gameObject.AddComponent<PlayerInput>();
        if ((playerInput.actions = Resources.Load<InputActionAsset>("PlayerInput")) == null) return;
        playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
        var actionMap = playerInput.actions.FindActionMap("Gameplay");
        if (actionMap != null)
        {
            var clickAction = actionMap.FindAction("Click");
            if (clickAction != null) {clickAction.performed += OnClick; Debug.Log("IH Metodo OnClick suscrito a la accion Click"); }
            // Todas las acciones de un mapa de acciones estan desactivadas por defecto. Como vrgs iba a saberlo?
            clickAction.Enable();
        }
    }

    public static InputHandler Instance { get { return instance; } }

    public void AddInstance(GameObject gm) { instantiatedObjcts.Add(gm); }

    private void ReConfigure()
    {
        mainCamera = Camera.main;
        Canvas[] canvases;
        canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas c in canvases)
        {
            if(c.name == "CanvasMain")
            {
                // Debug.Log("Canvas Encontrado");
                mainCanvas = c;
            }
        }
        detailsInterface = GameObject.Find("InterfazDetalles");
        if (detailsInterface != null)
        {
            detailsInterface.SetActive(false);
            txtDetails = detailsInterface.transform.Find("Detalles").GetComponent<TextMeshProUGUI>();
        }

        Debug.Log("IH Carga escena");
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("IH OnCLick");
        if (!context.performed  // Posible llamda sin un click
            || DialogsManager.Instance.DialogueInProgress
            || BtnBarricada.Instance.enPausa)
            return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit) return;

        if(rayHit.collider.gameObject.CompareTag("Bases") && !activeInterface)
        {
            Instance.clickedObject = rayHit.collider.gameObject;   // objeto clicado
            Instance.InstantiateInterface(baseInterface);
            return;
        }

        if (rayHit.collider.gameObject.CompareTag("Torre") && !activeInterface)
        {
            Torreta tower = rayHit.collider.gameObject.GetComponent<Torreta>();
            if (tower == null) { Debug.Log("Componente Torreta no encontrado"); return; }
            Instance.txtDetails.text = tower.GetDetailsTower();
            Instance.clickedObject = rayHit.collider.gameObject;
            Instance.InstantiateInterface(Instance.towersInterface);
            return;
        }
    }

    public void RemoveInstance(GameObject g)
    {
        if (instantiatedObjcts.Remove(g)) Debug.Log("Objeto hallado y removido");
    }

    public GameObject SearchObject(string nameTower)
    {
        // Reemplazar por metodo Find
        foreach (GameObject tower in allObjects)
        {
            if (tower.name == nameTower) return tower;
        }
        return null;
    }

    public void InstantiateInterface(GameObject interface_)
    {
        _interface = Instantiate(interface_, mainCanvas.transform);
        Vector3 worldPosition = clickedObject.transform.position;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        _interface.transform.position = screenPosition;
        _interface.GetComponent<RectTransform>().localScale = Vector3.one;
        detailsInterface.SetActive(true);
        activeInterface = true;
    }

    public void DeleteInterface()
    {
        if (!activeInterface) return;
        
        detailsInterface.SetActive(false);
        Destroy(_interface);
        activeInterface = false;
    }

    public void DeleteInterfaceAndButton()
    {
        if (!activeInterface) return;

        detailsInterface.SetActive(false);
        RemoveInstance(clickedObject);
        Destroy(clickedObject);
        Destroy(_interface);
        objectToInstantiate = null;
        activeInterface = false;
    }
}