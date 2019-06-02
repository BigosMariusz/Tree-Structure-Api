using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Services.Services
{
    public class DataValidator : IDataValidator
    {
        public bool Validate(string text)
        {
            if (text == null || text.Length > 30 )
                return false;

            return true;
        }
    }
}
