using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Repositorio.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index(string Mensagem)
        {
            ViewBag.Erro = Mensagem;         
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logar(string Login, string Senha)
        {
            if (String.IsNullOrEmpty(Login) || String.IsNullOrEmpty(Senha))
            {
                return RedirectToAction("Index", new { Mensagem = "Preencha todos os Campos" });
            }
            else
            {
                var json = System.IO.File.ReadAllText(HttpContext.Server.MapPath(@"\Models\LoginModel.json"));
                var usuario = JObject.Parse(json)["Usuarios"].ToArray().Where(x=>x["Login"].ToString() == Login && x["Senha"].ToString() == Senha).ToArray();

                if (usuario.Length < 1)
                {
                   return RedirectToAction("Index", new { Mensagem = "Usuario ou Senha Incorreto(s)" });
                }
                else
                {
                    Session.Timeout = 5;
                    Session["Usuario"] = usuario[0];
                    return RedirectToAction("Index", "Home");
                }               
            }
        }
    }
}