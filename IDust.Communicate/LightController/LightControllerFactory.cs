using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Communicate.LightController;

public enum LightControllerType
{
    CST,
    MR_one
}

public class LightControllerFactory
{
    private ILightControllable _lightController;

    public LightControllerFactory(LightControllerType type, in SerialPortParma parma)
    {
        _lightControllerType = type;
        switch (type)
        {
            case LightControllerType.CST:
                _lightController = new CstLightController(parma);
                break;
            case LightControllerType.MR_one:
                _lightController = new MrLightController(parma);
                break;
            default:
                _lightController = new MrLightController(parma);
                break;
        }
    }

    public ILightControllable Intance => _lightController;
}

