using System.Collections.Concurrent;
using System.IO.Compression;
using System;
using System.Linq;
using System.Collections.Generic;
using static System.Console;
using Cripto.Models;

namespace EFPrueba
{
    class Program
    {
        static void CrearBD()
        {
            using (var db = new CryptoContext())
            {
                bool deleted = db.Database.EnsureDeleted();
                WriteLine($"Database deleted: {deleted}");
                bool created = db.Database.EnsureCreated();
                WriteLine($"Database created: {created}");
            }
        }
        static void CrearMonedasYCarteras()
        {
            using (var db = new CryptoContext())
            {

                db.Moneda.RemoveRange(db.Moneda);
                db.Cartera.RemoveRange(db.Cartera);
                db.Contrato.RemoveRange(db.Contrato);
                db.SaveChanges();

                db.Moneda.AddRange(
                    new Moneda { MonedaId = "Bitcoin", Maximo = 80M, Actual = 70M },
                    new Moneda { MonedaId = "Etherum", Maximo = 70M, Actual = 60M },
                    new Moneda { MonedaId = "Litecoin", Maximo = 60M, Actual = 60M },
                    new Moneda { MonedaId = "Cardano", Maximo = 50M, Actual = 40M },
                    new Moneda { MonedaId = "Polkadot", Maximo = 40M, Actual = 40M },
                    new Moneda { MonedaId = "Stellar", Maximo = 30M, Actual = 20M },
                    new Moneda { MonedaId = "Dogecoin", Maximo = 20M, Actual = 20M },
                    new Moneda { MonedaId = "ShibaInu", Maximo = 10M, Actual = 10M }
                );
                db.SaveChanges();

                db.Cartera.AddRange(
                    new Cartera { CarteraId = 1, Nombre = "Cartera1", Exchange = "Binance" },
                    new Cartera { CarteraId = 2, Nombre = "Cartera2", Exchange = "Kucoin" },
                    new Cartera { CarteraId = 3, Nombre = "Cartera3", Exchange = "Kraken" },
                    new Cartera { CarteraId = 4, Nombre = "Cartera4", Exchange = "Coinbase" },
                    new Cartera { CarteraId = 5, Nombre = "Cartera5", Exchange = "Binance" },
                    new Cartera { CarteraId = 6, Nombre = "Cartera6", Exchange = "Kucoin" },
                    new Cartera { CarteraId = 7, Nombre = "Cartera7", Exchange = "Binance" }
                );
                db.SaveChanges();
            }
        }

        static void CrearContratos()
        {
            using (var db = new CryptoContext())
            {

                db.Contrato.RemoveRange(db.Contrato);
                db.SaveChanges();

                // Lista de contratos a implementar
                // Cartera #1: ("Bitcoin",2),("Litecoin",3),("Polkadot",4)
                // Cartera #2: ("Dogecoin",3),("ShibaInu",4),("Litecoin",3)
                // Cartera #3: ("Etherum",4),("Cardano",2),("Stellar",1),("Dogecoin",4)
                // Cartera #4: ("Bitcoin",2),("ShibaInu",3),("Stellar",4),("Litecoin",3)
                // Cartera #5: ("Polkadot",3),("Cardano",1)
                // Cartera #6: ("Etherum",4),("Litecoin",2),("Polkadot",1)
                // Cartera #7: ("ShibaInu",2),("Stellar",4)

                // Creacion de contratos
                db.Contrato.AddRange(
                    new Contrato { ContratoId = 1, MonedaId = "Bitcoin", CarteraId = 1, Cantidad = 2 },
                    new Contrato { ContratoId = 2, MonedaId = "Litecoin", CarteraId = 1, Cantidad = 3 },
                    new Contrato { ContratoId = 3, MonedaId = "Polkadot", CarteraId = 1, Cantidad = 4 },

                    new Contrato { ContratoId = 4, MonedaId = "Dogecoin", CarteraId = 2, Cantidad = 3 },
                    new Contrato { ContratoId = 5, MonedaId = "BitcShibaInuoin", CarteraId = 2, Cantidad = 4 },
                    new Contrato { ContratoId = 6, MonedaId = "Litecoin", CarteraId = 2, Cantidad = 3 },

                    new Contrato { ContratoId = 7, MonedaId = "Etherum", CarteraId = 3, Cantidad = 4 },
                    new Contrato { ContratoId = 8, MonedaId = "Cardano", CarteraId = 3, Cantidad = 2 },
                    new Contrato { ContratoId = 9, MonedaId = "Stellar", CarteraId = 3, Cantidad = 1 },
                    new Contrato { ContratoId = 10, MonedaId = "Dogecoin", CarteraId = 3, Cantidad = 4 },

                    new Contrato { ContratoId = 11, MonedaId = "Bitcoin", CarteraId = 4, Cantidad = 2 },
                    new Contrato { ContratoId = 12, MonedaId = "ShibaInu", CarteraId = 4, Cantidad = 3 },
                    new Contrato { ContratoId = 13, MonedaId = "Stellar", CarteraId = 4, Cantidad = 4 },
                    new Contrato { ContratoId = 14, MonedaId = "Litecoin", CarteraId = 4, Cantidad = 3 },

                    new Contrato { ContratoId = 15, MonedaId = "Polkadot", CarteraId = 5, Cantidad = 3 },
                    new Contrato { ContratoId = 16, MonedaId = "Cardano", CarteraId = 5, Cantidad = 1 },

                    new Contrato { ContratoId = 17, MonedaId = "Etherum", CarteraId = 6, Cantidad = 4 },
                    new Contrato { ContratoId = 18, MonedaId = "Litecoin", CarteraId = 6, Cantidad = 2 },
                    new Contrato { ContratoId = 19, MonedaId = "Polkadot", CarteraId = 6, Cantidad = 1 },

                    new Contrato { ContratoId = 20, MonedaId = "ShibaInu", CarteraId = 7, Cantidad = 2 },
                    new Contrato { ContratoId = 21, MonedaId = "Stellar", CarteraId = 7, Cantidad = 4 }
                    
                );

                db.SaveChanges();
            }
        }

        static void ConsultarCriptoDB()
        {
            using (var db = new CryptoContext())
            {
                // 0 Ejemplo
                WriteLine("0.- Ejemplo de presentación de datos anónimos");
                var list0 = new string[] { 
                                "Brachiosaurus",
                                "Amargasaurus",
                                "Mamenchisaurus" 
                            }
                            .Select((d,i) => new {
                                id = i,
                                nombre = d
                            })
                            .ToList();
                list0.ForEach(Console.WriteLine);

                // 1
                WriteLine("1.- Monedas con valor actual superior a 50€ ordenadas alfabéticamente");
                var list1 = db.Moneda.Where(a => a.Actual > 50).OrderByDescending(x => x.MonedaId).ToList();
                list1.ForEach(Console.WriteLine);

                // 1
                WriteLine("2.- Carteras con más de 2 monedas contratadas");
                var list2 = db.Contrato.GroupBy(c => c.CarteraId).Select(c => new{
                CarteraId=c.Key,
                TotalMonedas=c.Count()
                }).Where(c => c.TotalMonedas > 2).ToList();
                list2.ForEach(Console.WriteLine);

                // 1
                WriteLine("3.- Exchanges ordenados por números de carteras");
                var list3 = db.Cartera.GroupBy(c => c.Exchange).Select(f => new {
                Exchange=f.Key,
                TotalCarteras=f.Count()
                }).OrderByDescending(f=>f.TotalCarteras).ToList();
                list3.ForEach(Console.WriteLine);
                

                // 2
                WriteLine("4.- Exchanges ordenados por cantidad de monedas");
                var list4 = db.Cartera.Select(m => new
                {
                    Exchange = m.Exchange,
                    cantidadMonedas = db.Contrato.Select(m => m.Cantidad)
                }).OrderBy(m => m.cantidadMonedas).ToList();
                list4.ForEach(Console.WriteLine);


                // 2
                WriteLine("5.- Monedas en contratos ordenadas por valor total actual");
                var list5 = db.Moneda.Select(m => new {
                    moneda = m.MonedaId,
                    valorActual = m.Actual
                }).OrderBy(m => m.valorActual).ToList();
                list5.ForEach(Console.WriteLine);

                // 2
                WriteLine("6.- Monedas en contratos ordenadas por valor actual total en todos los contratos");
                var list6 = list5.GroupBy(x => x.moneda).Select(a => new {
                    Moneda = a.Key,
                    ValorTotal = a.Sum(x => x.valorActual)
                }).OrderByDescending(e => e.ValorTotal).ToList();
                list6.ForEach(Console.WriteLine);

                // 2
                WriteLine("7.- Idem contando en cuantos contratos aparecen y ordenado por número de contratos");
                var list7 = db.Moneda.ToList();
                list7.ForEach(Console.WriteLine);

                // 3 
                WriteLine("8.- Idem pero con Exchanges ordenados por valor total");
                var list8 = db.Moneda.ToList();
                list8.ForEach(Console.WriteLine);

                // 3
                var porcentaje = 90M;
                WriteLine($"9.- Las Contratos y Monedas de Binance con monedas cuyo valor actual es inferior al {porcentaje}% del valor máximo"); 
                // var list9 = db.Cartera.Where(c => c.Exchange == "Binance")
                // .SelectMany(db.Moneda.Select(a, o) => new{
                //     Moneda = a.MonedaId,
                //     Contrato = a.ContratoId,
                //     Maximo = a.Maximo,
                //     Actual = a.Actual,
                //     Porcentaje = Maximo *porcentaje / 100
                // }).Where(a => a.Porcentaje < porcentaje).OrderBy(a => a.Moneda).ToList();
                //list9.ForEach(Console.WriteLine);

            }
        }
        static void Main(string[] args)
        {
            CrearBD();
            CrearMonedasYCarteras();
            CrearContratos();
            ConsultarCriptoDB();
        }
    }
}
