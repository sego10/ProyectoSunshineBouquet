using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoSunshineBouquet.Models;

namespace ProyectoSunshineBouquet.Controllers
{
    public class ProductosController : Controller
    {
        private DBSBProductoEntities1 db = new DBSBProductoEntities1();

        // GET: Productos
        public ActionResult Index()
        {
            return View(db.Producto.ToList());
        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult MantenimientoProducto()
        {
            return View("MantenimientoProducto");
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoId,ProductoNombre,ProductoEspecie")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Producto.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MantenimientoProducto([Bind(Include = "ProductoId,ProductoCodigo,ProductoNombre,ProductoEspecie")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Producto.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        //codigo para previsualizar la imagen
        [HttpPost]
        public ActionResult SendImage(HttpPostedFileBase img, string base64, string contenttype)
        {
            if (base64 != null && contenttype != null && img == null)
            {
                // Aquí podríamos guardar la imagen (en base64 tenemos los datos)
                return View("Step2");
            }
            var data = new byte[img.ContentLength];
            img.InputStream.Read(data, 0, img.ContentLength);
            var base64Data = Convert.ToBase64String(data);
            ViewBag.ImageData = base64Data;
            ViewBag.ImageContentType = img.ContentType;
            return View("Index");
        }

        public ActionResult Preview(string file)
        {
            var path = ControllerContext.HttpContext.Server.MapPath("/");
            if (!string.IsNullOrEmpty(file))
            {
                if (System.IO.File.Exists(Path.Combine(path, file)))
            {
                return File(Path.Combine(path, file), "image/jpeg");
            }
            }

            return new HttpNotFoundResult();
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoId,ProductoNombre,ProductoEspecie")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            db.Producto.Remove(producto);
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
