namespace Villa.Logging
{
    public class Logging : ILogging
    {
        public void Log(string Message, string Type)
        {
            if (Type == "error")
            {
                Console.Error.WriteLine("Error :- ", Message);
            }
            else
            {
                Console.Error.WriteLine(Message);
            }
        }
    }
}
