using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U3RazasPerros.Areas.Admin.Models;
using U3RazasPerros.Models;

namespace U3RazasPerros.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class RazasController : Controller
    {
        public perrosContext Context { get; }
        public RazasController(perrosContext context)
        {
            Context = context;
        }
        public IActionResult Index()
        {
            var p = Context.Razas.OrderBy(x => x.Nombre);
            return View(p);
        }
        [HttpGet]
        public IActionResult Agregar()
        {
            AgregarViewModel e = new AgregarViewModel();
            e.Paises = Context.Paises.OrderBy(x => x.Nombre);
            return View(e);
        }
        [HttpPost]
        public IActionResult Agregar(AgregarViewModel c)
        {
            if (c.Caracteristicasfisicas.Id!=c.Razas.Id)
            {
                ModelState.AddModelError("", "Error");
            }
            if (string.IsNullOrWhiteSpace(c.Razas.Nombre) ||
                string.IsNullOrWhiteSpace(c.Razas.Descripcion) ||
                string.IsNullOrWhiteSpace(c.Razas.OtrosNombres) ||
                string.IsNullOrWhiteSpace(c.Caracteristicasfisicas.Cola) ||
                string.IsNullOrWhiteSpace(c.Caracteristicasfisicas.Color) ||
                string.IsNullOrWhiteSpace(c.Caracteristicasfisicas.Patas) ||
                string.IsNullOrWhiteSpace(c.Caracteristicasfisicas.Pelo) ||
                string.IsNullOrWhiteSpace(c.Caracteristicasfisicas.Hocico) ||
                c.Razas.AlturaMax == 0 ||
                c.Razas.PesoMax == 0 ||
                c.Razas.AlturaMin == 0 ||
                c.Razas.PesoMin == 0 ||
                c.Razas.IdPais == 0 ||
                c.Razas.EsperanzaVida == 0)
            {
                ModelState.AddModelError("", "Favor de llenar todos los campos");
            }
            if (Context.Razas.Any(x=>x.Nombre==c.Razas.Nombre))
            {
                ModelState.AddModelError("", "Raza ya incluida");
            }
            else
            {
                Context.Add(c.Razas);
                Context.Add(c.Caracteristicasfisicas);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Editar(int id)
        {
            AgregarViewModel vm = new AgregarViewModel();
            var r = Context.Razas.FirstOrDefault(x => x.Id == id);
            if (r==null)
            {
                return RedirectToAction("Index");
            }
            var c = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == id);
            vm.Razas = r;
            vm.Caracteristicasfisicas = c;
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(Razas c, Caracteristicasfisicas b)
        {
           
            var car = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == b.Id);

            var ras = Context.Razas.FirstOrDefault(x => x.Id == c.Id);

            if (ras == null||car==null)
            {
                ModelState.AddModelError("", "Error no se encuentra la raza");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(c.Nombre) ||
                string.IsNullOrWhiteSpace(c.Descripcion) ||
                string.IsNullOrWhiteSpace(c.OtrosNombres) ||
                string.IsNullOrWhiteSpace(b.Cola) ||
                string.IsNullOrWhiteSpace(b.Color) ||
                string.IsNullOrWhiteSpace(b.Patas) ||
                string.IsNullOrWhiteSpace(b.Pelo) ||
                string.IsNullOrWhiteSpace(b.Hocico) ||
                c.AlturaMax == 0 ||
                c.PesoMax == 0 ||
                c.AlturaMin == 0 ||
                c.PesoMin == 0 ||
                c.IdPais == 0 ||
                c.EsperanzaVida == 0
                )
                {
                    ModelState.AddModelError("", "Favor de llenar todos los campos");
                }
                if (Context.Razas.Any(x => x.Nombre == c.Nombre && x.Id != c.Id))
                {
                    ModelState.AddModelError("", "Raza ya incluida");
                }

                ras.Nombre = c.Nombre;
                ras.Descripcion = c.Descripcion;
                ras.AlturaMax = c.AlturaMax;
                ras.AlturaMin = c.AlturaMin;
                ras.EsperanzaVida = c.EsperanzaVida;
                ras.OtrosNombres = c.OtrosNombres;
                ras.PesoMax = c.PesoMax;
                ras.PesoMin = c.PesoMin;
                ras.Caracteristicasfisicas.Cola = b.Cola;
                ras.Caracteristicasfisicas.Color = b.Color;
                ras.Caracteristicasfisicas.Patas = b.Patas;
                ras.Caracteristicasfisicas.Pelo = b.Pelo;
                ras.Caracteristicasfisicas.Hocico = b.Hocico;
                

                Context.Update(ras);
                Context.SaveChanges();

                return RedirectToAction("Index");
            }

       

            return View(ras);
            
        }
        [HttpGet]
        public IActionResult Eliminar(int id, int od)
        {
            var cat = Context.Razas.FirstOrDefault(x => x.Id == id);
            var car = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == od);
            if (cat==null )
            {
                return RedirectToAction("Index");
            }
            
            return View(cat);
        }
        [HttpPost]
        public IActionResult Eliminar(Razas razas,Caracteristicasfisicas car)
        {
            var ras = Context.Razas.FirstOrDefault(x => x.Id == razas.Id);
            var d = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == car.Id);
            if (ras==null||d==null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                
                Context.Remove(d);
                Context.Remove(ras);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
    }
}
