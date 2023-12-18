namespace FTS.UI
{
    /// Attach to main controller for the UI component.
    /// <typeparam name="T">Listen to T type UI component</typeparam>
    internal interface IButtonHandler<in T>
    {
        // Interact on Mouse/Touch Enter
        public void Enter(T t);
        
        // Interact on Mouse/Touch Exit 
        public void Exit(T t);
        
        // Interact on Mouse/Touch Press
        public void Press(T t);
    }
}