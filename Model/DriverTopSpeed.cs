using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Model
{
    public class DriverTopSpeed : IData<DriverTopSpeed>
    {
        public string Name { get; set; }
        public int Speed { get; set; }

        public void Add(List<DriverTopSpeed> data)
        {
            DriverTopSpeed match = data.Find(d => d.Name.Equals(Name));
            if (match != null)
            {
                if (match.Speed < Speed)
                    match.Speed = Speed;
            }
            else
                data.Add(this);
        }

        public string GetBestParticipant(List<DriverTopSpeed> data)
        {
            DriverTopSpeed res = data.First(x => x.Speed == data.Max(y => y.Speed));
            return (res == null) ? "" : res.Name;
        }
    }
}
