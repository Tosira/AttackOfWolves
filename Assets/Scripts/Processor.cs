using System.Collections.Generic;

/*
    No es realemente necesario implementar la clase Processor, solo se hace para aprender.
    Idea:
    Los objetos que necesiten actualizar su estado cuando ocurra un evento,
    deben incribir al procesador la funcion que actualice su estado.
    
    Procesos:
        - Cambio escena -> Busqueda txt dinero y vidas
        - Cambio escena ->  Busqueda txt details
*/

public delegate void Process();

public class Processor
{
    private static Processor instance;
    private List<Process> sceneUpdateProcess = new List<Process>();
    
    private Processor(){}
    public static Processor Instance
    {
        get
        {
            if (instance == null) instance = new Processor();
            return instance;
        }
    }
    public void AddSceneUpdateProcess(Process process) { sceneUpdateProcess.Add(process); }
    public void RunSceneUpdateProcess()
    {
        foreach (Process p in sceneUpdateProcess) p();
    }

} // Processor