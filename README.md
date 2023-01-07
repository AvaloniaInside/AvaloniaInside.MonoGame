# AvaloniaInside.MonoGame

Integration of MonoGame for Avalonia

## Install

Install the package to your project using command below or open the [package nuget](https://www.nuget.org/packages/AvaloniaInside.MonoGame) to find other way to install.

```bash
dotnet add package AvaloniaInside.MonoGame
```

## Usage

Add the namespace to your view 
```
xmlns:monoGame="clr-namespace:AvaloniaInside.MonoGame;assembly=AvaloniaInside.MonoGame"
```

Add the game control. At the example below we use `CurrentGame` property to the game.
```xml
<monoGame:MonoGameControl Game="{Binding CurrentGame}" />
```

```csharp
public Game CurrentGame { get; set; } = new MyExampleGame();
```

![image](https://user-images.githubusercontent.com/956077/211166326-10a244a2-f265-4846-947a-6991133ce25a.png)
