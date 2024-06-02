using Silk.NET.OpenAL;

namespace Lamoon.Audio; 

public class SoundBuffer : IDisposable {
    public uint Buffer;

    public SoundBuffer() {
        Buffer = OpenAL.Api.GenBuffer();
    }

    public unsafe void SetData(BufferFormat format, void* data, int size, int freq) {
        OpenAL.Api.BufferData(Buffer, format, data, size, freq);
    }
    
    public unsafe void SetData<TElement>(BufferFormat format, TElement[] data, int freq) where TElement : unmanaged {
        OpenAL.Api.BufferData(Buffer, format, data, freq);
    }

    public void Dispose() {
        OpenAL.Api.DeleteBuffer(Buffer);
    }
}