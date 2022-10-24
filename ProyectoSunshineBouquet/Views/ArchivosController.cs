using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoSunshineBouquet.Models;

namespace ProyectoSunshineBouquet.Views
{
    public class ArchivosController : Controller
    {
        private DBSBProductoEntities1 db = new DBSBProductoEntities1();

        // GET: Archivos
        public ActionResult Index()
        {
            var archivo = db.Archivo.Include(a => a.Producto);
            return View(archivo.ToList());
        }

        // GET: Archivos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivo.Find(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            return View(archivo);
        }

        // GET: Archivos/Create
        public ActionResult Create()
        {
            ViewBag.ProductoId = new SelectList(db.Producto, "ProductoId", "ProductoNombre");
            return View();
        }

        // POST: Archivos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArchivoId,ProductoId,ArchivoNombre,ArchivoContenido,ArchivoExtension")] Archivo archivo)
        {
            if (ModelState.IsValid)
            {
                db.Archivo.Add(archivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductoId = new SelectList(db.Producto, "ProductoId", "ProductoNombre", archivo.ProductoId);
            return View(archivo);
        }

        // GET: Archivos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivo.Find(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductoId = new SelectList(db.Producto, "ProductoId", "ProductoNombre", archivo.ProductoId);
            return View(archivo);
        }

        // POST: Archivos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArchivoId,ProductoId,ArchivoNombre,ArchivoContenido,ArchivoExtension")] Archivo archivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductoId = new SelectList(db.Producto, "ProductoId", "ProductoNombre", archivo.ProductoId);
            return View(archivo);
        }

        // GET: Archivos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivo.Find(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            return View(archivo);
        }

        // POST: Archivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Archivo archivo = db.Archivo.Find(id);
            db.Archivo.Remove(archivo);
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
