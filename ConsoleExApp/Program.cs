using ConsoleEX;

namespace ConsoleExApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<ConsoleMenuItem> opciones = new List<ConsoleMenuItem>()
            {
                new ConsoleMenuItem() { Title = "Decimal a Binario", Action = onOption1 },
                new ConsoleMenuItem() { Title = "Invertir Numero" , Action = onOption2 },
                new ConsoleMenuItem() { Title =  "Salir", Action = onSalir}
            };


            ConsoleEx.Menu("", opciones);


            for (; ; )
            {

            }
        }

        static async Task onOption1()
        {

        }
        static async Task onOption2()
        {
         
        }


        static async Task onSalir()
        {
            Environment.Exit(0);
        }
    }
}
