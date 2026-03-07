using System;
using System.Net;
public class MainApp
{
    public static void Run()
    {
        int port = 5000; 
        string url = $"http://localhost:{port}/";

       
        bool isConnected = DatabaseConnections.ConnectToDatabase();

        if (isConnected)
        {
            Console.WriteLine($"Server is active and connected to the database successfully on port {port}.");
        }
        else
        {
            Console.WriteLine($"Server is active but failed to connect to the database on port {port}.");
        }

    
        /*using (HttpListener listener = new HttpListener())
        {
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine($"🌐 Backend listening on {url}...");

            // Esperamos por una sola solicitud como ejemplo
            Console.WriteLine("Waiting for incoming requests...");
            var context = listener.GetContext(); // Bloqueante
            var response = context.Response;
            string responseString = "<html><body>Backend is running!</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();

            listener.Stop();
        }*/
    }
}