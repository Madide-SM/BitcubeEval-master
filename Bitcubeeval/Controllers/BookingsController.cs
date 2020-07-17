
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Bitcubeeval.Models;
using System;

namespace Bitcubeeval.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult ListofBookings()
        {
            return View(db.Bookings.ToList());
        }
        //public ActionResult ListofConfirmedBookings()
        //{
        //    return View(db.Bookings.Where(i => i.BStatus == "Confirmed").ToList());
        //}
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Location);
            return View(bookings.ToList());
        }
        //public ActionResult Confirmed(int? id)
        //{
        //    Booking booking = db.Bookings.Find(id);
        //    string search = "";
        //    search =
        //    Confirm(id);
        //    ViewBag.Search = search;
        //    return View(booking);
        //}
        //public ActionResult ConfirmForUser(int? id)
        //{
        //    Booking booking = db.Bookings.Find(id);
        //    string search = "";
        //    search =
        //    Confirm(id);
        //    ViewBag.Search = search;
        //    Booking p = db.Bookings.ToList().Last();
        //    return View(booking);
        //}

        //public ActionResult Declined(int? id)
        //{
        //    Booking booking = db.Bookings.Find(id);
        //    string search = "";
        //    search =
        //    Decline(id);
        //    ViewBag.Search = search;
        //    return View(booking);
        //}
        //public ActionResult DeclineForUser(int? id)
        //{
        //    Booking booking = db.Bookings.Find(id);
        //    string search = "";
        //    search =
        //    Decline(id);
        //    ViewBag.Search = search;
        //    return View(booking);
        //}
        //public string Confirm(int? BookId)
        //{
        //    Booking a = db.Bookings.Find(BookId);
        //    a.BStatus = "Confirmed";
        //    db.SaveChanges();
        //    return "Booking has been Confirmed";
        //}

        //public string Decline(int? BookId)
        //{
        //    Booking a = db.Bookings.Find(BookId);
        //    a.BStatus = "Declined";
        //    db.SaveChanges();
        //    return "Booking has been Declined. Please book another picnic";
        //}
        //public ActionResult ViewStatus()
        //{
        //    string m = HttpContext.User.Identity.Name;
        //    List<Booking> a = db.Bookings.ToList().FindAll(p => p.MemberEmail == m);
        //    foreach (var item in a)
        //    {
        //        if (item.BStatus == "Confirmed")
        //        {
        //            ViewBag.c = "Confirmed";
        //        }
        //        if (item.BStatus == "Declined")
        //        {
        //            ViewBag.d = "Declined";
        //        }
        //        if (item.BStatus == "Not Yet Confirmed")
        //        {
        //            ViewBag.n = "Not Yet Confirmed";
        //        }
        //    }
        //    return View(a);

        //}
        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,LocationId,EventDate,StartTime,BookingDiscount,DepositFee,NumOfPeople,TotalCost,UserId,EndTime,BStatus,SelectedFruits,SelectedItems")] Booking booking)
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", booking.LocationId);
            var currentBooking = db.Bookings
                .Where(b => b.LocationId == booking.LocationId 
                 && b.EventDate == booking.EventDate 
                 && booking.StartTime < b.StartTime).FirstOrDefault();
            if (currentBooking != null)
            {
             ViewBag.Error = "Venue Not Available for ";
            }

            else if (currentBooking == null)
            {
                //booking.BStatus = "Not Yet Confirmed";
                if (ModelState.IsValid)
                {
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    booking.FoodItems = db.FoodItems.ToList();
                    return RedirectToAction("AddFoodItems", new { id = booking.BookingId });
                }

                ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", booking.LocationId);
                return View();
            }
            return View();
        }

        public ActionResult AddFoodItems(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            booking.FoodItems = db.FoodItems.ToList();
            //var fooditems = string.Join(",", booking.SelectedFruits);
            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFoodItems(Booking b)
        {
            if (ModelState.IsValid)
            {
                var fooditems = string.Join(",", b.SelectedFruits);
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddItems", new { id = b.BookingId });
            }
            return View(b);
        }
        public ActionResult AddItems(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            booking.Items = db.Items.ToList();
            //var SelectedItems = string.Join(",", booking.SelectedItems);
            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItems(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                var items = string.Join(",", booking.Items);
                booking.UserId = User.Identity.GetUserId();
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ConfirmBooking", new { id = booking.BookingId });
            }
            return Content("Error 404");
        }

        public ActionResult ConfirmBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            booking.Items = db.Items.ToList();
            return View(booking);

        }
        [HttpPost, ActionName("ConfirmBooking")]
        public ActionResult ConfirmBooking(Booking b)
        {
            b.UserId = User.Identity.GetUserId();
            db.Entry(b).State = EntityState.Modified;
            db.SaveChanges();
            return View(b);

        }
        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", booking.LocationId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,LocationId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", booking.LocationId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
