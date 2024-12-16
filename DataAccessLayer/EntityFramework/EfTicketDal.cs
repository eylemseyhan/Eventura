using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repository;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.EntityFramework
{
    public class EfTicketDal : GenericRepository<Ticket>, ITicketDal
    {
        public void AddRange(IEnumerable<Ticket> tickets)
        {
            using var c = new Context();
            c.Set<Ticket>().AddRange(tickets);
            c.SaveChanges();
        }

        public void DeleteRange(List<Ticket> tickets)
        {
            using var c = new Context();
            c.Tickets.RemoveRange(tickets);
            c.SaveChanges();
        }


    }
}
