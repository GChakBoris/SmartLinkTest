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
                string information = rabbit.Recieve("locationQueue");
                if ($"{information}".Length != 0)
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
    }
    public class Resolver
    {
        public static string Resolve(string information)
        {
            try
            {
                if (information.Contains("location="))
                {
                    string informationLocation = information.Substring(information.IndexOf("location=") + 9);
                    switch (informationLocation.Substring(0, Separator.GetSeparator(informationLocation)))
                    {
                        case "Russia":
                            information = information.Replace($"location={informationLocation.Substring(0, Separator.GetSeparator(informationLocation))}", "location=1");
                            break;
                        default:
                            information = information.Replace($"location={informationLocation.Substring(0, Separator.GetSeparator(informationLocation))}", "location=2");
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