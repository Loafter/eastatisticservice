using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace EAStatistic
{
    public class ModStatDat
    {
        public string name;
        public IEnumerable data;
    }


    public class DiskStat
    {
        public string name;
        public long data;
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

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "modality_stat_{from}_{to}.js")]
        public ModStatDat[] ModalityStat(string from, string to)
        {
            this.setCors();
            var f = DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var t = DateTime.ParseExact(to, "yyyy-MM-dd" ,CultureInfo.InvariantCulture);
            using (var dm = new EAStatistic.DCMARCH1Entities())
            {
                //.Where(r => r.C00080020.Value.Date == DateTime.Today)
                var req = dm.tblDICOMStudy.Where(r => ((r.C00080020.Value>f) &&(r.C00080020.Value < t))).GroupBy(p => p.C00080061);
                var ret = new ModStatDat[2];
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

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "device_stat_{from}_{to}.js")]
        public ModStatDat[] DeviceDayStat(string from, string to)
        {
            this.setCors();
            var f = DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var t = DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            using (var dm = new EAStatistic.DCMARCH1Entities())
            {
                //.Where(r => r.C00080020.Value.Date == DateTime.Today)
                var req = dm.tblDICOMStudy.Where(r =>( (r.C00080020.Value > f) && (r.C00080020.Value < t))).GroupBy(p => p.C00081010);
                var ret = new ModStatDat[2];
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
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "disk_stat.js")]
        public DiskStat[] DiskStat()
        {
            this.setCors();
            return DriveInfo.GetDrives().Select(v => new DiskStat
            {
                data = v.TotalFreeSpace*100/v.TotalSize,
                name = v.Name
            }).ToArray();

        }
    }
}
