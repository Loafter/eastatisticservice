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
        ModStatDat[] DayModality();
    }
}
