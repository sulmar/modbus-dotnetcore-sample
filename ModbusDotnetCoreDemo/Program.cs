using NModbus;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ModbusDotnetCoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            Task.Run(()=>ModbusTest());

            Console.ReadKey();


        }


        private static async Task SlaveTest()
        {
            string hostname = "localhost";
            int port = 9000;

            var TcpHost = IPAddress.Parse("127.0.0.1");

            var slaveListener = new TcpListener(TcpHost, port);
            
            var factory = new ModbusFactory();

            using (var client = new TcpClient())
            using (var slave = factory.CreateSlaveNetwork(slaveListener))
            {
                slave.AddSlave(factory.CreateSlave(1));

                await slave.ListenAsync();

            }
        }

        private static async Task  ModbusTest()
        {
            string hostname = "localhost";
            int port = 502;

            var factory = new ModbusFactory();

            using (var client = new TcpClient())
            using (var master = factory.CreateMaster(client))
            {
                await client.ConnectAsync(hostname, port);

                var inputs = await master.ReadInputRegistersAsync(1, 30001, 10);

                var outputs = new bool[] { true, false, true, true };

                await master.WriteMultipleCoilsAsync(1, 17, outputs);

                var outputs2 = await master.ReadCoilsAsync(1, 17, 10);
            }

        }

    }
}