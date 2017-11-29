using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buscaminas
{
    class Program
    {
        const int bombaOculta = -9;
        const int bombaMostrada = 9;
        const int ceroMostrado = 10;
        static int[,] tablero;
        static int tableroFilas = 8;
        static int tableroColumnas = 8;
        static int filaJugador;
        static int columnaJugador;
        static int cantidadBombas;
        static int celdasAbiertas;
        static int puntuacionMax;
        static bool vivo;
        static string esc;
        static string creditos;
        static string[] menu;
        static string[] juego;
        static string[] opciones;
        static int idiomaActual = 0;
        static string[] idiomas =
            {
                "Castellano",
                "English",
                "Français"
            };

        static void Main()
        {
            cantidadBombas = Convert.ToInt32(tableroFilas * tableroColumnas * 0.15625);
            puntuacionMax = tableroFilas * tableroColumnas - cantidadBombas;
            Console.SetWindowSize(tableroColumnas * 5, tableroFilas + 15);
            CambiarIdioma(idiomaActual);

            int seleccion = 1;
            while (true)
            {
                Console.Clear();

                Console.WriteLine(menu[0]);
                Console.WriteLine();

                for (int i = 1; i < menu.Length; i++)
                {
                    if (seleccion == i)
                        Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine(menu[i]);

                    if (seleccion == i)
                        Console.ResetColor();
                }

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (seleccion != 1)
                            seleccion--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (seleccion != 4)
                            seleccion++;
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.RightArrow:
                        switch (seleccion)
                        {
                            case 1:
                                Jugar();
                                break;
                            case 2:
                                Opciones();
                                break;
                            case 3:
                                Creditos();
                                break;
                            case 4:
                                Environment.Exit(0);
                                break;
                            default:
                                break;
                        }

                        break;
                    default:
                        break;
                }
            }

        } // Fin del Main

        static void Opciones()
        {
            int seleccion = 0;
            int seleccionIdioma = idiomaActual;

            while (true)
            {
                Console.Clear();

                for (int i = 0; i < opciones.Length; i++)
                {
                    if (seleccion == i)
                        Console.ForegroundColor = ConsoleColor.Yellow;

                    if (i == 3)
                        Console.WriteLine();

                    Console.Write(opciones[i]);

                    if (i == 0)
                        Console.WriteLine(tableroFilas);
                    if (i == 1)
                        Console.WriteLine(tableroColumnas);
                    if (i == 2)
                        Console.WriteLine(idiomas[seleccionIdioma]);

                    if (seleccion == i)
                        Console.ResetColor();
                }

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (seleccion != 0)
                        {
                            seleccion--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (seleccion != 3)
                        {
                            seleccion++;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (tableroFilas > 8 && seleccion == 0)
                            tableroFilas--;
                        else if (tableroColumnas > 8 && seleccion == 1)
                            tableroColumnas--;
                        else if (seleccion == 2 && seleccionIdioma != 0)
                        {
                            seleccionIdioma--;
                            idiomaActual = seleccionIdioma;
                            CambiarIdioma(idiomaActual);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (seleccion < 2 && tableroFilas + tableroColumnas < 36)
                        {
                            if (seleccion == 0)
                                tableroFilas++;
                            else if (seleccion == 1)
                                tableroColumnas++;
                        }
                        else if (seleccion == 2 && seleccionIdioma != idiomas.Length - 1)
                        {
                            seleccionIdioma++;
                            idiomaActual = seleccionIdioma;
                            CambiarIdioma(idiomaActual);
                        }  
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        if (seleccion == 3)
                            Main();
                        break;
                    default:
                        break;
                }

            } // Fin de bucle

        } // Fin de opciones

        static void GenerarTablero()
        {
            tablero = new int[tableroFilas, tableroColumnas];

            Random random = new Random();
            int num1, num2;

            for (int i = 0; i < cantidadBombas; i++)
            {
                num1 = random.Next(0, tableroFilas);
                num2 = random.Next(0, tableroColumnas);

                if (tablero[num1, num2] != bombaOculta) // Cambiar esto para ver
                    tablero[num1, num2] = bombaOculta; // Cambiar esto para ver
                else
                    i--;
            } // Colocar bombas

            int contador;
            for (int i = 0; i < tableroFilas; i++)
            {
                for (int j = 0; j < tableroColumnas; j++)
                {
                    if (tablero[i, j] != bombaOculta) // Cambiar esto para ver
                    {
                        contador = 0;

                        for (int k = i - 1; k < i + 2; k++)
                        {
                            for (int l = j - 1; l < j + 2; l++)
                            {
                                if (k >= 0 && k < tableroFilas && l >= 0 && l < tableroColumnas)
                                {
                                    if (tablero[k, l] == bombaOculta) // Cambiar esto para ver
                                    {
                                        contador--; // Cambiar esto para ver
                                    }
                                }
                            }
                        }

                        tablero[i, j] = contador;
                    }
                }
            } // Colocar advertencias

        } // Fin de GenerarTablero

        static void Jugar()
        {
            vivo = true;
            filaJugador = 0;
            columnaJugador = 0;
            celdasAbiertas = 1;
            GenerarTablero();

            while (vivo)
            {
                PintarTablero();

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (filaJugador > 0)
                            filaJugador--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (filaJugador < tableroFilas - 1)
                            filaJugador++;
                        break;
                    case ConsoleKey.RightArrow:
                        if (columnaJugador < tableroColumnas - 1)
                            columnaJugador++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (columnaJugador > 0)
                            columnaJugador--;
                        break;
                    case ConsoleKey.B:
                        if (tablero[filaJugador, columnaJugador] == -100)
                        {
                            tablero[filaJugador, columnaJugador] = 0;
                        }
                        else if (tablero[filaJugador, columnaJugador] <= -10)
                        {
                            tablero[filaJugador, columnaJugador] /= 10;
                        }
                        else if (tablero[filaJugador, columnaJugador] < 0)
                        {
                            tablero[filaJugador, columnaJugador] *= 10;
                        }
                        else if (tablero[filaJugador, columnaJugador] == 0)
                        {
                            tablero[filaJugador, columnaJugador] -= 100;
                        }
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        if (celdasAbiertas == puntuacionMax)
                        {
                            tablero[filaJugador, columnaJugador] *= -1;
                            PintarTablero();
                            vivo = false;
                            Console.WriteLine();
                            Console.WriteLine(juego[0]);

                            while (Console.ReadKey().Key != ConsoleKey.Escape)
                            {
                                Console.WriteLine(esc);
                            }

                        }

                        if (tablero[filaJugador, columnaJugador] <= -10)
                        {
                            break;
                        }
                        else if (tablero[filaJugador, columnaJugador] == bombaOculta)
                        {
                            tablero[filaJugador, columnaJugador] *= -1;
                            PintarTablero();
                            vivo = false;
                            Console.WriteLine();
                            Console.WriteLine(juego[1]);
                            Console.WriteLine();

                            while (Console.ReadKey().Key != ConsoleKey.Escape)
                            {
                                Console.WriteLine(esc);
                            }


                        }
                        else if (tablero[filaJugador, columnaJugador] < 0)
                        {
                            tablero[filaJugador, columnaJugador] *= -1;
                            celdasAbiertas++;
                        }
                        else if (tablero[filaJugador, columnaJugador] == 0)
                        {
                            CeroPulsado(filaJugador, columnaJugador);
                        }
                        break;
                    case ConsoleKey.Escape:
                        Main();
                        break;
                    default:
                        break;
                }
            }

        } // Fin de Jugar

        static void PintarTablero()
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine(juego[2] + celdasAbiertas + " / " +
                puntuacionMax + juego[3] + cantidadBombas);
            Console.WriteLine();
            Console.Write("    ");

            for (int a = 1; a < tableroColumnas + 1; a++)
            {
                Console.Write("  " + a);
                if (a < 10)
                    Console.Write(".");
            } // Fila 0

            Console.WriteLine();

            for (int i = 0; i < tableroFilas; i++) // Columnas 0-8
            {
                Console.Write(" " + (i + 1));
                if (i < 9)
                    Console.Write(".");
                Console.Write(" ");

                for (int j = 0; j < tableroColumnas; j++)
                {
                    Pintura(i, j);
                    Console.ResetColor();
                }

                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;

                if (filaJugador == i && columnaJugador == tableroColumnas - 1)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine("|");
                Console.ResetColor();
            }

        } // Fin de PintarTablero

        static void Pintura(int fila, int columna)
        {
            if (filaJugador == fila && columnaJugador == columna)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");
            }
            else if (filaJugador == fila && columnaJugador == columna - 1)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("|");
            }

            if (tablero[fila, columna] <= -10)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.Write(" B ");
            }
            else if (tablero[fila, columna] <= 0)
            {
                Console.Write(" - ");
                // Console.Write(" " + tablero[fila, columna] + " "); // BORRAAAAAAAAAR
            }
            else
            {
                switch (tablero[fila, columna])
                {
                    case bombaMostrada:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(" x ");
                        break;
                    case ceroMostrado:
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write("   ");
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write(" " + tablero[fila, columna] + " ");
                        break;
                }
            }
        } // Fin de Pintor

        static void CeroPulsado(int fila, int columna)
        {
            for (int k = fila - 1; k < fila + 2; k++)
            {
                for (int l = columna - 1; l < columna + 2; l++)
                {
                    if (k >= 0 && k < tableroFilas && l >= 0 && l < tableroColumnas)
                    {
                        if (tablero[k, l] == -100)
                            tablero[k, l] = 0;

                        if (tablero[k, l] == 0)
                        {
                            tablero[k, l] = ceroMostrado;
                            celdasAbiertas++;
                            CeroPulsado(k, l);
                        }
                        else if (tablero[k, l] < 0)
                        {
                            if (tablero[k, l] <= -10)
                                tablero[k, l] /= 10;

                            tablero[k, l] *= -1;
                            celdasAbiertas++;
                        }
                    }
                }
            }
        } // Fin CeroPulsado

        static void Creditos()
        {
            Console.Clear();
            Console.SetWindowSize(60, 18);

            Console.WriteLine("                       __");
            Console.WriteLine("                     .'  '.");
            Console.WriteLine("                 _.-'/  |  \\");
            Console.WriteLine("    ,        _.-\"   ,|  / 0 `-.");
            Console.WriteLine("    |\\    .-\"       `--\"\" -.__.'=====================-,");
            Console.WriteLine("    \\ '-'`        .___.--._)=========================|");
            Console.WriteLine("     \\            .'      |                          |");
            Console.WriteLine("      |     /,_.-'        |       " + creditos + "      |");
            Console.WriteLine("    _/   _.'(             |                          |");
            Console.Write("   /  ,-' \\  \\            |         ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("¡");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("R");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("a");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("f");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("a");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("!");
            Console.ResetColor();
            Console.Write(" :3");
            Console.WriteLine("        |");
            Console.WriteLine("   \\  \\    `-'            |                          |");
            Console.WriteLine("    `-'                   '--------------------------'");
            Console.WriteLine();
            Console.WriteLine(esc);

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Escape:
                        Main();
                        break;
                    default:
                        break;
                }
            }
        } // Fin de Creditos

        static void CambiarIdioma(int idioma)
        {

            string escES = " (Pulsa Esc para volver)";
            string creditosES = "Desarrollador";
            string[] menuES =
            {
            " ¡Bienvenido al Buscaminas de Consola!",
            " Jugar",
            " Opciones",
            " Creditos",
            " Salir"
        };
            string[] juegoES =
            {
            " ¡GANASTE!",
            " ¡Has perdido!",
            " Puntuacion: ",
            "     Bombas: "
        };
            string[] opcionesES =
            {
            " Filas: ",
            " Columnas: ",
            " Idioma: ",
            " Volver"
        };

            string escEN = " (Press Esc to return)";
            string creditosEN = "  Developer  ";
            string[] menuEN =
            {
            " Welcome to Console Minesweeper!",
            " Play",
            " Options",
            " Credits",
            " Exit"
        };
            string[] juegoEN =
            {
            " ¡YOU WON!",
            " ¡You loose!",
            " Score: ",
            "     Bombs: "
        };
            string[] opcionesEN =
            {
            " Rows: ",
            " Columns: ",
            " Idiom: ",
            " Return"
        };

            string escFR = " (Appuyez sur Echap pour revenir)";
            string creditosFR = " Développeur ";
            string[] menuFR =
            {
            " Bienvenue dans la console Minesweeper!",
            " Jouer",
            " Options",
            " Crédits",
            " Sortir"
        };
            string[] juegoFR =
            {
            " Vous avez gagné!",
            " Tu as perdu!",
            " Score: ",
            "     Bombes: "
        };
            string[] opcionesFR =
            {
            " Lignes: ",
            " Colonnes: ",
            " Langage: ",
            " Revenir"
        };

            switch (idioma)
            {
                case 0:
                    esc = escES;
                    creditos = creditosES;
                    menu = menuES;
                    juego = juegoES;
                    opciones = opcionesES;
                    break;
                case 1:
                    esc = escEN;
                    creditos = creditosEN;
                    menu = menuEN;
                    juego = juegoEN;
                    opciones = opcionesEN;
                    break;
                case 2:
                    esc = escFR;
                    creditos = creditosFR;
                    menu = menuFR;
                    juego = juegoFR;
                    opciones = opcionesFR;
                    break;
                default:
                    break;
            }
        }

    }
}
