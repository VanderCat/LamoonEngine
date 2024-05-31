using Silk.NET.OpenAL;

namespace Lamoon.Audio;

public unsafe class SoundContext : IDisposable {
    public Context* Handle;
    private SoundDevice _device;
    public SoundContext(SoundDevice device) {
        _device = device;
        var alc = ALContext.GetApi();
        Handle = alc.CreateContext(_device.Handle, null);
        alc.MakeContextCurrent(Handle);
    }

    public void Dispose() {
        var alc = ALContext.GetApi();
        alc.DestroyContext(Handle);
    }
}