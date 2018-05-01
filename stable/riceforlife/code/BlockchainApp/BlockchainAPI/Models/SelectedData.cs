using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Models
{
    public class SelectedData<T>
    {
        public T data { get; set; }
        public bool selected { get; set; }
    }
}
