using Microsoft.VisualBasic;

namespace SmartLink
{
    public class Mainer
    {
        public Dictionary<string, Dictionary<string, string>> LinkValues = new Dictionary<string, Dictionary<string, string>>()
        {
            {"1", new Dictionary<string, string>
            {
                {"1", "Первый вариант" },
                {"2", "Второй вариант" }
            }
            },
            {"2", new Dictionary<string, string>
            {
                {"1", "Третий варинат" }
            }
            }
        };
        public string GetLink(string information)
        {
            string firstKey = information.Substring(information.IndexOf("location=")+9);
            firstKey = firstKey.Substring(0, GetSeparator(firstKey));
            Console.WriteLine(firstKey);
            string secondKey = information.Substring(information.IndexOf("time=")+5);
            secondKey = secondKey.Substring(0, GetSeparator(secondKey));
            Console.WriteLine(firstKey);
            string result = String.Empty;
            try
            {
                result = LinkValues[firstKey][secondKey];
            }
            catch
            {
                result = "Стандартная ссылка";
            }
            return result;
        }
        public int GetSeparator(string stroka)
        {
            int separator = -1;
            try
            {
                if (stroka.Contains(','))
                {
                    separator = stroka.IndexOf(',');
                }
                else if (stroka.Contains(' '))
                {
                    separator = stroka.IndexOf(' ');
                }
                else
                {
                    separator = stroka.IndexOf('}');
                }
            }
            catch
            {

            }
            return separator;
        }
    }
}
