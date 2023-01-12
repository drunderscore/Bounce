using System.Text;

namespace Bounce
{
    public class XnbContext : IDisposable
    {
        public const byte XnaGameStudio4 = 5;
        private static readonly byte[] Magic = {(byte) 'X', (byte) 'N', (byte) 'B'};
        private readonly List<IXnbAsset> _assets = new List<IXnbAsset>();
        private readonly Stream _dataStream = new MemoryStream();

        private XnbContext(IXnbAsset primary)
        {
            AddAsset(primary);
            _dataStream.Flush();
            _dataStream.Seek(0, SeekOrigin.Begin);
        }

        public void AddAsset(IXnbAsset asset)
        {
            using var bw = new BinaryWriter(_dataStream, Encoding.UTF8, true);
            if (asset == null)
                bw.Write7BitEncodedInt(0);
            else
            {
                _assets.Add(asset);
                bw.Write7BitEncodedInt(_assets.Count);
                asset.Serialize(bw, this);
            }
        }

        public static void Write(Stream stream, IXnbAsset primary, Platform target, bool hiDef)
        {
            var context = new XnbContext(primary);
            using var bw = new BinaryWriter(stream, Encoding.UTF8, true);
            bw.Write(Magic);
            bw.Write((char) target);
            bw.Write(XnaGameStudio4);
            bw.Write((byte) (hiDef ? 1 : 0));
            var sizePosition = stream.Position;
            stream.Seek(sizeof(uint), SeekOrigin.Current);
            bw.Write7BitEncodedInt(context._assets.Count);
            foreach (var asset in context._assets)
            {
                bw.Write(asset.Name);
                bw.Write(asset.Version);
            }

            // TODO: Support shared resources
            bw.Write7BitEncodedInt(0);
            context._dataStream.CopyTo(stream);
            stream.Seek(sizePosition, SeekOrigin.Begin);
            bw.Write((int) stream.Length);
        }

        public void Dispose()
        {
            _dataStream.Dispose();
        }
    }
}