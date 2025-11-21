# AvaloniaInside.MonoGame

[![NuGet](https://img.shields.io/nuget/v/AvaloniaInside.MonoGame.svg)](https://www.nuget.org/packages/AvaloniaInside.MonoGame/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AvaloniaInside.MonoGame.svg)](https://www.nuget.org/packages/AvaloniaInside.MonoGame/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**AvaloniaInside.MonoGame** enables seamless integration of MonoGame content within Avalonia UI applications. Embed MonoGame games and graphics directly into your cross-platform Avalonia controls.

## ✨ Features

- 🎮 **Easy Integration** - Add MonoGame content to Avalonia apps with just a control
- 🖼️ **Control-Based** - Use MonoGame as a standard Avalonia XAML control
- 🎯 **Resolution Rendering** - Built-in `ResolutionRenderer` for resolution-independent rendering
- 🔄 **Cross-Platform** - Works on Desktop platforms (Windows, macOS, Linux)
- ⚡ **Simple API** - Minimal setup required to get started

## 📦 Installation

Install the package to your project using the command below or visit the [NuGet package page](https://www.nuget.org/packages/AvaloniaInside.MonoGame) for other installation methods.

```bash
dotnet add package AvaloniaInside.MonoGame
```

Or via Package Manager Console:
```powershell
Install-Package AvaloniaInside.MonoGame
```

## 📋 Requirements

- **.NET 8.0** or **.NET 10.0**
- **Avalonia** 11.3.9 or higher
- **MonoGame.Framework.DesktopGL** 3.8.4.1 or higher

## 🚀 Quick Start

### 1. Create Your MonoGame Class

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class MyExampleGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public MyExampleGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        // Your game logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Your drawing code here

        base.Draw(gameTime);
    }
}
```

### 2. Add the Namespace to Your Avalonia View

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:monoGame="clr-namespace:AvaloniaInside.MonoGame;assembly=AvaloniaInside.MonoGame"
             x:Class="YourNamespace.YourView">

    <monoGame:MonoGameControl Game="{Binding CurrentGame}" />

</UserControl>
```

### 3. Set Up Your ViewModel

```csharp
public class MainViewModel : ViewModelBase
{
    public Game CurrentGame { get; set; } = new MyExampleGame();
}
```

![MonoGame running in Avalonia](https://user-images.githubusercontent.com/956077/211166326-10a244a2-f265-4846-947a-6991133ce25a.png)

## 📚 Examples

Check out the [example project](./src/AvaloniaInside.MonoGameExample) in this repository for comprehensive usage examples including:

- Basic MonoGame integration
- Auto-playing Pong game
- Input handling patterns
- Content loading and management

## 🔧 Advanced Usage

### Resolution-Independent Rendering

Use the `ResolutionRenderer` class for resolution-independent rendering:

```csharp
public class MyGame : Game
{
    private ResolutionRenderer _resolutionRenderer;

    protected override void Initialize()
    {
        _resolutionRenderer = new ResolutionRenderer(
            this,
            new Point(1920, 1080), // Virtual resolution
            new Point(1920, 1080)  // Actual resolution
        );

        base.Initialize();
    }

    protected override void Draw(GameTime gameTime)
    {
        _resolutionRenderer.BeginDraw();

        // Your drawing code here

        base.Draw(gameTime);
    }
}
```

## ⚠️ Known Issues

1. **Mobile platforms** - Not currently supported (iOS, Android)
2. **Input handling** - Device input should be managed through native Avalonia input system rather than MonoGame's input
3. **Performance** - Performance optimizations are ongoing; may not be optimal for all scenarios yet

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues, fork the repository, and create pull requests.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Links

- 📦 [NuGet Package](https://www.nuget.org/packages/AvaloniaInside.MonoGame/)
- 📝 [GitHub Repository](https://github.com/AvaloniaInside/AvaloniaInside.MonoGame/)
- 🐛 [Report Issues](https://github.com/AvaloniaInside/AvaloniaInside.MonoGame/issues)
- 🌐 [Avalonia UI](https://avaloniaui.net/)
- 🎮 [MonoGame](https://www.monogame.net/)

---

Made with ❤️ by the AvaloniaInside team
