using Silk.NET.OpenAL;

namespace Lamoon.Audio; 

public static class OpenAL {
    private static AL? _api; 
    private static ALContext? _context; 
    
    public static AL Api => _api??=AL.GetApi();

    public static ALContext ContextApi => _context??=ALContext.GetApi();
}