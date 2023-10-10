using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Base
{
    /// <summary>
    /// 此类属于所有 IDust 库中实体类的基类
    /// </summary>
    public class Component
    {
        /// <summary>
        /// 整个软件是否处在启动运行状态
        /// </summary>
        protected bool IsSoftRunning;

        public void SoftRunningStatusChanged(bool value)
        {
            IsSoftRunning = value;
        }
    }
}
