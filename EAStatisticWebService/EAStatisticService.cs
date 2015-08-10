using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Web;

namespace WcfJsonRestService
{
    public class IntWal
    {
        public int v;
    }

    public class ModStatDat
    {
      public  string name;
      public IEnumerable data;
    }
    public class EAStatisticService : IEAStatisticService
    {
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "day_modality.js")]
        public ModStatDat[] DayModality()
        {
            //разрешает доступ для всех доменов; если надо разрешить определенные домены, то передаем их имена через запятую во 2 параметр вместо *
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            //разрешает выполнение GET запроса; если надо еще POST, то опять же через запятую добавляем во 2 параметр
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET");
            //определяет необходимо ли требовать передачу credentials при выполнении данного метода; в моем случае они не нужны
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Credentials", "false");
                    using (var dm = new DCMARCH1Entities())
            {
                //.Where(r => r.C00080020.Value.Date == DateTime.Today)
                var ret = new ModStatDat[2];
                ret[0]=new ModStatDat()
                {
                    name = "Modality",
                    data = dm.tblDICOMStudy.GroupBy(p => p.C00080061).Select(g => g.Key).ToArray()
                };
                ret[1] = new ModStatDat()
                {
                    name = "Modality",
                    data = dm.tblDICOMStudy.GroupBy(p => p.C00080061).Select(g =>  g.Count()).ToArray()
                };

                return ret;
            };
        }
    }
 
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
