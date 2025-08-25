using System.IO;
using System.Text;

class Program
{
    public static void Main()
    {
        Console.WriteLine("укажите полный путь до файла...");
        string PATH = Console.ReadLine();

        if (!File.Exists(PATH)) {
            Console.WriteLine("файл не найден!");
            return;
        }

        Console.WriteLine("введите ключ...");
        string KEY = StringToBinaryCode(Console.ReadLine());
        
        string TEXT = StringToBinaryCode(File.ReadAllText(PATH));
        
        while (KEY.Length < TEXT.Length)
        {
            KEY += KEY;
        }

        string RESULT = DecoderOrEncoder(TEXT, KEY);
        
        Console.Clear();
        Console.WriteLine("укажите полный путь до файла для сохранения...");
        string PATH2 = Console.ReadLine();
        
        File.WriteAllText(PATH2, RESULT);
    }

    private static string BinaryToString(string bin)
    {
        if (bin.Length % 8 != 0) throw new Exception("строка в файле не кратна 8!");
        
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
        string result = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != key[i]) result += "1";
            else result += "0";
        }
        return BinaryToString(result);
    }
}