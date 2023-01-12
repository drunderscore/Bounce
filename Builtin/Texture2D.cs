using Bounce.Builtin.Xna;

namespace Bounce.Builtin
{
    public class Texture2D : IXnbAsset
    {
        public string Name => "Microsoft.Xna.Framework.Content.Texture2DReader";

        private readonly uint[,] _pixels;
        private readonly SurfaceFormat _format;

        public Texture2D(uint[,] pixels, SurfaceFormat format)
        {
            if (format != SurfaceFormat.Color)
                throw new NotImplementedException($"SurfaceFormat {format} not implemented for Texture2D");
            _pixels = pixels;
            _format = format;
        }

        public void Serialize(ExtendedBinaryWriter bw, XnbContext context)
        {
            bw.Write((int) _format);
            var width = _pixels.GetLength(0);
            var height = _pixels.GetLength(1);
            bw.Write(width);
            bw.Write(height);
            // TODO: support mip mapping
            bw.Write(1);
            bw.Write(_pixels.Length * 4);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    // RGBA on Windows, ABGR on Xbox
                    bw.Write(_pixels[x, y]);
                }
            }
        }
    }
}