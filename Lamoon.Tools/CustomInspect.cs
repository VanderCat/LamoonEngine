namespace Lamoon.Tools; 

public class CustomInspectorAttribute : Attribute {
    public Type InspectType;

    public CustomInspectorAttribute(Type inspectType) {
        InspectType = inspectType;
    }
}