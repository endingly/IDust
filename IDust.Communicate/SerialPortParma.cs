using HslCommunication;
using IDust.Base;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;

namespace IDust.Communicate;

public struct SerialPortParma
{
    public string PortName = "COM1";
    public int BaudRate = 9600;
    public Parity Parity = Parity.None;
    public int DataBits = 7;
    public StopBits StopBits = StopBits.One;

    public SerialPortParma()
    {

    }
}

public static class SerialPortExtension
{
    /// <summary>
    /// 从串口中接收数据，接收动作为: 
    /// <para>等待一段时间之后开始接收数据，且这段数据的长度应当大于 length_min 而小于 limit_max，以字节记</para>
    /// <para>否则到达时间限制 timeout 之后将报超时错误</para>
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="length_min">数据最小长度</param>
    /// <param name="limit_max">数据最大长度</param>
    /// <param name="timeout">超时时间</param>
    /// <param name="sleep">间歇休眠时间</param>
    /// <returns></returns>
    public static RunResult<string> ReceiveMessage(this SerialPort sp, int length_min, int limit_max, int timeout, int sleep)
    {
        MemoryStream memoryStream = new MemoryStream();
        DateTime now = DateTime.Now;
        while (true)
        {
            Thread.Sleep(sleep);
            try
            {
                if (sp.BytesToRead < length_min && (DateTime.Now - now).TotalMilliseconds > timeout)
                {
                    memoryStream.Dispose();
                    return new RunResult<string>(ErrorCode.SerialPortFailToRead, $"Timeout: {timeout}");
                }
                else
                {
                    int count = Math.Min(limit_max, sp.BytesToRead);
                    byte[] array = new byte[count];
                    int num = sp.Read(array, 0, count);
                    if (num > 0)
                    {
                        memoryStream.Write(array, 0, num);
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                memoryStream.Dispose();
                return new RunResult<string>(ErrorCode.SerialPortFailToRead, ex);
            }
        }
        return new RunResult<string>(ErrorCode.SerialPortReadSuccess, Encoding.UTF8.GetString(memoryStream.ToArray()));
    }
}