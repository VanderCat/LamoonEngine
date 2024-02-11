namespace Lamoon.Engine; 

public static class Time {
    public static double Delta { get; internal set; }
    public static float DeltaF => (float)Delta;
    public static double CurrentTime { get; internal set; }
    public static float CurrentTimeF => (float) CurrentTime;
}