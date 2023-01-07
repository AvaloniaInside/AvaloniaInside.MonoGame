using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace AvaloniaInside.MonoGameExample.Android;

[Activity(Label = "AvaloniaInside.MonoGameExample.Android", Theme = "@style/MyTheme.NoActionBar",
	Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTop,
	ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class MainActivity : AvaloniaMainActivity
{
}
