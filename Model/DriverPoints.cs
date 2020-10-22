using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class DriverPoints : IData<DriverPoints>
    {
        public string Name { get; set; }
        public int Points { get; set; }

        public void Add(List<DriverPoints> data)
        {
            DriverPoints match = data.Find(d => d.Name.Equals(Name));
            if (match != null)
                match.Points += Points;
            else
                data.Add(this);
        }

        public string GetBestParticipant(List<DriverPoints> data)
        {
            var res = data.First(x => x.Points == data.Max(y => y.Points));
            return (res == null) ? "" : res.Name;
        }
    }
}