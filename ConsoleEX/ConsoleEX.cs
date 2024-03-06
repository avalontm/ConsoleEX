using System.Diagnostics;
using System.Reflection;

namespace ConsoleEX
{

    public static class ConsoleEx
    {
        static bool isWait { get; set; } = false;
        static ConsoleSpinner spinner;


        /// <summary>
        /// Oculta lo que se escriba en consola
        /// </summary>
        /// <returns></returns>
        public static string ReadPassword()
        {
            // Leer la contraseña sin mostrarla en la consola
            var password = "";
            while (true)
            {
                var tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (tecla.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.CursorLeft--;
                        Console.Write(" ");
                        Console.CursorLeft--;
                    }
                }
                else
                {
                    password += tecla.KeyChar;
                    Console.Write("*");
                }
            }

            Console.WriteLine();
            return password;
        }


        /// <summary>
        /// Terminamos al animacion de esperar
        /// </summary>
        /// <returns></returns>
        public static async Task WaitEnd()
        {
            if (spinner != null)
            {
                isWait = false;
            }

            await Task.Delay(100);
        }

        /// <summary>
        /// Animacion de esperar
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static async Task WaitStart(string message = "Espere", int sequence = 5)
        {
#pragma warning disable CS4014 // Dado que no se esperaba esta llamada, la ejecución del método actual continuará antes de que se complete la llamada
            Task.Run(() =>
            {
                isWait = true;
                spinner = new ConsoleSpinner();
                spinner.Delay = 300;
                while (isWait)
                {
                    spinner.Turn(displayMsg: message, sequenceCode: sequence);
                }

                spinner.End();
            });
#pragma warning restore CS4014 // Dado que no se esperaba esta llamada, la ejecución del método actual continuará antes de que se complete la llamada

            await Task.Delay(1000);
        }


        /// <summary>
        /// Crea una tabla en base al modelo d lista
        /// </summary>
        /// <param name="datos"></param>
        public static void CreateTable(List<ConsoleTableItem> datos, ConsoleColor title_color = ConsoleColor.Yellow, ConsoleColor PrimaryColor = ConsoleColor.White, ConsoleColor SecundaryColor = ConsoleColor.Gray)
        {
            if (datos == null || datos.Count == 0)
            {
                Console.WriteLine("No se proporcionan datos. No se puede generar la tabla.");
                return;
            }

            const int maxTableWidth = 100; // Limitar ancho total de la tabla

            Console.WriteLine();

            // Encontrar el título más largo
            int maxTitleLength = datos.Max(item => item.Title.Length) + 4;

            // Definimos la altura de la tabla
            int maxHeight = datos.Max(item => item.Items.Count);

            // Creamos una matriz para almacenar los items formateados y centrados
            string[,] formattedItems = new string[maxHeight, datos.Count];

            // Calcular ancho máximo de las columnas de items
            int[] columnWidths = new int[datos.Count];
            int maxColumnIndex = -1;
            int maxColumnWidth = 0;

            for (int col = 0; col < datos.Count; col++)
            {
                int maxWidth = 0;

                for (int row = 0; row < maxHeight; row++)
                {
                    if (row < datos[col].Items.Count)
                    {
                        object item = datos[col].Items[row];
                        string formattedItem = FormatItem(item); // Función para formateo condicional
                        formattedItem = formattedItem.PadRight(formattedItem.Length + 2); // Añadimos padding
                        formattedItems[row, col] = formattedItem;
                        maxWidth = Math.Max(maxWidth, formattedItem.Length);
                    }
                }

                columnWidths[col] = Math.Min(maxWidth, maxTableWidth / datos.Count); // Limitar ancho de columna

                if (maxWidth > maxColumnWidth)
                {
                    maxColumnWidth = maxWidth;
                    maxColumnIndex = col;
                }
            }

            // Imprimir la línea de separación
            Console.WriteLine(new string('-', Console.WindowWidth));

            Console.ForegroundColor = title_color;
            // Imprimir encabezados de columna
            Console.WriteLine(string.Join(" ", datos.Select(item => item.Title.PadLeft((columnWidths[maxColumnIndex] - (item.Title.Length / 2))-1).PadRight(Math.Max(columnWidths[maxColumnIndex] -1, (item.Title.Length/2) )+1))));
            Console.ResetColor();

            // Imprimir la línea de separación
            Console.WriteLine(new string('-', Console.WindowWidth));

            // Contador para alternar colores
            int colorCounter = 0;

            // Imprimir el título y items
            for (int row = 0; row < maxHeight; row++)
            {
                Console.ForegroundColor = (ConsoleColor)(colorCounter % 2 == 0 ? PrimaryColor : SecundaryColor); // Alternar colores

                for (int col = 0; col < datos.Count; col++)
                {
                    // Calcular padding para centrar el título
                    int paddingLength = (maxTitleLength - datos[col].Title.Length) / 2;

                    // Imprimir item formateado o espacio en blanco
                    if (row < datos[col].Items.Count)
                    {
                        Console.Write(formattedItems[row, col].PadLeft((columnWidths[maxColumnIndex] - (formattedItems[row, col].Length / 2))).PadRight(Math.Max(columnWidths[maxColumnIndex]-1, (formattedItems[row, col].Length/2) )+2)); // Añadir padding de 2 espacios 
                    }
                    else
                    {
                        Console.Write(new string(' ', columnWidths[maxColumnIndex]));
                    }
                }
                Console.ResetColor(); // Restablecer color después de la fila
                Console.WriteLine();
                colorCounter++; // Incrementar contador para alternar colores
            }

            Console.ResetColor(); // Restablecer color después de la fila

            // Imprimir la línea de separación
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.WriteLine();

        }

        // Función para formatear condicionalmente los items
        static string FormatItem(object item)
        {
            if (item is string)
            {
                return item.ToString();
            }
            else if (item is DateTime)
            {
                return ((DateTime)item).ToString("dd/MM/yyyy");
            }
            else if (item is decimal)
            {
                return ((decimal)item).ToString("N2");
            }
            else
            {
                return item.ToString();
            }
        }

        /// <summary>
        /// Establece un menu en consola
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="opciones"></param>
        /// <returns></returns>
        public static async Task Menu(string titulo, List<ConsoleMenuItem> opciones)
        {
            // Posición actual del cursor
            int cursorPos = 0;
            // Posición actual de la barra
            int barraPos = 0;

            // Mostrar el menú con marco
            const char EsquinaSuperiorIzquierda = '╔';
            const char EsquinaSuperiorDerecha = '╗';
            const char EsquinaInferiorIzquierda = '╚';
            const char EsquinaInferiorDerecha = '╝';
            const char LineaHorizontal = '═';
            const char LineaVertical = '║';

            while (true)
            {
                int anchoConsola = Console.WindowWidth;
                int anchoMarco = anchoConsola - 10;

                // Limpiar la pantalla
                Console.Clear();

                // Calcular la posición central del título
                int anchoTitulo = titulo.Length;
                int centroTitulo = (anchoConsola - anchoTitulo) / 2;

                // Imprimir el título antes del marco
                Console.SetCursorPosition(centroTitulo, Console.CursorTop);

                Console.WriteLine(titulo);

                // Imprimir el marco con separación
                Console.Write("    ");
                Console.Write(EsquinaSuperiorIzquierda);
                Console.Write(new string(LineaHorizontal, anchoMarco));
                Console.WriteLine(EsquinaSuperiorDerecha);

                // Mostrar el menú
                for (int i = 0; i < opciones.Count; i++)
                {
                    Console.Write("    ");

                    Console.Write(LineaVertical);
                    Console.BackgroundColor = (i == cursorPos) ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = (i == cursorPos) ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write("  {0,-" + (anchoMarco - 6) + "}    ", i + 1 + ". " + opciones[i].Title);
                    Console.ResetColor();
                    Console.Write(LineaVertical);
                    Console.WriteLine();

                    // Mostrar la barra solo en la opción actual
                    if (i == barraPos)
                    {
                        Console.SetCursorPosition(1, Console.CursorTop - 1);
                        Console.SetCursorPosition(Console.CursorLeft + (anchoMarco - 2), Console.CursorTop);
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                }

                Console.Write("    ");
                Console.Write(EsquinaInferiorIzquierda);
                Console.Write(new string(LineaHorizontal, anchoMarco));
                Console.WriteLine(EsquinaInferiorDerecha);

                // Leer la tecla pulsada
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                // Navegar por el menú
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        cursorPos = (cursorPos - 1 + opciones.Count) % opciones.Count;
                        barraPos = cursorPos;
                        break;
                    case ConsoleKey.DownArrow:
                        cursorPos = (cursorPos + 1) % opciones.Count;
                        barraPos = cursorPos;
                        break;
                    case ConsoleKey.Enter:
                        // Seleccionar la opción actual
                        int opcionSeleccionada = cursorPos;

                        // Llamamos la accion del menu
                        if (opcionSeleccionada < opciones.Count)
                        {
                            await opciones[opcionSeleccionada].Action?.Invoke();
                        }

                        break;
                }
            }
        }
    }
}
