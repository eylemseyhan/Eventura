using DataAccessLayer.Concrete;
using EventsProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class _UILayoutPopulerEventsComponent : ViewComponent
{
    private readonly Context _context;

    public _UILayoutPopulerEventsComponent(Context context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var populerEvents = _context.UserFavorites
            .GroupBy(uf => uf.EventId)
            .Select(group => new
            {
                EventId = group.Key,
                FavoriteCount = group.Count()
            })
            .OrderByDescending(x => x.FavoriteCount)
            .Take(5)
            .ToList();

        var eventIds = populerEvents.Select(x => x.EventId).ToList();

        var events = _context.Events
            .Where(e => eventIds.Contains(e.EventId))
            .ToList();

        //var eventPrices = _context.EventsTickets
        //    .Where(et => eventIds.Contains(et.EventId))
        //    .GroupBy(et => et.EventId)
        //    .Select(group => new
        //    {
        //        EventId = group.Key,
        //        Price = group.FirstOrDefault() == null ? 0 : group.FirstOrDefault().Price
        //    })
        //    .ToList();

        var eventsWithPrices = _context.Events
                                    .Select(e => new EventWithPriceViewModel
                                    {
                                        EventId = e.EventId,
                                        EventName = e.Title,
                                        EventImageUrl = e.ImageUrl,
                                        CategoryName = e.Category.Name,
                                        Price = _context.EventsTickets
                                                         .Where(et => et.EventId == e.EventId)
                                                         .Select(et => et.Price)
                                                         .FirstOrDefault() // İlk kaydı alır
                                    }).Take(5)
                                    .ToList();

        return View(eventsWithPrices);
    }

}
