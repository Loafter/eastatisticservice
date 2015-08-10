using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Web;

namespace WcfJsonRestService
{
    public class ModStatDat
    {
        public string name;
        public IEnumerable data;
    }
    public class EAStatisticService : IEAStatisticService
    {

        public void setCors()
        {
            //разрешает доступ для всех доменов; если надо разрешить определенные домены, то передаем их имена через запятую во 2 параметр вместо *
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            //разрешает выполнение GET запроса; если надо еще POST, то опять же через запятую добавляем во 2 параметр
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET");
            //определяет необходимо ли требовать передачу credentials при выполнении данного метода; в моем случае они не нужны
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Credentials", "false");
        }

        public ModStatDat[] ModalityStat()
        {

            this.setCors();
            using (var dm = new DCMARCH1Entities())
            {
                //.Where(r => r.C00080020.Value.Date == DateTime.Today)
                var ret = new ModStatDat[2];
                var req = dm.tblDICOMStudy.GroupBy(p => p.C00080061);
                ret[0] = new ModStatDat()
                {
                    name = "Modality",
                    data = req.Select(g => g.Key).ToArray()
                };
                ret[1] = new ModStatDat()
                {
                    name = "Count",
                    data = req.Select(g => g.Count()).ToArray()
                };
                return ret;
            };
        }
        public ModStatDat[] DeviceDayStat()
        {
            this.setCors();
            using (var dm = new DCMARCH1Entities())
            {
                //.Where(r => r.C00080020.Value.Date == DateTime.Today)
                var ret = new ModStatDat[2];
                var req = dm.tblDICOMStudy.Where(e=>(e.C00080020==DateTime.Today)).GroupBy(p => p.C00101000);
                ret[0] = new ModStatDat()
                {
                    name = "Modality",
                    data = req.Select(g => g.Key).ToArray()
                };
                ret[1] = new ModStatDat()
                {
                    name = "Count",
                    data = req.Select(g => g.Count()).ToArray()
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
