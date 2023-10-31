using HalconDotNet;
using IDust.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Vision.Tools;

public class ToolBase
{
    public virtual string ToolName { get; set; } = "DefaltToolName";
    public string ToolWorkDir { get => Path.Combine(Environment.CurrentDirectory, ToolName); }
    public bool IsReady { get; set; } = false;
    internal string ParmaSaveFilePath { get => Path.Combine(this.ToolWorkDir, "parma.json"); }
    
    public ToolBase()
    {
        if (!Directory.Exists(ToolWorkDir))
        {
            Directory.CreateDirectory(ToolWorkDir);
        }
    }

    #region virtual
    public virtual RunResult Init()
    {
        throw new NotImplementedException();
    }

    public virtual RunResult Run(HImage image, InParma inParma, OutParma outParma)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult Save()
    {
        throw new NotImplementedException();
    }
    #endregion

    
}

