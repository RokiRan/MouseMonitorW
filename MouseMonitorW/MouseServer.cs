using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;  

namespace MouseMonitorW
{
    class MouseServer
    {
        private static int port = 5000;
        public volatile static bool running = true;
        static UdpClient udpServer = new UdpClient(port);

        public static void startWork()
        {
            if (running == true) 
            {
                running = false;
                Thread.Sleep(500);
            }

            running = true;
            Thread workerThread = new Thread(run);
            workerThread.Start();
            Console.WriteLine("Starting worker thread...");
        }

        public static void stop()
        {
            running = false;
        }

        public static void run()
        {
            while (running)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, port);
                var data = udpServer.Receive(ref remoteEP); 
                // Console.Write("receive data from " + data.ToString());

                sbyte[] recvData = getByteArraysByString(System.Text.Encoding.Default.GetString(data));
				controlMouseByDate(recvData);
            }
        }
        public static sbyte[] getByteArraysByString(String data)
        {
            String revc = data.Replace("[", "");
            revc = revc.Replace("]", "");
            if (revc.Contains(","))
            {
                String[] datas = revc.Split(',');

                sbyte[] byteDatas = new sbyte[datas.Length];

                for (int i = 0; i < datas.Length; i++)
                {
                    Console.WriteLine(datas[i].Trim());
                    byteDatas[i] = sbyte.Parse(datas[i].Trim());
                }
                return byteDatas;
            }
            else
            {
                sbyte[] byteData = { sbyte.Parse(revc) };
                return byteData;
            }
        }

        public static void controlMouseByDate(sbyte[] datas) {
            if (datas == null || datas.Length == 0) {
                return;
            }
            switch (datas[0]) {
            // 左键单击
            case 1:
                Mouse.leftSingle();
                break;

            // 左键双击
            case 2:
                Mouse.leftDouble();
                break;

            // 鼠标滑动;
            case 3:
                int offset_x = byteArrayToInt_x(datas);
                int offset_y = byteArrayToInt_y(datas);
                
                Mouse.move(-offset_x, -offset_y);
                break;

            // 滚轮
            case 4:
                
                int wheelAmt = byteArrayToInt_scroll(datas);
                if (wheelAmt > 0) {
                    wheelAmt = 1;
                }else if(wheelAmt < 0){
                    wheelAmt = -1;
                }else {
                    wheelAmt = 0;
                }

                Mouse.wheel(wheelAmt);
                break;
            case 5:
                //右键单击
                Mouse.right();
                break;
            default:
                break;
            }
        }

        public static int byteArrayToInt_x(sbyte[] data)
        {
            sbyte[] byte_x = new sbyte[4];
            byte_x[0] = data[1];
            byte_x[1] = data[2];
            byte_x[2] = data[3];
            byte_x[3] = data[4];

            int value;
            value = (int)((byte_x[3] & 0xFF) | ((byte_x[2] & 0xFF) << 8)
                    | ((byte_x[1] & 0xFF) << 16) | ((byte_x[0] & 0xFF) << 24));
            return value;
        }

        public static int byteArrayToInt_y(sbyte[] data)
        {
            sbyte[] byte_y = new sbyte[4];
            byte_y[0] = data[5];
            byte_y[1] = data[6];
            byte_y[2] = data[7];
            byte_y[3] = data[8];
            int value;
            value = (int)((byte_y[3] & 0xFF) | ((byte_y[2] & 0xFF) << 8)
                    | ((byte_y[1] & 0xFF) << 16) | ((byte_y[0] & 0xFF) << 24));
            return value;
        }

        public static int byteArrayToInt_scroll(sbyte[] data)
        {
            sbyte[] byte_scroll = new sbyte[4];
            byte_scroll[0] = data[1];
            byte_scroll[1] = data[2];
            byte_scroll[2] = data[3];
            byte_scroll[3] = data[4];
            int value;
            value = (int)((byte_scroll[3] & 0xFF) | ((byte_scroll[2] & 0xFF) << 8)
                    | ((byte_scroll[1] & 0xFF) << 16) | ((byte_scroll[0] & 0xFF) << 24));
            return value;
        }
    }
}
