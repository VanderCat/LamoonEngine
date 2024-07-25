using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using FlappyBird;
using Lamoon.Data;
using Lamoon.Data.YamlSupport;
using Lamoon.Engine;
using Lamoon.Engine.YamlExtras;
using Lamoon.Graphics;
using Serilog;
using Serilog.Events;
using Silk.NET.OpenGLES;

[assembly: SupportedOSPlatform("browser")]

namespace WebGL.Sample;

public static class Test
{
	public static Uri? BaseAddress { get; internal set; }
	[UnmanagedCallersOnly]
	public static int Frame(double time, nint userData)
	{
		BirdGame.Instance.Update(time);
		BirdGame.Instance.Draw(time);
		
		return 1;
	}

	private static int CanvasWidth { get; set; }
	private static int CanvasHeight { get; set; }
	public static void CanvasResized(int width, int height)
	{
		CanvasWidth = width;
		CanvasHeight = height;
	}

	public async static Task Main(string[] args)
	{
		Console.WriteLine($"Hello from dotnet!");

		var display = EGL.GetDisplay(IntPtr.Zero);
		if (display == IntPtr.Zero)
			throw new Exception("Display was null");

		if (!EGL.Initialize(display, out int major, out int minor))
			throw new Exception("Initialize() returned false.");

		int[] attributeList = new int[]
		{
			EGL.EGL_RED_SIZE  , 8,
			EGL.EGL_GREEN_SIZE, 8,
			EGL.EGL_BLUE_SIZE , 8,
			EGL.EGL_DEPTH_SIZE, 24,
			EGL.EGL_STENCIL_SIZE, 8,
			EGL.EGL_SURFACE_TYPE, EGL.EGL_WINDOW_BIT,
			EGL.EGL_RENDERABLE_TYPE, EGL.EGL_OPENGL_ES3_BIT,
			EGL.EGL_SAMPLES, 16, //MSAA, 16 samples
			EGL.EGL_NONE
		};

		var config = IntPtr.Zero;
		var numConfig = IntPtr.Zero;
		if (!EGL.ChooseConfig(display, attributeList, ref config, (IntPtr)1, ref numConfig))
			throw new Exception("ChoseConfig() failed");
		if (numConfig == IntPtr.Zero)
			throw new Exception("ChoseConfig() returned no configs");

		if (!EGL.BindApi(EGL.EGL_OPENGL_ES_API))
			throw new Exception("BindApi() failed");

		int[] ctxAttribs = new int[] { EGL.EGL_CONTEXT_CLIENT_VERSION, 3, EGL.EGL_NONE };
		var context = EGL.CreateContext(display, config, (IntPtr)EGL.EGL_NO_CONTEXT, ctxAttribs);
		if (context == IntPtr.Zero)
			throw new Exception("CreateContext() failed");

		// now create the surface
		var surface = EGL.CreateWindowSurface(display, config, IntPtr.Zero, IntPtr.Zero);
		if (surface == IntPtr.Zero)
			throw new Exception("CreateWindowSurface() failed");

		if (!EGL.MakeCurrent(display, surface, surface, context))
			throw new Exception("MakeCurrent() failed");

		//_ = EGL.DestroyContext(display, context);
		//_ = EGL.DestroySurface(display, surface);
		//_ = EGL.Terminate(display);

		TrampolineFuncs.ApplyWorkaroundFixingInvocations();

		var gl = Silk.NET.OpenGL.GL.GetApi(EGL.GetProcAddress);

		Interop.Initialize();
		ArgumentNullException.ThrowIfNull(BaseAddress);
		
		
		const string outputTemplate = "{Timestamp:HH:mm:ss} [{Level}] {Name}: {Message}{Exception}{NewLine}";
		Log.Logger = new LoggerConfiguration()
#if DEBUG
			.MinimumLevel.Verbose()
#else
            .MinimumLevel.Information()
#endif
			.Enrich.FromLogContext()
			.WriteTo.Console(LogEventLevel.Verbose, outputTemplate)
			.CreateLogger()
			.ForContext("Name", "LamoonEngine");
        
		Definition.TypeConverters.Add(new ObjectRefConverter());
		Definition.TypeConverters.Add(new TextureConverter());
		Definition.TypeConverters.Add(new MaterialConverter());
		Definition.TypeConverters.Add(new SpriteConverter());
		Definition.TypeConverters.Add(new OggSoundFileConverter());
		Definition.TypeConverters.Add(new FilesystemPathsResolver());

		GraphicsReferences.OpenGl = gl;

		try {
			BirdGame.Instance.Load();
		}
		catch (Exception e) {
			Console.WriteLine(e);
			return;
		}

		unsafe
		{
			Emscripten.RequestAnimationFrameLoop((delegate* unmanaged<double, nint, int>)&Frame, nint.Zero);
		}
	}
}
