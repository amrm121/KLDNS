using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Aviso_Remoto
{
    class Program
    {
        static WindowsMediaPlayer wplayer; // Reference to wmp.dll (\windows\system32\wmp.dll)
                                          // using WMPLib;
        static void Main(string[] args)
        {
           
            TcpListener serverSocket = new TcpListener(8888);
            int requestCount = 0;
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine(" >> Servidor iniciado... [ " + DateTime.Now + " ]");
            clientSocket = serverSocket.AcceptTcpClient();
            Console.WriteLine(" >> Um cliente conectou-se... [ " + DateTime.Now + " ]");
            requestCount = 0;
            string guardaNota = String.Empty;

            while ((true))
            {
                try
                {
                    NetworkStream networkStream = clientSocket.GetStream();
                    byte[] bytesFrom = new byte[10025];
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                    bool teste = string.IsNullOrEmpty(dataFromClient);
                    bool teste2 = string.Compare(dataFromClient, guardaNota, true) == 0;

                    if (!teste && !teste2)
                    { 

                        requestCount = requestCount + 1;
                        Console.WriteLine(" >> " +requestCount+ " Notificação recebida - " /*+ dataFromClient*/);
                        string serverResponse = "Última notificação recebida - " + dataFromClient;
                        Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
                        Console.WriteLine(" >> " + serverResponse + Environment.NewLine);
                        Console.WriteLine(" >> Favor reiniciar esta janela (feche e abra novamente)...");

                        guardaNota = dataFromClient;

                        wplayer = new WMPLib.WindowsMediaPlayer();
                        wplayer.URL = "zap_zap.mp3";
                        wplayer.controls.play();
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.ToString());
                    //MessageBox.Show(ex.ToString());
                }
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> Saindo...");
            Console.ReadLine();
        }

        }
}
