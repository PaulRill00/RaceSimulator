using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class DriverBroken : IData<DriverBroken>
    {
        public String Name { get; set; }
        public int Count { get; set; } = 1;
        
        public void Add(List<DriverBroken> data)
        {
            DriverBroken match = data.Find(d => d.Name.Equals(Name));
            if (match != null)
                match.Count++;
            else
                data.Add(this);
        }

        public string GetBestParticipant(List<DriverBroken> data)
        {
            var res = data.First(x => x.Count == data.Min(y => y.Count));
            return (res == null) ? "" : res.Name;
        }
    }
}
