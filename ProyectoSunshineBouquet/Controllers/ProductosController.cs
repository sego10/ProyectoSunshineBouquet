using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
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
        public ActionResult MantenimientoProducto([Bind(Include = "ProductoId,ProductoCodigo,ProductoNombre,ProductoEspecie,ProductoImagen,ProductoImgExt")] Producto producto)
        {
            HttpPostedFileBase FileBase = Request.Files[0];


            if (FileBase.ContentLength == 0)
            {
                ModelState.AddModelError("Imagen", "Debe seleccionar una imagen.");
            }
            else
            {
                if (FileBase.FileName.EndsWith(".jpg") || FileBase.FileName.EndsWith(".png"))
                {
                    WebImage image = new WebImage(FileBase.InputStream);
                    producto.ProductoImagen = image.GetBytes();
                    producto.ProductoImgExt = image.ImageFormat.ToString();
                }
                else
                {
                    ModelState.AddModelError("Imagen", "La imagen debe ser .jpg o .png");
                }
            }
           
                       

            if (ModelState.IsValid)
            {
                try
                {
                    db.Producto.Add(producto);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(String.Empty, "Error: "+e.Message); 
                }
                
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
        public ActionResult EditarProducto(int? id)
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
        public ActionResult Edit([Bind(Include = "ProductoId,ProductoCodigo,ProductoNombre,ProductoEspecie,ProductoImagen,ProductoImgExt")] Producto producto)
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProducto([Bind(Include = "ProductoId,ProductoCodigo,ProductoNombre,ProductoEspecie,ProductoImagen,ProductoImgExt")] Producto producto)
        {
            //byte[] imagenActual = null;
            HttpPostedFileBase FileBase = Request.Files[0];

           Producto _producto = new Producto(); 

            if (FileBase.ContentLength == 0)
            {
                _producto = db.Producto.Find(producto.ProductoId);
                producto.ProductoImagen = _producto.ProductoImagen;
                producto.ProductoImgExt = _producto.ProductoImgExt;
            }
            else
            {
                if (FileBase.FileName.EndsWith(".jpg") || FileBase.FileName.EndsWith(".png"))
                {
                    WebImage image = new WebImage(FileBase.InputStream);
                    producto.ProductoImagen = image.GetBytes();
                    producto.ProductoImgExt = image.ImageFormat.ToString();
                }
                else
                {
                    ModelState.AddModelError("Imagen", "La imagen debe ser .jpg o .png");
                }
               
            }
            if (ModelState.IsValid)
            {
                    try
                {
               
                        db.Entry(_producto).State = EntityState.Detached;
                        db.Entry(producto).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                
                }
                catch (Exception e)
                {

                    ModelState.AddModelError(String.Empty, "Error: " + e.InnerException.InnerException);
                }
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

        public ActionResult getImage(int id)
        {
            Producto producto = db.Producto.Find(id);
            byte[] byteImage = producto.ProductoImagen;

            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image image = Image.FromStream(memoryStream);

            memoryStream = new MemoryStream();
            if (!string.IsNullOrEmpty(producto.ProductoImgExt))
            {
                switch (producto.ProductoImgExt.ToUpper())
                {
                    case "PNG":
                        image.Save(memoryStream, ImageFormat.Png);
                        break;
                    case "JPEG":
                        image.Save(memoryStream, ImageFormat.Jpeg);
                        break;
                    case "JPG":
                        image.Save(memoryStream, ImageFormat.Jpeg);
                        break;
                    case "TIFF":
                        image.Save(memoryStream, ImageFormat.Tiff);
                        break;
                    case "BMP":
                        image.Save(memoryStream, ImageFormat.Bmp);
                        break;
                    case "GIF":
                        image.Save(memoryStream, ImageFormat.Gif);
                        break;

                    default:
                        image.Save(memoryStream, ImageFormat.Png);
                        break;
                }
            }
            memoryStream.Position = 0;


            return File(memoryStream,"image/"+producto.ProductoImgExt);

        }
    }
}
