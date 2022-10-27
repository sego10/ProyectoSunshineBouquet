using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoSunshineBouquet.Models;

namespace ProyectoSunshineBouquet.Controllers
{
    public class VariedadesController : Controller
    {
        private DBSBProductoEntities1 db = new DBSBProductoEntities1();

        // GET: Variedades
        public ActionResult Index()
        {
            return View(db.Variedad.ToList());
        }

        // GET: Variedades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Variedad variedad = db.Variedad.Find(id);
            if (variedad == null)
            {
                return HttpNotFound();
            }
            return View(variedad);
        }

        // GET: Variedades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Variedades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VariedadId,VariedadNombre,VariedadColor")] Variedad variedad)
        {
            if (ModelState.IsValid)
            {
                db.Variedad.Add(variedad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(variedad);
        }

        // GET: Variedades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Variedad variedad = db.Variedad.Find(id);
            if (variedad == null)
            {
                return HttpNotFound();
            }
            return View(variedad);
        }

        // POST: Variedades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VariedadId,VariedadNombre,VariedadColor")] Variedad variedad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(variedad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(variedad);
        }

        // GET: Variedades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Variedad variedad = db.Variedad.Find(id);
            if (variedad == null)
            {
                return HttpNotFound();
            }
            return View(variedad);
        }

        // POST: Variedades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Variedad variedad = db.Variedad.Find(id);
            db.Variedad.Remove(variedad);
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
