using System.IO.Pipes;
using System.Text;

namespace EnterpriseChat.Client
{
    internal class Program
    {
        private static string pipeName = "enterprise-pipe";
        private static Random random = new Random();

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    using (var pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous))
                    {
                        pipeClient.Connect();
                        Console.WriteLine($"Connected to server on {pipeName}");

                        var numMessges = random.Next(5, 10);

                        for (int i = 0; i < numMessges; i++)
                        {
                            Thread.Sleep(random.Next(500, 2000));
                            var message = $"Message {i + 1}";

                            var buffer = Encoding.UTF8.GetBytes(message);
                            pipeClient.Write(buffer, 0, buffer.Length);
                        }

                        var readBuffer = new byte[1024];
                        var numBytes = 0;

                        do
                        {
                            numBytes = pipeClient.Read(readBuffer, 0, readBuffer.Length);
                            if (numBytes > 0)
                            {
                                var message = Encoding.UTF8.GetString(readBuffer, 0, numBytes);
                                Console.WriteLine($"Received message: {message}");
                            }
                        } while (numBytes > 0);

                        pipeClient.Close();
                        Console.WriteLine("Disconnected from server.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.ReadLine();
        }
    }
}
