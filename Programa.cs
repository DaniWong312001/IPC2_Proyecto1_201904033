namespace IPC2_Proyecto1
{
    // Punto de entrada de la aplicación de diagnóstico epidemiológico.
    // Universidad San Carlos de Guatemala – IPC2 Proyecto 1
    class Program
    {
        static void Main(string[] args)
        {
            MenuPrincipal menu = new MenuPrincipal();
            menu.Ejecutar();
        }
    }
}