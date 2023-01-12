using System.Text;

namespace Bounce
{
    public class ExtendedBinaryWriter : BinaryWriter
    {
        internal ExtendedBinaryWriter(Stream stream, Encoding enc, bool leaveOpen) : base(stream, enc, leaveOpen)
        {
        }

        public new void Write7BitEncodedInt(int val)
        {
            base.Write7BitEncodedInt(val);
        }
    }
}