using HslCommunication;
using IDust.Base;
using IDust.Communicate.Plc;
using IoTClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Communicate.LightController
{
    public class CstLightController : LightControllerBase, ILightControllable
    {
        public CstLightController(in SerialPortParma parma) : base(parma)
        {
        }

        #region interface ILightControllable
        public RunResult ConnectClose()
        {
            try
            {
                if (IsConnect)
                {
                    serialPort.Close();
                }
                return new RunResult(ErrorCode.LightControllerDisconnected);
            }
            catch (Exception ex)
            {
                return new RunResult(ErrorCode.LightControllerFailToDisconnect, ex);
            }
        }

        public RunResult ConnectOpen()
        {
            try
            {
                serialPort.Open();
                return new RunResult(ErrorCode.LightControllerConnected);
            }
            catch (Exception ex)
            {
                return new RunResult(ErrorCode.LightControllerFailToConnect, ex);
            }
        }

        public bool GetLgihtValue(int chanel, out int value)
        {
            if (IsConnect)
            {
                serialPort.Write("S" + ConvertChanelToString(chanel) + "#");
                var result = serialPort.ReceiveMessage(5, 10, 1000, 100);
                if (result.isSuccess)
                {
                    StringBuilder str = new StringBuilder(result.message);
                    str.Remove(0, 1);
                    str.Remove(str.Length - 1, 1);
                    value = int.Parse(str.ToString());
                    return true;
                }
            }
            value = int.MinValue;
            return false;
        }

        public bool SetLightValue(int chanel, int value)
        {
            bool flag = false;
            if (IsConnect)
            {
                string message = GetCommunicationMessage(chanel, value);
                this.serialPort.Write(message);
                flag = true;
            }
            return flag;
        }

        public async Task<bool> SetLightValueAsync(int chanel, int value)
        {
            return await Task.Run(() => SetLightValue(chanel, value));
        }
        #endregion

        #region private methods
        private string ConvertChanelToString(int chanel)
        {
            return chanel switch
            {
                1 => "A",
                2 => "B",
                3 => "C",
                4 => "D",
                5 => "E",
                6 => "F",
                7 => "G",
                8 => "H",
                _ => string.Empty
            };
        }

        private string GetCommunicationMessage(int chanel,int value)
        {
            string address = ConvertChanelToString(chanel);
            string data = Convert.ToString(value);
            for (int length = data.Length; length < 4; ++length)
                data = "0" + data;
            return "S" + address + data + "#";
        }   
        #endregion
    }
}
