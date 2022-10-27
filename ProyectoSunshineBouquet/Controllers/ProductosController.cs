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
                if (FileBase.FileName.EndsWith(".jpg") || FileBase.FileName.EndsWith(".png") || FileBase.FileName.EndsWith(".png") || FileBase.FileName.EndsWith(".PNG") || FileBase.FileName.EndsWith(".JPG"))
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
                    producto.Insertar(l_indices, l_indicesVar);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(String.Empty, "Error: Id ya existe, porvafor ingrese un valor diferente."); 
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
                    Producto prod = new Producto();
                    prod.Actualizar(producto.ProductoId, l_indices, l_indicesVar);
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
            Producto prod = new Producto();
            var res = prod.Eliminar(id);
            //Producto producto = db.Producto.Find(id);
            //db.Producto.Remove(producto);
            //db.SaveChanges();
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

        // GET: Lista de Grados

        private static List<string> l_indices = new List<string>();
        private static List<string> l_grados = new List<string>();
        
        public ActionResult Modal_ListaGrados()
        {
            return View(db.Grado.ToList());
        }
        
        public string agr_atr(string id, string nom)
        {
            string res = "";
            int cont = 0;
            foreach (var w in l_indices)
            {
                if (w.Equals(id))
                {
                    cont++;
                }
            }
            if (cont == 0)
            {
                if (l_grados.Count < 8)
                {
                    string boton_bor = "<button class=\"btn btn-danger\" type='button'"
                        + " onclick=\"bor_atr('" + id + "')\""
                        + "><span class=\"glyphicon glyphicon-trash\"> Borrar</span></button>"; //--Esta variable boton_bor representa un boton
                    l_grados.Add(
                        "<tr><td>" + id + "</td>"
                        + "<td>" + nom + "</td>"
                        + "<td>" + boton_bor + "</td></tr>"
                        );
                    l_indices.Add(id);
                }
            }
            foreach (var a in l_grados)
            {
                res = res + a;
            }
            return res;
        }

        //--------busqueda
        [HttpPost]
        public String bus_atr(string dato_bus)
        {
            string res = "";
            var grados = new List<Grado>();

            Producto prod = new Producto();

            grados = prod.bus_atr(dato_bus);
            foreach (var a in grados)
            {
                int id = a.GradoId;                
                string nom = a.GradoNombre;              
                string boton_sel = "<button class=\"btn btn-warning\" type='button'"
                    + " onclick=\"agr_atr('" + id + "','" + nom + "')\""
                    + " data-dismiss='modal'><span class=\"glyphicon glyphicon-check\"> Añadir</span></button>";
                res = res +
                   "<tr><td>" + id + "</td>"
                    + "<td>" + nom + "</td>"
                    + "<td>" + boton_sel + "</td></tr>";
            }
            return res;
        }
        //--------------Limpieza--------------------
        [HttpPost]
        public void limpiar_atr()
        {
            l_grados.Clear();
            l_indices.Clear();
        }

        //-------------- Borrar de Lista -----------
        public String bor_atr(string id)
        {
            string res = "";
            l_grados.RemoveAt(l_indices.IndexOf(id));
            l_indices.RemoveAt(l_indices.IndexOf(id));
            foreach (var a in l_grados)
            {
                res = res + a;
            }
            return res;
        }

        // GET: Lista de Variedades

        private static List<string> l_indicesVar = new List<string>();
        private static List<string> l_variedades = new List<string>();

        public ActionResult Modal_ListaVariedades()
        {
            return View(db.Variedad.ToList());
        }

        public string agr_variedad(string id, string variedad, string color)
        {
            string res = "";
            int cont = 0;
            foreach (var w in l_indicesVar)
            {
                if (w.Equals(id))
                {
                    cont++;
                }
            }
            if (cont == 0)
            {
                if (l_variedades.Count < 8)
                {
                    string boton_bor = "<button class=\"btn btn-danger\" type='button'"
                        + " onclick=\"bor_atr('" + id + "')\""
                        + "><span class=\"glyphicon glyphicon-trash\"> Borrar</span></button>"; //--Esta variable boton_bor representa un boton
                    l_variedades.Add(
                        "<tr><td>" + id + "</td>"
                        + "<td>" + variedad + "</td>"
                        + "<td>" + color + "</td>"
                        + "<td>" + boton_bor + "</td></tr>"
                        );
                    l_indicesVar.Add(id);
                }
            }
            foreach (var a in l_variedades)
            {
                res = res + a;
            }
            return res;
        }

        //--------busqueda
        [HttpPost]
        public String bus_variedad(string dato_bus)
        {
            string res = "";
            var variedad = new List<Variedad>();

            Producto prod = new Producto();

            variedad = prod.bus_variedad(dato_bus);
            foreach (var a in variedad)
            {
                int id = a.VariedadId;
                string nom = a.VariedadNombre;
                string color = a.VariedadColor;
                string boton_sel = "<button class=\"btn btn-warning\" type='button'"
                    + " onclick=\"agr_variedad('" + id + "','" + nom + "','" + color + "')\""
                    + " data-dismiss='modal'><span class=\"glyphicon glyphicon-check\"> Añadir</span></button>";
                res = res +
                   "<tr><td>" + id + "</td>"
                    + "<td>" + nom + "</td>"
                    + "<td>" + color + "</td>"
                    + "<td>" + boton_sel + "</td></tr>";
            }
            return res;
        }
        //--------------Limpieza--------------------
        [HttpPost]
        public void limpiar_variedad()
        {
            l_variedades.Clear();
            l_indicesVar.Clear();
        }

        //-------------- Borrar Alumno de Lista -----------
        public String bor_variedad(string id)
        {
            string res = "";
            l_variedades.RemoveAt(l_indicesVar.IndexOf(id));
            l_indicesVar.RemoveAt(l_indicesVar.IndexOf(id));
            foreach (var a in l_variedades)
            {
                res = res + a;
            }
            return res;
        }

        //----Detalle Actualizar

        public String det_actualizar_sec(string id)
        {
            string res = "";
            var grado = new List<Grado>();
            Producto producto = new Producto();
            grado = producto.det_sec(id);
            foreach (var a in grado)
            {
                string boton_bor = "<button class=\"btn btn-danger\" type='button'"
                        + " onclick=\"bor_atr('" + a.GradoId + "')\""
                        + "><span class=\"glyphicon glyphicon-trash\"> Borrar</span></button>";
                l_grados.Add(
                        "<tr><td>" + a.GradoId + "</td>"
                        + "<td>" + a.GradoNombre + "</td>"
                        + "<td>" + boton_bor + "</td></tr>"
                        );
                l_indices.Add(a.GradoId.ToString());
            }
            foreach (var a in l_grados)
            {
                res = res + a;
            }
            return res;
        }

        public String det_actualizar_variedad(string id)
        {
            string res = "";
            var variedad = new List<Variedad>();
            Producto producto = new Producto();
            variedad = producto.det_variedad(id);
            foreach (var a in variedad)
            {
                string boton_bor = "<button class=\"btn btn-danger\" type='button'"
                        + " onclick=\"bor_variedad('" + a.VariedadId + "')\""
                        + "><span class=\"glyphicon glyphicon-trash\"> Borrar</span></button>";
                l_variedades.Add(
                        "<tr><td>" + a.VariedadId + "</td>"
                        + "<td>" + a.VariedadNombre + "</td>"
                        + "<td>" + a.VariedadColor + "</td>"
                        + "<td>" + boton_bor + "</td></tr>"
                        );
                l_indicesVar.Add(a.VariedadId.ToString());
            }
            foreach (var a in l_variedades)
            {
                res = res + a;
            }
            return res;
        }


        //------------ Detalle Seccion ----
        [HttpPost]
        public String detalle_grado(string id)
        {
            l_grados.Clear();
            l_indices.Clear();
            string res = "";
            var grado = new List<Grado>();
            Producto producto = new Producto();
            grado = producto.det_sec(id);
            foreach (var a in grado)
            {
                l_grados.Add(
                     "<tr><td>" + a.GradoId + "</td>"
                    + "<td>" + a.GradoNombre + "</td></tr>"
                       );
                l_indices.Add(a.GradoId.ToString());
            }
            foreach (var a in l_grados)
            {
                res = res + a;
            }
            return res;
        }

        [HttpPost]
        public String detalle_variedad(string id)
        {
            l_variedades.Clear();
            l_indicesVar.Clear();
            string res = "";
            var variedad = new List<Variedad>();
            Producto producto = new Producto();
            variedad = producto.det_variedad(id);
            foreach (var a in variedad)
            {
                l_variedades.Add(
                     "<tr><td>" + a.VariedadId + "</td>"
                    + "<td>" + a.VariedadNombre + "</td>"
                    + "<td>" + a.VariedadColor + "</td></tr>"
                       );
                l_indicesVar.Add(a.VariedadId.ToString());
            }
            foreach (var a in l_variedades)
            {
                res = res + a;
            }
            return res;
        }

        [HttpPost]
        public ActionResult bus_sec(string tipo_bus, string dato_bus_sec)
        {
            
            if (dato_bus_sec == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var producto = new List<Producto>();
            producto = db.Producto.Where(p => p.ProductoCodigo == dato_bus_sec).ToList();
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        //----------------------------Busqueda General-------------------
        //[HttpPost]
        //public String bus_sec(string dato_bus_sec)
        //{
        //    string res = "";
        //    string tipo = "";

        //    var lista = new List<Producto>();
        //    Producto prod = new Producto();
        //    lista = prod.bus_sec(dato_bus_sec);
        //    var cur = new RegistroSeccion.Models.Model1().Curso.ToList();
        //    foreach (var l in lista)
        //    {
        //        string boton1 = "<button class='btn btn-secondary' type='button' "
        //                    + "data-target='#modal_detalle_seccion' data-toggle='modal' "
        //                    + "data-backdrop='static' data-keyboard='false' "
        //                    + "onclick=\"detalle_seccion('" + l.id_sec + "')\"><span class=\"glyphicon glyphicon-eye-open\"> Detalle Seccion</span></button>";

        //        string boton2 = "<button class='btn btn-warning' type='button' id='btn_act' "
        //                    + "name='btn_act' onclick=\"location.href='../Seccion/Actualizar?id=" + l.id_sec + "'\">"
        //                    + "<span class=\"glyphicon glyphicon-edit\"> Actualizar</span></button>";

        //        string boton3 = "<button class='btn btn-danger' type='button' id='btn_eli' name='btn_eli' "
        //                    + "onclick=\"location.href='../Seccion/Eliminar?id=" + l.id_sec + "'\"><span class=\"glyphicon glyphicon-trash\"> Eliminar</span></button>";
        //        string estado = "Activo";
        //        if (l.estado_sec.Equals(false))
        //        {
        //            estado = "No " + estado;
        //        }
        //        foreach (var c in cur)
        //        {
        //            if (l.id_cur == c.id_cur)
        //            {
        //                res = res +
        //            "<tr><td>" + l.id_sec + "</td>"
        //            + "<td>" + l.aula_sec + "</td>"
        //            + "<td>" + c.descripcion_cur + "</td>"
        //            + "<td>" + estado + "</td>"
        //            + "<td>" + l.fecha_registro_sec.ToString("dd/MM/yyyy") + "</td>"
        //            + "<td>" + boton1 + " " + boton2 + " " + boton3 + "</td></tr>";
        //            }
        //        }

        //    }
        //    return res;
        //}

    }
}
