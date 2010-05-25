namespace TwinEditor.Execution
{
    using System;
    
    public class ExecutionChangedEventArgs : EventArgs
    {
        public string StandardOut { get; private set; }
        
        public ExecutionChangedEventArgs(string standardOut)
        {
            StandardOut = standardOut;
        }
    }
}
