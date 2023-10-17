using IDust.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IDust.Communicate.LightController
{
    public class MrLightController : LightControllerBase, ILightControllable
    {
        private string OrderStr;

        public MrLightController(in SerialPortParma parma, string oderStr = "#") : base(parma)
        {
            OrderStr = oderStr;
        }

        public RunResult ConnectOpen()
        {
            serialPort.Open();
            return new RunResult(ErrorCode.LightControllerConnected);
        }

        public RunResult ConnectClose()
        {
            serialPort.Close();
            return new RunResult(ErrorCode.LightControllerDisconnected);
        }

        public bool GetLgihtValue(int chanel, out int value)
        {
            value = int.MinValue;
            if (IsConnect)
            {
                serialPort.Write(GetLightStr(4, chanel, 100));
                var r = serialPort.ReceiveMessage(16, 20, 1000, 100);
                if (r.isSuccess)
                {
                    value = GetLightValue(r.message);
                    return true;
                }
            }
            return false;
        }

        public bool SetLightValue(int chanel, int value)
        {
            if (IsConnect)
            {
                var str = GetLightStr(3, chanel, value);
                serialPort.Write(str);
                Thread.Sleep(50);
                var r = serialPort.ReadExisting();
                if (r != null && r.Contains(this.OrderStr))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> SetLightValueAsync(int chanel, int value)
        {
            return await Task.Run(() => SetLightValue(chanel, value));
        }

        #region private methods
        /// <summary>
        /// 返回光源值的16进制表达
        /// </summary>
        /// <param name="light_value">光源值</param>
        /// <returns>长度最多为3个字符</returns>
        private string GetLightValueStr(int light_value)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat($"0{Convert.ToString(light_value, 16).ToUpper()}");
            if (str.Length < 3)
            {
                str.Insert(0, "0");
            }
            return str.ToString();
        }

        /// <summary>
        /// 获取一个字符串的 CRC 校验码
        /// </summary>
        /// <param name="crc_value">需要计算校验码的字符串</param>
        /// <returns>返回字符串只占2个字符</returns>
        private string GetCRCStr(string crc_value)
        {
            var bs = Encoding.ASCII.GetBytes(crc_value);
            var num = bs[0];
            for (int i = 1; i < bs.Length; i++)
            {
                num ^= bs[i];
            }
            return Convert.ToString(num, 16).ToUpper();
        }

        /// <summary>
        /// 返回光源通信格式字符串
        /// </summary>
        /// <param name="order">优先级</param>
        /// <param name="chanel">通道</param>
        /// <param name="value">光源值</param>
        /// <returns>占8个字符</returns>
        private string GetLightStr(int order, int chanel, int value)
        {
            // 1+1+1+3+2
            string code = OrderStr + order.ToString() + chanel.ToString() + GetLightValueStr(value);
            string crc = GetCRCStr(code);
            return code + crc;
        }

        /// <summary>
        /// 从串口获取的数据流中拆解出光源值
        /// </summary>
        /// <param name="value">数据流</param>
        /// <returns></returns>
        private int GetLightValue(string value)
        {
            int lightValue = -1;
            if (value.Length >= 6)
                lightValue = Convert.ToInt32(value.Substring(3, 3), 16);
            return lightValue;
        }
        #endregion
    }
}
