using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AvaloniaInside.MonoGame;

 /// <summary>
    /// The methods for resizing the screen
    /// </summary>
    public enum ResizeMethod
    {
        /// <summary>
        /// Just stretch/shrink the image in x/y to fit
        /// </summary>
        Stretch,
        /// <summary>
        /// Stretch/shrink the image by the smaller scale-difference in x/.
        /// Keeps aspect-ratio.
        /// This results in black bars around the image.
        /// </summary>
        Pillow,
        /// <summary>
        /// Stretch/shrink the image by the bigger scale-difference.
        /// Keeps aspect-ratio.
        /// Image will most likeley be bigger than the window
        /// </summary>
        Fill
    }

    /// <summary>
    /// Used to render independent from the screen's size
    /// </summary>
    public class ResolutionRenderer
    {
        /// <summary>
        /// The resolution that will be rendered on
        /// </summary>
        public Point VirtualResolution { get { return _virtualResolution; } set { _virtualResolution = value; _needsUpdate = true; } }
        private Point _virtualResolution;

        /// <summary>
        /// The actual screen-size
        /// </summary>
        public Point ScreenResolution { get { return _screenResolution; } set { _screenResolution = value; _needsUpdate = true; } }
        private Point _screenResolution;

        /// <summary>
        /// Method to be used when resizing the image
        /// Default: Stretch
        /// </summary>
        public ResizeMethod Method { get { return _method; } set { _method = value; _needsUpdate = true; } }
        private ResizeMethod _method;

        private Vector2 _scale;
        private Vector2 _methodScale;

        private GraphicsDevice _device;
        private RenderTarget2D _target;
        private SpriteBatch _batch;

        private RenderTargetBinding[] _prevTargets;

        private bool _needsUpdate;

        public Color DefaultColor { get; set; } = Color.Fuchsia;

        /// <summary>
        ///
        /// </summary>
        /// <param name="virtualResolution"></param>
        public ResolutionRenderer(Point virtualResolution, GraphicsDevice device)
        {
            _device = device;

            _virtualResolution = virtualResolution;
            _screenResolution.X = _device.Viewport.Width;
            _screenResolution.Y = _device.Viewport.Height;

            _method = ResizeMethod.Stretch;

            _batch = new SpriteBatch(_device);

            _needsUpdate = false;
        }

        private void Update()
        {
            _needsUpdate = false;

            _scale = ScreenResolution.ToVector2() / VirtualResolution.ToVector2();
            _target = new RenderTarget2D(_device, _virtualResolution.X, _virtualResolution.Y);

            switch(_method)
            {
                case ResizeMethod.Stretch:
                    _methodScale = _scale;
                    break;
                case ResizeMethod.Pillow:
                    var smaller = Math.Min(_scale.X, _scale.Y);
                    _methodScale = new Vector2(smaller, smaller);
                    break;
                case ResizeMethod.Fill:
                    var bigger = Math.Max(_scale.X, _scale.Y);
                    _methodScale = new Vector2(bigger, bigger);
                    break;
                default:
                    _methodScale = _scale;
                    break;
            }
        }

        /// <summary>
        /// Call before drawing anything
        /// </summary>
        public void Begin()
        {
            if (_needsUpdate)
                Update();

            _prevTargets = _device.GetRenderTargets();
            _device.SetRenderTarget(_target);
            _device.Clear(DefaultColor);
        }

        /// <summary>
        /// Call after drawing is finished
        /// </summary>
        public void End()
        {
            var pos = ScreenResolution.ToVector2() / 2f;
            var origin = VirtualResolution.ToVector2() / 2f;

            _device.SetRenderTargets(_prevTargets);
            _device.Clear(DefaultColor);

            _batch.Begin();
            _batch.Draw(_target, pos, null, Color.White, 0, origin, _methodScale, SpriteEffects.None, 0);
            _batch.End();
        }
    }
