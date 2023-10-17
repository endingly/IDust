using IDust.Communicate.Plc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Communicate.LightController
{
    public interface ILightControllable : IConnectCloseable
    {
        public Task<bool> SetLightValueAsync(int chanel, int value);

        public bool SetLightValue(int chanel, int value);

        public bool GetLgihtValue(int chanel, out int value);
    }
}
