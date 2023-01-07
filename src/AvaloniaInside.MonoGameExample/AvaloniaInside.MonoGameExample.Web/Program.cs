﻿using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Web;
using Avalonia.ReactiveUI;
using AvaloniaInside.MonoGameExample;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
	private static void Main(string[] args) => BuildAvaloniaApp()
		.UseReactiveUI()
		.SetupBrowserApp("out");

	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>();
}
