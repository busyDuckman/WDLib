/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDMedia.Rendering
{
    public interface IPRNativeObject
    {
        object NativeObject { get; set;}
    }

    public interface IPRImage : IPRNativeObject
    {
        Bitmap Image {get;}
    }

    public interface IPRShader : IPRNativeObject
    {
        IList<PRShaderFormat> ShaderFormats {get;}
        object GetShaderCode(PRShaderFormat format);
    }

    public enum PRShaderFormat
    {
        None = 0,
        FragmentShader = 1
    }

    public enum PRBlendMode
    {
        Alpha = 0,
        Add = 1,
        Multiply = 2,
        None = 3
    }

    public interface I2DPerformanceRenderer
    {
        //Render mode
        /// <summary>
        /// The active blend mode for current rendering operations.
        /// </summary>
        PRBlendMode BlendMode {get; set;}

        /// <summary>
        /// The texture shader active for all IPRImage rendering.
        /// Set to null to disable.
        /// </summary>
        IPRShader Shader {get; set;}

        //Resources
        bool RegisterNativeResources(IPRImage image);
        bool RegisterNativeResources(IPRShader shader);

        bool FreeNativeResources(IPRImage image);
        bool FreeNativeResources(IPRShader shader);

        //Rendering
        void Clear(Color color);
        void DrawImage(IPRImage image, int xWorld, int yWorld);
        void DrawSubImage(IPRImage image, Rectangle region, int xWorld, int yWorld);

        /// <summary>
        /// Finish pending redering tasks
        /// </summary>
        void Flush();
        /*
        void Draw(Drawable drawable);
        void Draw(Drawable drawable, RenderStates states);
        void Draw(Vertex[] vertices, PrimitiveType type);
        void Draw(Vertex[] vertices, PrimitiveType type, RenderStates states);
        void Draw(Vertex[] vertices, uint start, uint count, PrimitiveType type);
        void Draw(Vertex[] vertices, uint start, uint count, PrimitiveType type, RenderStates states);
        View GetView();
        IntRect GetViewport(View view);
        Vector2i MapCoordsToPixel(Vector2f point);
        Vector2i MapCoordsToPixel(Vector2f point, View view);
        Vector2f MapPixelToCoords(Vector2i point);
        Vector2f MapPixelToCoords(Vector2i point, View view);
        void PopGLStates();
        void PushGLStates();
        void ResetGLStates();
        void SetView(View view);*/
    }
}
