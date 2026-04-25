class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== LIVE CHAT ===");
        Console.WriteLine("1 - Servidor");
        Console.WriteLine("2 - Cliente");
        Console.Write("Escolha: ");
        
        string escolha = Console.ReadLine() ?? "";

        if (escolha == "1")
        {
            // Rodar como servidor
            Server servidor = new Server();
            servidor.Iniciar();
        }
        else if (escolha == "2")
        {
            // Rodar como cliente
            Client cliente = new Client();
            cliente.Conectar();
        }
        else
        {
            Console.WriteLine("Opção inválida");
        }
    }
}