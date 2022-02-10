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
        public async Task<ActionResult> Query1(int ValorActual = 50)
        {
            // Ejemplo de método en controlador
            var list = await db.Moneda.Where(a => a.Actual > ValorActual).OrderBy(g => g.MonedaId).ToListAsync();

            return Ok(new
            {
                ValorActual = "1",
                Descripcion = "Monedas con valor actual superior a 50€ ordenadas alfabéticamente",
                Valores = list,
            });
        }

        [HttpGet("2")]
        public async Task<ActionResult> Query2()
        {
            var list = await db.Contrato.GroupBy(c => c.CarteraId).Select(a => new{
                CarteraId=a.Key,
                TotalMonedas=a.Count()
            }).Where(b => b.TotalMonedas > 2).ToListAsync();

            return Ok(new
            {
                ValorActual = "2",
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
            }).OrderByDescending(b=>b.TotalCarteras).ToListAsync();

            return Ok(new
            {
                ValorActual = "3",
                Descripcion = "Exchanges ordenados por números de carteras",
                Valores = list,
            });

        }

        [HttpGet("4")]
        public async Task<ActionResult> Query4()
        {
            var list= await db.Cartera.Join(db.Contrato, c=> c.CarteraId, o=> o.CarteraId, (c, o) => new {
                exchange = c.Exchange,
                TotalMonedas = o.Cantidad
            }).GroupBy(w => w.exchange)
            .Select(l=> new {
                exchange = l.Key,
                TotalMonedas = l.Count()
            }).OrderByDescending(h => h.TotalMonedas).ToListAsync();

            return Ok(new
            {
                ValorActual = "4",
                Descripcion = "Exchanges ordenados por cantidad de monedas",
                Valores = list,
            });
    
        }

        [HttpGet("5")]
        public async Task<ActionResult> Query5()
        {
            var list= await db.Contrato.Join(db.Moneda, c=> c.MonedaId, o=> o.MonedaId, (c, o) => new {
                moneda = c.MonedaId,
                contrato = c.MonedaId + c.CarteraId,
                valorContrato = c.Cantidad * o.Actual
            }).OrderByDescending(x=> x.valorContrato).ToListAsync();

            return Ok(new
            {
                ValorActual = "5",
                Descripcion = "Monedas en contratos ordenadas por valor total actual",
                Valores = list,
            });

        }

        [HttpGet("6")]
        public async Task<ActionResult> Query6()
        {
            var list= await db.Contrato.Join(db.Moneda, c=> c.MonedaId, o=> o.MonedaId, (c, o) => new {
                moneda = c.MonedaId,
                valorContrato = c.Cantidad * o.Actual
            }).GroupBy(y=> y.moneda).Select (j=> new {
                moneda = j.Key,
                ValorTotal = j.Sum(ñ=> ñ.valorContrato)
            }).OrderByDescending(v=> v.ValorTotal).ToListAsync();

            return Ok(new
            {
                ValorActual = "6",
                Descripcion = "Monedas en contratos ordenadas por valor actual total en todos los contratos",
                Valores = list,
            });

        }

        [HttpGet("7")]
        public async Task<ActionResult> Query7()
        {
            var list= await db.Contrato.Join(db.Moneda, c=> c.MonedaId, o=> o.MonedaId, (c, o) => new {
                moneda = c.MonedaId,
                valorTotal = c.Cantidad * o.Actual
            }).GroupBy(y=> y.moneda).Select (j=> new {
                moneda = j.Key,
                ValorTotal = j.Sum(ñ=> ñ.valorTotal), 
                Contratos = j.Count()
            }).OrderByDescending(v=> v.Contratos).ToListAsync();

            return Ok(new
            {
                ValorActual = "7",
                Descripcion = "Idem contando en cuantos contratos aparecen y ordenado por número de contratos",
                Valores = list,
            });

        }

    }
}
