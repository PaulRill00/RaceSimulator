using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class RaceInfo<T> where T : IData<T>
    {
        private List<T> _list = new List<T>();

        public void AddToList(T t1)
        {
            t1.Add(_list);
        }

        public List<T> GetList()
        {
            return _list;
        }

        public string GetBestParticipant()
        {
            return _list.First().GetBestParticipant(_list);
        }
    }
}
