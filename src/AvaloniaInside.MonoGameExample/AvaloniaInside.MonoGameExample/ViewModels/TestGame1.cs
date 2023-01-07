using System.Linq;
using AvaloniaInside.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AvaloniaInside.MonoGameExample.ViewModels;

public class TestGame1 : Game
{
	private Matrix _world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
	private readonly Matrix _view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
	private readonly Matrix _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);

	/// <summary>
	/// Gets the graphics device manager.
	/// </summary>
	private GraphicsDeviceManager GraphicsDeviceManager { get; }

	private Model _monkeyModel;
	private SpriteBatch _spriteBatch;
	private ResolutionRenderer _res;

	private int _lastWidth, _lastHeight;

	public Vector3 DiffuseColor { get; set; } = new (1f, 0.2f, 0.2f);
	public Vector3 SpecularColor { get; set; } = new (0, 1, 0);
	public Vector3 AmbientLightColor { get; set; } = new (0.2f, 0.2f, 0.2f);
	public Vector3 EmissiveColor { get; set; } = new (1, 0, 0);

	public TestGame1()
	{
		GraphicsDeviceManager = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
	}

	protected override void Initialize()
	{
		_lastWidth = GraphicsDevice.Viewport.Width;
		_lastHeight = GraphicsDevice.Viewport.Height;

		_res = new ResolutionRenderer(new Point(
			GraphicsDevice.Adapter.CurrentDisplayMode.Width,
			GraphicsDevice.Adapter.CurrentDisplayMode.Height), GraphicsDevice)
		{
			ScreenResolution = new Point(_lastWidth, _lastHeight),
			Method = ResizeMethod.Fill
		};

		base.Initialize();
	}

	protected override void LoadContent()
	{
		// Create a new SpriteBatch, which can be used to draw textures.
		_spriteBatch = new SpriteBatch(GraphicsDevice);


		_monkeyModel = Content.Load<Model>("monkey/monkey");

		base.LoadContent();
	}

	protected override void Update(GameTime gameTime)
	{
		_world = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds);

		if (_lastWidth != GraphicsDevice.Viewport.Width ||
		    _lastHeight != GraphicsDevice.Viewport.Height)
		{
			_lastWidth = GraphicsDevice.Viewport.Width;
			_lastHeight = GraphicsDevice.Viewport.Height;

			_res.ScreenResolution = new Point(_lastWidth, _lastHeight);
		}

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		_res.Begin();
		_spriteBatch.Begin();
		GraphicsDevice.Clear(Color.CornflowerBlue);

		DrawModel(_monkeyModel, _world, _view, _projection);

		_spriteBatch.End();
		_res.End();

		base.Draw(gameTime);
	}

	private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
	{
		foreach (var mesh in model.Meshes)
		{
			foreach (var effect in mesh.Effects.Cast<BasicEffect?>())
			{
				// effect.EnableDefaultLighting();
				effect.LightingEnabled = true; // Turn on the lighting subsystem.

				effect.DirectionalLight0.DiffuseColor = DiffuseColor; // a reddish light
				effect.DirectionalLight0.Direction = new Vector3(1, 0, 0); // coming along the x-axis
				effect.DirectionalLight0.SpecularColor = SpecularColor; // with green highlights

				effect.AmbientLightColor = AmbientLightColor; // Add some overall ambient light.
				effect.EmissiveColor = EmissiveColor; // Sets some strange emmissive lighting.  This just looks weird.

				effect.World = world;
				effect.View = view;
				effect.Projection = projection;
			}

			mesh.Draw();
		}
	}


	private void DrawModel2(Model model, Matrix world, Matrix view, Matrix projection)
	{
		foreach (var mesh in model.Meshes)
		{
			foreach (var part in mesh.MeshParts)
			{
				var effect = part.Effect;
				if (part.PrimitiveCount <= 0) continue;

				if (effect is BasicEffect basicEffect)
				{
					basicEffect.World = world;
					basicEffect.View = view;
					basicEffect.Projection = projection;

					basicEffect.Alpha = 1;
				}
				else
				{
					effect.Parameters["WorldViewProjection"]
						.SetValue(Matrix.Identity * view * projection);
				}

				GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
				GraphicsDevice.Indices = part.IndexBuffer;
				for (int i = 0; i < effect.CurrentTechnique.Passes.Count; i++)
				{
					effect.CurrentTechnique.Passes[i].Apply();
					GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset,
						part.StartIndex, part.PrimitiveCount);
				}
			}
		}
	}
}
