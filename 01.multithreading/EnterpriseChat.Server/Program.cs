using System.IO.Pipes;
using System.Text;

namespace EnterpriseChat.Server
{
    internal class Program
    {
        private static int historySize = 10;
        private static Queue<string> messageHistory = new Queue<string>(historySize);
        private static string pipeName = "enterprise-pipe";

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    using (var pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut,
                        NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous))
                    {
                        Console.WriteLine($"Waiting for clients on {pipeName}...");
                        pipeServer.WaitForConnection();

                        Console.WriteLine($"Client connected.");

                        var buffer = new byte[1024];
                        var numBytes = 0;

                        while (pipeServer.IsConnected)
                        {
                            try
                            {
                                numBytes = pipeServer.Read(buffer, 0, buffer.Length);
                                if (numBytes > 0)
                                {
                                    var message = Encoding.UTF8.GetString(buffer, 0, numBytes);
                                    Console.WriteLine($"Received message: {message}");
                                    
                                    messageHistory.Enqueue(message);
                                    if (messageHistory.Count > historySize)
                                    {
                                        messageHistory.Dequeue();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                        }

                        Console.WriteLine($"Client disconnected.");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    break;
                }
            }

            Console.ReadLine();
        }
    }
}
