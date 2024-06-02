using Silk.NET.OpenAL;

namespace Lamoon.Audio; 

public unsafe class SoundDevice : IDisposable {

    public Device* Handle;
    public SoundDevice(string deviceName = "") {
        Handle = OpenAL.ContextApi.OpenDevice(deviceName);
        if (Handle == null)
            throw new AudioDeviceException("Could not open an Audio Device");
    }

    public void Dispose() {
        OpenAL.ContextApi.CloseDevice(Handle);
    }
}