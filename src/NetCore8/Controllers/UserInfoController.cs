using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using NetCore8.Models;

namespace NetCore8.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserInfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // 定义脚本路径
                var scriptPath = Path.Combine($"{Directory.GetCurrentDirectory()}\\Files", "LocalUserInfo.ps1");

                // 创建 PowerShell 进程
                using var powerShell = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                // 启动 PowerShell 进程并读取输出
                powerShell.Start();
                var result = powerShell.StandardOutput.ReadToEnd();
                powerShell.WaitForExit();

                // 检查是否有错误输出
                if (powerShell.ExitCode != 0)
                {
                    var error = powerShell.StandardError.ReadToEnd();
                    throw new Exception($"Error executing PowerShell script: {error}");
                }

                var users = JsonSerializer.Deserialize<IEnumerable<LocalUserInfo>>(result);

                // 返回用户信息集合
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
