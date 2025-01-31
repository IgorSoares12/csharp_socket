using System.Net;
using System.Net.Sockets;
using System.Text;

var listener = new TcpListener(IPAddress.Any, 5000);
listener.Start();

System.Console.WriteLine("Servidor iniciado. Aguardando conexões...");

while(true)
{
    var client = await listener.AcceptTcpClientAsync();
    System.Console.WriteLine("Cliente conectado");

    _ = Task.Run(async () =>
    {
        using (var networkStream = client.GetStream())
        {
            var buffer = new byte[1024];
            int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            System.Console.WriteLine($"Mensagem recebida: {message}");
        }
        client.Close();
    });
}