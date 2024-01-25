using System.Collections.Generic;

namespace Application.Common.ViewModels
{
    public class ListVm<T>
    {
        public ListVm()
        {
            Data = new List<T>();
        }

        public List<T> Data { get; set; }
    }
}