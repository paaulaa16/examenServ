using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cripto.Models;

namespace CriptoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly CryptoContext db;

        public QueryController(CryptoContext context)
        {
            db = context;
        }

        [HttpGet("1")]
        public ActionResult Query1(int ValorActual = 50)
        {
            // Ejemplo de método en controlador
            var list = db.Moneda.ToListAsync();

            return Ok(new
            {
                ValorActual = "Parámetros para usar cuando sea posible",
                Descripcion = "Ejemplo en MODO NO ASYNC - DEBE SER ASÍNCRONOS",
                Valores = list,
            });
        }

        [HttpGet("2")]
        public async Task<ActionResult> Query2()
        {
            var list = await db.Contrato.GroupBy(c => c.CarteraId).Select(c => new{
                CarteraId=c.Key,
                TotalMonedas=c.Count()
            }).Where(c => c.TotalMonedas > 2).ToListAsync();

            return Ok(new
            {
                ValorActual = "Parámetros para usar cuando sea posible",
                Descripcion = "Carteras con más de 2 monedas contratadas",
                Valores = list,
            });

        }

        [HttpGet("3")]
        public async Task<ActionResult> Query3()
        {
            var list= await db.Cartera.GroupBy(c => c.Exchange).Select(f => new{
                Exchange=f.Key,
                TotalCarteras=f.Count()
            }).OrderByDescending(f=>f.TotalCarteras).ToListAsync();

            return Ok(new
            {
                ValorActual = "Parámetros para usar cuando sea posible",
                Descripcion = "Exchanges ordenados por números de carteras",
                Valores = list,
            });

        }

        [HttpGet("4")]
        public async Task<ActionResult> Query4()
        {
            var list= await db.Cartera.Select(m => new
                {
                    Exchange = m.Exchange,
                    cantidadMonedas = db.Contrato.Select(m => m.Cantidad)
                }).OrderBy(m => m.cantidadMonedas).ToListAsync();

            return Ok(new
            {
                ValorActual = "Parámetros para usar cuando sea posible",
                Descripcion = "Exchanges ordenados por números de carteras",
                Valores = list,
            });

        }

        [HttpGet("5")]
        public async Task<ActionResult> Query5()
        {
            var list= await db.Moneda.Select(m => new {
                    moneda = m.MonedaId,
                    valorActual = m.Actual
                }).OrderBy(m => m.valorActual).ToListAsync();

            return Ok(new
            {
                ValorActual = "Parámetros para usar cuando sea posible",
                Descripcion = "Exchanges ordenados por números de carteras",
                Valores = list,
            });

        }

        [HttpGet("6")]
        public async Task<ActionResult> Query6()
        {
            var list= await db.Moneda.GroupBy(x => x.MonedaId).Select(a => new {
                    Moneda = a.Key,
                    ValorTotal = a.Sum(x => x.Actual)
                }).OrderByDescending(e => e.ValorTotal).ToListAsync();
                list.ForEach(Console.WriteLine);

            return Ok(new
            {
                ValorActual = "Parámetros para usar cuando sea posible",
                Descripcion = "Exchanges ordenados por números de carteras",
                Valores = list,
            });

        }

    }
}
