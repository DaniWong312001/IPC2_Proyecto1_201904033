using System;

namespace IPC2_Proyecto1_201904033

{
    class Program

    {
        static void Main(string[] args)

        {

            char entrada;

            Console.Write("Elija una Opción\n" +
            "1. Analizar un Paciente\n" +
            "2. Generar Archivo de Paciente\n" +
            "3. Limpiar información Cargada\n" +
            "4. Salir del Programa\n");

            entrada = Convert.ToChar(Console.ReadLine());

            switch (entrada)
            {
                case '1':
                    Console.WriteLine("Analizar un Paciente");
                    break;
                case '2':
                    Console.WriteLine("Generar Archivo de Paciente");
                    break;
                case '3':
                    Console.WriteLine("Limpiar información Cargada");
                    break;
                case '4':
                    Console.WriteLine("Fin del Programa, Gracias");
                    break;
                default:
                    Console.WriteLine("Opcion no Válida");
                    break;
            }

        }
    }

}