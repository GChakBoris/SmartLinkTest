namespace SmartLink
{
    public class Mainer
    {
        public static void Main()
        {
            // Принятие данных
            //var reciever = Factory.CreateReceiver();
            //var reciever = new Reciever();
            while (true)
            {
                var rabbit = new Rabbit();
                string information = rabbit.Recieve("timeQueue");
                //if (!CheckEmptyLine(information))
                {
                    // Обработка данных
                    //var resolver = Factory.CreateResolver(reciever.information);
                    //var resolver = new Resolver(reciever.information);
                    Task.Run(() =>
                    {
                        rabbit.Send(Resolver.Resolve(information), $"{information}Queue");
                    }
                    );
                }
            }
            // Обратная отсылка 
        }
        /*public static bool CheckEmptyLine(string information)
        {
            if (information.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/
    }
    public class Resolver
    {
        public static string Resolve(string information)
        {
            try
            {
                if (information.Contains("time="))
                {
                    string informationLocation = information.Substring(information.IndexOf("time=") + 5);
                    switch (TimeOnly.Parse(informationLocation.Substring(0, Separator.GetSeparator(informationLocation))))
                    {
                        case TimeOnly timeOnly when timeOnly > TimeOnly.Parse("12:00:00"):
                            information = information.Replace($"time={informationLocation.Substring(0, Separator.GetSeparator(informationLocation))}", "time=1");
                            break;
                        default:
                            information = information.Replace($"time={informationLocation.Substring(0, Separator.GetSeparator(informationLocation))}", "time=2");
                            break;
                    }
                }
                return information;
            }
            catch
            {
                return information;
            }
        }
    }
    public class Separator
    {

        public static int GetSeparator(string stroka)
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