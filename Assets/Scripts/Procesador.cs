using System.Collections.Generic;

public delegate void Process();

public class Processor
{
    private Queue<Process> processQueue = new Queue<Process>();
    public void AddProcess(Process process) { processQueue.Enqueue(process); }
    public void RunProcesses()
    {
        while (processQueue.Count > 0) { processQueue.Dequeue()(); }
    }
}