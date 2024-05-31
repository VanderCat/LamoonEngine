using Silk.NET.OpenAL;

namespace Lamoon.Audio; 

public unsafe class SoundDevice : IDisposable {

    public Device* Handle;
    public SoundDevice(string deviceName = "") {
        var alc = ALContext.GetApi();
        var al = AL.GetApi();
        Handle = alc.OpenDevice(deviceName);
        if (Handle == null)
            throw new AudioDeviceException("Could not open an Audio Device");
    }

    public void Dispose() {
        var alc = ALContext.GetApi();
        alc.CloseDevice(Handle);
    }
}