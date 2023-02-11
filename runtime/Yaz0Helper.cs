using Yaz0Library;

namespace BotwRegistryToolkit.Runtime
{
    public static class Yaz0Helper
    {
        public static bool IsYaz0(string file, out byte[] data)
        {
            data = File.ReadAllBytes(file);
            if (data.AsSpan()[0..4].SequenceEqual("Yaz0"u8)) {
                data = Yaz0.Decompress(data).ToArray();
                return true;
            }

            return false;
        }
    }
}
