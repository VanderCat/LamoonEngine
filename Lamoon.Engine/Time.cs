namespace Lamoon.Engine; 

public static class Time {
    public static double Delta { get; internal set; }
    public static double DeltaF => (float)Delta;
}