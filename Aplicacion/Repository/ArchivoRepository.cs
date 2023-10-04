using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;
using Persistencia;

namespace Aplicacion.Repository;
    public class ArchivoRepository : GenericRepository<Archivo>, IArchivo
    {
        private readonly DbAppContext context;
        public ArchivoRepository(DbAppContext context) : base(context)
        {
            this.context = context;
        }
    }
