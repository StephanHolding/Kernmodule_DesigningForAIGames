using UnityEngine;

public class BT_Log : BTBaseNode
{

    private readonly string messageToLog;
    
    public BT_Log(string _logMessage)
    {
        messageToLog = _logMessage;
    }
    
    public override TaskStatus Run()
    {
        Debug.Log(messageToLog);
        return TaskStatus.Success;
    }

    public override void Reset()
    {
      
    }
}
