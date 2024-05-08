namespace Lamoon.Input;

public class InputAction {
    public struct CallbackContext {
        public readonly InputAction Action;
        public readonly InputControl Control;

        public readonly double Duration;
        
        public readonly IInputInteraction Interaction;
        public readonly InputActionPhase Phase => Action.Phase;
        
        public readonly bool Started;
        public readonly bool Performed;
        public readonly bool Canceled;
    }
    
    public delegate void InputCallbak(CallbackContext ctx);

    public event InputCallbak Started;
    public event InputCallbak Performed;
    public event InputCallbak Canceled;

    public InputActionPhase Phase;
    
    public bool PerfomedThisFrame => throw new NotImplementedException();
    
    public bool Pressed => throw new NotImplementedException();
    public bool PressedThisFrame => throw new NotImplementedException();
    public bool ReleasedThisFrame => throw new NotImplementedException();
    
    public InputAction(string name) {}

    public void AddBinding(string binding) {
        throw new NotImplementedException();
    }
    
    public void AddCompositeBinding(string binding) {
        throw new NotImplementedException();
    }

    public T ReadValue<T>() {
        throw new NotImplementedException();
    }
}