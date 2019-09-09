/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox;
using WD_toolbox.AplicationFramework;

namespace WDMedia.Rendering
{
    public class SFMLPerformanceRenderer : I2DPerformanceRenderer
    {
        public PRBlendMode BlendMode { get; set; }
        public IPRShader Shader { get; set; }
        RenderTarget _target;
        public RenderTarget Target
        {
          get { return _target; }
          set { _target = value; }
        }

        RenderStates _states;
        public RenderStates States
        {
            get { return _states; }
            set { _states = value; }
        }
        //public  RenderTarget Target { get; set; }
        //public  RenderStates States { get; set; }

        public SFMLPerformanceRenderer(RenderTarget target, RenderStates states)
        {
            Target = target;
            States = states;
        }

        public bool RegisterNativeResources(IPRImage image)
        {
            if(image.NativeObject != null)
            {
                //reloading, or new renderer?
                FreeNativeResources(image);
            }
            System.Drawing.Bitmap b = image.Image;
            byte[] pixels = b.GetCopyOfBytesABGR32();
            image.NativeObject = new Sprite(new Texture(new Image((uint)b.Width, (uint)b.Height, pixels)));
                                                //new IntRect(0, 0, b.Width, b.Height));

            //image.NativeObject = new Sprite(new Texture(@"c:\temp\dump.bmp"));
            return true;
        }

        public bool RegisterNativeResources(IPRShader shader)
        {
            throw new NotImplementedException();
        }

        public bool FreeNativeResources(IPRImage image)
        {
            if (image.NativeObject != null)
            {
                if (image.NativeObject is Sprite)
                {
                    ((Sprite)image.NativeObject).Dispose();
                }
                if (image.NativeObject is IDisposable)
                {
                    ((IDisposable)image.NativeObject).Dispose();
                    WDAppLog.LogNeverSupposedToBeHere();
                }
                else
                {
                    WDAppLog.LogNeverSupposedToBeHere();
                }
                image.NativeObject = null;
            }
            return true;
        }

        public bool FreeNativeResources(IPRShader shader)
        {
            throw new NotImplementedException();
        }

        public void Clear(System.Drawing.Color color)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(IPRImage image, int xWorld, int yWorld)
        {
            if (image.NativeObject == null)
            {
                RegisterNativeResources(image);
            }

            Sprite sprite = image.NativeObject as Sprite;
            if (sprite != null)
            {
                //TODO: anything more sensible
                sprite.Position = new SFML.Window.Vector2f(xWorld, yWorld);
                sprite.Draw(_target, _states);

                //debug code
                /*CircleShape cir = new CircleShape(8, 6);
                _states.Shader = null;
                cir.Position = new Vector2f(xWorld, yWorld);
                cir.FillColor = Color.Blue;
                cir.OutlineThickness = 3;
                cir.OutlineColor = Color.Green;

                _target.Draw(cir, _states);*/
            }
        }

        public void DrawSubImage(IPRImage image, System.Drawing.Rectangle region, int xWorld, int yWorld)
        {
            if (image.NativeObject == null)
            {
                RegisterNativeResources(image);
            }

            Sprite sprite = image.NativeObject as Sprite;
            if (sprite != null)
            {
                //TODO: anything more sensible
                sprite.Position = new SFML.Window.Vector2f(xWorld, yWorld);
                sprite.TextureRect = new IntRect(region.Left, region.Top, region.Width, region.Height);
                sprite.Scale = new Vector2f(1, 1);
                //sprite.TextureRect = new IntRect(0,0,128,128);
                sprite.Draw(_target, _states);
            }
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }
    }
}
