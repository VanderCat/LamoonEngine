using Silk.NET.OpenAL;

namespace Lamoon.Audio;

public unsafe class SoundContext : IDisposable {
    public Context* Handle;
    private SoundDevice _device;
    public SoundContext(SoundDevice device) {
        _device = device;
        Handle = OpenAL.ContextApi.CreateContext(_device.Handle, null);
        OpenAL.ContextApi.MakeContextCurrent(Handle);
    }

    public void Dispose() {
        OpenAL.ContextApi.DestroyContext(Handle);
    }
}