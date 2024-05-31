using NVorbis;
using Silk.NET.OpenAL;

namespace Lamoon.Audio; 

public class OggSoundFile : ISoundFile {

    public int Channels { get; private set; }
    public int SampleRate { get; private set; }
    public int SampleBits { get; }
    public double TotalTime { get; private set; }
    public bool Stream { get; private set;}
    public short[]? Buffer { get; private set;}

    private VorbisReader? _reader;
    private Stream _stream;
    
    public OggSoundFile(Stream stream, bool useStreaming = false) {
        _stream = new MemoryStream();
        stream.CopyTo(_stream);
        _reader = new VorbisReader(_stream);
        Channels = _reader.Channels;
        TotalTime = _reader.TotalTime.TotalSeconds;
        SampleRate = _reader.SampleRate;
        Stream = useStreaming;
        if (Stream) return;
        var floatbuf = new float[_reader.TotalSamples];
        var totalSamples = _reader.ReadSamples(floatbuf);
        Buffer = ISoundFile.FloatToOpenAL(floatbuf, totalSamples);
        _reader.Dispose();
        _reader = null;
    }

    public int GetStreamBuffer(out float[] buffer) {
        if (_reader is null) throw new AudioException("Trying to stream a non-streaming file");
        
        buffer = new float[SampleRate*Channels];
        
        return _reader.ReadSamples(buffer, 0, buffer.Length);
    }

    public void SeekTo(long location) {
        if (_reader is null) throw new AudioException("Trying to stream a non-streaming file");
        
        _reader.SeekTo(location);
    }

    public void Dispose() {
        if (_reader is not null) _reader.Dispose();
    }
}