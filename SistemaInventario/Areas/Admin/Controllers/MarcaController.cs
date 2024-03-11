using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MarcaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public MarcaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Marca Marca = new Marca();

            if (id == null)
            {
                //Crear una nueva Marca
                Marca.Estado = true;
                return View(Marca);
            }
            Marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
            if (Marca == null)
            {
                return NotFound();
            }
            return View(Marca);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Marca Marca)
        {
            if (ModelState.IsValid)
            {
                if(Marca.Id == 0)
                {
                    await _unidadTrabajo.Marca.Agregar(Marca);
                    TempData[DS.Exitosa] = "Marca creada exitosamente";
                }else
                {
                    _unidadTrabajo.Marca.Actualizar(Marca);
                    TempData[DS.Exitosa] = "Marca actualizada exitosamente";
                }            
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Marca";
            return View(Marca);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Marca.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var MarcaDb = await _unidadTrabajo.Marca.Obtener(id);
            if(MarcaDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Marca" });
            }
            _unidadTrabajo.Marca.Remover(MarcaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id=0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Marca.ObtenerTodos();

            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }

            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        #endregion 
    }
}
