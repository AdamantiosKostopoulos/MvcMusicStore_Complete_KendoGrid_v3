using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;
using System.Diagnostics;

namespace MvcMusicStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StoreManagerController : Controller
    {
        //New Model (Replaced Musis Store Entities)
        private NewModel db = new NewModel();
        
        
        //return partial view
        //ok
        public ActionResult KendoGrid()
        {
            return View();
        }

        //CRUD Opertions :

        //Read
        //ok
        public ActionResult Read()
        {            
            return Json(GetAlbums(), JsonRequestBehavior.AllowGet);            
        }      
                   
        public class AlbumDTO
        {
            public int AlbumId { get; set; } 
            public string Artist { get; set; }
            public string Genre { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
        }

        public List<AlbumDTO> GetAlbums()
        {
            return (from a in db.Albums 
                    select new AlbumDTO{ 
                        AlbumId = a.AlbumId,
                        Artist = a.Artist.Name,
                        Genre = a.Genre.Name,
                        Title = a.Title,
                        Price = a.Price
                    }).ToList();
        }
        //         

        //Delete
        //ok
        public ActionResult DeleteAlbum(int id)
        {
           Album album = db.Albums.Find(id); //find album with specific id
           db.Albums.Remove(album); //delete album     
           db.SaveChanges(); // working - move to new button (submit) <<
           //return RedirectToAction("KendoGrid"); //redirect - no
           //test:
           return View();           
        }


        //testing >>>>>
        //Submit 
        public ActionResult Submit(int total, int id, Artist Artist, Genre Genre, string Title, int Price)
        {
            foreach (var entity in db.Albums)
            {
                db.Albums.Remove(entity);                
            }

            //db.SaveChanges();
            
            for (var i = 0; i < total; i++)
            {
                var album = new Album
                {
                    AlbumId = id,
                    Artist = Artist,
                    Genre = Genre,
                    Title = Title,
                    Price = Price
                };

                db.Albums.Add(album);               
            }

            db.SaveChanges();

            return View();

        }        

        //








        //
        // GET: /StoreManager/

        public ViewResult Index()
        {
            var albums = db.Albums.Include(a => a.Genre).Include(a => a.Artist);
            return View(albums.ToList());
        }       


        //
        // GET: /StoreManager/Details/5

        public ViewResult Details(int id)
        {
            Album album = db.Albums.Find(id);
            return View(album);
        }

        //
        // GET: /StoreManager/Create

        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name");
            return View();
        } 

        //
        // POST: /StoreManager/Create

        [HttpPost]
        public ActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }
        
        //
        // GET: /StoreManager/Edit/5
 
        public ActionResult Edit(int id)
        {
            Album album = db.Albums.Find(id);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        public ActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // GET: /StoreManager/Delete/5
 
        public ActionResult Delete(int id)
        {
            Album album = db.Albums.Find(id);
            return View(album);
        }

        //
        // POST: /StoreManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}