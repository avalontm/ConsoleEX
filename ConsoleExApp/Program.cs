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
            List<ConsoleTableItem> tabla = new List<ConsoleTableItem>();

            tabla.Add(new ConsoleTableItem() { Title = "ID", Items = [1, 2, 3] });
            tabla.Add(new ConsoleTableItem() { Title = "Fecha", Items = [DateTime.Now, DateTime.Now.AddDays(2), DateTime.Now.AddDays(1)] });
            tabla.Add(new ConsoleTableItem() { Title = "Name", Items = ["rojo", "verde", "azul"] });
            tabla.Add(new ConsoleTableItem() { Title = "Puntos", Items = [1.23, 1.456, 2.345] });
            tabla.Add(new ConsoleTableItem() { Title = "Estado", Items = [true, false, true] });

            ConsoleEX.ConsoleEx.CreateTable(tabla);

            Console.WriteLine("Preciona una tecla para continuar...");
            Console.ReadKey();
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
