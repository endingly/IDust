using HslCommunication;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.ModBus;
using HslCommunication.Core.IMessage;

namespace IDust.Communicate.Plc;

public class PlcModbusTcp : PlcBase
{
    #region member
    private ModbusTcpNet modbusTcpNet;
    #endregion

    #region ctor
    public PlcModbusTcp(PlcParma parma)
    {
        this.parma = parma;
        modbusTcpNet = new ModbusTcpNet();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}