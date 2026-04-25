using System.Net.Sockets;
using System.Text;

class Client
{
    private TcpClient tcpClient;
    private NetworkStream stream;
    private string nomeUsuario;

    public void Conectar(string host = "localhost", int porta = 5000)
    {
        try
        {
            // 1. PEDIR nome do usuário
            Console.WriteLine("Digite seu nome:");
            nomeUsuario = Console.ReadLine() ?? "Anônimo";

            // 2. CRIAR conexão TCP
            tcpClient = new TcpClient();
            tcpClient.Connect(host, porta);
            stream = tcpClient.GetStream();
            Console.WriteLine($"[CLIENTE] Conectado ao servidor em {host}:{porta}");

            // 3. ENVIAR nome para o servidor
            byte[] dadosNome = Encoding.UTF8.GetBytes(nomeUsuario);
            stream.Write(dadosNome, 0, dadosNome.Length);

            // 4. INICIAR thread para receber mensagens (não bloqueia input do user)
            Thread threadReceber = new Thread(ReceberMensagens);
            threadReceber.IsBackground = true; // fecha com a aplicação
            threadReceber.Start();

            // 5. LOOP para enviar mensagens (thread principal)
            EnviarMensagens();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO] Falha ao conectar: {ex.Message}");
        }
        finally
        {
            stream?.Close();
            tcpClient?.Close();
        }
    }

    // Thread para receber mensagens do servidor
    private void ReceberMensagens()
    {
        try
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                
                // Read() bloqueia até receber dados
                int bytes = stream.Read(buffer, 0, buffer.Length);

                if (bytes == 0)
                {
                    Console.WriteLine("[SERVIDOR] Desconectado pelo servidor");
                    break;
                }

                // Exibir mensagem recebida
                string mensagem = Encoding.UTF8.GetString(buffer, 0, bytes);
                Console.WriteLine($"\n{mensagem}\n> ");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO ao receber] {ex.Message}");
        }
    }

    // Thread principal para enviar mensagens
    private void EnviarMensagens()
    {
        while (true)
        {
            Console.Write("> ");
            string msg = Console.ReadLine() ?? "";

            if (msg == "//quit")
            {
                Console.WriteLine("Saindo...");
                break;
            }

            if (msg == "//clear")
            {
                Console.Clear();
                continue;
            }

            if (string.IsNullOrWhiteSpace(msg))
                continue;

            // Enviar mensagem para o servidor
            try
            {
                byte[] dados = Encoding.UTF8.GetBytes(msg);
                stream.Write(dados, 0, dados.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] {ex.Message}");
                break;
            }
        }
    }
}
