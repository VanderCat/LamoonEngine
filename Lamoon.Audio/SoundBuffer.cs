using Silk.NET.OpenAL;

namespace Lamoon.Audio; 

public class SoundBuffer : IDisposable {
    public uint Buffer;

    public SoundBuffer() {
        var al = AL.GetApi();
        Buffer = al.GenBuffer();
    }

    public unsafe void SetData(BufferFormat format, void* data, int size, int freq) {
        var al = AL.GetApi();
        al.BufferData(Buffer, format, data, size, freq);
    }
    
    public unsafe void SetData<TElement>(BufferFormat format, TElement[] data, int freq) where TElement : unmanaged {
        var al = AL.GetApi();
        al.BufferData(Buffer, format, data, freq);
    }

    public void Dispose() {
        var al = AL.GetApi();
        al.DeleteBuffer(Buffer);
    }
}