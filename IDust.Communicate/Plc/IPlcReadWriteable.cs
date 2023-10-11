using HslCommunication;
using HslCommunication.Core;
using IDust.Base;
using System.Text;

namespace IDust.Communicate.Plc;

public interface IPlcReadWriteable<T> where T : IReadWriteDevice
{
    /// <summary>
    /// 获取由 hsl 库提供的核心客户端
    /// <para>需要重载</para>
    /// </summary>
    protected T CoreClient { get; }

    public IPlcReadWriteable<T> ReadWriteHandle { get; }

    public RunResult ReadValue(string address, out int value)
    {
        OperateResult<int> result = CoreClient.ReadInt32(address);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = int.MinValue;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, out float value)
    {
        OperateResult<float> result = CoreClient.ReadFloat(address);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = float.NaN;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, out double value)
    {
        OperateResult<double> result = CoreClient.ReadDouble(address);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = double.NaN;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, out bool value)
    {
        OperateResult<bool> result = CoreClient.ReadBool(address);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = false;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, out short value)
    {
        OperateResult<short> result = CoreClient.ReadInt16(address);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = short.MinValue;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, out long value)
    {
        OperateResult<long> result = CoreClient.ReadInt64(address);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = long.MinValue;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, out ushort value)
    {
        OperateResult<ushort> result = CoreClient.ReadUInt16(address);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = ushort.MinValue;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, out uint value)
    {
        OperateResult<uint> result = CoreClient.ReadUInt32(address);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = uint.MinValue;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, out ulong value)
    {
        OperateResult<ulong> result = CoreClient.ReadUInt64(address);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = ulong.MinValue;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, int length, out string value)
    {
        OperateResult<string> result = CoreClient.ReadString(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = string.Empty;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, out byte value)
    {
        OperateResult<byte[]> result = CoreClient.Read(address, 1);
        if (result.IsSuccess)
        {
            value = result.Content[0];
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = byte.MinValue;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }

    public RunResult WriteValue(string address, int value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, float value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, double value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, bool value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, short value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, long value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, ushort value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, uint value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, ulong value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, string value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, byte value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }

    public RunResult ReadValue(string address, int length, out int[] value)
    {
        OperateResult<int[]> result = CoreClient.ReadInt32(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = [];
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, int length, out float[] value)
    {
        OperateResult<float[]> result = CoreClient.ReadFloat(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = [];
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, int length, out double[] value)
    {
        OperateResult<double[]> result = CoreClient.ReadDouble(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = [];
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, int length, out bool[] value)
    {
        OperateResult<bool[]> result = CoreClient.ReadBool(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = [];
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, int length, out short[] value)
    {
        OperateResult<short[]> result = CoreClient.ReadInt16(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = [];
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, int length, out long[] value)
    {
        OperateResult<long[]> result = CoreClient.ReadInt64(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = [];
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, int length, out ushort[] value)
    {
        OperateResult<ushort[]> result = CoreClient.ReadUInt16(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = [];
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, int length, out uint[] value)
    {
        OperateResult<uint[]> result = CoreClient.ReadUInt32(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = [];
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, int length, out ulong[] value)
    {
        OperateResult<ulong[]> result = CoreClient.ReadUInt64(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = [];
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult ReadValue(string address, int length, out byte[] value)
    {
        OperateResult<byte[]> result = CoreClient.Read(address, (ushort)length);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = [];
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }

    public RunResult WriteValue(string address, int[] value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, float[] value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, double[] value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, bool[] value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, short[] value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, long[] value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, ushort[] value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, uint[] value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, ulong[] value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }
    public RunResult WriteValue(string address, byte[] value)
    {
        OperateResult result = CoreClient.Write(address, value);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcWriteSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToWrite);
        }
    }

    /// <summary>
    /// 清空寄存器
    /// </summary>
    /// <param name="address">地址</param>
    /// <param name="length">长度，以字节为单位</param>
    /// <returns></returns>
    public RunResult ClearValue(string address, int length)
    {
        byte[] buffer = new byte[length];
        Array.Fill(buffer, byte.MinValue);
        var r = WriteValue(address, buffer);
        if (r.isSuccess)
        {
            r.Reset(ErrorCode.PlcClearDataSuccess);
            return r;
        }
        else
        {
            r.Reset(ErrorCode.PlcFailToClearData);
            return r;
        }
    }

    public RunResult ReadUnicodeString(string address, int length, out string value)
    {
        OperateResult<string> result = CoreClient.ReadString(address, (ushort)length, Encoding.UTF8);
        if (result.IsSuccess)
        {
            value = result.Content;
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            value = string.Empty;
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
    public RunResult WriteUnicodeString(string address, string value)
    {
        OperateResult result = CoreClient.Write(address, value, Encoding.UTF8);
        if (result.IsSuccess)
        {
            return new RunResult(ErrorCode.PlcReadSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToRead);
        }
    }
}

