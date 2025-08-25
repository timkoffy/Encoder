using System.Text;
using Avalonia;
using Encoder;
class Program
{
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
    
    public static string EncodeText(string text, string key)
    {
        while (key.Length < text.Length)
        {
            key += key;
        }
        string result = DecoderOrEncoder(text, key);
        return result;
    }
    public static AppBuilder BuildAvaloniaApp() 
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
    
    private static string BinaryToString(string bin)
    {
        if (bin.Length % 8 != 0) throw new Exception("The line in the file is not a multiple of 8.");
        
        List<byte> bytes = new List<byte>();
        
        for (int i = 0; i < bin.Length; i+=8)
        {
            string byteString = bin.Substring(i, 8);
            bytes.Add(Convert.ToByte(byteString, 2));
        }
        return Encoding.UTF8.GetString(bytes.ToArray());
    }
    private static string StringToBinaryCode(string str)
    {
        string result = "";
        for (int i = 0; i < str.Length; i++)
        {
            int digit = str[i];
            result += digit.ToString("B8");
        }
        return result;
    }

    private static string DecoderOrEncoder(string text, string key)
    {
        text = StringToBinaryCode(text.TrimEnd('\r', '\n'));
        key = StringToBinaryCode(key);
        
        string result = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != key[i]) result += "1";
            else result += "0";
        }
        return BinaryToString(result);
    }
}