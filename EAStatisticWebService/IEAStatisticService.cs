using System;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace EAStatistic
{
    [ServiceContract]
    public interface IEAStatisticService
    {
        [OperationContract]
        ModStatDat[] ModalityStat(string from, string to);

        [OperationContract]
        ModStatDat[] DeviceDayStat(string from, string to);
        [OperationContract]
        DiskStat[] DiskStat();
    }
}
