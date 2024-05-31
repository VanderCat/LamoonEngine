using System.Numerics;
using Silk.NET.OpenAL;

namespace Lamoon.Audio; 

public unsafe class SoundSource : IDisposable {
    private static AL al = AL.GetApi();
    public uint Source;
    
    public SoundSource() {
        Source = al.GenSource();
    }

    public void Pause() => al.SourcePause(Source);

    public void Play() => al.SourcePlay(Source);
    public void Stop() => al.SourceStop(Source);
    public void Rewind() => al.SourceRewind(Source);
    

    public void QueueBuffers(IEnumerable<SoundBuffer> buffers) {
        al.SourceQueueBuffers(Source, buffers.Select(b => b.Buffer).ToArray());
    }

    public void UnqueueBuffers(IEnumerable<SoundBuffer> buffers) {
        al.SourceUnqueueBuffers(Source, buffers.Select(b => b.Buffer).ToArray());
    }
    
    public void QueueBuffer(SoundBuffer buffer) {
        al.SourceQueueBuffers(Source, new uint[]{buffer.Buffer});
    }

    public void UnqueueBuffer(SoundBuffer buffer) {
        al.SourceUnqueueBuffers(Source, new uint[]{buffer.Buffer});
    }
    public void UnqueueBuffer(uint buffer) {
        al.SourceUnqueueBuffers(Source, new uint[]{buffer});
    }

    public void Dispose() {
        al.DeleteSource(Source);
    }

    public bool Looping {
        get {
            al.GetSourceProperty(Source, SourceBoolean.Looping, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceBoolean.Looping, value);
    }
    
    public bool SourceRelative {
        get {
            al.GetSourceProperty(Source, SourceBoolean.SourceRelative, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceBoolean.SourceRelative, value);
    }
    
    public float Gain {
        get {
            al.GetSourceProperty(Source, SourceFloat.Gain, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.Gain, value);
    }
    
    public float Pitch {
        get {
            al.GetSourceProperty(Source, SourceFloat.Pitch, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.Pitch, value);
    }
    
    public float MaxDistance {
        get {
            al.GetSourceProperty(Source, SourceFloat.MaxDistance, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.MaxDistance, value);
    }
    
    public float MaxGain {
        get {
            al.GetSourceProperty(Source, SourceFloat.MaxGain, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.MaxGain, value);
    }
    
    public float ReferenceDistance {
        get {
            al.GetSourceProperty(Source, SourceFloat.ReferenceDistance, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.ReferenceDistance, value);
    }
    
    public float MinGain {
        get {
            al.GetSourceProperty(Source, SourceFloat.MinGain, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.MinGain, value);
    }
    
    public float RolloffFactor {
        get {
            al.GetSourceProperty(Source, SourceFloat.RolloffFactor, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.RolloffFactor, value);
    }
    
    public float SecOffset {
        get {
            al.GetSourceProperty(Source, SourceFloat.SecOffset, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.SecOffset, value);
    }
    
    public float ConeInnerAngle {
        get {
            al.GetSourceProperty(Source, SourceFloat.ConeInnerAngle, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.ConeInnerAngle, value);
    }
    
    public float ConeOuterAngle {
        get {
            al.GetSourceProperty(Source, SourceFloat.ConeOuterAngle, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.ConeOuterAngle, value);
    }
    
    public float ConeOuterGain {
        get {
            al.GetSourceProperty(Source, SourceFloat.ConeOuterGain, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceFloat.ConeOuterGain, value);
    }
    
    public Vector3 Direction {
        get {
            al.GetSourceProperty(Source, SourceVector3.Direction, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceVector3.Direction, value);
    }
    
    public Vector3 Position {
        get {
            al.GetSourceProperty(Source, SourceVector3.Position, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceVector3.Position, value);
    }
    
    public Vector3 Velocity {
        get {
            al.GetSourceProperty(Source, SourceVector3.Velocity, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceVector3.Velocity, value);
    }
    
    public SourceType SourceType {
        get {
            al.GetSourceProperty(Source, GetSourceInteger.SourceType, out var isLooping);
            return (SourceType)isLooping;
        }
        set => al.SetSourceProperty(Source, SourceInteger.SourceType, (int)value);
    }
    
    public SourceState SourceState {
        get {
            al.GetSourceProperty(Source, GetSourceInteger.SourceState, out var isLooping);
            return (SourceState)isLooping;
        }
    }
    
    public int BufferHandle {
        get {
            al.GetSourceProperty(Source, GetSourceInteger.Buffer, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceInteger.Buffer, value);
    }
    
    public int BuffersProcessed {
        get {
            al.GetSourceProperty(Source, GetSourceInteger.BuffersProcessed, out var isLooping);
            return isLooping;
        }
    }
    
    public int BuffersQueued {
        get {
            al.GetSourceProperty(Source, GetSourceInteger.BuffersQueued, out var isLooping);
            return isLooping;
        }
    }
    public int ByteOffset {
        get {
            al.GetSourceProperty(Source, GetSourceInteger.ByteOffset, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceInteger.ByteOffset, value);
    }
    
    public int SampleOffset {
        get {
            al.GetSourceProperty(Source, GetSourceInteger.SampleOffset, out var isLooping);
            return isLooping;
        }
        set => al.SetSourceProperty(Source, SourceInteger.SampleOffset, value);
    }
}