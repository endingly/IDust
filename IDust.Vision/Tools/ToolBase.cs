using HalconDotNet;
using IDust.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Vision.Tools;

/// <summary>
/// 所有工具类的基类
/// </summary>
public class ToolBase
{
    /// <summary>
    /// 工具名称
    /// <para>默认值为 DefaltToolName</para>
    /// </summary>
    public string ToolName = "DefaltToolName";
    /// <summary>
    /// 工具的工作目录
    /// <para>默认值为当前目录下的工具名称文件夹</para>
    /// </summary>
    public string ToolWorkDir { get => Path.Combine(Environment.CurrentDirectory, ToolName); }
    /// <summary>
    /// 工具是否准备好
    /// </summary>
    public bool IsReady { get; set; } = false;
    /// <summary>
    /// 工具参数文件保存路径
    /// <para>默认值为工具工作目录下的 parma.json</para>
    /// </summary>
    internal string ParmaSaveFilePath { get => Path.Combine(this.ToolWorkDir, "parma.json"); }
    
    public ToolBase()
    {
        if (!Directory.Exists(ToolWorkDir))
        {
            Directory.CreateDirectory(ToolWorkDir);
        }
    }

    #region virtual

    public virtual RunResult Run(ref HImage image, InParma inParma, OutParma outParma)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult Save()
    {
        throw new NotImplementedException();
    }
    #endregion

    
}

