using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PR1
{
    
    class Program
    {
     
        static void Main(string[] args)
        {
            string task = File.ReadAllText("orig.txt");
            
            string result_encryp;

            int num, j;
            int key;

            char[] message = task.ToCharArray();
            char[] alphabetLow = { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я' };

            for (int i = 0; i < message.Length; i++) //для нижнего регистра
            {
                for (j = 0; j < alphabetLow.Length; j++)
                {
                    if (message[i] == alphabetLow[j])
                    {
                        break;
                    }
                }
               
                if (j != 33)
                {
                    num = j;
                    key = num + 3;

                    if (key > 32)
                    {
                        key = key - 33;
                    }

                    message[i] = alphabetLow[key];
                   
                }
            }
            
            result_encryp = new string(message);
            File.WriteAllText("shifr.txt", result_encryp);
   
            DeleteSimbolsTask("orig.txt"); //убираем лишнии символы в исходнике
            DeleteSimbolsAnswer("shifr.txt"); //убираем лишнии символы в зашифрованном тексте
            Decrypt("orig_DeleteSim.txt", "shifr_DeleteSim.txt", "shifr.txt"); //расшифровка              
       
        }
        
        public static string DeleteSimbolsTask(string encryptText)
        {
            string text = File.ReadAllText(encryptText);
            var charsToRemove = new string[] { "@", ",", ".", ";", "'", " ", "(", ")", "!", "—", "?", "\n", "\r", "0", "1", "2", "7", "8", "5", "6", "<", ">" };
            foreach (var c in charsToRemove)
            {
                text = text.Replace(c, string.Empty);
            }
            File.WriteAllText("orig_DeleteSim.txt", text);
            return text;
        }
        public static string DeleteSimbolsAnswer(string encryptText)
        {
            string text = File.ReadAllText(encryptText);
            var charsToRemove = new string[] { "@", ",", ".", ";", "'", " ", "(", ")", "!", "—", "?", "\n", "\r", "0", "1", "2", "7", "8", "5", "6", "<", ">" };
            foreach (var c in charsToRemove)
            {
                text = text.Replace(c, string.Empty);
            }
            File.WriteAllText("shifr_DeleteSim.txt", text);
            return text;
        }
        public static string Decrypt(string orig_DeleteSim, string shifr_DeleteSim, string shifruem)
        {
            Console.WriteLine("Исходный текст\n");
            string text=File.ReadAllText(orig_DeleteSim); //считаем частоту в исходнике
            
            var groups = SwimParts(text, 1).GroupBy(str => str);
            Dictionary<string, double> orig = groups.ToDictionary(g => g.Key, g => (g.Count() * 100.0) / (double)text.Length);
            orig = orig.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var item in orig)
                Console.WriteLine(item.Key + " " + item.Value); //вывод в консоль

            Console.WriteLine("\nЗашифрованый текст методом Цезаря\n");

            text = File.ReadAllText(shifr_DeleteSim); //считаем частоту в зашифрованом
            
            groups = SwimParts(text, 1).GroupBy(str => str);
            Dictionary<string, double> shifr = groups.ToDictionary(g => g.Key, g => (g.Count() * 100.0) / (double)text.Length);
            shifr = shifr.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var item in shifr)
                Console.WriteLine(item.Key + " " + item.Value); //вывод в консоль
            //=====================================================================================================
            var dict_shifr = shifr.ToList(); // к этому моменту все словари уже упорядоченны, наиболее встречающиеся буквы в начале
            
            text = File.ReadAllText(shifruem);

            for (int i = 0; i < dict_shifr.Count; i++)
                {
                    var simbol = orig.ElementAt(i);
                text = text.Replace(dict_shifr[i].Key, simbol.Key);
                }
            
            File.WriteAllText("finale.txt", text);
            

            return text;
        }
        //orig - файл, который НАДО зашифровать
        //shifr - файл, который УЖЕ зашифрован
        //dict_shifr - список, зашифр
        
        public static IEnumerable<string> SwimParts(string source, int length)
        {
            for (int i = length; i <= source.Length; i++)
                yield return source.Substring(i - length, length);
        }
    }
}
/*=======================
 *          string stroka = Console.ReadLine();
 *          IEnumerable<int> strokaInt = stroka.Select(ch => Alphabet[ch]);
            List<int> strokaList = strokaInt.ToList();
            Console.WriteLine(string.Join ("\r\n", strokaInt));
=======================
           string text = "аааббввввггг";
           var freqChars = text.GroupBy(x => x).Select(x => new { Char = x.Key, Count = x.Count() });
           Console.WriteLine(string.Join("\n", freqChars));*/
