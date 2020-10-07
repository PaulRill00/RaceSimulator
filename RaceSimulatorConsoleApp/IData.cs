using System.Collections.Generic;

namespace Model
{
    public interface IData<T>
    {
        public string Name { get; set; }

        public void Add(List<T> data);

        public string GetBestParticipant(List<T> data);
    }
}
