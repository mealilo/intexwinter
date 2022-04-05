using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Models
{
    public class EFAccidents : IAccidents
    {

        private AccidentContext context { get; set; }


        public EFAccidents(AccidentContext temp)
        {
            context = temp;
        }

        public IQueryable<Accident> Accidents => context.Accidents;

        public void Delete(Accident accident)
        {
            context.Remove(accident);
            context.SaveChanges();
        }
    }
}
