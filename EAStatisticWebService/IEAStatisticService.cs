using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WcfJsonRestService
{
    [ServiceContract]
    public interface IEAStatisticService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",ResponseFormat = WebMessageFormat.Json,UriTemplate = "modality_stat.js")]
        ModStatDat[] ModalityStat();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "day_device_stat.js")]
        ModStatDat[] DeviceDayStat();
    }
}
