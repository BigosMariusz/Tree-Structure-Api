using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IDataValidator
    {
        bool Validate(string text);
    }
}
