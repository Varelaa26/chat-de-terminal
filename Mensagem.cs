class Mensagem
{
    public string Remetente { get; set; }
    public string Conteudo { get; set; }
    public DateTime Timestamp { get; set; }

    public void Exibir()
    {
        Console.WriteLine($"[{Timestamp}] {Remetente}: {Conteudo}");
    }

    public void Apagar()
    {
        Remetente = null;
        Conteudo = null;
        Timestamp = DateTime.MinValue;
    }
}