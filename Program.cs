using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

var logFilePath = "/var/lib/jenkins/workspace/teste-pipeline/application.log";
using var logWriter = new StreamWriter(logFilePath, append: true);

var listener = new TcpListener(IPAddress.Any, 5000);
listener.Start();

logWriter.WriteLine("Servidor iniciado. Aguardando conexões...");
logWriter.Flush();

while (true)
{
    var client = await listener.AcceptTcpClientAsync();
    logWriter.WriteLine("Cliente conectado");
    logWriter.Flush();

    _ = Task.Run(async () =>
    {
        using (var networkStream = client.GetStream())
        {
            var buffer = new byte[1024];
            int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            logWriter.WriteLine($"Mensagem recebida: {message}");
            logWriter.Flush();
        }
        client.Close();
    });
}