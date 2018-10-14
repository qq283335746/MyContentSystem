using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace TygaSoft.TaskWS.Afd
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "TygaSoft.Afd.Service";
            service.Description = "后台服务：为阿凡达物流提供（队列、百度地图API）后台运行支持！技术支持：天涯孤岸，QQ283335746";
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
