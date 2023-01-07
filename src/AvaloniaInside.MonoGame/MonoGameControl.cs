using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AvaloniaInside.MonoGame;

public sealed class MonoGameControl : Control
{
	/// <summary>
	/// Avalonia property for <see cref="FallbackBackground" />.
	/// </summary>
	public static readonly DirectProperty<MonoGameControl, IBrush> FallbackBackgroundProperty =
		AvaloniaProperty.RegisterDirect<MonoGameControl, IBrush>(
			nameof(FallbackBackground),
			o => o.FallbackBackground,
			(o, v) => o.FallbackBackground = v);

	/// <summary>
	/// Avalonia property for <see cref="Game" />.
	/// </summary>
	public static readonly DirectProperty<MonoGameControl, Game?> GameProperty =
		AvaloniaProperty.RegisterDirect<MonoGameControl, Game?>(
			nameof(Game),
			o => o.Game,
			(o, v) => o.Game = v);

	private readonly Stopwatch _stopwatch = new();
	private readonly GameTime _gameTime = new();
	private readonly PresentationParameters _presentationParameters = new()
	{
		BackBufferWidth = 1,
		BackBufferHeight = 1,
		BackBufferFormat = SurfaceFormat.Color,
		DepthStencilFormat = DepthFormat.Depth24,
		PresentationInterval = PresentInterval.Immediate,
		IsFullScreen = false
	};

	private byte[] _bufferData = Array.Empty<byte>();
	private WriteableBitmap? _bitmap;
	private bool _isInitialized;
	private Game? _game;

	/// <summary>
	/// Initializes a new instance of the <see cref="MonoGameControl" /> class.
	/// </summary>
	public MonoGameControl()
	{
		Focusable = true;
	}

	/// <summary>
	/// Gets or sets the fallback background brush.
	/// </summary>
	public IBrush FallbackBackground { get; set; } = Brushes.Purple;

	/// <summary>
	/// Gets or sets the game.
	/// </summary>
	public Game? Game
	{
		get => _game;
		set
		{
			if (_game == value) return;
			_game = value;

			if (_isInitialized)
			{
				Initialize();
			}
		}
	}

	public override void Render(DrawingContext context)
	{
		if (Game is not { } game
		    || Game?.GraphicsDevice is not { } device
		    || _bitmap is null
		    || Bounds is { Width: < 1, Height: < 1 }
		    || !HandleDeviceReset(device))
		{
			context.DrawRectangle(FallbackBackground, null, new Rect(Bounds.Size));
			return;
		}

		// Execute a frame
		RunFrame(game);
		// Capture the executed frame into the bitmap
		CaptureFrame(device, _bitmap);
		// Flush the bitmap to context
		context.DrawImage(_bitmap, new Rect(_bitmap.Size), Bounds);
	}

	protected override Size ArrangeOverride(Size finalSize)
	{
		finalSize = base.ArrangeOverride(finalSize);
		if (finalSize != _bitmap?.Size && Game?.GraphicsDevice is { } device)
		{
			ResetDevice(device, finalSize);
		}

		return finalSize;
	}

	protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
	{
		base.OnAttachedToVisualTree(e);
		Start();
	}

	private bool HandleDeviceReset(GraphicsDevice device)
	{
		if (device.GraphicsDeviceStatus == GraphicsDeviceStatus.NotReset)
		{
			ResetDevice(device, Bounds.Size);
		}

		return device.GraphicsDeviceStatus == GraphicsDeviceStatus.Normal;
	}

	private void Initialize()
	{
		if (this.GetVisualRoot() is Window { PlatformImpl: { } } window)
			_presentationParameters.DeviceWindowHandle = window.PlatformImpl.Handle.Handle;

		if (Game is not { } game) return;

		if (game.GraphicsDevice is { } device)
			ResetDevice(device, Bounds.Size);

		RunFrame(game);
	}

	private void Start()
	{
		if (_isInitialized)
		{
			return;
		}

		Initialize();
		_stopwatch.Start();
		_isInitialized = true;
	}

	private void ResetDevice(GraphicsDevice device, Size newSize)
	{
		var newWidth = Math.Max(1, (int)Math.Ceiling(newSize.Width));
		var newHeight = Math.Max(1, (int)Math.Ceiling(newSize.Height));

		device.Viewport = new Viewport(0, 0, newWidth, newHeight);
		_presentationParameters.BackBufferWidth = newWidth;
		_presentationParameters.BackBufferHeight = newHeight;
		device.Reset(_presentationParameters);

		_bitmap?.Dispose();
		_bitmap = new WriteableBitmap(
			new PixelSize(device.Viewport.Width, device.Viewport.Height),
			new Vector(96d, 96d),
			PixelFormat.Rgba8888,
			AlphaFormat.Opaque);
	}

	private void RunFrame(Game game)
	{
		_gameTime.ElapsedGameTime = _stopwatch.Elapsed;
		_gameTime.TotalGameTime += _gameTime.ElapsedGameTime;
		_stopwatch.Restart();

		try
		{
			game.RunOneFrame();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
		finally
		{
			Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Render);
		}
	}

	private void CaptureFrame(GraphicsDevice device, WriteableBitmap bitmap)
	{
		using var bitmapLock = bitmap.Lock();
		var size = bitmapLock.RowBytes * bitmapLock.Size.Height;
		if (_bufferData.Length < size)
		{
			//_bufferData = new byte[size];
			Array.Resize(ref _bufferData, size);
		}

		device.GetBackBufferData(_bufferData, 0, size);
		Marshal.Copy(_bufferData, 0, bitmapLock.Address, size);
	}
}
