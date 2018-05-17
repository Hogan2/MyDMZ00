using System.Net.Sockets;

namespace CommunicationModule
{
    interface ICommunicate
    {
        Socket Communicate(int localPort);
        void SendData(string outIp, string outPort, string localPort, byte[] data);
        byte[] ReceiveData(string ip, string port, string localPort);
    }
}
