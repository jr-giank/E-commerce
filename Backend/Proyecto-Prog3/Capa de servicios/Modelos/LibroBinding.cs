﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Capa_de_servicios.Modelos
{
    public class LibroBinding
    {
        public int Idlibro { get; set; }
        public string Nombre { get; set; }
        public decimal? Precio { get; set; }
        public string Autor { get; set; }
        public string Año { get; set; }
        public string Editorial { get; set; }
        public int? NumeroPaginas { get; set; }
        public string Idioma { get; set; }
        public int? IdCategoria { get; set; }
        public string RutaFoto { get; set; }
        public IFormFile Foto { get; set; }


    }
}
