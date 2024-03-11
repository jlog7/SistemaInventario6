using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext _db; 

        public MarcaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Marca Marca)
        {
            var MarcaBD = _db.Marcas.FirstOrDefault(b => b.Id == Marca.Id);
            if (MarcaBD != null)
            {
                MarcaBD.Nombre = Marca.Nombre;
                MarcaBD.Descripcion = Marca.Descripcion;
                MarcaBD.Estado = Marca.Estado;    
                _db.SaveChanges();
            }
        }
    }
}
