using IDust.Base;
using IDust.Communicate.Plc;
using System.IO.Ports;

namespace IDust.Communicate.LightController;

public class LightControllerBase
{
    public SerialPortParma serialPortParma;
    protected SerialPort serialPort;

    public LightControllerBase(in SerialPortParma parma)
    {
        serialPortParma = parma;
        serialPort = new SerialPort()
        {
            PortName = serialPortParma.PortName,
            BaudRate = serialPortParma.BaudRate,
            Parity = serialPortParma.Parity,
            DataBits = serialPortParma.DataBits,
            StopBits = serialPortParma.StopBits
        };
    }

    public bool IsConnect => this.serialPort != null && this.serialPort.IsOpen;
}

