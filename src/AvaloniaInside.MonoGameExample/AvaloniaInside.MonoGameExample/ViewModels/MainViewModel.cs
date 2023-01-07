using Avalonia.Controls.Converters;
using Microsoft.Xna.Framework;

namespace AvaloniaInside.MonoGameExample.ViewModels;

public class MainViewModel : ViewModelBase
{
	public TestGame1 CurrentGame { get; set; } = new TestGame1();

	public Avalonia.Media.Color DiffuseColor
	{
		get => ToColor(CurrentGame.DiffuseColor);
		set => CurrentGame.DiffuseColor = ToVector3(value);
	}
	public Avalonia.Media.Color SpecularColor {
		get => ToColor(CurrentGame.SpecularColor);
		set => CurrentGame.SpecularColor = ToVector3(value);
	}
	public Avalonia.Media.Color AmbientLightColor {
		get => ToColor(CurrentGame.AmbientLightColor);
		set => CurrentGame.AmbientLightColor = ToVector3(value);
	}
	public Avalonia.Media.Color EmissiveColor {
		get => ToColor(CurrentGame.EmissiveColor);
		set => CurrentGame.EmissiveColor = ToVector3(value);
	}

	private static Avalonia.Media.Color ToColor(Vector3 v) =>
		new(byte.MaxValue, (byte)(v.X * byte.MaxValue), (byte)(v.Y * byte.MaxValue), (byte)(v.Z * byte.MaxValue));

	private static Vector3 ToVector3(Avalonia.Media.Color c) => new Vector3((float)c.R / (float)byte.MaxValue,
		(float)c.G / (float)byte.MaxValue, (float)c.B / (float)byte.MaxValue);
}
