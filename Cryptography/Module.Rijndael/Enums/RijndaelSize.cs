namespace Module.Rijndael.Enums;

public class RijndaelSize
{
    public static RijndaelSize S128 { get; } = new(16, 4);
    public static RijndaelSize S192 { get; } = new(24, 6);
    public static RijndaelSize S256 { get; } = new(32, 8);

    public int ByteCount { get; }
    public int WordCount { get; }

    private RijndaelSize(int byteCount, int wordCount)
    {
        ByteCount = byteCount;
        WordCount = wordCount;
    }
}