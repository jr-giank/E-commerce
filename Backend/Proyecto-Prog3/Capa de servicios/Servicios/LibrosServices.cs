﻿using Capa_de_datos;
using Capa_de_servicios.Request;
using Capa_de_servicios.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capa_de_servicios.Modelos;

namespace Capa_de_servicios.Servicios
{
    public class LibrosServices : ILibroServices
    {
        private readonly E_CommerceContext _context;

        public LibrosServices(E_CommerceContext context)
        {
            _context = context;
        }

        public async Task<Respuestas> AddLibro(LibroBinding libro)
        {
            try
            {
                var respuesta = new Respuestas();

                var book = new Libro
                {
                    Nombre = libro.Nombre,
                    Precio = libro.Precio,
                    Autor = libro.Autor,
                    Año = libro.Año,
                    Editorial = libro.Editorial,
                    NumeroPaginas = libro.NumeroPaginas,
                    Idioma = libro.Idioma,
                    IdCategoria = libro.IdCategoria,
                    RutaFoto = libro.RutaFoto,
                    EnVenta = true
                };
                await _context.Libros.AddAsync(book);
                await _context.SaveChangesAsync();
                respuesta.Mensaje = "El libro ha sido agregado correctamente";
                respuesta.Exito = 1;
                return respuesta;
            }
            catch (Exception ex)
            {
                var respuesta = new Respuestas();
                respuesta.Data = 0;
                respuesta.Mensaje = "Ha ocurrido un error el error es :" + ex.Message;
                return respuesta;
            }
        }

        public async Task<Respuestas> EliminarLibro(int eliminar)
        {
            try
            {

                {
                    var respuesta = new Respuestas();
                    var libro = await _context.Libros.FirstOrDefaultAsync(a => a.IdLibro == eliminar);
                    libro.EnVenta = false;
                    _context.Update(libro);
                    await _context.SaveChangesAsync();

                    respuesta.Mensaje = "el libro ha sido eliminado correctamente";
                    respuesta.Exito = 1;
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                var respuesta = new Respuestas();
                respuesta.Data = 0;
                respuesta.Mensaje = "Ha ocurrido un error el error es :" + ex.Message;
                return respuesta;
            }



        }

        public async Task<Respuestas> Getbooks()
        {
            try
            {

                Respuestas oRespuesta = new Respuestas();
                var listaLibro = await (from libros in _context.Libros
                                        join categoria in _context.Categoria
                                        on libros.IdCategoria equals categoria.IdCategoria
                                        where libros.EnVenta == true
                                        select new LibroViewModel
                                        {
                                            Nombre = libros.Nombre,
                                            Autor = libros.Autor,
                                            Categoria = categoria.CategoriaLibro,
                                            Editorial = libros.Editorial,
                                            NumeroPaginas = libros.NumeroPaginas,
                                            Idioma = libros.Idioma,
                                            RutaFoto = libros.RutaFoto,
                                            IdCategoria = libros.IdCategoria,
                                            Anio = libros.Año,
                                            Idlibro = libros.IdLibro,
                                            Precio = libros.Precio,
                                            EnVenta = libros.EnVenta
                                        }).ToListAsync();

                int cantidad = 0;
                double averagef = 0;

                foreach (var libro in listaLibro)
                {
                    var average = (from califica in _context.Calificaciones
                                   where califica.IdLibro == libro.Idlibro
                                   select califica.Calificación
                               ).Average();

                    cantidad = (from califica in _context.Calificaciones
                                where califica.IdLibro == libro.Idlibro
                                select califica.Calificación
                                   ).Count();

                    if (average == null)
                        average = 0;

                    averagef = (double)(Math.Floor((decimal)(average * 10)) / 10);

                    libro.CantidadCalificado = cantidad;
                    libro.PromedioCalificacion = averagef;
                }
                oRespuesta.Data = listaLibro;
                return oRespuesta;
            }
            catch (Exception ex)
            {
                var respuesta = new Respuestas();
                respuesta.Data = 0;
                respuesta.Mensaje = "Ha ocurrido un error el error es :" + ex.Message;
                return respuesta;
            }

        }

        public async Task<Respuestas> GetbooksAdmin()
        {
            try
            {

                Respuestas oRespuesta = new Respuestas();
                var listaLibro = await (from libros in _context.Libros
                                        join categoria in _context.Categoria
                                        on libros.IdCategoria equals categoria.IdCategoria
                                        where libros.EnVenta == true
                                        select new LibroViewModel
                                        {
                                            Nombre = libros.Nombre,
                                            Autor = libros.Autor,
                                            Categoria = categoria.CategoriaLibro,
                                            Editorial = libros.Editorial,
                                            NumeroPaginas = libros.NumeroPaginas,
                                            Idioma = libros.Idioma,
                                            RutaFoto = libros.RutaFoto,
                                            IdCategoria = libros.IdCategoria,
                                            Anio = libros.Año,
                                            Idlibro = libros.IdLibro,
                                            Precio = libros.Precio,
                                            EnVenta = libros.EnVenta
                                        }).ToListAsync();

                oRespuesta.Data = listaLibro;
                return oRespuesta;
            }
            catch (Exception ex)
            {
                var respuesta = new Respuestas();
                respuesta.Data = 0;
                respuesta.Mensaje = "Ha ocurrido un error el error es :" + ex.Message;
                return respuesta;
            }

        }

        public async Task<Respuestas> EditBooks(LibroBinding omodel)
        {
            Respuestas orepuesta = new Respuestas();
            try
            {
                Libro olibro = await _context.Libros.FindAsync(omodel.Idlibro);
                olibro.Nombre = omodel.Nombre;
                olibro.Precio = omodel.Precio;
                olibro.Autor = omodel.Autor;
                olibro.Año = omodel.Año;
                olibro.Editorial = omodel.Editorial;
                olibro.NumeroPaginas = omodel.NumeroPaginas;
                olibro.Idioma = omodel.Idioma;
                olibro.IdCategoria = omodel.IdCategoria;

                if (omodel.RutaFoto != null)
                    olibro.RutaFoto = omodel.RutaFoto;

                orepuesta.Mensaje = "el libro ha sido editado correctamente";
                _context.Entry(olibro).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
                orepuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                orepuesta.Mensaje = ex.Message;
            }
            return orepuesta;
        }

        public async Task<Respuestas> GetbookByID(int id, string NombreUsuario = "")
        {
            try
            {
                Respuestas orepuesta = new Respuestas();

                int cantidad = 0;
                double averagef = 0;
                int? Calificado = 0;
                bool Permiso = false;

                var average = (from califica in _context.Calificaciones
                               where califica.IdLibro == id
                               select califica.Calificación
                               ).Average();

                cantidad = (from califica in _context.Calificaciones
                            where califica.IdLibro == id
                            select califica.Calificación
                               ).Count();

                if (average == null)
                    average = 0;

                averagef = (double)(Math.Floor((decimal)(average * 10)) / 10);

                if (NombreUsuario != "")
                {
                    Calificado = _context.Calificaciones.FirstOrDefault(a => a.IdLibro == id && a.NombreUsuario == NombreUsuario)?.Calificación ?? 0;

                    if (Calificado == 0)
                    {
                        Permiso = true;
                    }
                }

                var Libro = from libros in _context.Libros
                            join categoria in _context.Categoria
                            on libros.IdCategoria equals categoria.IdCategoria
                            where libros.IdLibro == id && libros.EnVenta == true
                            select new LibroViewModel
                            {
                                Nombre = libros.Nombre,
                                Autor = libros.Autor,
                                Categoria = categoria.CategoriaLibro,
                                Editorial = libros.Editorial,
                                NumeroPaginas = libros.NumeroPaginas,
                                Idioma = libros.Idioma,
                                RutaFoto = libros.RutaFoto,
                                IdCategoria = libros.IdCategoria,
                                Anio = libros.Año,
                                Idlibro = libros.IdLibro,
                                Precio = libros.Precio,
                                EnVenta = libros.EnVenta,
                                CantidadCalificado = cantidad,
                                PromedioCalificacion = averagef,
                                PermisoCalificaar = Permiso
                            };
                orepuesta.Data = Libro;
                return orepuesta;
            }
            catch (Exception ex)
            {
                var respuesta = new Respuestas();
                respuesta.Data = 0;
                respuesta.Mensaje = "Ha ocurrido un error el error es :" + ex.Message;
                return respuesta;
            }
        }

        public async Task<Respuestas> GetbookByName(string nombre)
        {
            try
            {
                Respuestas orepuesta = new Respuestas();

                var Libro = from libros in _context.Libros
                            join categoria in _context.Categoria
                            on libros.IdCategoria equals categoria.IdCategoria
                            where libros.Nombre.Contains(nombre) && libros.EnVenta == true
                            select new LibroViewModel
                            {
                                Nombre = libros.Nombre,
                                Autor = libros.Autor,
                                Categoria = categoria.CategoriaLibro,
                                Editorial = libros.Editorial,
                                NumeroPaginas = libros.NumeroPaginas,
                                Idioma = libros.Idioma,
                                RutaFoto = libros.RutaFoto,
                                IdCategoria = libros.IdCategoria,
                                Anio = libros.Año,
                                Idlibro = libros.IdLibro,
                                Precio = libros.Precio,
                                EnVenta = libros.EnVenta
                            };
                orepuesta.Data = Libro;
                return orepuesta;
            }
            catch (Exception ex)
            {
                var respuesta = new Respuestas();
                respuesta.Data = 0;
                respuesta.Mensaje = "Ha ocurrido un error el error es :" + ex.Message;
                return respuesta;
            }
        }

        public List<LibroViewModel> GetbookByGender(List<int?> genero, List<LibroViewModel> libros)
        {
            if (genero.Count > 0)
            {

                var librosCategoria = libros.Where(a => genero.Contains(a.IdCategoria)).ToList();
                return librosCategoria;

            }
            return libros;
        }

        public bool GetStarAverage(int idLibro, int calificacion)
        {
            var average = (from a in _context.Calificaciones
                           where a.IdLibro == idLibro
                           select a.Calificación
                            ).Average();

            return average >= calificacion;
        }

        public List<LibroViewModel> GetbookByCalificacion(int calificacion, List<LibroViewModel> libros)
        {
            List<LibroViewModel> librosCalificacion = new List<LibroViewModel>();

            if (calificacion > 0)
            {
                foreach (var libro in libros)
                {
                    bool cierto = GetStarAverage(libro.Idlibro, calificacion);
                    if (cierto == true)
                        librosCalificacion.Add(libro);
                }
                return librosCalificacion;
            }

            else
                return libros;
        }

        private List<LibroViewModel> GetbookByCost(int cost, List<LibroViewModel> libros)
        {
            if (cost == 1)
            {
                var librosPrecio = libros.Where(a => (a.Precio <= 20 && a.Precio >= 10)).ToList();
                return librosPrecio;
            }
            else if (cost == 2)
            {
                var librosPrecio = libros.Where(a => (a.Precio <= 40 && a.Precio >= 21)).ToList();
                return librosPrecio;
            }
            else if (cost == 3)
            {
                var librosPrecio = libros.Where(a => (a.Precio <= 60 && a.Precio >= 41)).ToList();
                return librosPrecio;
            }
            else if (cost == 4)
            {
                var librosPrecio = libros.Where(a => (a.Precio <= 80 && a.Precio >= 61)).ToList();
                return librosPrecio;
            }
            else if (cost == 5)
            {
                var librosPrecio = libros.Where(a => (a.Precio >= 81)).ToList();
                return librosPrecio;
            }
            else
                return libros;

        }

        private List<LibroViewModel> GetbookByLanguage(int Idioma, List<LibroViewModel> LibrosIdiomas)
        {
            if (Idioma == 1)
            {
                var librosIdioma = LibrosIdiomas.Where(a => (a.Idioma == "En")).ToList();
                return librosIdioma;
            }
            else if (Idioma == 2)
            {
                var librosIdioma = LibrosIdiomas.Where(a => (a.Idioma == "Es")).ToList();
                return librosIdioma;
            }
            else
                return LibrosIdiomas;
        }

        public async Task<Respuestas> FilterBooks(LibroFiltradoBinding Filtro)
        {
            Respuestas orespuesta = new Respuestas();

            //var listaLibro = await (from libros in _context.Libros
            //                        join categoria in _context.Categoria
            //                        on libros.IdCategoria equals categoria.IdCategoria
            //                        where libros.EnVenta == true
            //                        select new LibroViewModel
            //                        {
            //                            Nombre = libros.Nombre,
            //                            Autor = libros.Autor,
            //                            Categoria = categoria.CategoriaLibro,
            //                            Editorial = libros.Editorial,
            //                            NumeroPaginas = libros.NumeroPaginas,
            //                            Idioma = libros.Idioma,
            //                            RutaFoto = libros.RutaFoto,
            //                            IdCategoria = libros.IdCategoria,
            //                            Anio = libros.Año,
            //                            Idlibro = libros.IdLibro,
            //                            Precio = libros.Precio,
            //                            EnVenta = libros.EnVenta
            //                        }).ToListAsync();

            //int cantidad = 0;
            //double averagef = 0;

            //foreach (var libro in listaLibro)
            //{
            //    var average = (from califica in _context.Calificaciones
            //                   where califica.IdLibro == libro.Idlibro
            //                   select califica.Calificación
            //               ).Average();

            //    cantidad = (from califica in _context.Calificaciones
            //                where califica.IdLibro == libro.Idlibro
            //                select califica.Calificación
            //                   ).Count();

            //    if (average == null)
            //        average = 0;

            //    averagef = (double)(Math.Floor((decimal)(average * 10)) / 10);

            //    libro.CantidadCalificado = cantidad;
            //    libro.PromedioCalificacion = averagef;
            //}

            var LibrosGenero = GetbookByGender(Filtro.Genero, Filtro.Libros);
            var LibrosPrecio = GetbookByCost(Filtro.Precio, LibrosGenero);
            var LibrosCalificado = GetbookByCalificacion(Filtro.Calificacion, LibrosPrecio);
            var LibrosIdioma = GetbookByLanguage(Filtro.Idioma, LibrosCalificado);

            orespuesta.Data = LibrosIdioma;
            orespuesta.Exito = 1;
            return orespuesta;
        }

        public async Task<Respuestas> RateBook(CalificacionBinding calificacion)
        {
            try
            {
                Respuestas orespuesta = new Respuestas();

                var calificar = new Calificacione
                {
                    Calificación = calificacion.Calificacion,
                    NombreUsuario = calificacion.NombreUsuario,
                    IdLibro = calificacion.IdLibro
                };

                await _context.Calificaciones.AddAsync(calificar);
                await _context.SaveChangesAsync();

                orespuesta.Mensaje = "Calificacion agregada con exito!!!";
                orespuesta.Exito = 1;

                return orespuesta;
            }

            catch (Exception ex)
            {
                var respuesta = new Respuestas();
                respuesta.Data = 0;
                respuesta.Mensaje = "Ha ocurrido un error el error es :" + ex.Message;
                return respuesta;
            }
        }
    }
}