using Microsoft.Xna.Framework;

namespace AvaloniaInside.MonoGameExample.ViewModels;

public class MainViewModel : ViewModelBase
{
	public Game CurrentGame { get; set; } = new TestGame1();
}
