using Newtonsoft.Json.Linq;
using Repositorio.Metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Repositorio.Controllers
{
    [VerificaSession]
    public class HomeController : Controller
    {
        public Abstracao.Pasta PastaAbstrata = new Abstracao.Pasta();

        [HttpGet]
        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Cards()           
        {
            var usuariologado = JObject.Parse(Session["Usuario"].ToString());

            var Pastadousuario = new System.IO.DirectoryInfo(HttpContext.Server.MapPath(@"Repositorio\" + usuariologado["Login"]));

            var cardsdousuario = Pastadousuario.GetDirectories();

            var cards = new List<Abstracao.Card>();

            for (int i = 0; i < cardsdousuario.Length; i++)
            {
                var json = System.IO.File.ReadAllText(HttpContext.Server.MapPath(@"Repositorio\" + usuariologado["Login"] + @"\Imagens.json"));

                var imagem = JObject.Parse(json)["Imagens"].ToArray().FirstOrDefault(x=>x["Foto"].ToString() == $"~/Content/{cardsdousuario[i].Name}");

                cards.Add(new Abstracao.Card { Caminho = cardsdousuario[i].FullName, Titulo = cardsdousuario[i].Name, Imagem = imagem == null ? "~/Content/Empty.jpg" : imagem["Foto"].ToString() });
            }

            return PartialView(cards);
        }

        

        public List<Abstracao.Pasta> RecursaodePastas(System.IO.DirectoryInfo Pastapai, Abstracao.Pasta path)
        {                      
            var pastasdapastapai = Pastapai.GetDirectories();

            var arquivospastapai = Pastapai.GetFiles();

            path.Caminho = Pastapai.FullName;

            path.Titulo = Pastapai.Name;

            if (path.Pastas == null)
            {
                path.Pastas = new List<Abstracao.Pasta>();
            }

            if (path.Arquivos == null)
            {
                path.Arquivos = new List<Abstracao.Arquivos>();
            }

            for (int i = 0; i < pastasdapastapai.Length; i++)
            {
                var pastavirtualizada = new Abstracao.Pasta { Caminho = pastasdapastapai[i].FullName, Titulo = pastasdapastapai[i].Name, Arquivos = new List<Abstracao.Arquivos>(), Pastas = new List<Abstracao.Pasta>() };
                RecursaodePastas(pastasdapastapai[i], pastavirtualizada);
                path.Pastas.Add(pastavirtualizada);
            }

            for (int i = 0; i < arquivospastapai.Length; i++)
            {              
                path.Arquivos.Add(new Abstracao.Arquivos { Caminho = arquivospastapai[i].FullName, Titulo = arquivospastapai[i].FullName });
            }

            return new List<Abstracao.Pasta> { path };
        }
       
        public ActionResult Pasta(string destino)
        {
            var MinhaPasta = new Abstracao.Pasta();

            var pastafisica = new System.IO.DirectoryInfo(destino);

            MinhaPasta.Pastas = new List<Abstracao.Pasta>();

            MinhaPasta.Arquivos = new List<Abstracao.Arquivos>();

            MinhaPasta.Titulo = pastafisica.Name;

            MinhaPasta.Caminho = pastafisica.FullName;

            var pastasdapastafisica = pastafisica.GetDirectories();

            var arquivosdapastafisica = pastafisica.GetFiles();

            for (int i = 0; i < arquivosdapastafisica.Length; i++)
            {
                MinhaPasta.Arquivos.Add(new Abstracao.Arquivos { Titulo = arquivosdapastafisica[i].Name, Caminho = arquivosdapastafisica[i].FullName });
            }

            for (int i = 0; i < pastasdapastafisica.Length; i++)
            {
                MinhaPasta.Pastas.AddRange(RecursaodePastas(pastasdapastafisica[i], new Abstracao.Pasta()));
            }

            return PartialView(MinhaPasta);
        }
    }
}