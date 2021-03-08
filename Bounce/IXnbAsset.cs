namespace Bounce
{
    // TODO: Support shared resources
    public interface IXnbAsset
    {
        string Name { get; }
        int Version => 0;

        void Serialize(ExtendedBinaryWriter bw, XnbContext context);
    }
}