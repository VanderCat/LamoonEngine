namespace Lamoon.Audio; 

public interface ISoundFile : IDisposable {
    public int Channels { get; }
    public int SampleRate { get; }
    public int SampleBits { get; }
    public double TotalTime { get; }
    public short[] Buffer { get; }
    
    public bool Stream { get; }

    public int GetStreamBuffer(out float[] buffer);
    public void SeekTo(long location);

    public static short[] FloatToOpenAL(float[] floatBuff, int sampleCount) {
        var buffer = new short[sampleCount];
        for (var i = 0; i < sampleCount; ++i)
        {
            buffer[i] = (short)(32768.0f * floatBuff[i]);
        }
        return buffer;
    }
}