namespace ISKL
{
    class RootExeption : Exception
    {
        public RootExeption(Severity severity):base() {}
    }    
    enum Severity
    {
        Warning,
        Error
    }
    class Program
    {
        static int a, b, c;
        static string equation = "a * x^2 + b * x + c = 0";
        static string[] menuOptions = { "a : ", "b : ", "c : ", "Найти корни" };
        static void Main(string[] args)
        {
            int selectedIndex = 0;
            bool running = true;

            while (running)
            {
                ShowMainMenu(selectedIndex);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                try
                {
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedIndex--;
                            if (selectedIndex < 0) selectedIndex = menuOptions.Length - 1;
                            break;

                        case ConsoleKey.DownArrow:
                            selectedIndex++;
                            if (selectedIndex >= menuOptions.Length) selectedIndex = 0;
                            break;

                        case ConsoleKey.Enter:

                            switch (selectedIndex)
                            {
                                case 0:
                                    Console.SetCursorPosition(6, 1);
                                    GettingValue("a", out a, selectedIndex);
                                    break;
                                case 1:
                                    Console.SetCursorPosition(6, 2);
                                    GettingValue("b", out b, selectedIndex);
                                    break;
                                case 2:
                                    Console.SetCursorPosition(6, 3);
                                    GettingValue("c", out c, selectedIndex);
                                    break;
                                case 3:
                                    SearchRoot();
                                    running = false;
                                    Console.ReadLine();
                                    break;
                            }
                            break;
                    }
                }                
                catch (FormatException ex)
                {
                    Console.SetCursorPosition(0, 5);
                    Console.WriteLine("--------------------------------------------------");
                    FormatData(ex.Message,Severity.Error);
                    Console.ReadLine();                    
                }

                catch (RootExeption)
                {                      
                    Console.WriteLine("--------------------------------------------------");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Корней нет");
                    Console.ResetColor();
                    Console.ReadLine();
                    ResetProgram();
                    ClearScreen();
                }
                catch (OverflowException)
                {
                    HandleOverflow();
                }
            }
        }                
        static void ResetProgram()
        {
            a = 0;
            b = 0;
            c = 0;
            equation = "a * x^2 + b * x + c = 0";
            for (int i = 0; i < menuOptions.Length - 1; i++)
            {
                menuOptions[i] = $"{menuOptions[i].Split(':')[0]}: ";
            }
        }    
        static void ClearScreen()
        {
            Console.Clear();
        }
        static void ShowMainMenu(int selectedIndex)
        {
            ClearScreen();
            Console.WriteLine(equation);

            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {menuOptions[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {menuOptions[i]}");
                }
            }
        }

        static int GetDiscriminant(int a, int b, int c)
        {            
            return  b * b - 4 * a * c;
        }      
        static void SearchRoot()
        {
            int dis = GetDiscriminant(a, b, c);
            switch (dis)
            {
                case (< 0):
                    throw new RootExeption(Severity.Warning);

                case 0:
                    int x;
                    x = (b + (int)Math.Pow(dis, 2)) / 2 * a;
                    Console.WriteLine($"Корень уравнения равен : x1 = {x}");
                    break;

                case > 0:
                    int x1, x2;
                    x1 = (-b + (int)Math.Sqrt(dis)) / 2 * a;
                    x2 = (-b - (int)Math.Sqrt(dis)) / 2 * a;
                    Console.WriteLine($"Корни уравнений равны : x1 = {x1} , x2 = {x2}");
                    break;
            }

        }
        static void GettingValue(string variableName, out int value, int selectedIndex)
        {
            string input = Console.ReadLine();

            if (!long.TryParse(input, out long longValue))
            {
                throw new FormatException($"Неверный формат параметра {variableName}.");
            }

            if (longValue < int.MinValue || longValue > int.MaxValue)
            {
                throw new OverflowException();
            }

            equation = equation.Replace(variableName, input);

            value = (int)longValue;

            menuOptions[selectedIndex] = $"{variableName} : " + value;
        }        
        
        static void FormatData(string message,Severity severity)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;

            Console.WriteLine($"{message}");
            Console.ResetColor();
        }
        static void HandleOverflow()
        {
            string message = "--------------------------------------------------\r\n" +
                             "| Значение выходит за пределы типа int.         |\r\n" +
                             "| Допустимые значения: от " + int.MinValue + " до " + int.MaxValue + ". |\r\n" +
                             "--------------------------------------------------";

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ReadLine();                        
            ShowMainMenu(0);
        }
    }
}


