﻿using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repository;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
namespace DataAccessLayer.EntityFramework
{
    public class EfTicketDal : GenericRepository<Ticket>, ITicketDal
    {
        public List<Ticket> GetTicketsWithEvents()
        {
            using (var context = new Context())
            {
                return context.Tickets
                    .Include(t => t.Event)
                    .ThenInclude(e => e.Artist) // Örnek olarak Artist'e erişim
                    .ToList();

            }
        }

    }
}