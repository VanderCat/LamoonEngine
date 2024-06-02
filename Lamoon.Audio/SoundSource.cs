using System.Numerics;
using Silk.NET.OpenAL;

namespace Lamoon.Audio; 

public unsafe class SoundSource : IDisposable {
    public uint Source;
    
    public SoundSource() {
        Source = OpenAL.Api.GenSource();
    }

    public void Pause() => OpenAL.Api.SourcePause(Source);

    public void Play() => OpenAL.Api.SourcePlay(Source);
    public void Stop() => OpenAL.Api.SourceStop(Source);
    public void Rewind() => OpenAL.Api.SourceRewind(Source);
    

    public void QueueBuffers(IEnumerable<SoundBuffer> buffers) {
        OpenAL.Api.SourceQueueBuffers(Source, buffers.Select(b => b.Buffer).ToArray());
    }

    public void UnqueueBuffers(IEnumerable<SoundBuffer> buffers) {
        OpenAL.Api.SourceUnqueueBuffers(Source, buffers.Select(b => b.Buffer).ToArray());
    }
    
    public void QueueBuffer(SoundBuffer buffer) {
        OpenAL.Api.SourceQueueBuffers(Source, new uint[]{buffer.Buffer});
    }

    public void UnqueueBuffer(SoundBuffer buffer) {
        OpenAL.Api.SourceUnqueueBuffers(Source, new uint[]{buffer.Buffer});
    }
    public void UnqueueBuffer(uint buffer) {
        OpenAL.Api.SourceUnqueueBuffers(Source, new uint[]{buffer});
    }

    public void Dispose() {
        OpenAL.Api.DeleteSource(Source);
    }

    public bool Looping {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceBoolean.Looping, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceBoolean.Looping, value);
    }
    
    public bool SourceRelative {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceBoolean.SourceRelative, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceBoolean.SourceRelative, value);
    }
    
    public float Gain {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.Gain, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.Gain, value);
    }
    
    public float Pitch {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.Pitch, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.Pitch, value);
    }
    
    public float MaxDistance {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.MaxDistance, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.MaxDistance, value);
    }
    
    public float MaxGain {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.MaxGain, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.MaxGain, value);
    }
    
    public float ReferenceDistance {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.ReferenceDistance, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.ReferenceDistance, value);
    }
    
    public float MinGain {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.MinGain, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.MinGain, value);
    }
    
    public float RolloffFactor {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.RolloffFactor, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.RolloffFactor, value);
    }
    
    public float SecOffset {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.SecOffset, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.SecOffset, value);
    }
    
    public float ConeInnerAngle {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.ConeInnerAngle, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.ConeInnerAngle, value);
    }
    
    public float ConeOuterAngle {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.ConeOuterAngle, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.ConeOuterAngle, value);
    }
    
    public float ConeOuterGain {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceFloat.ConeOuterGain, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceFloat.ConeOuterGain, value);
    }
    
    public Vector3 Direction {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceVector3.Direction, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceVector3.Direction, value);
    }
    
    public Vector3 Position {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceVector3.Position, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceVector3.Position, value);
    }
    
    public Vector3 Velocity {
        get {
            OpenAL.Api.GetSourceProperty(Source, SourceVector3.Velocity, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceVector3.Velocity, value);
    }
    
    public SourceType SourceType {
        get {
            OpenAL.Api.GetSourceProperty(Source, GetSourceInteger.SourceType, out var isLooping);
            return (SourceType)isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceInteger.SourceType, (int)value);
    }
    
    public SourceState SourceState {
        get {
            OpenAL.Api.GetSourceProperty(Source, GetSourceInteger.SourceState, out var isLooping);
            return (SourceState)isLooping;
        }
    }
    
    public int BufferHandle {
        get {
            OpenAL.Api.GetSourceProperty(Source, GetSourceInteger.Buffer, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceInteger.Buffer, value);
    }
    
    public int BuffersProcessed {
        get {
            OpenAL.Api.GetSourceProperty(Source, GetSourceInteger.BuffersProcessed, out var isLooping);
            return isLooping;
        }
    }
    
    public int BuffersQueued {
        get {
            OpenAL.Api.GetSourceProperty(Source, GetSourceInteger.BuffersQueued, out var isLooping);
            return isLooping;
        }
    }
    public int ByteOffset {
        get {
            OpenAL.Api.GetSourceProperty(Source, GetSourceInteger.ByteOffset, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceInteger.ByteOffset, value);
    }
    
    public int SampleOffset {
        get {
            OpenAL.Api.GetSourceProperty(Source, GetSourceInteger.SampleOffset, out var isLooping);
            return isLooping;
        }
        set => OpenAL.Api.SetSourceProperty(Source, SourceInteger.SampleOffset, value);
    }
}