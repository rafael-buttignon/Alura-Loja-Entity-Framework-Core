using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Alura.Loja.Testes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var fulano = new Cliente();
            fulano.Nome = "Fulaninho de Tal";
            fulano.EnderecoDeEntrega = new Endereco()
            {
                Numero = 12,
                Logradoro = "Rua dos Nullables",
                Complemento = "Rua vazia",
                Bairro = "Centro",
                Cidade = "Cidade"
            };

            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                contexto.Clientes.Add(fulano);
                contexto.SaveChanges();
            }

        }

        private static void RelacioMentoMuitosParaMuitos()
        {
            var p1 = new Produto() { Nome = "Suco de Laranja", Categoria = "Bebidas", PrecoUnitario = 8.79, Unidade = "Litros" };
            var p2 = new Produto() { Nome = "Café", Categoria = "Bebidas", PrecoUnitario = 12.45, Unidade = "Gramas" };
            var p3 = new Produto() { Nome = "Macarrão", Categoria = "Alimentos", PrecoUnitario = 4.23, Unidade = "Gramas" };

            var promocaoDePascoa = new Promocao();
            promocaoDePascoa.Descricao = "Páscoa Feliz";
            promocaoDePascoa.DataInicio = DateTime.Now;
            promocaoDePascoa.DataTermino = DateTime.Now.AddMonths(3);

            promocaoDePascoa.IncluiProduto(p1);
            promocaoDePascoa.IncluiProduto(p2);
            promocaoDePascoa.IncluiProduto(p3);

            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                //contexto.Promocoes.Add(promocaoDePascoa);
                var promocao = contexto.Promocoes.Find(1);
                contexto.Promocoes.Remove(promocao);
                contexto.SaveChanges();

            }
        }

        private static void ExemploPaoFrancesClassCompra()
        {
            var paoFrances = new Produto();
            paoFrances.Nome = "Pão Francês";
            paoFrances.PrecoUnitario = 0.40;
            paoFrances.Unidade = "Unidade";
            paoFrances.Categoria = "Padaria";

            var compra = new Compra();
            compra.Quantidade = 6;
            compra.Produto = paoFrances;
            compra.Preco = paoFrances.PrecoUnitario * compra.Quantidade;

            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());


                contexto.Compras.Add(compra);

                ExibeEntries(contexto.ChangeTracker.Entries());

                contexto.SaveChanges();
            }
        }

        private static void ExibeEntries(IEnumerable<EntityEntry> entries)
        {
            foreach (var e in entries)
            {
                Console.WriteLine(e.Entity.ToString() + " - " + e.State);
            }
        }


        //private static void ChangeTrackerGerenciamentoDeEstados()
        //{

        //    using (var contexto = new LojaContext())
        //    {
        //        var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
        //        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        //        loggerFactory.AddProvider(SqlLoggerProvider.Create());

        //        var produtos = contexto.Produtos.ToList();

        //        ExibeEntries(contexto.ChangeTracker.Entries());


        //        var novoProduto = new Produto()
        //        {
        //            Nome = "Sabão em Pó",
        //            Categoria = "Limpeza",
        //            PrecoUnitario = 12.99
        //        };
        //        contexto.Produtos.Add(novoProduto);

        //        ExibeEntries(contexto.ChangeTracker.Entries());

        //        contexto.Produtos.Remove(novoProduto);

        //        ExibeEntries(contexto.ChangeTracker.Entries());

        //        //contexto.SaveChanges();
        //        var entry = contexto.Entry(novoProduto);
        //        Console.WriteLine(entry.Entity.ToString() + " - " + entry.State);

        //    }
        //}

        //private static void ExibeEntries(IEnumerable<EntityEntry> entries)
        //{
        //    foreach (var e in entries)
        //    {
        //        Console.WriteLine(e.Entity.ToString() + " - " + e.State);
        //    }
        //}

        //private static void AtualizarProduto()
        //{
        //    GravarUsandoEntity();
        //    RecuperarProdutos();

        //    using (var repo = new ProdutoDAOEntity())
        //    {
        //        Produto primeiro = repo.Produtos().First();
        //        primeiro.Nome = "Rafael Royale - Editado";
        //        repo.Atualizar(primeiro);
        //    }
        //    RecuperarProdutos();
        //}

        //private static void ExcluirProdutos()
        //{
        //    using (var repo = new ProdutoDAOEntity())
        //    {
        //        IList<Produto> produtos = repo.Produtos();
        //        foreach (var item in produtos)
        //        {
        //            repo.Remover(item);
        //        }
        //    }
        //}

        //private static void RecuperarProdutos()
        //{
        //    using (var repo = new ProdutoDAOEntity())
        //    {
        //        IList<Produto> produtos = repo.Produtos();
        //        Console.WriteLine("Foram encontrados {0} produto(s).", produtos.Count);
        //        foreach (var item in produtos)
        //        {
        //            Console.WriteLine(item.Nome);
        //        }
        //    }
        //}

        //private static void GravarUsandoEntity()
        //{
        //    Produto p = new Produto();
        //    p.Nome = "Cassino Royale";
        //    p.Categoria = "Filmes";
        //    p.PrecoUnitario = 19.89;

        //    using (var repo = new ProdutoDAOEntity())
        //    {
        //        repo.Adicionar(p);
        //    }
        //}

        //private static void GravarUsandoAdoNet()
        //{
        //    Produto p = new Produto();
        //    p.Nome = "Harry Potter e a Ordem da Fênix";
        //    p.Categoria = "Livros";
        //    p.PrecoUnitario = 19.89;

        //    using (var repo = new ProdutoDAO())
        //    {
        //        repo.Adicionar(p);
        //    }
        //}
    }
}
