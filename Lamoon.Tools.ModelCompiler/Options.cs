using CommandLine;

namespace Lamoon.Tools.ModelCompiler; 

public class Options {
    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }
    
    [Option('f', "forced", Required = false, HelpText = "Compile model even if there is already compiled model at output")]
    public bool Forced { get; set; }
    
    [Value(0, MetaName = "input", HelpText = "Path to model defention")]
    public string? InputFile { get; set; }
    
    [Value(1, MetaName = "output", HelpText = "Path to output")]
    public string? Output { get; set; }
}