using IDust.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Communicate.Plc
{
    public interface IConnectCloseable
    {
        /// <summary>
        /// 打开连接
        /// </summary>
        /// <returns></returns>
        public RunResult ConnectOpen();

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public RunResult ConnectClose();
    }
}
