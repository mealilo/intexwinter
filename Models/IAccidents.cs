using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Models
{
    public interface IAccidents
    {

        IQueryable<Accident> Accidents { get; }
    }
}
