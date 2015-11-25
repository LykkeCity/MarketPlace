using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpSockets
{


    public class ServerTcpSocket<TSerializer, TService>
        where TSerializer : ITcpSerializer, new()
        where TService : ITcpService
    {
        private TcpListener _serverSocket;
        private int _id;

        private readonly Connections _connections;
        private readonly IPEndPoint _ipEndPoint;
        public ISocketLog Log { get;}

        private readonly Func<TService> _srvFactory;

        public ServerTcpSocket(IPEndPoint ipEndPoint, ISocketLog log, Func<TService> srvFactory)
        {
            _srvFactory = srvFactory;
            _ipEndPoint = ipEndPoint;
            Log = log;
            _connections = new Connections(log);


            SocketStatistic = new SocketStatistic();
        }

        public void AcceptSocketThread()
        {

            _serverSocket = new TcpListener(_ipEndPoint);
            _serverSocket.Start();
            Log.Add("Started listening tcp socket: " + _ipEndPoint.Port);

            while (_thread != null)
            {
                try
                {
                    var acceptedSocket = _serverSocket.AcceptTcpClient();
                    var service = _srvFactory();
                    var connection = new TcpConnection(service, new TSerializer(), acceptedSocket, SocketStatistic, Log, _id++);
                    _connections.AddSocket(connection);
                    connection.StartReadData();
                }
                catch (Exception ex)
                {
                    Log.Add("Error accepting socket: " + ex.Message);
                }
            }

        }

        public TService[] AllConnections
        {
            get { return _connections
                .AllConnections
                .Select(connection => connection.TcpService)
                .Cast<TService>().ToArray(); }
        }

        private Thread _thread;
        public void Start()
        {
            _thread = new Thread(AcceptSocketThread);
            _thread.Start();
        }

        public void Stop()
        {
            _thread = null;
            _serverSocket.Stop();
        }
   
        /// <summary>
        /// Число активных соединений
        /// </summary>
        public int Count => _connections.Count;

        public SocketStatistic SocketStatistic { get; }


    }
}

