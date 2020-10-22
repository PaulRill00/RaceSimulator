using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class DriverTime : IData<DriverTime>
    {
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public Section Section { get; set; }

        public void Add(List<DriverTime> data)
        {
            DriverTime match = (DriverTime)data.Find(d => d.Name.Equals(Name));
            if (match != null)
                match.Time = Time;
            else
                data.Add(this);
        }

        public string GetBestParticipant(List<DriverTime> data)
        {
            var res = data.First(x => x.Time == data.Min(y => y.Time));
            return (res == null) ? "" : res.Name;
        }
    }
}
