using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class RaceInfo<T>
    {
        private List<T> _list = new List<T>();

        public void AddToList(T t1)
        {
            _list.Add(t1);
        }

        public List<T> GetList()
        {
            return _list;
        }
    }
}
